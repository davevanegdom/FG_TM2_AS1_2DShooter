using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cs_Target : MonoBehaviour
{
    
    private cs_UIManager uiManager;

    private void Awake()
    {
        uiManager = GameObject.FindGameObjectWithTag("UI Holder").GetComponent<cs_UIManager>(); 
    }

    // Start is called before the first frame update
    void Start()
    {
        
        Destroy(gameObject, 4f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name != "Player")
        {
            uiManager.UpdateShooterScore();
            Destroy(gameObject);
        }
    }
}
