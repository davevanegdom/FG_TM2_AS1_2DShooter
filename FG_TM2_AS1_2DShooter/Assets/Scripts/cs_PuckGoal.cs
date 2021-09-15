using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cs_PuckGoal : MonoBehaviour
{
    public enum teamGoal {blue, red}
    public teamGoal TeamGoal;

    [SerializeField] GameObject puckPrefab;
    public int collectedPucks;
    public int maxCollectablePucks;

    [SerializeField] cs_GameManager gameManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Puck" && collectedPucks < maxCollectablePucks)
        {
            displayPucks(collectedPucks + 1);
            Destroy(collision.gameObject);
        }

        if(collision.gameObject.tag == "Player" && collectedPucks > 0)
        {
            PickUpPucks();
        }
    }

    void displayPucks(int pucks)
    {

        if(transform.childCount > 0)
        {
            //Clear all previous stored pucks
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }

        for (int i = 0; i < pucks; i++)
        {
            Instantiate(puckPrefab, transform);
        }

        float intervalDistance = 0.1f;
        float length = intervalDistance * (pucks - 1);
        float startPos = transform.position.y - length * 1.5f;
        int loopInt = 0;

        foreach(Transform puck in transform)
        {
            puck.transform.position = new Vector2(transform.position.x, startPos + (loopInt * intervalDistance));
            loopInt++;
        }

        collectedPucks = pucks;

        switch (TeamGoal)
        {
            case teamGoal.blue:
                gameManager.uiManager.uiBlueTeamPucks.text = collectedPucks.ToString();
                break;
            case teamGoal.red:
                gameManager.uiManager.uiRedTeamPucks.text = collectedPucks.ToString();
                break;
        }
    }

    void PickUpPucks()
    {
        gameManager.playerPucks += collectedPucks;
        gameManager.uiManager.uiPuckCount.text = gameManager.playerPucks.ToString();

        foreach (Transform puck in transform)
        {
            Destroy(puck.gameObject);
        }

        collectedPucks = 0;
        switch (TeamGoal)
        {
            case teamGoal.blue:
                gameManager.uiManager.uiBlueTeamPucks.text = collectedPucks.ToString();
                break;
            case teamGoal.red:
                gameManager.uiManager.uiRedTeamPucks.text = collectedPucks.ToString();
                break;
        }
    }
}
