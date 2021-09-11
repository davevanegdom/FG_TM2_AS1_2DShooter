using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cs_DebugPanel : MonoBehaviour
{
    public cs_PlayerController playerController;

    [SerializeField] Text debugPanelText;
    [SerializeField] GameObject toggleSystem;
    [SerializeField] GameObject sliderDashForce;
    [SerializeField] GameObject sliderMomentumRet;
    [SerializeField] GameObject toggleDrawDebugs;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OpenDebug()
    {
        debugPanelText.text = "close debug";
        toggleSystem.SetActive(true);
        sliderDashForce.SetActive(true);
        sliderMomentumRet.SetActive(true);
        toggleDrawDebugs.SetActive(true);
    }

    public void CloseDebug()
    {
        debugPanelText.text = "open debug";
        toggleSystem.SetActive(false);
        sliderDashForce.SetActive(false);
        sliderMomentumRet.SetActive(false);
        toggleDrawDebugs.SetActive(false);
    }

}
