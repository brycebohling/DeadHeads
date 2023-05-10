using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VcamRotations : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    

    void Update()
    {
        transform.eulerAngles = mainCamera.transform.eulerAngles;
    }
}
