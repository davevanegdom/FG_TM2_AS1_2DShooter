using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class cs_MainMenu : MonoBehaviour
{
    public AudioSource OpenMenu;
    public void QuitGame()
        {
        Application.Quit();
        }
    public void StartGame()
        {

        SceneManager.LoadScene("IceHockeyGameSceneAfterMerge");
        Time.timeScale = 1f;
        }

    public void PlaySound()
         {
        OpenMenu.Play();
         }
    }
 
