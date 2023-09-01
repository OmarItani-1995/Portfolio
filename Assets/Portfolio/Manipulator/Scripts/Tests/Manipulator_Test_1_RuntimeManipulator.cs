using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manipulator_Test_1_RuntimeManipulator : MonoBehaviour
{
    public Transform TestingTransform;
    public void Run()
    {
        TestingTransform
            .Manipulate(duration: 2, manipulator =>
            {
                manipulator
                    .Add<MWorker_MoveToPosition>(worker =>
                    {
                        worker.SetDestination(TestingTransform.localPosition + Vector3.forward * 10).UseSpace(Space.Self);
                    })
                    .Add<MWorker_ScaleTo>(worker =>
                    {
                        worker.SetEndScale(Vector3.one * 2);
                    });
            })
            .Chain(2, manipulator =>
            {
                manipulator
                    .Add<MWorker_MoveToPosition>(worker =>
                    {
                        worker.SetDestination(Vector3.zero).UseSpace(Space.Self);
                    })
                    .Add<MWorker_ScaleTo>(worker =>
                    {
                        worker.SetEndScale(Vector3.one);
                    });
            })
            .Run();
    }
}
