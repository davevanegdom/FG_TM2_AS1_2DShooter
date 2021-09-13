using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cs_GameManager : MonoBehaviour
{
    [SerializeField] GameObject prefabPlayer;
    cs_PlayerController playerController;
    [SerializeField] cs_UIManager uiManager;
    private bool gameIsActive = true;

    [Header("Player")]
    public int playerLifes;
    [SerializeField] Transform playerLifesPanel;
    [SerializeField] List<Sprite> playerLifeIcons;
    [SerializeField] Sprite prefabPlayerLifeIcon;
    public int playerPucks;
    public int playerPucksInNet;
    [SerializeField] Transform playerSpawnPoint;

    [Header("Enemy Waves")]
    [SerializeField] private float waveInterval;
    private int waveIndex;
    private int waveProgress;

    [Header("UI")]
    public int savedSeconds = 0;
    public int minutes = 0;
    public int hours = 0;

    private void Start()
    {
        minutes--;
        startGame(playerLifes, playerPucks, waveIndex, (savedSeconds + (minutes * 60) + (hours * 3600)));
    }


    void startGame(int currentPlayerLifes, int currentPlayerPucks, int currentWave, float seconds)
    {
        //Spawn player
        SpawnPlayer(playerSpawnPoint.position);

        //Spawn player hearts
        for (int i = 0; i < currentPlayerLifes; i++)
        {
            Sprite playerSingleLifeIcon = Instantiate(prefabPlayerLifeIcon, playerLifesPanel);
            playerLifeIcons.Add(playerSingleLifeIcon);
        }

        //Update the puck count on the player controller
        playerController.puckCount = currentPlayerPucks;
        uiManager.uiPuckCount.text = currentPlayerPucks.ToString();

        //Set the wave index of the UI
        uiManager.uiWaveIndex.text = currentWave.ToString();

        //Start game timer
        //StartCoroutine(gameTimer(seconds));

        InvokeRepeating("displayTime", 1, 1);
    }

    void SpawnPlayer(Vector2 spawnPos)
    {
        if(GameObject.FindGameObjectWithTag("Player") == null)
        {
            GameObject player = Instantiate(prefabPlayer, spawnPos, Quaternion.identity);
            playerController = player.GetComponent<cs_PlayerController>();
        }
        else
        {
            GameObject existingPlayer = GameObject.FindGameObjectWithTag("Player");
            Destroy(existingPlayer);
            GameObject player = Instantiate(prefabPlayer, spawnPos, Quaternion.identity);
            playerController = player.GetComponent<cs_PlayerController>();
        }


    }

    //public IEnumerator gameTimer(float seconds)
    //{
    //    float time = seconds;
    //    Debug.Log("Timer Started");

    //    while (gameIsActive)
    //    {
    //        time += Time.deltaTime;
    //        Mathf.FloorToInt(time);
    //        uiManager.uiGameTimer.text = displayTime((int)time);
    //        yield return null;
    //    }

    //    savedSeconds = Mathf.FloorToInt(time);
    //}


    void displayTime()
    {
        savedSeconds++;

        string timeSeconds = "";
        string timeMinutes = "";
        string timeHours = "";
        
        float correctSeconds = 0;

        if(hours < 1)
        {
            correctSeconds = savedSeconds - ((minutes - 1) * 60);
        }
        else
        {
            correctSeconds = savedSeconds - ((minutes - 1)* 60) - ((hours - 1) * 3600);
        }
        

        #region Seconds
        if (correctSeconds < 10)
        {
            timeSeconds = ":0" + correctSeconds;
        }
        else if(correctSeconds >= 10 && correctSeconds < 60)
        {
            timeSeconds = ":" + correctSeconds.ToString();
        }
        else
        {
            minutes++;
            timeSeconds = ":00";
        }
        #endregion
        #region Minutes
        if (minutes < 1)
        {
            timeMinutes = "00";
        }
        else if(minutes >= 1 && minutes < 10)
        {
            timeMinutes = "0" + minutes;
        }
        else if(minutes >= 10 && minutes < 60)
        {
            timeMinutes = minutes.ToString();
        }
        else
        {
            timeMinutes = "00";
            hours++;
            Debug.Log(hours);
        }
        #endregion minutes;
        #region Hours
        if (hours < 1)
        {
            timeHours = "";
        }
        else if(hours >= 1 && hours < 10)
        {
            timeHours = "0" + hours + ":";
        }
        else
        {
            timeHours = hours + ":";
        }
        #endregion 
        
        uiManager.uiGameTimer.text = timeHours + timeMinutes + timeSeconds;
    }
}
