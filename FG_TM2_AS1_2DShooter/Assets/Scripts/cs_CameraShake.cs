using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cs_CameraShake : MonoBehaviour
{

    public Camera MainCam;

    float shakeAmount = 0;
   


    // Update is called once per frame
    void Awake()
    {
        if (MainCam == null)
           MainCam = Camera.main;
    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Shake(0.1f, 0.2f);
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
            camPos.x += OffsetY;

            MainCam.transform.position = camPos;
        }
    }

    void StopShake()
    {
        CancelInvoke("BeginShake");
        MainCam.transform.localPosition = Vector3.zero;
    }
}


