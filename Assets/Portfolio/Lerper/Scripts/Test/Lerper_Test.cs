using System.Collections;
using System.Collections.Generic;
using Lib.Lerping;
using UnityEngine;

namespace Lib.Lerping.Test
{
    public class Lerper_Test : MonoBehaviour
    {
        public Transform BodyTransform;
        public Renderer BodyRenderer;

        public Lerper_Vector3 Vector3TestLerp;
        public Lerper_Color ColorTestLerp;

        public void LerpPosition()
        {
            Vector3TestLerp.Start(
                onUpdate: MovePosition,
                onDone: () =>
                {
                    Debug.Log("LerpPosition Done");
                });
        }


        public void LerpColor()
        {
            ColorTestLerp.Start(
                onUpdate: ChangeColor,
                onDone: () =>
                {
                    Debug.Log("LerpColor Done");
                });
        }


        public void LerpPositionThenColor()
        {
            Vector3TestLerp.Start(
                onUpdate: MovePosition,
                onDone: () =>
                {
                    Debug.Log("LerpPosition Done");
                    ColorTestLerp.Start(
                        onUpdate: ChangeColor,
                        onDone: () =>
                        {
                            Debug.Log("LerpColor Done");
                        });
                });
        }

        public void FlipPositionAndLerp() 
        {
            Vector3TestLerp.Flip();
            Vector3TestLerp.Start(
                onUpdate: MovePosition,
                onDone: () =>
                {
                    Debug.Log("LerpPosition Done");
                });
        }
        private void MovePosition()
        {
            BodyTransform.position = Vector3TestLerp.GetValue();
        }
        private void ChangeColor()
        {
            BodyRenderer.material.SetColor("_Color", ColorTestLerp.GetValue());
        }
    }
}
