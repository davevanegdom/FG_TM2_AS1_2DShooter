using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cs_PickUp : MonoBehaviour
{
    public cs_GameManager gameManager;
    

    void PickUpPuckPlayer(GameObject collidedPuck)
    {
        gameManager.playerPucks++;
        gameManager.uiManager.uiPuckCount.text = gameManager.playerPucks.ToString();
        Destroy(collidedPuck);
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Puck")
        {
            PickUpPuckPlayer(collision.gameObject);
        }
    }
}
