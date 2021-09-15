using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cs_Puck : MonoBehaviour
{
    private CircleCollider2D puckCollider;
    public Transform player;
    public float pickUpDistance = 0.25f;

    // Start is called before the first frame update
    void Start()
    {
        puckCollider = GetComponent<CircleCollider2D>();
        puckCollider.enabled = false;
    }

    private void Update()
    {
        if(puckCollider.enabled == false && Vector2.Distance(player.position, transform.position) > pickUpDistance * 1.5f)
        {
            EnableCollision();
        }
    }

    void EnableCollision()
    {
        puckCollider.enabled = true;
    }
}
