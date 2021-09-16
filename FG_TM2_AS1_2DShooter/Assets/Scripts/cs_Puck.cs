using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cs_Puck : MonoBehaviour
{

    private Rigidbody2D rbPuck;
    private ParticleSystem pulseEffect;
    private TrailRenderer puckTrail;
    private CircleCollider2D puckCollider;
    public Transform player;
    public float pickUpDistance = 0.25f;

    // Start is called before the first frame update
    void Start()
    {
        rbPuck = GetComponent<Rigidbody2D>();
        pulseEffect = GetComponent<ParticleSystem>();
        puckTrail = GetComponent<TrailRenderer>();
        puckCollider = GetComponent<CircleCollider2D>();
        puckCollider.enabled = false;
    }

    private void Update()
    {
        if(puckCollider.enabled == false && Vector2.Distance(player.position, transform.position) > pickUpDistance * 1.5f)
        {
            EnableCollision();
        }

        //if(rbPuck.velocity.magnitude < 1 && puckTrail.enabled)// && !pulseEffect.isPlaying && puckTrail.enabled)
        //{
        //    pulseEffect.Play();
        //    puckTrail.enabled = false;
        //    Debug.Log("Play Effect");
        //}
        //else
        //{
        //    if(pulseEffect.isPlaying && !puckTrail.enabled)
        //    {
        //        pulseEffect.Stop();
        //        puckTrail.enabled = true;
        //    }
        //}
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
