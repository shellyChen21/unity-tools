using System;
using PrimeTween;
using TMPro;
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

        public static Tween TypeWrite(this TextMeshProUGUI text, string content, float duration = 1f, Action onComplete = null)
        {
            text.maxVisibleCharacters = 0;
            text.text                 = content;
            text.ForceMeshUpdate();

            return Tween.Custom(0, content.Length, duration,
                            val => text.maxVisibleCharacters = (int)val, Ease.Linear)
                        .OnComplete(() => onComplete?.Invoke());
        }

        public static Sequence ButtonPress(this Transform t,
            float pressScale = 0.8f, float pressDuration = 0.05f, float releaseDuration = 0.05f, Ease ease = Ease.Linear)
            => Sequence.Create()
                       .Chain(Tween.Scale(t, pressScale, pressDuration, ease))
                       .Chain(Tween.Scale(t, 1f, releaseDuration, ease));
    }
}