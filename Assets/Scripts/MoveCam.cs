using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCam : MonoBehaviour
{
    [SerializeField] Transform cameraPos;

// test me
    void Update()
    {
        transform.position = cameraPos.position;
    }
}
