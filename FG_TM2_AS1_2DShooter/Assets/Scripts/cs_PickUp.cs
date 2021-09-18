using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cs_PickUp : MonoBehaviour
{
    public cs_GameManager gameManager;

    void PickUpPuckPlayer(GameObject player)
    {
        gameManager.playerPucks++;
        gameManager.uiManager.uiPuckCount.text = gameManager.playerPucks.ToString();
        player.GetComponent<cs_PlayerController>().displayPuck(1);
        Destroy(gameObject);
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PickUpPuckPlayer(collision.gameObject);
        }
    }
}
