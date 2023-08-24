using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manipulator_Updater : MonoBehaviour
{
    private static Manipulator_Updater instance = null;
    [SerializeField] private List<Manipulator> manipulators = new List<Manipulator>();

    public static void Add(Manipulator manipulator)
    {
        if (instance == null)
        {
            instance = new GameObject("Manipulator_Updater").AddComponent<Manipulator_Updater>();
        }

        instance.manipulators.Add(manipulator);
    }

    public static void Remove(Manipulator manipulator)
    {
        if (instance == null)
        {
            return;
        }

        instance.manipulators.Remove(manipulator);
    }

    void Update()
    {
        if (manipulators.Count > 0)
        {
            for (int i = manipulators.Count - 1; i >= 0; i--)
            {
                manipulators[i]?.Update();
            }
        }
    }
}
