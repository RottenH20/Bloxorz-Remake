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

    FloorComponent FloorComponent;

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
            if (true) // Check here if player is vertical
            {
                StartCoroutine(bugFix());
            }
        }
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
            int currentFloor = Mathf.FloorToInt((int)playerCube.transform.position.y / 10) + 1;
            if (currentFloor == 1)
                returnValue = 16f;
            else
                returnValue = 16f + ((float)currentFloor * yValueChange) - yValueChange - 0.75f;
            return returnValue;
        }
    }
}
