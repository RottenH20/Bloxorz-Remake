using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*  
 *  This code is for objects to be pushed by player object.
 *  for falling we use FallCheck script for these objects
 *  This script just moves the object on X and Z axis if player "pushes" it
 *  
 *  Each child has its on script that sends a "signal" to this script if player is "pushing" it
 */

public class PushableObject : MonoBehaviour
{
    bool moving = false;

    public float moveSpeed = 2.5f; // Need consistant speed at which the object moves once player touches it

    public void PushObject(GameObject collider)
    {
        if (!moving) // Make sure game object is not already moving
        {
            moving = true;

            switch (collider.name)
            {
                case "xTop":
                    StartCoroutine(initiateMovablePush(0, -1));
                    break;
                case "xBottom":
                    StartCoroutine(initiateMovablePush(0, 1));
                    break;
                case "zTop":
                    StartCoroutine(initiateMovablePush(1, 0));
                    break;
                case "zBottom":
                    StartCoroutine(initiateMovablePush(-1, 0));
                    break;
                default:
                    Debug.Log(collider.name);
                    break;
            }
        }
    }

    IEnumerator initiateMovablePush(int x, int z) // Same script as fallCheck just going different direction
    {
        //Debug.Log("PushObject");
        Vector3 currentPos = gameObject.transform.position;
        
        Vector3 newPosition = new Vector3(gameObject.transform.position.x + x, gameObject.transform.position.y, gameObject.transform.position.z + z);
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime * moveSpeed;
            gameObject.transform.position = Vector3.Lerp(currentPos, newPosition, t);
            yield return null;
        }
        moving = false;
    }
}
