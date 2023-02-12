using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tail : MonoBehaviour
{
    public float rotSpeed = 40.0f;

    void Start()
    {
        
    }

    void Update()
    {
        transform.Rotate(0.0f, rotSpeed, 0.0f);
    }
}
