using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace UnityCommon
{
    public static class UnityCommon
    {
        public static IEnumerator IDoCount(float startNum, float endNum, float time, Action<float> action, Ease easeType, bool isDepentUpdate = true)
        {
            float startTime = GetTime(isDepentUpdate);
            float currentTime = startTime;
            float endTime = currentTime += time;
            while (true)
            {
                currentTime = GetTime(isDepentUpdate);
                if (currentTime < endTime)
                {
                    action(DOVirtual.EasedValue(startNum, endNum, (currentTime - startTime) / time, easeType));
                    yield return null;
                }
                else
                {
                    action(endNum);
                    break;
                }
            }

        }

        public static Coroutine DoCount(this MonoBehaviour ctx, float startNum, float endNum, float time, Action<float> action, bool isDepentUpdate = true, Ease easeType = Ease.OutCirc)
        {
            return ctx.StartCoroutine(IDoCount(startNum, endNum, time, action, easeType, isDepentUpdate));
        }

        public static float GetTime(bool isDepentUpdate)
        {
            return isDepentUpdate ? Time.unscaledTime : Time.time;
        }

        public static bool IsRaycastBlocking(this Canvas canvas, Vector2 screenPoint, Camera camera)
        {
            var graphicList = GraphicRegistry.GetGraphicsForCanvas(LevelGUI.Instance.canvas);
            for (int i = 0; i < graphicList.Count; i++)
            {
                if (graphicList[i].rectTransform.IsUnderMouse(screenPoint, camera)) return true;
            }
            return false;
        }

        public static bool IsUnderMouse(this RectTransform rectTransform, Vector2 screenPoint, Camera camera)
        {
            return RectTransformUtility.RectangleContainsScreenPoint(rectTransform, screenPoint, camera);
        }
        public static Tweener DoBlink(this Graphic graphic, float blinkAlpha, float duration = 0.5f)
        {
            return graphic.DOFade(blinkAlpha, duration).SetLoops(-1, LoopType.Yoyo).SetUpdate(true);
        }

    }
    public class ThreadBlocker : CustomYieldInstruction
    {
        public bool blocking;
        public override bool keepWaiting => blocking;

    }
    public class FunctionalThreadBlocker : CustomYieldInstruction
    {
        public FunctionalThreadBlocker(BoolAction isBlocking)
        {
            this.isBlocking = isBlocking;
        }
        public BoolAction isBlocking;
        public override bool keepWaiting => isBlocking();

    }

    public delegate bool BoolAction();



}

