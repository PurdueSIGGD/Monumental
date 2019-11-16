using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_MonumentMenu : MonoBehaviour
{
    [HideInInspector]
    private bool skip_first_enable = true;

    private void OnEnable()
    {
        if (skip_first_enable)
        {
            skip_first_enable = false;
            this.gameObject.SetActive(false);
        }
    }
}
