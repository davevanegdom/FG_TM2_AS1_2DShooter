using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cs_CinemachineCamera : MonoBehaviour
{
    private Cinemachine.CinemachineVirtualCamera camera;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<Cinemachine.CinemachineVirtualCamera>();

        player = GameObject.FindGameObjectWithTag("Player");
        camera.Follow = player.transform;
        camera.LookAt = player.transform;
    }

}
