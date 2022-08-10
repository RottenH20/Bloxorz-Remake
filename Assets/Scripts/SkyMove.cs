using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyMove : MonoBehaviour
{
    public float rotateSpeed = 1.2f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Skybox>().material.SetFloat("_Rotation", Time.time * rotateSpeed);
    }
}
