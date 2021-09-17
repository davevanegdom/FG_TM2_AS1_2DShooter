using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cs_PuckGoal : MonoBehaviour
{
    public enum teamGoal {blue, red}
    public teamGoal TeamGoal;

    [SerializeField] GameObject puckPrefab;
    public int collectedPucks;
    public int maxCollectablePucks;

    [SerializeField] private Text numberDisplay;
    [SerializeField] cs_GameManager gameManager;

    private void Start()
    {
        numberDisplay.text = "0";
        numberDisplay.gameObject.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, transform.position.normalized.x * 90));
    }

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
                if (child.GetSiblingIndex() > 0)
                {
                    Destroy(child.gameObject);
                }
            }
        }

        for (int i = 0; i < pucks; i++)
        {
            Instantiate(puckPrefab, transform);
        }

        float intervalDistance = 0.15f;
        float length = intervalDistance * (pucks - 1);
        float startPos = transform.position.y - length * 1.5f;
        int loopInt = 0;

        foreach(Transform puck in transform)
        {
            if (puck.GetSiblingIndex() > 0)
            {
                puck.transform.position = new Vector2((transform.position.x + transform.position.normalized.x * -0.175f), startPos + (loopInt * intervalDistance));
                loopInt++;
            }
            
        }

        collectedPucks = pucks;
        numberDisplay.text = collectedPucks.ToString();
    }

    void PickUpPucks()
    {
        gameManager.playerPucks += collectedPucks;
        gameManager.uiManager.uiPuckCount.text = gameManager.playerPucks.ToString();

        foreach (Transform puck in transform)
        {
            if(puck.GetSiblingIndex() > 0)
            {
                Destroy(puck.gameObject);
            }
            
        }

        collectedPucks = 0;
        numberDisplay.text = collectedPucks.ToString();
        
    }
}
