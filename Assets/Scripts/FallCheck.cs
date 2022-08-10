using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallCheck : MonoBehaviour
{
    /* 
     * TO DO
     * Figure out direction if statement for diagonal fall
     * Figure out last swipe for direction of fall
     * Figure out how to simulate fall
     */

    public bool isSingleWidthHole = true;
    public int movableFallTime = 5;

    [HideInInspector]
    public GameObject playerBlock;

    private Rigidbody rbPlayerBlock;
    public AudioSource audioSourceForSingleHole;
    
    public CanvasHandlerLevel CanvasHandlerLevel; // Used for flipInput() method
    public CharacterController CharacterController;

    void Start()
    {
        playerBlock = GameObject.Find("Block Player");
        rbPlayerBlock = playerBlock.GetComponent<Rigidbody>();
    }

    public void OnTriggerEnter(Collider other) // On enter of edge
    {
        if (other.tag == "Player" && playerBlock.transform.position.y > .99f && isSingleWidthHole)
        {
            //Debug.Log("Trigger");
            StartCoroutine(initiateFallWin());
        }
        else if (other.tag == "Player" && !isSingleWidthHole)
        {
            StartCoroutine(initiateFall());
        }
        else if (other.tag == "Movable")
        {
            StartCoroutine(initiateMovableFall(other.gameObject)); // GameObject that entered trigger (the specific movable)
        }
    }

    IEnumerator initiateMovableFall(GameObject movableGameObject)
    {
        //Debug.Log("Falling");
        Vector3 currentPos = movableGameObject.transform.position;
        Vector3 newPosition = new Vector3(movableGameObject.transform.position.x, movableGameObject.transform.position.y - 10f, movableGameObject.transform.position.z);
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / movableFallTime;
            movableGameObject.transform.position = Vector3.Lerp(currentPos, newPosition, t);
            yield return null;
        }
    }

    IEnumerator initiateFallWin() // Win "Single Hole True"
    {
        CanvasHandlerLevel.flipInput(); // Flip input to prevent player from making any more moves while falling
        audioSourceForSingleHole.Play();
        rbPlayerBlock.isKinematic = false; // Set gravity to true to simulate falling
        rbPlayerBlock.useGravity = true; // Kinematic needs to be false in order for gravity to work
        yield return new WaitForSeconds(3); // Waits 3 seconds to show the player falling
        CanvasHandlerLevel.playerDied();
        rbPlayerBlock.isKinematic = true; // Renable gravity and kinematic to save processor power
        rbPlayerBlock.useGravity = false;
    }

    IEnumerator initiateFall() // Normal Fall
    {
        CanvasHandlerLevel.flipInput();
        rbPlayerBlock.isKinematic = false;
        rbPlayerBlock.useGravity = true;
        yield return new WaitForSeconds(3);
        CanvasHandlerLevel.playerDied();
        rbPlayerBlock.isKinematic = true;
        rbPlayerBlock.useGravity = false;
    }
}