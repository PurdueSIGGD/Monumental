using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Purchase_Button : MonoBehaviour
{

    public GameObject resourceLocation = null;
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

        foreach (Resource r in price.getBag())
        {
            GameObject obj = Instantiate(Resources.Load("UI/ResourceCounter", typeof(GameObject))) as GameObject;
            obj.transform.parent = this.transform;
            obj.transform.position = resourceLocation.transform.position;
            resources.Add(obj);
        }
    }

}
