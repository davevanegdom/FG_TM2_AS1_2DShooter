using UnityEngine;
using UnityEngine.AI;

public class cs_CameraFollow : MonoBehaviour
{

    [SerializeField] private Transform target;
    [SerializeField] [Range(0.01f, 10f)]
    private float smoothSpeed = 4f;
    [SerializeField] private Vector3 offset;
    private Vector3 velocity = Vector3.zero;

    public float MinDist = 2;
    public float MaxDist = 5;
    public float MaxVelocity = 2;
    
    public Rigidbody2D playerBody;


    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        float TargetDist = MinDist + (MaxDist - MinDist) * Mathf.Min(1.0f, playerBody.velocity.magnitude / MaxVelocity);
        float cSize = TargetDist;
        Camera.main.orthographicSize = cSize;

        transform.position = Damp_Vector3(transform.position, new Vector3(target.transform.position.x, target.transform.position.y, - 10), smoothSpeed, Time.deltaTime);
        
    }

    
    public static Vector3 Damp_Vector3(Vector3 a, Vector3 b, float lambda, float dt)
    {
        return Vector3.Lerp(a, b, 1 - Mathf.Exp(-lambda * dt));
    }
}
