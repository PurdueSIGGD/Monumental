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
        button = GetComponentInChildren<UI_Purchase_Button>();
        if (button)
        {
            ResourceBag bag = new ResourceBag();
            bag.addResource(ResourceName.STONE, 420);
            bag.addResource(ResourceName.GOLD, 69);
            button.myBase = GameObject.Find("NetworkManager").GetComponent<MonumentalNetworkManager>().baseList[team].GetComponent<Base>();
            Upgrade up = new Upgrade(UpgradeType.Gather, 1);
            button.setPrice(up);

        }
    }

}
