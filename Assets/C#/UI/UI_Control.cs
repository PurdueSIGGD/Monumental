using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class UI_Control : NetworkBehaviour
{

    public Text healthBar = null;
    public Button upgradeButton = null;

    /* THE SACRED TEXTS! */
    public List<Text> resource_texts = new List<Text>();
    private List<Text> resource_team_texts = new List<Text>();
    private GameObject upgradeMenu = null;

    public Player player = null;
    public Base myBase;

    void Start()
    {
        for (int i = 0; i < resource_texts.Count; i++)
        {
            GameObject obj = resource_texts[i].gameObject;
            GameObject clone = Instantiate(obj);
            clone.transform.SetParent(obj.transform.parent);
            clone.transform.position = obj.transform.position;
            clone.transform.localScale = obj.transform.localScale;
            Vector3 delta = new Vector3(0, -obj.GetComponent<RectTransform>().sizeDelta.y, 0);
            clone.transform.localPosition += delta;

            Text team_text = clone.GetComponent<Text>();
            resource_team_texts.Add(team_text);
            team_text.color = new Color(0,1,0);

            

        }

        if (upgradeButton)
        {
            upgradeButton.onClick.AddListener(onUpgradeButton);
        }

    }

    public void LateUpdate()
    {
        if (player)
        {
            updateHealth();
            updateResources();
            if (!player.isInBase && upgradeMenu)
            {
                Destroy(upgradeMenu);
                upgradeMenu = null;
            }
        }
        /* Toggle upgrade menu */
        if (Input.GetKeyDown(KeyCode.E))
        {
            onUpgradeButton();
        }

    }

    public void updateHealth()
    {
        healthBar.text = player.health + "/" + player.stats.health;
    }

    public void updateResources()
    {

        for (int i = 0; i < resource_texts.Count; i++)
        {
            resource_texts[i].text = "" + player.resources.getAmount((ResourceName)(i+1));
        }
        if (myBase == null) {
            myBase = GameObject.Find("NetworkManager").GetComponent<MonumentalNetworkManager>().baseList[player.teamIndex].GetComponent<Base>();
        }
        for (int i = 0; i < resource_team_texts.Count; i++)
        {
            resource_team_texts[i].text = "" + myBase.resPool.getAmount((ResourceName)(i+1));
        }

    }

    void onUpgradeButton()
    {
        if (player.isInBase)
        {
            if (upgradeMenu)
            {
                Destroy(upgradeMenu);
                upgradeMenu = null;
            }
            else
            {
                upgradeMenu = Instantiate(Resources.Load("UI/UpgradeMenu", typeof(GameObject))) as GameObject;
                Debug.Log(player.teamIndex);
                upgradeMenu.GetComponent<UI_UpgradeMenu>().reset(player.teamIndex);
            }
        }
    }

}
