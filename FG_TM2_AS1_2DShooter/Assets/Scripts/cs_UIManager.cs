using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cs_UIManager : MonoBehaviour
{
    public cs_GameManager gameManager;

    public Text uiWaveIndex;
    public Text uiWaveProgress;
    public Text uiRedTeamPucks;
    public Text uiBlueTeamPucks;
    public Text uiGameTimer;
    public Text uiPuckCount;

    public void waveUI(int wave)
    {
        uiWaveIndex.text = "WAVE " + wave;
    }
    private void OnEnable()
    {
        cs_WaveManager.setWaveIndex += waveUI;
    }
    private void OnDisable()
    {
        cs_WaveManager.setWaveIndex -= waveUI;
    }
}
