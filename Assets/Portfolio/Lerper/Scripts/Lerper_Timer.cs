using System;
using UnityEngine;

namespace Lib.Lerping
{
    [System.Serializable]
    public class Lerper_Timer
    {
        [SerializeField] private AnimationCurve Curve;
        [SerializeField] private float Duration = 1;

        private bool started = false;
        private bool finished = false;
        private float startedTime;
        private System.Action onDone;
        private float ElapsedTime
        {
            get
            {
                if (finished) return Duration;
                if (!started) return 0;
                if (Time.time - startedTime >= Duration)
                {
                    if (!finished)
                    {
                        finished = true;
                        started = true;
                        onDone.Invoke();
                    }
                    return Duration;
                }
                else
                    return Time.time - startedTime;
            }
        }

        public void Start(System.Action onDone)
        {
            started = true;
            startedTime = Time.time;
            this.onDone = onDone;
            finished = false;
        }

        public float GetTime()
        {
            return Curve.Evaluate(ElapsedTime / Duration);
        }

        public Lerper_Timer Clone()
        {
            return new Lerper_Timer()
            {
                Curve = new AnimationCurve(Curve.keys),
                Duration = Duration
            };
        }

        public void SetDuration(float duration)
        {
            Duration = duration;
        }
    }
}
