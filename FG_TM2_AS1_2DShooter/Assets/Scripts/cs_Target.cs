using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cs_Target : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 4f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name != "Player")
        {
            Destroy(gameObject);
        }
    }
}
