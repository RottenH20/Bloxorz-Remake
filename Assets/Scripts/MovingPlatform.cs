﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    bool movingToB = true; // Used to check which way target is moving
    bool moving = false; // Used to check if target is moving
    public float speed = 3f; // Speed of movement
    public float yValueChange = 9.75f;
    public GameObject playerCube, MainCamera; // We use this to move the character with the moving platform for it to seem as it is being pushed by it

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            moving = true;
        }
    }

    void Update() // Replaying loop, also need to disable all input while moving
    {
        if (moving)
        {
            if (transform.position.y > -0.25f && !movingToB)
            {
                playerCube.transform.position = playerCube.transform.position + new Vector3(0f, -speed * Time.deltaTime, 0f);
                transform.position = transform.position + new Vector3(0f, -speed * Time.deltaTime, 0f);
                return;
            }
            else if (transform.position.y <= -0.25f && !movingToB)
            {
                moving = !moving;
                movingToB = !movingToB;
                playerCube.transform.position = new Vector3(playerCube.transform.position.x, 1f, playerCube.transform.position.z);
                transform.position = new Vector3(transform.position.x, -0.25f, transform.position.z);
                MainCamera.transform.position = new Vector3(MainCamera.transform.position.x, MainCamera.transform.position.y - yValueChange, MainCamera.transform.position.z);
                return;
            }
            else if (transform.position.y < yValueChange && movingToB) // Stops in less than 1 percent difference of original value, need workaround  
            {
                playerCube.transform.position = playerCube.transform.position + new Vector3(0f, speed * Time.deltaTime, 0f);
                transform.position = transform.position + new Vector3(0f, speed * Time.deltaTime, 0f);
                return;
            }
            else if (transform.position.y >= yValueChange && movingToB) 
            {
                moving = !moving;
                movingToB = !movingToB;
                playerCube.transform.position = new Vector3(playerCube.transform.position.x, yValueChange + 1.25f, playerCube.transform.position.z);
                transform.position = new Vector3(transform.position.x, yValueChange, transform.position.z);
                MainCamera.transform.position = new Vector3(MainCamera.transform.position.x, MainCamera.transform.position.y + yValueChange, MainCamera.transform.position.z);
                return;
            }
        }
    }
}
