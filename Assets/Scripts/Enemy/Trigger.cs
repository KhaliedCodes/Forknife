using System;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    public event Action<Collider> EnteredTrigger;
    public event Action<Collider> ExitedTrigger;

    void OnTriggerEnter(Collider collider)
    {
        EnteredTrigger?.Invoke(collider);
    }

    void OnTriggerExit(Collider collider)
    {
        ExitedTrigger?.Invoke(collider);
    }
}
