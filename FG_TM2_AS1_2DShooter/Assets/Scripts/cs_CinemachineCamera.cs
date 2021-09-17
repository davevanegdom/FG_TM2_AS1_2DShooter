using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class cs_CinemachineCamera : MonoBehaviour
{
    public static cs_CinemachineCamera Instance { get; private set; }

    private CinemachineVirtualCamera camera;
    [SerializeField] float camMinSize = 2;
    [SerializeField] float camMaxSize = 4;
    private GameObject player;
    private cs_PlayerController playerController;
    private Rigidbody2D rbPlayer;

    public float smoothRate;
    public float shakeTimer;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
       
        camera = GetComponent<CinemachineVirtualCamera>();

        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<cs_PlayerController>();
        rbPlayer = player.GetComponent<Rigidbody2D>();
        camera.Follow = player.transform;
        camera.LookAt = player.transform;
    }


    private void FixedUpdate()
    {
        float cSize = Mathf.Lerp(camMinSize, camMaxSize, (rbPlayer.velocity.magnitude / playerController.maxMoveSpeed));
        float size = Mathf.Lerp(camera.m_Lens.OrthographicSize, cSize, smoothRate * Time.deltaTime);
        camera.m_Lens.OrthographicSize = size;

        if(shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;

        }
        else if(shakeTimer <= 0f)
        {
            CinemachineBasicMultiChannelPerlin shakeComponent = camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            shakeComponent.m_AmplitudeGain = 0f;
        }
    }

    public void ShakeCamera(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin shakeComponent = camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        shakeComponent.m_AmplitudeGain = intensity;

    }



}
