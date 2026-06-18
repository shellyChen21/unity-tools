using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Shelly.SceneTransition
{
    /// <summary>
    /// 一段可在 inspector 編輯的場景轉場流程：拉一串步驟（疊場景、延遲、淡入淡出、退層），
    /// 執行時依序 await。用 <see cref="SceneLoader.Run"/> 觸發。
    /// </summary>
    [CreateAssetMenu(fileName = "SceneWorkflow", menuName = "Shelly/Scene Workflow")]
    public class SceneWorkflowAsset : ScriptableObject
    {
        [Tooltip("轉場 prefab（含 SceneTransitionBehaviour 子類，例如內建的 SceneFader）。TransitionIn/Out 步驟會用到。")]
        [SerializeField] private SceneTransitionBehaviour faderPrefab;

        [SerializeReference] private List<SceneStep> steps = new();

        public async UniTask Execute()
        {
            var ctx = new WorkflowContext(faderPrefab);

            foreach (var step in steps)
            {
                if (step != null)
                    await step.Execute(ctx);
            }
        }

#if UNITY_EDITOR
        // 給自訂 inspector 使用
        public List<SceneStep> Steps => steps;
#endif
    }
}
