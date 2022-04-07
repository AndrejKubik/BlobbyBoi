using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOutOfScreen : MonoBehaviour
{
    private float xBound = -40.0f;

    void Update()
    {
        //is an object moves beyond the bound along the X axis destroy it
        if(transform.position.x <= xBound)
        {
            Destroy(gameObject);
        }
    }
}
