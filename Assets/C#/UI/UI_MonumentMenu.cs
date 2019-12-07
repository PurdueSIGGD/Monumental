using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_MonumentMenu : MonoBehaviour
{
    [HideInInspector]
    private bool skip_first_enable = true;
    [HideInInspector]
    private MonumentHolder monHolder;

    private void OnEnable()
    {
        if (skip_first_enable)
        {
            skip_first_enable = false;
            //this.gameObject.SetActive(false);
        }
    }

    public void reset(int team)
    {
        Base myBase = GameObject.Find("NetworkManager").GetComponent<MonumentalNetworkManager>().baseList[team].GetComponent<Base>();
        if (!monHolder) {
            monHolder = GameObject.Find("MonumentHolder").GetComponent<MonumentHolder>();
        }

        UI_Purchase_Button[] buttonList = GetComponentsInChildren<UI_Purchase_Button>();
        //Debug.Log("Num Buts: " + buttonList.Length);
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].myBase = myBase;
            Upgrade up = monHolder.monuments[i];
            buttonList[i].setPrice(up);
        }
    }
}
