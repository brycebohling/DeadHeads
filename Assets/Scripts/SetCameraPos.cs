using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCameraPos : MonoBehaviour
{
    [SerializeField] Transform fakeCameraPos;
    void Update()
    {
        transform.position = fakeCameraPos.position;
    }
}
