using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Control : MonoBehaviour
{
    /* THE SACRED TEXTS! */
    public List<Text> resource_texts = new List<Text>();
    private List<Text> resource_team_texts = new List<Text>();

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

            setResource(i, 0, false);
            setResource(i, 0, true);

        }
    }

    public void setResource(int index, int value, bool team)
    {
        if (index < 0 || index >= resource_texts.Count)
        {
            return;
        }

        Text counter = (team ? resource_team_texts[index] : resource_texts[index]);

        counter.text = "" + value;

    }

}
