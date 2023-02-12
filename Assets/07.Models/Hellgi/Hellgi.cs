using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hellgi : MonoBehaviour
{
    float h = 0.0f, v = 0.0f, a = 0.0f;
    public float Speed = 15.0f;
    public float RotSpeed = 95.0f;
    void Start()
    {
        
    }

    void HellgiContorl()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        a = Input.GetAxis("Fire3");

        transform.Translate(Vector3.right * h * Speed * Time.deltaTime);
        transform.Translate(Vector3.up * a * Speed * Time.deltaTime);
        transform.Translate(Vector3.forward * v * Speed * Time.deltaTime);
    }

    void HellgiRotation()
    {
        float r = Input.GetAxis("Mouse X");
        transform.Rotate(Vector3.up * r * RotSpeed * Time.deltaTime);
    }
    
    void Update()
    {
        HellgiContorl();
        HellgiRotation();

    }
}
