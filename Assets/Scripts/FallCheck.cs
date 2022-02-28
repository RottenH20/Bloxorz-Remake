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
    public bool fallZ = false;
    public GameObject playerBlock;
    private Rigidbody rbPlayerBlock;
    public AudioSource audioSourceForSingleHole;
    
    public CanvasHandlerLevel CanvasHandlerLevel; // Used for flipInput() method
    public CharacterController CharacterController;

    private bool fallingDiagonally = false; // Used for Update() lerp

    void Awake()
    {
        rbPlayerBlock = playerBlock.GetComponent<Rigidbody>();
    }

    public void OnTriggerEnter(Collider other) // On enter of edge
    {
        if (other.tag == "Player" && playerBlock.transform.position.y > .99f && isSingleWidthHole)
        {
            //Debug.Log("Trigger");
            StartCoroutine(initiateFallWin());
        }
        if (other.tag == "Player" && !isSingleWidthHole && 1 < playerBlock.transform.position.y) // Make sure only half is over 
        {
            StartCoroutine(initiateDiagnalFall());
        }
        if (other.tag == "Player" && !isSingleWidthHole)
        {
            StartCoroutine(initiateFall());
        }
    }

    IEnumerator initiateFallWin() // Win "Single Hole True"
    {
        CanvasHandlerLevel.flipInput(); // Flip input to prevent player from making any more moves while falling
        audioSourceForSingleHole.Play();
        rbPlayerBlock.isKinematic = false; // Set gravity to true to simulate falling
        rbPlayerBlock.useGravity = true; // Kinematic needs to be false in order for gravity to work
        yield return new WaitForSeconds(3); // Waits 3 seconds to show the player falling
        CanvasHandlerLevel.ActivateWinMenu(); // Activates menu dependent on whether player is alive or dead
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

    IEnumerator initiateDiagnalFall() // Used for "flip" effect
    {
        // Dont need to disable or renable kinematic && gravity because its controlled falling
        CanvasHandlerLevel.flipInput();
        fallingDiagonally = true; // Used in Update()
        yield return new WaitForSeconds(3);
        fallingDiagonally = false;
    }

    void Update()
    {
        while (fallingDiagonally) // Fall direction is determined by last swipe (which caused the fall)
        {
            /* 
             * Direction.Left = xManager = -1
             * Direction.Right = xManager = 1
             * Direction.Up = yManager = 1
             * Direction.Down = yManager = -1
             */

            if (CharacterController.y == -1) // Still need to determine how to only check last move
                playerBlock.transform.position = playerBlock.transform.position + new Vector3(Time.deltaTime, -Time.deltaTime, 0); // Change fall direction

            else if (CharacterController.y == 1)
                playerBlock.transform.position = playerBlock.transform.position + new Vector3(-Time.deltaTime, -Time.deltaTime, 0);

            if (CharacterController.x == -1)
                playerBlock.transform.position = playerBlock.transform.position + new Vector3(0, -Time.deltaTime, Time.deltaTime);

            else if (CharacterController.x == 1)
                playerBlock.transform.position = playerBlock.transform.position + new Vector3(0, -Time.deltaTime, -Time.deltaTime);
        }
    }  
}