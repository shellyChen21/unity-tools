using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Shelly.SceneTransition
{
    public static class SceneLoader
    {
        public static int StackCount => SceneStack.Count;

        public static void Run(SceneWorkflowAsset workflow, Action onComplete = null)
        {
            RunInternal(workflow, onComplete).Forget();
        }

        /// <summary>執行流程並回傳可 await 的 UniTask。</summary>
        public static UniTask RunAsync(SceneWorkflowAsset workflow)
        {
            return RunInternal(workflow, null);
        }

        private static async UniTask RunInternal(SceneWorkflowAsset workflow, Action onComplete)
        {
            if (workflow == null)
            {
                Debug.LogError("[SceneTransition] SceneLoader.Run 傳入的 workflow 為 null。");
                return;
            }

            await workflow.Execute();
            onComplete?.Invoke();
        }
    }
}
