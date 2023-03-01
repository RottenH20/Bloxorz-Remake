using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FallCheck : MonoBehaviour
{
    /* 
     * 8/30/22
     * Fix pushableObject not alligning correctlly
     * 
     * 9/12/22
     * Fall check now shows where the player will fall, no need for directional diagonal fall
     * Need to address problem when falling on 2 or more fall checks
     */

    public bool isSingleWidthHole = true;
    public float movableFallTime = 5f;
    public float rotationSpeed = 0.1f;
    public float movementSpeed = 0.1f;

    [HideInInspector]
    public GameObject playerBlock;
    private bool complete = true;
    private GameObject Outliner;

    private Rigidbody rbPlayerBlock;
    public AudioSource audioSourceForSingleHole;
    
    public CanvasHandlerLevel CanvasHandlerLevel; // Used for flipInput() method
    public CharacterController CharacterController;

    public float timeToReachTarget = 4f; // Time until block gets into position (off slighty)
    private float t; // Used to calculate time

    private float oldYValue;
    private float newYValue;

    public SaveData SaveData;

    void Start()
    {
        playerBlock = GameObject.Find("Block Player");
        rbPlayerBlock = playerBlock.GetComponent<Rigidbody>();
        Outliner = GameObject.Find("Outliner");
        SaveData = GameObject.Find("EventSystem").GetComponent<SaveData>();
        //CanvasHandlerLevel = GameObject.Find("Canvas").GetComponent<CanvasHandlerLevel>();
        //CharacterController = playerBlock.GetComponent<CharacterController>();
    }

    public void OnTriggerEnter(Collider other) // On enter of edge
    {
        if (other.tag == "Player" && playerBlock.transform.position.y > 1.99f && isSingleWidthHole) // Win game
        {
            //Debug.Log("Trigger");
            StartCoroutine(initiateFallWin());
        }
        else if (other.tag == "Player" && !isSingleWidthHole) // Normal fall
        {
            StartCoroutine(initiateFall());
        }
        else if (other.tag == "Movable")
        {
            //StartCoroutine(initiateMovableFall(other.gameObject)); // GameObject that entered trigger (the specific movable)
        }
    }

    // Potentially change to enable and disable gravity
    // Check what floor box is on to determine where to fall
    /*IEnumerator initiateMovableFall(GameObject movableGameObject)
    {
        //Debug.Log("Falling");
        yield return new WaitForSeconds(0.3f); // Wait for box to complete horizontal movement from other script
        foreach (Transform child in movableGameObject) // Set all children under the box inactive until done
        {
            child.movableGameObject.SetActive(!child.movableGameObject.activeSelf);
        }
        Vector3 currentPos = movableGameObject.transform.position;
        Vector3 newPosition = new Vector3(movableGameObject.transform.position.x, movableGameObject.transform.position.y - 10f, movableGameObject.transform.position.z);
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime * movableFallTime;
            movableGameObject.transform.position = Vector3.Lerp(currentPos, newPosition, t);
            yield return null;
        }
        foreach (Transform child in movableGameObject) // Set all children under the box back to active (start checking for collisions)
        {
            child.movableGameObject.SetActive(!child.movableGameObject.activeSelf);
        }
    }*/

    IEnumerator initiateFallWin() // Win "Single Hole True"
    {
        CanvasHandlerLevel.flipInput(); // Flip input to prevent player from making any more moves while falling
        audioSourceForSingleHole.Play();
        rbPlayerBlock.isKinematic = false; // Set gravity to true to simulate falling
        rbPlayerBlock.useGravity = true; // Kinematic needs to be false in order for gravity to work
        yield return new WaitForSeconds(1);
        this.gameObject.transform.GetChild(0).GetComponent<ParticleSystem>().Play(true); // Play effect of win
        // Make sure child of flag has ParticleSystem
        yield return new WaitForSeconds(2); // Waits 3 seconds to show the player falling
        SaveData.SaveLevel(int.Parse(SceneManager.GetActiveScene().name.Substring(5)), CharacterController.numberOfMoves);
        CanvasHandlerLevel.playerDied();
        rbPlayerBlock.isKinematic = true; // Renable gravity and kinematic to save processor power
        rbPlayerBlock.useGravity = false;
    }

    // TODO
    // Fix multiple instances running
    IEnumerator initiateFall() // Normal Fall
    {
        if (CharacterController.fallingCheck()) // Break out if character is already falling
            yield break;
        CharacterController.setFallingTrue(); // Set this true so cant re call this
        /*
        int randomNumber = Random.Range(-100000, 100000);
        this.gameObject.tag = ("ActiveBounds");
        this.gameObject.name = ("ActiveBounds " + randomNumber);
        yield return new WaitForSeconds(0.1f);
        GameObject[] BoundsArray = GameObject.FindGameObjectsWithTag("Bounds");
        foreach (GameObject go in BoundsArray)
        {
            go.SetActive(false);
        }
        yield return new WaitForSeconds(0.1f);
        GameObject[] ActiveBoundsArray = GameObject.FindGameObjectsWithTag("ActiveBounds");
        if (ActiveBoundsArray.Length > 1)
        {
            if (int.Parse(this.name.Substring(12)) > int.Parse(ActiveBoundsArray[0].name.Substring(12))
                Destroy(ActiveBoundsArray[0]);
            else
                Destroy(ActiveBoundsArray[1]);

            Debug.Log("1" = ActiveBoundsArray[1].name.Substring(12));
            Debug.Log("2" = ActiveBoundsArray[0].name.Substring(12));
        }
        */


        CanvasHandlerLevel.flipInput();
        yield return new WaitForSeconds(0.5f); // Wait for character to stop horizontally moving
        Outliner.transform.position = new Vector3(this.transform.position.x, 1, this.transform.position.z);
        StartCoroutine(blinkOutlier()); // Enables and disables outliner over fixed period
        complete = false;
        while (!complete) // Keep delaying script until character is in place
            yield return new WaitForSeconds(0.1f);

        yield return new WaitForSeconds(1f); // Pause for player to know where outliner is blinking
        rbPlayerBlock.isKinematic = false; 
        rbPlayerBlock.useGravity = true;
        yield return new WaitForSeconds(3); // We allow player to fall free for 3 seconds (until off screen) then stop it to save data
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
            Vector3 relativePos = this.transform.position + playerBlock.transform.position; // Find where player should go
            playerBlock.transform.rotation = Quaternion.Lerp(playerBlock.transform.rotation, Quaternion.Euler(0, 0, 0), t); // Start rotating player correctly
            playerBlock.transform.position = Vector3.Lerp(playerBlock.transform.position, new Vector3(this.transform.position.x, this.transform.position.y + 2f, this.transform.position.z), t); // Move to spot over certain time frame (t)
            if (playerBlock.transform.rotation == Quaternion.Euler(0, 0, 0)) // Once player rotation is 0, 0, 0 we continue script
                complete = true;
        }
    }
}