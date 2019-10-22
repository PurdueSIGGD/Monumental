using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

[RequireComponent(typeof(NetworkManager))]
public class MonumentalNetworkMenu : MonoBehaviour
{
    bool escapeIsPressed = false;
    bool menuIsShowing = true;
    NetworkManager manager;

    [SerializeField]
    GameObject menu;
    InputField text;
     

    void Awake()
    {
        manager = GetComponent<NetworkManager>();
        text = GetComponentInChildren<InputField>(menu);
    }

    void Update()
    {
        float esc = Input.GetAxis("Cancel");
        if (!escapeIsPressed && esc > 0.5f)
        {
            escapeIsPressed = true;
            menuIsShowing = !menuIsShowing;
            menu.SetActive(menuIsShowing);
        }
        else if(escapeIsPressed && esc == 0)
        {
            escapeIsPressed = false;
        }
    }

    public void startHosting()
    {
        manager.StartHost();
    }

    public void connectLAN()
    {
        manager.networkAddress = text.text;
        manager.StartClient();
    }

    public void quitGame()
    {
        if (NetworkServer.active || NetworkClient.isConnected)
        {
            manager.StopHost();
        }
    }
}
