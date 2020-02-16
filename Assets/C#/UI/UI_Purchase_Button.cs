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
    private List<GameObject> resourceSprites = new List<GameObject>();
    public int up;
    public Base myBase;
    public Monuments myMon;
    public bool isMonument = false;
    private MonumentalNetworkManager mnm = null;

    public void setPrice(int upgrade, bool isMon)
    {
        up = upgrade;
        isMonument = isMon;

        if (!resourceLocation)
        {
            return;
        }

        for (int i = 0; i < resourceSprites.Count; i++)
        {
            Destroy(resourceSprites[i]);
        }
        resourceSprites.Clear();

        int[] rsc;

        if (!isMon)
        {
            rsc = myBase.resourceCostForUpgrade(up, myBase.getUpgradeLevel(up));
        }
        else
        {
            rsc = myMon.GetCost(up);
        }
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
            resourceSprites.Add(obj);
            objectOffset++;
        }

        text.text = "Purchase";
        if (isMon)
        {
            title.text = "Monument " + up;
            if (!mnm)
            {
                mnm = GameObject.Find("NetworkManager").GetComponent<MonumentalNetworkManager>();
            }
            mnm.monuments.CmdSetMonumentButton(button, up);
        }
        else
        {
            title.text = getType(up) + " " + up + " - Tier " + myBase.getUpgradeLevel(up);
        }
        
        if (button)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(makePurchase);
        }
    }

    public void updatePrice()
    {
        setPrice(up, isMonument);
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
        if (isMonument)
        {
            myBase.purchaseMonument(up, this);
        }
        else
        {
            myBase.purchaseUpgrade(up, this);
        }
    }

}
