using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpikesBehavior : MonoBehaviour
{

    public CanvasHandlerLevel CanvasHandlerLevel;

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            CanvasHandlerLevel.playerDied();
        }
    }
}
