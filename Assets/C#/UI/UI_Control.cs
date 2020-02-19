using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class UI_Control : NetworkBehaviour
{
    public Button shopButton = null;
    public Button swapButton = null;
    public Button classButton = null;
    public GameObject upgradeMenu = null;
    public GameObject monumentMenu = null;
    public List<Image> monumentIcons = null;
    public List<Sprite> classIcons = null;
    public Text centerText = null;
    public Image classImageFront = null;
    public Image classImageBack = null;

    /* THE SACRED TEXTS! */
    public List<Text> resource_texts = new List<Text>();
    private List<Text> resource_team_texts = new List<Text>();

    private GameObject currentMenu = null;
    private float[] cooldownTimes = new float[2];// {Start, End}
    private Monuments monuments = null;

    public Player player = null;
    public Base myBase = null;

    void Start()
    {
        currentMenu = upgradeMenu;
        for (int i = 0; i < resource_texts.Count; i++)
        {
            GameObject obj = resource_texts[i].gameObject;
            GameObject clone = Instantiate(obj);
            clone.transform.SetParent(obj.transform.parent);
            clone.transform.position = obj.transform.position;
            clone.transform.localScale = obj.transform.localScale;
            Vector3 delta = new Vector3(0, -obj.GetComponent<RectTransform>().sizeDelta.y * clone.transform.localScale.y, 0);
            clone.transform.localPosition += delta;

            Text team_text = clone.GetComponent<Text>();
            resource_team_texts.Add(team_text);
            team_text.color = new Color(0,1,0);

        }

        for (int i = 1; i <= monumentIcons.Count; i++)
        {
            updateMonument(i, -1);
        }

        if (shopButton)
        {
            shopButton.onClick.AddListener(onShopButton);
        }

        if (classButton)
        {
            classButton.onClick.AddListener(onClassButton);
        }

        if (swapButton)
        {
            swapButton.onClick.AddListener(onSwapButton);
        }

        monuments = GameObject.Find("NetworkManager").GetComponent<MonumentalNetworkManager>().monuments;

    }

    public void clear()
    {
        centerText.text = "";
        currentMenu.SetActive(false);
        swapButton.transform.parent.gameObject.SetActive(false);
    }

    public void LateUpdate()
    {
        if (player)
        {
            updateResources();
            if (!player.isInBase && currentMenu.activeInHierarchy)
            {
                currentMenu.SetActive(false);
                swapButton.transform.parent.gameObject.SetActive(false);
            }
            shopButton.interactable = player.isInBase;
            classButton.interactable = player.isInBase;

            if (Time.time < cooldownTimes[1])
            {
                float ratio = (Time.time - cooldownTimes[0]) / (cooldownTimes[1] - cooldownTimes[0]);
                Vector2 size = classImageBack.rectTransform.sizeDelta;
                size = new Vector2(size.x, size.y * ratio);
                classImageFront.rectTransform.sizeDelta = size;
            }

        }
        /* Keyboard shortcuts */
        if (Input.GetKeyDown(KeyCode.E))
        {
            onShopButton();
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            onClassButton();
        }
        else if (Input.GetKeyDown(KeyCode.Tab))
        {
            onSwapButton();
        }

        UI_MonumentMenu monumentMenuScript = monumentMenu.GetComponent<UI_MonumentMenu>();
        for (int i = 1; i <= monumentMenuScript.buttonList.Length; i++)
        {
            int owner = monuments.GetOwner(i);
            updateMonument(i, owner);
            if (monumentMenu.activeInHierarchy)
            {
                monumentMenuScript.buttonList[i - 1].button.interactable = (owner == -1);
            }
        }

    }

    public void setCooldown(float duration)
    {
        cooldownTimes[0] = Time.time;
        cooldownTimes[1] = Time.time + duration;
    }
	
    public void updateResources()
    {
        bool isFull = player.resources.getSumOfBag() >= player.stats.getCarryCapacity();
        for (int i = 0; i < resource_texts.Count; i++)
        {
            resource_texts[i].text = "" + player.resources.getAmount(i+1);
            if (isFull)
            {
                resource_texts[i].color = Color.red;
            }
            else
            {
                resource_texts[i].color = Color.white;
            }
        }
        if (myBase == null) {
            myBase = player.myBase;
        }
        for (int i = 0; i < resource_team_texts.Count; i++)
        {
            resource_team_texts[i].text = "" + myBase.resPool.getAmount(i+1);
        }

    }

    public void updateMonument(int monument, int owner)
    {
        monument--; //To array index
        if (monument < 0 || monument >= monumentIcons.Count)
        {
            return;
        }
        monumentIcons[monument].color = (owner == -1 ? Color.gray : (owner == 0 ? Color.blue : Color.red));
    }

    void onClassButton()
    {
        if (player.isInBase)
        {
            player.changeClass();
            classImageFront.sprite = classIcons[player.stats.Class];
            classImageBack.sprite = classIcons[player.stats.Class];
        }
    }

    void onShopButton()
    {
        if (player.isInBase)
        {
            if (currentMenu.activeInHierarchy)
            {
                currentMenu.SetActive(false);
                swapButton.transform.parent.gameObject.SetActive(false);
            }
            else
            {
                currentMenu.SetActive(true);
                swapButton.transform.parent.gameObject.SetActive(true);
                if (currentMenu == upgradeMenu)
                {
                    currentMenu.GetComponent<UI_UpgradeMenu>().reset(player.teamIndex);
                }
                else
                {
                    currentMenu.GetComponent<UI_MonumentMenu>().reset(player.teamIndex);
                }
            }
        }
    }

    void onSwapButton()
    {
        currentMenu.SetActive(false);
        swapButton.transform.parent.gameObject.SetActive(false);
        if (currentMenu == upgradeMenu)
        {
            currentMenu = monumentMenu;
            currentMenu.SetActive(true);
            swapButton.GetComponentInChildren<Text>().text = "Upgrades";
            currentMenu.GetComponent<UI_MonumentMenu>().reset(player.teamIndex);
        }
        else
        {
            currentMenu = upgradeMenu;
            currentMenu.SetActive(true);
            swapButton.GetComponentInChildren<Text>().text = "Monuments";
            currentMenu.GetComponent<UI_UpgradeMenu>().reset(player.teamIndex);
        }
        swapButton.transform.parent.gameObject.SetActive(true);
    }

}
