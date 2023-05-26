using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
	[SerializeField] Transform camTransform;
	[SerializeField] float shakeDuration = 0f;
    float duration;
	[SerializeField] float shakeAmount = 0.7f;
	[SerializeField] float decreaseFactor = 1.0f;
    bool shakeCamera;
	Vector3 originalPos;
	
	void OnEnable()
	{
		originalPos = camTransform.localPosition;
	}

	void Update()
	{
        if (shakeCamera)
        {
            if (duration > 0)
            {
                camTransform.localPosition = Vector3.Lerp(camTransform.localPosition,originalPos + Random.insideUnitSphere * shakeAmount,Time.deltaTime * 3);
                
                duration -= Time.deltaTime * decreaseFactor;
            }
            else
            {
                camTransform.localPosition = originalPos;
                shakeCamera = false;
            }
        }
	}

    public void Shake()
    {
        shakeCamera = true;
        duration = shakeDuration;
    }
}
