using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cs_Puck : MonoBehaviour
{

    private Rigidbody2D rbPuck;
    private ParticleSystem pulseEffect;
    private TrailRenderer puckTrail;
    private CircleCollider2D puckCollider;
    private cs_PuckPulseEffect pulseScript;

    public Transform player;
    public float pickUpDistance = 0.25f;



    // Start is called before the first frame update
    void Start()
    {
        rbPuck = GetComponent<Rigidbody2D>();
        pulseEffect = GetComponent<ParticleSystem>();
        puckTrail = GetComponent<TrailRenderer>();
        puckCollider = GetComponent<CircleCollider2D>();
        pulseScript = GetComponentInChildren<cs_PuckPulseEffect>();
        puckCollider.enabled = false;
    }

    private void Update()
    {
        if(puckCollider.enabled == false && Vector2.Distance(player.position, transform.position) > pickUpDistance * 1.5f)
        {
            EnableCollision();
        }

        if (rbPuck.velocity.magnitude < 1)// && !pulseEffect.isPlaying && puckTrail.enabled)
        {
            pulseScript.isPlaying = true;
            if(puckTrail.enabled)puckTrail.enabled = false;
        }
        else
        {
            pulseScript.isPlaying = false;
            if (!puckTrail.enabled) puckTrail.enabled = true;
        }
    }

    void EnableCollision()
    {
        puckCollider.enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Arena")
        {
            Debug.Log("Spawn Wall Hit effect");
        }
    }


}
