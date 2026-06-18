using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Shelly.SceneTransition
{
    /// <summary>
    /// 轉場表現的抽象契約。繼承它就能自訂任意轉場（淡入淡出、滑動、遮罩動畫…），
    /// 把實作做成 prefab 拖進 <c>SceneWorkflowAsset</c> 的 Fader Prefab 欄位即可。
    /// 內建預設實作見 <see cref="SceneFader"/>。
    /// </summary>
    public abstract class SceneTransitionBehaviour : MonoBehaviour
    {
        public abstract UniTask TransitionIn();

        public abstract UniTask TransitionOut();
    }
}
