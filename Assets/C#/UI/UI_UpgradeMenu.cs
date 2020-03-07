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
            //Debug.Log("finding");
            myBase = GameObject.Find("NetworkManager").GetComponent<MonumentalNetworkManager>().baseList[team].GetComponent<Base>();
        }
        //else Debug.Log("already");
        myBase.upgradeMenu = this;
        UI_Purchase_Button[] buttonList = GetComponentsInChildren<UI_Purchase_Button>();
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].myBase = myBase;
            buttonList[i].isMonument = false;
            buttonList[i].setPrice(i+1, this);
            UpgradeDisplay[i].text = ""+myBase.getUpgradeLevel(i+1);
            UpgradeDisplay[i].color = Color.white;
        }
        SpeedText.text = string.Format("{0:0.##}", myBase.baseStats.baseMovementSpeed/10);
        GatherText.text = string.Format("{0:0.##}", myBase.baseStats.baseGatherAmount);
        SpeedText.color = Color.white;
        GatherText.color = Color.white;
    }

    public void preview(int up)
    {
        if (!(myBase))
        {
            return;
        }
        float[] prev = myBase.previewUpgrade(up);
        //Debug.Log(string.Join(" ", prev));
        for (int i = 0; i < UpgradeDisplay.Length; i++)
        {
            if (prev[i] != 0)
            {
                UpgradeDisplay[i].text = ""+prev[i];
                UpgradeDisplay[i].color = Color.green;
            }
        }
        if (prev[6] != 0)
        {
            SpeedText.text = string.Format("{0:0.##}", prev[6] / 10);
            SpeedText.color = Color.green;
        }
        else
        {
            GatherText.text = string.Format("{0:0.##}", prev[7]);
            GatherText.color = Color.green;
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
            UpgradeDisplay[i].color = Color.white;
        }

        SpeedText.text = string.Format("{0:0.##}", myBase.baseStats.baseMovementSpeed / 10);
        GatherText.text = string.Format("{0:0.##}", myBase.baseStats.baseGatherAmount);
        SpeedText.color = Color.white;
        GatherText.color = Color.white;
    }
}
