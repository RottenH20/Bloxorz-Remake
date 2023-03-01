using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleButton : MonoBehaviour
{
    // Script for circular button, requires box in scene and to be pushed into button trigger.
    // Activates so long as box stays on the button.
    public GameObject bounds;
    public HueChange[] HueChange;


    public void OnTriggerEnter(Collider other) // On enter, swap everything
    {
        if (other.tag == "Movable")
        {
            bounds.SetActive(!bounds.activeSelf); // Swaps the active state of the gameObject
            for (int i = 0; i < HueChange.Length; i++)
                HueChange[i].SwitchMaterial();
        }
    }

    public void OnTriggerExit(Collider other) // On exit, swap everything back
    {
        if (other.tag == "Movable")
        {
            bounds.SetActive(!bounds.activeSelf); // Swaps the active state of the gameObject
            for (int i = 0; i < HueChange.Length; i++)
                HueChange[i].SwitchMaterial();
        }
    }
}
