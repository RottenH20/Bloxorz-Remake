using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderPushable : MonoBehaviour
{
    public PushableObject PushableObject;

    void OnTriggerEnter(Collider other)
    {
        PushableObject.PushObject(gameObject);
    }
}
