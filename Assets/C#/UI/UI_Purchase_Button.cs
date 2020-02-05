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
    private bool isInitialized = false;
    private int upLevel = 1;

    public void setPrice(int upgrade, bool isMon)
    {
        up = upgrade;
        isMonument = isMon;
        isInitialized = true;

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
            //title.text = myMon.getMonumentName(up);
        }
        else
        {
            title.text = getType(up) + " " + up + " - Tier " + myBase.getUpgradeLevel(up);
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
        if (isMonument)
        {
            myBase.purchaseMonument(up);
        }
        else
        {
            myBase.purchaseUpgrade(up);
        }
    }

    private void FixedUpdate()
    {
        if (!isInitialized) return;

        if (isMonument)
        {
            if(myMon.GetOwner(up) != -1)
            {
                setPrice(up);
            }
            return;
        }
        if (myBase.purchaseUpgrade(up))
        {
            setPrice(up);
                if(myMon.GetOwner(up) == myBase.teamIndex)
                {
                    text.text = "Owned";
                }
                else
                {
                    text.text = "Sold out";
                }

                if (button)
                {
                    button.onClick.RemoveAllListeners();
                }
                isInitialized = false;
                for(int i=0; i < resourceSprites.Count; i++)
                {
                    Destroy(resourceSprites[i]);
                }
            }
        }
        else
        {
            if(myBase.getUpgradeLevel(up) != upLevel)
            {
                upLevel = myBase.getUpgradeLevel(up);
                int[] rsc = myBase.resourceCostForUpgrade(up, upLevel);
                int a = 0;
                title.text = getType(up) + " " + up + " - Tier " + upLevel;
                for (int i=0; i<rsc.Length; i++)
                {
                    if (rsc[i] == 0) continue;
                    resourceSprites[a].GetComponentInChildren<Text>().text = "" + rsc[i];
                    a++;
                }
            }
        }
    }
}
