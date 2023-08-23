using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorComponent : MonoBehaviour
{
    public short startingFloor = 1; // Floor we are starting on

    public void increaseFloor()
    {
        startingFloor++;
    }

    public void decreaseFloor()
    {
        startingFloor--;
    }
}
