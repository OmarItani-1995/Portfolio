using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lib.Lerping.Core;

namespace Lib.Lerping
{
    [System.Serializable]
    public abstract class Lerper<T>
    {
        public Lerper_Timer timer;
        private System.Action onDone = null;
        public void Start(System.Action onUpdate = null, System.Action onDone = null)
        {
            #if UNITY_EDITOR 
            if (!Application.isPlaying)
            {
                Debug.LogError("Lerper can only be started in play mode");
                return;
            }
            #endif
            if (onUpdate != null)
            {
                Lerper_Updater.Add(onUpdate);
                onDone += () => Lerper_Updater.Remove(onUpdate);
            }
            this.onDone = onDone;
            timer.Start(OnTimerDone);
        }

        public T GetValue()
        {
            return GetValue(timer.GetTime());
        }

        public abstract T GetValue(float time);
        public abstract Lerper<T> Flip();
        public Lerper<T> Clone()
        {
            var clone = (Lerper<T>)Activator.CreateInstance(this.GetType());
            clone.timer = timer.Clone();
            OnCloned(clone);
            return clone;
        }

        public Lerper<T> SetTimer(Lerper_Timer timer) 
        {
            this.timer = timer;
            return this;
        }

        public Lerper<T> SetDuration(float duration) 
        {
            timer.SetDuration(duration);
            return this;
        }

        protected virtual void OnCloned(Lerper<T> clone)
        {
        }

        private void OnTimerDone()
        {
            onDone?.Invoke();
        }
    }
}
