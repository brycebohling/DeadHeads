using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPCamera : MonoBehaviour
{
    public float Sensitivity {
		get { return sensitivity; }
		set { sensitivity = value; }
	}

	[SerializeField] Transform arms;
	[SerializeField] Transform body;
	[Range(0.1f, 900f)][SerializeField] float sensitivity = 2f;
	[Range(0f, 90f)][SerializeField] float yRotationLimit = 90f;

	private float xRot;

	void Update()
	{
		float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
		float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

		xRot -= mouseY;
		xRot = Mathf.Clamp(xRot, -yRotationLimit, yRotationLimit);

		arms.localRotation = Quaternion.Euler(new Vector3(xRot, 0, 0));
		body.Rotate(new Vector3(0, mouseX, 0));

	

	}
}
