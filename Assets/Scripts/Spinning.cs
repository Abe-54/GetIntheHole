using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinning : MonoBehaviour
{
    public float speedModifier = 1f;
    public bool canSpin = false;
    
    // Update is called once per frame
    void Update()
    {
        if(canSpin)
            transform.Rotate(Vector3.up * speedModifier);
    }
}
