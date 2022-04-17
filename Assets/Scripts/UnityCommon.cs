using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

namespace UnityCommon
{
    public static class UnityCommon
    {
        public static IEnumerator IDoCount(float startNum, float endNum, float time, Action<float> action, Ease easeType, bool isDepentUpdate = false)
        {
            float startTime = GetTime(isDepentUpdate);
            float currentTime = startTime;
            float endTime = currentTime += time;
            while (true)
            {
                currentTime = GetTime(isDepentUpdate);
                if(currentTime < endTime)
                {
                    action(DOVirtual.EasedValue(startNum, endNum, time / (currentTime - startTime), easeType));
                    yield return null;
                }
                else
                {
                    action(endNum);
                    break;
                }
            }
            
        }

        public static void DoCount(this MonoBehaviour ctx, float startNum, float endNum, float time, Action<float> action, bool isDepentUpdate = false, Ease easeType = Ease.InSine)
        {
            ctx.StartCoroutine(IDoCount(startNum,endNum,time,action,easeType, isDepentUpdate));
        }

        static float GetTime( bool isDepentUpdate)
        {
            return isDepentUpdate ? Time.fixedTime : Time.fixedUnscaledTime;
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

