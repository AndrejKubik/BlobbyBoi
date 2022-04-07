using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatBackground : MonoBehaviour
{
    private Vector3 startPos; //start coordinates

    private float triggerWidth; //X axis return bound

    private BoxCollider bgCollider; //collider used for measuring background size
    void Start()
    {
        //taking return coordinates
        startPos = transform.position;

        //using background collider
        bgCollider = GetComponent<BoxCollider>();

        //getting return bound value
        triggerWidth = bgCollider.size.x / 2;
    }

    void Update()
    {
        //if background move beyond the return bound, put it back to starting position
        if(transform.position.x < startPos.x - triggerWidth)
        {
            transform.position = startPos;
        }
    }
}
