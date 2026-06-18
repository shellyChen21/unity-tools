using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Shelly.SceneTransition
{
    /// <summary>
    /// 流程中的一個步驟。子類即是 <c>SceneWorkflowAsset</c> 步驟清單裡可選的型別
    /// （透過 <c>[SerializeReference]</c> 多型序列化），同時直接負責執行。
    /// </summary>
    [Serializable]
    public abstract class SceneStep
    {
        public abstract UniTask Execute(WorkflowContext ctx);
    }

    /// <summary>蓋住畫面：建立轉場實例（若尚未建立）並進場。</summary>
    [Serializable]
    public class TransitionInStep : SceneStep
    {
        public override async UniTask Execute(WorkflowContext ctx)
        {
            if (ctx.FaderPrefab == null)
            {
                Debug.LogError("[SceneTransition] SceneWorkflowAsset 未設定 Fader Prefab，無法執行 TransitionIn。");
                return;
            }

            if (ctx.CurrentFader == null)
            {
                ctx.CurrentFader = UnityEngine.Object.Instantiate(ctx.FaderPrefab);
                UnityEngine.Object.DontDestroyOnLoad(ctx.CurrentFader.gameObject);
            }

            await ctx.CurrentFader.TransitionIn();
        }
    }

    /// <summary>揭開畫面：退場並銷毀轉場實例。</summary>
    [Serializable]
    public class TransitionOutStep : SceneStep
    {
        public override async UniTask Execute(WorkflowContext ctx)
        {
            if (ctx.CurrentFader == null)
            {
                Debug.LogWarning("[SceneTransition] 沒有作用中的轉場可 TransitionOut（是否漏了 TransitionIn？）。");
                return;
            }

            await ctx.CurrentFader.TransitionOut();
            UnityEngine.Object.Destroy(ctx.CurrentFader.gameObject);
            ctx.CurrentFader = null;
        }
    }

    /// <summary>以 additive 疊上一個場景，並設為 active scene。</summary>
    [Serializable]
    public class PushSceneStep : SceneStep
    {
        [SceneName] public string sceneName;

        public override async UniTask Execute(WorkflowContext ctx)
        {
            if (string.IsNullOrEmpty(sceneName))
            {
                Debug.LogError("[SceneTransition] PushScene 步驟未設定 sceneName。");
                return;
            }

            await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            SceneStack.Push(sceneName);

            var loaded = SceneManager.GetSceneByName(sceneName);
            if (loaded.IsValid())
                SceneManager.SetActiveScene(loaded);
        }
    }

    /// <summary>卸載堆疊頂層場景，退回上一層。</summary>
    [Serializable]
    public class PopSceneStep : SceneStep
    {
        public override async UniTask Execute(WorkflowContext ctx)
        {
            if (SceneStack.Count == 0)
            {
                Debug.LogWarning("[SceneTransition] 場景堆疊為空，無法 Pop。");
                return;
            }

            var sceneName = SceneStack.Pop();
            await SceneManager.UnloadSceneAsync(sceneName);

            if (SceneStack.Count > 0)
            {
                var top = SceneManager.GetSceneByName(SceneStack.Peek());
                if (top.IsValid())
                    SceneManager.SetActiveScene(top);
            }
        }
    }

    /// <summary>卸載堆疊上所有 additive 場景（底層 boot 場景保留）。</summary>
    [Serializable]
    public class PopAllScenesStep : SceneStep
    {
        public override async UniTask Execute(WorkflowContext ctx)
        {
            while (SceneStack.Count > 0)
            {
                var sceneName = SceneStack.Pop();
                await SceneManager.UnloadSceneAsync(sceneName);
            }
        }
    }

    /// <summary>等待一段時間（秒）。</summary>
    [Serializable]
    public class DelayStep : SceneStep
    {
        [Min(0f)] public float seconds = 1f;

        public override async UniTask Execute(WorkflowContext ctx)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(seconds), ignoreTimeScale: true);
        }
    }
}
