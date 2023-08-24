using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lib.Lerping
{
    [System.Serializable]
    public class Lerper_Color : Lerper<Color>
    {
        [SerializeField] private Color fromColor;
        [SerializeField] private Color toColor;

        public override Color GetValue(float time)
        {
            return Color.Lerp(fromColor, toColor, time);
        }

        protected override void OnCloned(Lerper<Color> clone)
        {
            var cloneColor = (Lerper_Color)clone;
            cloneColor.fromColor = fromColor;
            cloneColor.toColor = toColor;
        }

        public override Lerper<Color> Flip()
        {
            var color = fromColor;
            fromColor = toColor;
            toColor = color;
            return this;
        }
    }
}
