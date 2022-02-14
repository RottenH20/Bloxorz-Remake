using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueButtonSpikes : MonoBehaviour
{
    public GameObject SpikesActivated; // TODO, HAVE SPIKES HIDDEN AND SPIKES NOT HIDDEN OVERLAPPING THEN SET ACTIVE ON AFTER BUTTON PRESS TO ANOTHER
    public GameObject SpikesNotActivated;
    public GameObject SpikesActivatedGhost;
    public GameObject SpikesNotActivatedGhost;
    bool buttonActive = false;

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !buttonActive)
        {
            SpikesActivated.SetActive(false);
            SpikesNotActivated.SetActive(false);
            SpikesActivatedGhost.SetActive(true);
            SpikesNotActivatedGhost.SetActive(true);
            buttonActive = !buttonActive;
        }
        else if (other.tag == "Player" && buttonActive)
        {
            SpikesActivated.SetActive(true);
            SpikesNotActivated.SetActive(true);
            SpikesActivatedGhost.SetActive(false);
            SpikesActivatedGhost.SetActive(false);
            buttonActive = !buttonActive;
        }
    }
}
