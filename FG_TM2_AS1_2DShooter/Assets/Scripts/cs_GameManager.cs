using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cs_GameManager : MonoBehaviour
{
    [SerializeField] GameObject prefabPlayer;
    cs_PlayerController playerController;
    public cs_UIManager uiManager;
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
    public int savedMinutes = 0;
    public int savedHours = 0;

    private void Start()
    {
        savedSeconds = (savedHours * 3600) + (savedMinutes * 60) + savedSeconds;
        startGame(playerLifes, playerPucks, waveIndex);
    }

    void startGame(int currentPlayerLifes, int currentPlayerPucks, int currentWave)
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
        uiManager.uiWaveIndex.text = "WAVE " + currentWave;

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
            playerController.gameManager = this;
            player.GetComponentInChildren<cs_PickUp>().gameManager = this;
        }
        else
        {
            GameObject existingPlayer = GameObject.FindGameObjectWithTag("Player");
            Destroy(existingPlayer);
            GameObject player = Instantiate(prefabPlayer, spawnPos, Quaternion.identity);
            playerController = player.GetComponent<cs_PlayerController>();
        }
    }

    void displayTime()
    {
        savedSeconds++;

        int hours = 0;
        int minutes = 0;
        int seconds = savedSeconds;

        string stringHours = "";
        string stringMinutes = "";
        string stringSeconds = "";

        if(seconds > 3600)
        {
            hours = Mathf.FloorToInt(seconds / 3600);
            savedHours = hours;
            seconds -= hours * 3600;
        }
        
        if(seconds > 60)
        {
            minutes = Mathf.FloorToInt(seconds / 60);
            savedMinutes = minutes;
            seconds -= minutes * 60;
        }

        if(seconds < 10)
        {
            stringSeconds = "0" + seconds;
        }
        else if(seconds < 60)
        {
            stringSeconds = seconds.ToString();
        }
        else
        {
            stringSeconds = "00";
            minutes++;
        }

        if (minutes < 10)
        {
            stringMinutes = "0" + minutes;
        }
        else if(minutes < 60)
        {
            stringMinutes = minutes.ToString();
        }
        else
        {
            stringMinutes = "00";
            hours++;
        }

        if (hours < 24)
        {
            stringHours = hours.ToString();
        }

        if(hours < 1)
        {
            uiManager.uiGameTimer.text = stringMinutes + ":" + stringSeconds;
        }
        else if (hours >= 1 && hours < 24)
        {
            uiManager.uiGameTimer.text = stringHours + ":" + stringMinutes + ":" + stringSeconds;
        }
        else 
        {
            uiManager.uiGameTimer.text = "GET A LIFE";
            uiManager.uiGameTimer.fontSize = 14;
        }
    }



}
