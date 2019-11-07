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
    private List<GameObject> resources = new List<GameObject>();

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

        SyncListResource rsc = price.getBag();
        for (int i = 0; i < rsc.Count; i++)
        {
            /* Important to instantiate with parent transform parameter */
            GameObject obj = Instantiate(Resources.Load("UI/ResourceCounter", typeof(GameObject)) as GameObject, resourceLocation.transform);
            obj.transform.position += new Vector3(80 * i, 0, 0);
            obj.GetComponentInChildren<Text>().text = "" + rsc[i].getAmount();
            obj.GetComponent<Image>().sprite = Resource.getSprite(rsc[i].getType());
            resources.Add(obj);
        }
    }

}
