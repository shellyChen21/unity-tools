using UnityEngine;

namespace Shelly.UITools
{
    public static class UIVisibility
    {
        public static void Show(this CanvasGroup canvasGroup)
        {
            canvasGroup.alpha          = 1f;
            canvasGroup.interactable   = true;
            canvasGroup.blocksRaycasts = true;
        }

        public static void Hide(this CanvasGroup canvasGroup)
        {
            canvasGroup.alpha          = 0f;
            canvasGroup.interactable   = false;
            canvasGroup.blocksRaycasts = false;
        }
    }
}
