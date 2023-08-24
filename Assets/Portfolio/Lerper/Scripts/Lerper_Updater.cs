using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lib.Lerping.Core
{
    public class Lerper_Updater : MonoBehaviour
    {
        private static Lerper_Updater instance = null;
        private List<System.Action> updaters = new List<System.Action>();
        public static System.Action Add(System.Action updateAction)
        {
            if (instance == null)
            {
                instance = new GameObject("Lerper_Updater").AddComponent<Lerper_Updater>();
            }

            instance.updaters.Add(updateAction);
            return updateAction;
        }

        public static void Remove(System.Action updateAction)
        {
            if (instance == null)
            {
                return;
            }

            instance.updaters.Remove(updateAction);
        }

        void Update() 
        {
            if (updaters.Count > 0) 
            {
                for (int i = updaters.Count - 1; i >= 0; i--)
                {
                    updaters[i]?.Invoke();
                }
            }
        }
    }
}