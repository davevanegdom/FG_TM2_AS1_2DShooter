using UnityEngine;
using UnityEngine.AI;

public class cs_CameraFollow : MonoBehaviour
{

    private GameObject player;
    [SerializeField] [Range(0.01f, 10f)]
    private float smoothSpeed = 4f;
    [SerializeField] private Vector3 offset;

    public float MinDist = 2;
    public float MaxDist = 5;
    public float MaxVelocity = 2;
    
    private Rigidbody2D playerBody;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerBody = player.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        float TargetDist = MinDist + (MaxDist - MinDist) * Mathf.Min(1.0f, (playerBody.velocity.magnitude / MaxVelocity) * .2f);
        float cSize = TargetDist;
        Camera.main.orthographicSize = cSize;

        transform.position = Damp_Vector3(transform.position, new Vector3(player.transform.position.x, player.transform.position.y, -10), smoothSpeed, Time.deltaTime);

    }


    public static Vector3 Damp_Vector3(Vector3 a, Vector3 b, float lambda, float dt)
    {
        return Vector3.Lerp(a, b, 1 - Mathf.Exp(-lambda * dt));
    }
}
