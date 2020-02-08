﻿using System.Collections;
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
    public Text centerText = null;

    /* THE SACRED TEXTS! */
    public List<Text> resource_texts = new List<Text>();
    private List<Text> resource_team_texts = new List<Text>();
    private GameObject currentMenu = null;

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
            Vector3 delta = new Vector3(0, -obj.GetComponent<RectTransform>().sizeDelta.y, 0);
            clone.transform.localPosition += delta;

            Text team_text = clone.GetComponent<Text>();
            resource_team_texts.Add(team_text);
            team_text.color = new Color(0,1,0);

        }

        for (int i = 0; i < monumentIcons.Count; i++)
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
        }
        /* Toggle shop menu */
        if (Input.GetKeyDown(KeyCode.E))
        {
            onShopButton();
        }

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
        if (monument < 0 || monument >= monumentIcons.Count)
        {
            return;
        }

        if (myBase == null || myBase.teamIndex == -1)
        {
            monumentIcons[monument].color = new Color(0.2f, 0.2f, 0.2f);
        }
        else if (myBase.teamIndex == owner)
        {
            monumentIcons[monument].color = Color.white;
        }
        else
        {
            monumentIcons[monument].color = Color.black;
        }
    }

    void onClassButton()
    {
        if (player.isInBase)
        {
            player.changeClass();
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
