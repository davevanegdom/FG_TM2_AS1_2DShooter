using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cs_GameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public cs_PlayerController playerController;
    public cs_UIManager uiManager;



    [SerializeField] Transform playerSpawnPosition;
    [SerializeField] Transform playerRespawnPosition;

    public int shootingScore = 0;

    // Start is called before the first frame update
    void Start()
    {
        //Spawn player at the center of the arena
        #region
        //Spawns the player prefab at the center of the arena position (PlayerSpawnPosition.position), with no rotation specified (Quaternion.identity) -- rotation as is!
        GameObject player = Instantiate(playerPrefab, playerSpawnPosition.position, Quaternion.identity);
        //I want to save a reference to the PlayerController component of the player, for later use or so.
        playerController = player.GetComponent<cs_PlayerController>();
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
