using UnityEngine;

public class cs_CameraFollow2 : MonoBehaviour
{

    [SerializeField] private Transform target;

    [SerializeField] [Range(0.01f, 10f)]

    private float smoothSpeed = 4f;
    
    [SerializeField] private Vector3 offset;

    
    private Vector3 velocity = Vector3.zero;


    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        transform.position = Damp_Vector3(transform.position, new Vector3(target.transform.position.x, target.transform.position.y, -10), smoothSpeed, Time.deltaTime);
    }

    public static float Damp(float a, float b, float lambda, float dt)
    {
        return Mathf.Lerp(a, b, 1 - Mathf.Exp(-lambda * dt));
    }

    public static Vector3 Damp_Vector3(Vector3 a, Vector3 b, float lambda, float dt)
    {
        return Vector3.Lerp(a, b, 1 - Mathf.Exp(-lambda * dt));
    }
}
