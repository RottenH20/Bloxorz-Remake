using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MovingPlatform : MonoBehaviour
{
    /*
     * Need to create a feature that checks if the player is "vertical", 
     * if they are not vertical then dont let them ride
     */

    FloorComponent FloorComponent; // We use this to check what level the player is on, also the updating for what level the player is on

    public bool movingToB = true; // Used to check which way target is moving
    bool moving = false; // Used to check if target is moving
    public float speed = 3f; // Speed of movement
    public float yValueChange = 9.75f;
    private GameObject playerCube, MainCamera; // We use this to move the character with the moving platform for it to seem as it is being pushed by it
    public bool mainMenu = false;

    CanvasHandlerLevel CanvasHandlerLevel;

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(bugFix());
        }
    }

    private bool isVertical()
    {
        // Simple formula,
        // X is current floor
        // If player is on floor 1 they are y = 2, floor 2 they are y = 12, floor 3 they are y = 22, etc...
        // 
        // (10 * (X - 1)) + 2 = Player Y if vertical
        //Debug.Log("Current Floor: " + ((10 * (currentFloor() - 1)) + 2));
        //Debug.Log("Player Floor: " + (double)(playerCube.transform.position.y)); 
        //Debug.Log(playerCube.transform.position.y == ((10 * (currentFloor() - 1)) + 2)); // How the fuck is this false?, bug is right here

        if (((10*(currentFloor()-1)) + 2) + 0.1 >= (double)playerCube.transform.position.y && ((10 * (currentFloor() - 1)) + 2) - 0.1 <= (double)playerCube.transform.position.y)
        {
            return true;
        }
        else
            return false;
    }

    public int currentFloor()
    {
        return Mathf.FloorToInt((int)playerCube.transform.position.y / 10) + 1;
    }

    private void Start()
    {
        FloorComponent = GameObject.FindObjectOfType<FloorComponent>();
        playerCube = GameObject.Find("Block Player");
        MainCamera = GameObject.Find("Main Camera");
        CanvasHandlerLevel = GameObject.FindObjectOfType<CanvasHandlerLevel>();
    }

    IEnumerator bugFix()
    {
        CanvasHandlerLevel.movingPlatformCameraBug();
        CanvasHandlerLevel.flipInput();
        yield return new WaitForSeconds(0.75f);
        if (!isVertical())
        {
            CanvasHandlerLevel.flipInput();
            yield break;
        }
        moving = true;
    }

    // Some very VERY bad code. Need to work on nesting skills and limiting to 3 nests total.
    // However the code works so will just leave it as I shouldnt have to come back here.
    void Update()
    {
        if (mainMenu)
        {
            if (!movingToB)
            {
                transform.position = transform.position + new Vector3(0f, -speed * Time.deltaTime, 0f);
                if (transform.position.y <= -1.25f)
                {
                    movingToB = !movingToB;
                }
            }
            else
            {
                transform.position = transform.position + new Vector3(0f, +speed * Time.deltaTime, 0f);
                if (transform.position.y >= yValueChange)
                {
                    movingToB = !movingToB;
                }
            }
            return;
        }


        if (moving)
        {
            if (transform.position.y > 0.75f && !movingToB)
            {
                playerCube.transform.position = playerCube.transform.position + new Vector3(0f, -speed * Time.deltaTime, 0f);
                transform.position = transform.position + new Vector3(0f, -speed * Time.deltaTime, 0f);
                MainCamera.transform.position = MainCamera.transform.position + new Vector3(0f, -speed * Time.deltaTime, 0f);
            }
            else if (transform.position.y <= 0.75f && !movingToB)
            {
                FloorComponent.startingFloor--;
                moving = !moving;
                movingToB = !movingToB;
                playerCube.transform.position = new Vector3(playerCube.transform.position.x, 2f, playerCube.transform.position.z);
                transform.position = new Vector3(transform.position.x, 0.75f, transform.position.z);
                MainCamera.transform.position = new Vector3(MainCamera.transform.position.x, calculateCameraY(), MainCamera.transform.position.z);
                CanvasHandlerLevel.flipInput();
            }
            else if (transform.position.y < yValueChange && movingToB)  
            {
                playerCube.transform.position = playerCube.transform.position + new Vector3(0f, speed * Time.deltaTime, 0f);
                transform.position = transform.position + new Vector3(0f, speed * Time.deltaTime, 0f);
                MainCamera.transform.position = MainCamera.transform.position + new Vector3(0f, speed * Time.deltaTime, 0f);
            }
            else if (transform.position.y >= yValueChange && movingToB) 
            {
                FloorComponent.startingFloor++;
                moving = !moving;
                movingToB = !movingToB;
                playerCube.transform.position = new Vector3(playerCube.transform.position.x, yValueChange + 1.25f, playerCube.transform.position.z);
                transform.position = new Vector3(transform.position.x, yValueChange, transform.position.z);
                MainCamera.transform.position = new Vector3(MainCamera.transform.position.x, calculateCameraY(), MainCamera.transform.position.z);
                CanvasHandlerLevel.flipInput();
            }
        }

        float calculateCameraY()
        {
            float returnValue;
            if (currentFloor() == 1)
                returnValue = 16f;
            else
                returnValue = 16f + ((float)currentFloor() * yValueChange) - yValueChange - 0.75f;
            return returnValue;
        }
    }
}
