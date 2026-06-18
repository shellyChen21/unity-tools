using UnityEngine;
using UnityEngine.UI;

namespace Shelly.UITools
{
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(CanvasScaler))]
    public class CanvasResolutionHandler : MonoBehaviour
    {
        [SerializeField] private int planeDistance = 100;

        private CanvasScaler canvasScaler;
        private Canvas       canvas;

        private void Awake()
        {
            canvasScaler = GetComponent<CanvasScaler>();
            canvas       = GetComponent<Canvas>();

            canvas.worldCamera      = Camera.main;
            canvas.sortingLayerName = "UI";
            canvas.planeDistance    = planeDistance;

            SetScaler();
        }

        public void SetScaler()
        {
            float screenWidthScale  = Screen.width  / canvasScaler.referenceResolution.x;
            float screenHeightScale = Screen.height / canvasScaler.referenceResolution.y;

            canvasScaler.matchWidthOrHeight = screenWidthScale > screenHeightScale ? 1 : 0;
        }
    }
}
