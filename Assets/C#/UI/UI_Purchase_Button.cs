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
    public Upgrade up;
    public Base myBase;

    public void setPrice(ResourceBag price)
    {
        up = new Upgrade(UpgradeType.Gather, 1);
        
        if (!resourceLocation)
        {
            return;
        }

        for (int i = 0; i < resources.Count; i++)
        {
            Destroy(resources[i]);
        }
        resources.Clear();

        SyncListResource rsc = price.getBag();
        for (int i = 0; i < rsc.Count; i++)
        {
            /* Important to instantiate with parent transform parameter */
            GameObject obj = Instantiate(Resources.Load("UI/ResourceCounter", typeof(GameObject)) as GameObject, resourceLocation.transform);
            obj.GetComponentInChildren<Text>().text = "" + rsc[i].getAmount();
            obj.GetComponent<Image>().sprite = Resource.getSprite(rsc[i].getType());
            obj.transform.position += new Vector3(60 * i, 0, 0);
            resources.Add(obj);
        }

        if (button)
        {
            button.onClick.AddListener(makePurchase);
        }
        
    }

    public void setPrice(Upgrade upgrade)
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

        SyncListResource rsc = up.cost;
        if (up.type != UpgradeType.Monument || !((Monument)up).purchased)
        {
            for (int i = 0; i < rsc.Count; i++)
            {
                /* Important to instantiate with parent transform parameter */
                GameObject obj = Instantiate(Resources.Load("UI/ResourceCounter", typeof(GameObject)) as GameObject, resourceLocation.transform);
                obj.GetComponentInChildren<Text>().text = "" + rsc[i].getAmount();
                obj.GetComponent<Image>().sprite = Resource.getSprite(rsc[i].getType());
                float offset = obj.GetComponent<RectTransform>().rect.width * obj.transform.lossyScale.x * 1.5f;
                obj.transform.position += new Vector3(offset * i, 0, 0);
                resources.Add(obj);
            }
        }
        //title.text = up.type + " Tier " + up.tier + " - " + myBase.upgradeLevels[myBase.upgrades.IndexOf(up)];
        text.text = "Purchase";
        if (up.type == UpgradeType.Monument)
        {
            Monument mon = (Monument)up;
            if (mon.purchased)
            {
                title.text = mon.getName() + ": Team" + mon.owner;
                text.text = "";
                button.enabled = false;
            }
            else
            {
                title.text = mon.getName();
            }
        }
        else
        {
            title.text = up.type + " Tier " + up.tier + " - " + up.level;
        }
        
        if (button)
        {
            button.onClick.AddListener(makePurchase);
        }

    }

    public void makePurchase()
    {
        if (up.type == UpgradeType.Monument)
        {
            if (myBase.purchaseMonument((Monument)up))
            {
                setPrice(up);
            }
            return;
        }
        if (myBase.purchaseUpgrade(up))
        {
            setPrice(up);
        }
    }


}
