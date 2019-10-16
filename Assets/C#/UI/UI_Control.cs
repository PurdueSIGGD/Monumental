using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class UI_Control : NetworkBehaviour
{

    public Text healthBar = null;

    /* THE SACRED TEXTS! */
    public List<Text> resource_texts = new List<Text>();
    private List<Text> resource_team_texts = new List<Text>();

    public Player player = null;

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
    }

    public void LateUpdate()
    {
        if (player)
        {
            updateHealth();
            updateResources();
        }
    }

    public void updateHealth()
    {
        healthBar.text = player.stats.health + "/" + player.stats.maxHealth;
    }

    public void updateResources()
    {

        for (int i = 0; i < resource_texts.Count; i++)
        {
            resource_texts[i].text = "" + player.resources.getAmount((ResourceName)i);
        }

        for (int i = 0; i < resource_team_texts.Count; i++)
        {
            resource_team_texts[i].text = "" + player.resources.getAmount((ResourceName)i);
        }

    }

}
