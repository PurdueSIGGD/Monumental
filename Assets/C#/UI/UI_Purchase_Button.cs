using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class UI_Purchase_Button : NetworkBehaviour
{

    public GameObject resourceLocation = null;
    public Button button = null;
    public Text text = null;
    public Text title = null;
    private List<GameObject> resources = new List<GameObject>();
    public int up;
    public Base myBase;

    public void setPrice(ResourceBag price)
    {   
        if (!resourceLocation)
        {
            return;
        }

        for (int i = 0; i < resources.Count; i++)
        {
            Destroy(resources[i]);
        }
        resources.Clear();

        int[] rsc = price.getBag();
        for (int i = 0; i < rsc.Length; i++)
        {
            /* Important to instantiate with parent transform parameter */
            GameObject obj = Instantiate(Resources.Load("UI/ResourceCounter", typeof(GameObject)) as GameObject, resourceLocation.transform);
            obj.GetComponentInChildren<Text>().text = "" + rsc[i];
            obj.GetComponent<Image>().sprite = Resource.getSprite(i+1);
            obj.transform.position += new Vector3(60 * i, 0, 0);
            resources.Add(obj);
        }

        if (button)
        {
            button.onClick.AddListener(makePurchase);
        }
        
    }

    public void setPrice(int upgrade, bool isMonument)
    {
        up = upgrade;

        if (!resourceLocation)
        {
            return;
        }

        for (int i = 0; i < resources.Count; i++)
        {
            Destroy(resources[i]);
        }
        resources.Clear();

        int[] rsc = myBase.resourceCostForUpgrade(up, myBase.getUpgradeLevel(up));
        if (!isMonument)
        {
            int objectOffset = 0;
            for (int i = 0; i < rsc.Length; i++)
            {
                if (rsc[i] == 0) continue;
                /* Important to instantiate with parent transform parameter */
                GameObject obj = Instantiate(Resources.Load("UI/ResourceCounter", typeof(GameObject)) as GameObject, resourceLocation.transform);
                obj.GetComponentInChildren<Text>().text = "" + rsc[i];
                obj.GetComponent<Image>().sprite = Resource.getSprite(i+1);
                float offset = obj.GetComponent<RectTransform>().rect.width * obj.transform.lossyScale.x * 1.5f;
                obj.transform.position += new Vector3(offset * objectOffset, 0, 0);
                resources.Add(obj);
                objectOffset++;
            }
        }
        //Debug.Log("done");
        //title.text = up.type + " Tier " + up.tier + " - " + myBase.upgradeLevels[myBase.upgrades.IndexOf(up)];
        text.text = "Purchase";
        if (isMonument)
        {
            /*Monument mon = new Monument();
            if (mon.purchased)
            {
                title.text = mon.name + " " + mon.owner;
                text.text = "";
                button.enabled = false;
            }
            else
            {
                title.text = "???";
            }*/
        }
        else
        {
            title.text = "Upgrade " + up + " - Tier " + myBase.getUpgradeLevel(up);
        }
        
        if (button)
        {
            button.onClick.AddListener(makePurchase);
        }

    }

    private string getType(int upgrade)
    {
        if(upgrade % 2 == 0)
        {
            return "Health";
        }
        else
        {
            return "Damage";
        }
    }

    public void makePurchase()
    {
        /*if (up.type == UpgradeType.Monument)
        {
            if (myBase.purchaseMonument((Monument)up))
            {
                //Debug.Log("Monument obtained");
                setPrice(up, true);
            }
            return;
        }*/
        if (this.GetComponentInParent<UI_UpgradeMenu>().purchaseUp(myBase, up))
        {
            //Debug.Log("purchase successful");
            setPrice(up, false);
            //myBase.resPool.testBag();
        }
        else
        {
            //Debug.Log("purchase failed");
            //myBase.resPool.testBag();
        }
    }


}
