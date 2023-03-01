using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedButtonPlatform : MonoBehaviour
{
    public GameObject bounds;

    public HueChange[] HueChange;
    
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            bounds.SetActive(!bounds.activeSelf); // Swaps the active state of the gameObject
            for (int i = 0; i < HueChange.Length; i++)
            HueChange[i].SwitchMaterial();
        }
    }
}
