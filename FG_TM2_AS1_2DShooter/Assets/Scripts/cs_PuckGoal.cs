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

    }

    void displayPucks(int pucks)
    {

        if(transform.childCount > 0)
        {
            //Clear all previous stored pucks
            foreach (GameObject child in transform)
            {
                Destroy(child);
            }
        }

        if(pucks == 1)
        {
            Instantiate(puckPrefab, transform);
        }
        else
        {
            for (int i = 0; i < pucks; i++)
            {
                Instantiate(puckPrefab, transform);
            }

            float intervalDistance = 0.2f;
            float length = intervalDistance * pucks;
            float startPos = transform.position.y - (length / 2);
            int loopInt = 0;
            Debug.Log("About to spawn some pucks:" + collectedPucks);
            foreach(GameObject puck in transform)
            {
                puck.transform.position = new Vector2(0, startPos + (loopInt * intervalDistance));
                loopInt++;
            }
        }
    }
}
