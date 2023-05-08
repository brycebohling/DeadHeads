using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    [SerializeField] float sensX;
    [SerializeField] float sensY;
    [SerializeField] Transform orientation;
    float xRotaion;
    float yRotaion;
    [SerializeField] Transform followObject;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        transform.position = new Vector3(followObject.position.x, followObject.position.y, followObject.position.z + 0.15f);
    }

    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotaion += mouseX;

        xRotaion += mouseY;
        xRotaion = Mathf.Clamp(xRotaion, -90f, 90f);

        transform.rotation = Quaternion.Euler(-xRotaion, yRotaion, 0);
        orientation.rotation = Quaternion.Euler(0, yRotaion, 0);
    }
}
