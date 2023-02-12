using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera: MonoBehaviour
{
    [SerializeField]
    private Transform mainCam;
    private Transform thisCanvas;
    
    void Start()
    {
        mainCam = Camera.main.GetComponent<Transform>();
        thisCanvas = GetComponent<Transform>();
    }

    
    void Update()
    {
        thisCanvas.LookAt(mainCam.transform);
    }
}
