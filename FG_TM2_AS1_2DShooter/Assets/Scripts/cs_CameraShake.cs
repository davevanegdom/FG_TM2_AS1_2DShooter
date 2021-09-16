using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cs_CameraShake : MonoBehaviour
{

    private Camera MainCam;

    public float shakeAmount = 0;
   


    // Update is called once per frame
    void Awake()
    {
        MainCam = Camera.main;
    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Shake(shakeAmount, shakeAmount);
        }
    }

    public void Shake(float amt, float length)
    {
        shakeAmount = amt;
        InvokeRepeating("BeginShake", 0, 0.01f);
        Invoke("StopShake", length);
    }
    void BeginShake()
    {
       if (shakeAmount > 0)
        {
            Vector3 camPos = MainCam.transform.position;
            
            float OffsetX = Random.value * shakeAmount * 2 - shakeAmount;
            float OffsetY = Random.value * shakeAmount * 2 - shakeAmount;
            camPos.x += OffsetX;
            camPos.y += OffsetY;

            MainCam.transform.position = new Vector3(camPos.x, camPos.y, MainCam.transform.position.z);
        }
    }

    void StopShake()
    {
        CancelInvoke("BeginShake");
       
    }
}


