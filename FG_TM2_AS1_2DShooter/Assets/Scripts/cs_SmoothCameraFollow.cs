using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cs_SmoothCameraFollow : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private Transform player;

    [Header("Tweaks")]
    [SerializeField] private Vector3 offset = new Vector3(0, 0, -10);

    [Header("Bounds")]
    [SerializeField] private bool enableBounds = true;
    [SerializeField] private float bounds = 3f;

    [Header("Smooth")]
    [SerializeField] private bool enableSmooth = true;
    [SerializeField] private float smoothSpeed = 10.0f;
    private Vector3 desiredPosition;



    private void LateUpdate()
    {
        desiredPosition = player.position + offset;
    

    if (enableBounds)
        {
            float deltaX = player.position.x - transform.position.x;
            if (Mathf.Abs(deltaX) > bounds)
            {
                if(deltaX > 0)
                {
                    desiredPosition.x = player.position.x - bounds;
                }
            else
                { desiredPosition.x = player.position.x + bounds;
                }
            }
        }
     

   }
}
