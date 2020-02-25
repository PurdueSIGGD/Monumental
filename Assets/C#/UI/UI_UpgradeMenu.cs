using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Mirror;

public class UI_UpgradeMenu : NetworkBehaviour
{
    
    public Text[] UpgradeDisplay;
    public Text SpeedText;
    public Text GatherText;
    public Base myBase;
    public void reset(int team)
    {
        if (!(myBase))
        {
            Debug.Log("finding");
            myBase = GameObject.Find("NetworkManager").GetComponent<MonumentalNetworkManager>().baseList[team].GetComponent<Base>();
        }
        else Debug.Log("already");
        myBase.upgradeMenu = this;
        UI_Purchase_Button[] buttonList = GetComponentsInChildren<UI_Purchase_Button>();
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].myBase = myBase;
            buttonList[i].isMonument = false;
            buttonList[i].setPrice(i+1, this);
            UpgradeDisplay[i].text = ""+myBase.getUpgradeLevel(i+1);
        }
        SpeedText.text = string.Format("{0:0.##}", myBase.baseStats.baseMovementSpeed/10);
        GatherText.text = string.Format("{0:0.##}", myBase.baseStats.baseGatherAmount);
    }

    public void preview(int up)
    {
        if (!(myBase))
        {
            return;
        }
        for (int i = 0; i < UpgradeDisplay.Length; i++)
        {
            UpgradeDisplay[i].text = up+":" + myBase.getUpgradeLevel(i + 1);
        }
    }

    public void unPreview()
    {
        if (!(myBase))
        {
            return;
        }
        for (int i = 0; i < UpgradeDisplay.Length; i++)
        {

            UpgradeDisplay[i].text = ""+myBase.getUpgradeLevel(i + 1);
        }
    }
}
