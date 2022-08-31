using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FallCheck : MonoBehaviour
{
    /* 
     * 8/30/22
     * Fix pushableObject not alligning correctlly
     */

    public bool isSingleWidthHole = true;
    public int movableFallTime = 5;
    public float rotationSpeed = 0.1f;
    public float movementSpeed = 0.1f;

    [HideInInspector]
    public GameObject playerBlock;
    bool complete = true;
    private GameObject Outliner;

    private Rigidbody rbPlayerBlock;
    public AudioSource audioSourceForSingleHole;
    
    public CanvasHandlerLevel CanvasHandlerLevel; // Used for flipInput() method
    public CharacterController CharacterController;

    public float timeToReachTarget = 4f;
    private float t;

    void Start()
    {
        playerBlock = GameObject.Find("Block Player");
        rbPlayerBlock = playerBlock.GetComponent<Rigidbody>();
        Outliner = GameObject.Find("Outliner");
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

    // Potentially change to enable and disable gravity
    // Check what floor box is on to determine where to fall
    IEnumerator initiateMovableFall(GameObject movableGameObject)
    {
        //Debug.Log("Falling");
        yield return new WaitForSeconds(1f); // Wait for box to complete horizontal movement from other script
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
        yield return new WaitForSeconds(0.5f); // Wait for character to stop horizontally moving
        if (SceneManager.GetActiveScene().name == "Level2") // Level2 is outlier because its alligned y value is incorrect and this is temp fix
            Outliner.transform.position = new Vector3(this.transform.position.x, 0, this.transform.position.z);
        else
            Outliner.transform.position = new Vector3(this.transform.position.x, 1, this.transform.position.z);
        StartCoroutine(blinkOutlier()); // Enables and disables outliner over fixed period
        complete = false;
        while (!complete) // Keep delaying script until character is in place
            yield return new WaitForSeconds(0.1f);

        yield return new WaitForSeconds(1f); // Pause for player to know where outliner is blinking
        rbPlayerBlock.isKinematic = false;
        rbPlayerBlock.useGravity = true;
        yield return new WaitForSeconds(3);
        CanvasHandlerLevel.playerDied();
        rbPlayerBlock.isKinematic = true;
        rbPlayerBlock.useGravity = false;
    }

    IEnumerator blinkOutlier() // enables and disables gameobject for blink effect
    {
        for (int i = 0; i != 3; i++)
        {
            Outliner.SetActive(true);
            yield return new WaitForSeconds(0.25f);
            Outliner.SetActive(false);
            yield return new WaitForSeconds(0.25f);
        }
    }

    void Update()
    {
        if (!complete)
        {
            t += Time.deltaTime / timeToReachTarget; // Find time till completion
            Vector3 relativePos = this.transform.position + playerBlock.transform.position;
            playerBlock.transform.rotation = Quaternion.Lerp(playerBlock.transform.rotation, Quaternion.Euler(0, 0, 0), t);
            playerBlock.transform.position = Vector3.Lerp(playerBlock.transform.position, new Vector3(this.transform.position.x, this.transform.position.y + 2f, this.transform.position.z), t);
            if (playerBlock.transform.rotation == Quaternion.Euler(0, 0, 0)) // Once player rotation is 0, 0, 0 we continue script
                complete = true;
        }
    }
}