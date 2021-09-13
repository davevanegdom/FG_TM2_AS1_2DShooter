using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cs_UIManager : MonoBehaviour
{
    public cs_GameManager gameManager;

    [SerializeField] Text shootingScoreCounter;


    public void UpdateShooterScore()
    {
        gameManager.shootingScore++;
        shootingScoreCounter.text = gameManager.shootingScore.ToString();
    }
}
