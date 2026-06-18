using PrimeTween;
using UnityEngine;
using UnityEngine.UI;

namespace Shelly.UITools
{
    public static class UITween
    {
        public static Tween ScaleTo(this Transform t, float endValue, float duration = 0.3f, Ease ease = Ease.OutBack)
            => Tween.Scale(t, endValue, duration, ease);

        public static Tween ScaleIn(this Transform t, float duration = 0.3f, Ease ease = Ease.OutBack)
            => Tween.Scale(t, endValue: 1f, duration, ease);

        public static Tween ScaleOut(this Transform t, float duration = 0.3f, Ease ease = Ease.InBack)
            => Tween.Scale(t, endValue: 0f, duration, ease);

        public static Tween AlphaTo(this Graphic graphic, float endValue, float duration = 0.3f, Ease ease = Ease.Linear)
            => Tween.Alpha(graphic, endValue, duration, ease);

        public static Tween FadeIn(this Graphic graphic, float duration = 0.3f, Ease ease = Ease.Linear)
            => Tween.Alpha(graphic, endValue: 1f, duration, ease);

        public static Tween FadeOut(this Graphic graphic, float duration = 0.3f, Ease ease = Ease.Linear)
            => Tween.Alpha(graphic, endValue: 0f, duration, ease);
    }
}

