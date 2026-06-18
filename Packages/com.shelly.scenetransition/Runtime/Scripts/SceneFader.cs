using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Shelly.SceneTransition
{
    [RequireComponent(typeof(CanvasGroup))]
    public class SceneFader : SceneTransitionBehaviour
    {
        [SerializeField]            private CanvasGroup canvasGroup;
        [SerializeField, Min(0f)]   private float       fadeInDuration  = 0.3f;
        [SerializeField, Min(0f)]   private float       fadeOutDuration = 0.3f;

        private void Reset() => canvasGroup = GetComponent<CanvasGroup>();

        private void Awake()
        {
            if (canvasGroup == null)
                canvasGroup = GetComponent<CanvasGroup>();
        }

        public override async UniTask TransitionIn()
        {
            canvasGroup.blocksRaycasts = true;
            await Fade(0f, 1f, fadeInDuration);
        }

        public override async UniTask TransitionOut()
        {
            await Fade(1f, 0f, fadeOutDuration);
            canvasGroup.blocksRaycasts = false;
        }

        private async UniTask Fade(float from, float to, float duration)
        {
            if (duration <= 0f)
            {
                canvasGroup.alpha = to;
                return;
            }

            canvasGroup.alpha = from;

            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed          += Time.unscaledDeltaTime; // 不受 timeScale 影響
                canvasGroup.alpha = Mathf.Lerp(from, to, elapsed / duration);
                await UniTask.Yield();
            }

            canvasGroup.alpha = to;
        }
    }
}
