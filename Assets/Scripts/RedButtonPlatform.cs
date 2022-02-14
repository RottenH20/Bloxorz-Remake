using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedButtonPlatform : MonoBehaviour
{
    public GameObject bounds;
    public GameObject platform;
    
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            bounds.SetActive(!bounds.activeSelf); // Swaps the active state of the gameObject
            platform.SetActive(!platform.activeSelf);
        }
    }
}
