using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Mirror;

public class UI_UpgradeMenu : NetworkBehaviour
{

    UI_Purchase_Button button = null;

    void Start()
    {
        button = GetComponentInChildren<UI_Purchase_Button>();
        if (button)
        {
            ResourceBag bag = new ResourceBag();
            bag.addResource(ResourceName.STONE, 420);
            bag.addResource(ResourceName.GOLD, 69);
            button.setPrice(bag);
            
        }
    }

}
