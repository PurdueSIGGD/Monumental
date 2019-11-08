using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Mirror;

public class UI_UpgradeMenu : NetworkBehaviour
{

    UI_Purchase_Button button = null;
    [HideInInspector]
    //public int teamIndex;

    void Start()
    {
        /*
        button = GetComponentInChildren<UI_Purchase_Button>();
        if (button)
        {
            ResourceBag bag = new ResourceBag();
            bag.addResource(ResourceName.STONE, 420);
            bag.addResource(ResourceName.GOLD, 69);
            button.myBase = GameObject.Find("NetworkManager").GetComponent<MonumentalNetworkManager>().baseList[teamIndex].GetComponent<Base>();
            button.setPrice(bag);
            
        }*/
        reset(0);
    }

    public void reset(int team)
    {
        Base myBase = GameObject.Find("NetworkManager").GetComponent<MonumentalNetworkManager>().baseList[team].GetComponent<Base>();
        button = GetComponentInChildren<UI_Purchase_Button>();
        if (button)
        {
            ResourceBag bag = new ResourceBag();
            bag.addResource(ResourceName.STONE, 420);
            bag.addResource(ResourceName.GOLD, 69);
            button.myBase = myBase;
            Upgrade up = myBase.upgrades[0];
            button.setPrice(up);

        }
    }

}
