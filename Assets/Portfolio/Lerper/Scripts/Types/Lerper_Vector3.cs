using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lib.Lerping
{
    [System.Serializable]
    public class Lerper_Vector3 : Lerper<Vector3>
    {
        public List<Vector3> Points;
        public override Vector3 GetValue(float time)
        {
            return Lerp(Points.ToArray(), Points.Count, time);
        }

        Vector3 Lerp(Vector3[] points, int count, float time)
        {
            if (count == 0)
            {
                return Vector3.zero;
            }

            if (count == 1)
            {
                return points[0];
            }

            for (int i = 0; i < count - 1; i++)
            {
                points[i] = Vector3.Lerp(points[i], points[i + 1], time);
            }

            return Lerp(points, count - 1, time);
        }

        public override Lerper<Vector3> Flip() 
        {
            Points.Reverse();
            return this;
        }

        protected override void OnCloned(Lerper<Vector3> clone)
        {
            var cloneVector3 = (Lerper_Vector3)clone;
            cloneVector3.Points = new List<Vector3>(Points);
        }
    }
}
