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
    bool joining = false;
    NetworkManager manager;
    
    public GameObject menu;
    public InputField text;
    public Button joinButton, hostButton, quitButton, cancelConnectButton;

    void Awake()
    {
        manager = GetComponent<NetworkManager>();
    }

    void Update()
    {
        float esc = Input.GetAxis("Cancel");
        if (!escapeIsPressed && esc > 0.5f && (NetworkClient.isConnected || NetworkServer.active))
        {
            escapeIsPressed = true;
            menuIsShowing = !menuIsShowing;
            menu.SetActive(menuIsShowing);
        }
        else if(escapeIsPressed && esc == 0)
        {
            escapeIsPressed = false;
        }

        // Successfully joined a game!  Now clean up the menu
        if(joining && NetworkClient.isConnected)
        {
            joining = false;
            OnJoined();
        }
    }

    public void ConnectLAN()
    {
        if (string.IsNullOrEmpty(text.text))
            manager.networkAddress = "localhost";
        else
            manager.networkAddress = text.text;
        manager.StartClient();
        cancelConnectButton.gameObject.SetActive(true);
        joinButton.gameObject.SetActive(false);
        text.gameObject.SetActive(false);
        hostButton.gameObject.SetActive(false);
        joining = true;
    }

    public void CancelConnect()
    {
        manager.StopClient();
        DefaultMenu();
    }

    public void QuitGame()
    {
        if (NetworkServer.active || NetworkClient.isConnected)
        {
            manager.StopHost();
            DefaultMenu();
        }
    }

    public void HostGame()
    {
        manager.StartHost();
        OnJoined();
    }

    private void DefaultMenu()
    {
        cancelConnectButton.gameObject.SetActive(false);
        joinButton.gameObject.SetActive(true);
        text.gameObject.SetActive(true);
        hostButton.gameObject.SetActive(true);
    }

    private void OnJoined()
    {
        menuIsShowing = !menuIsShowing;
        menu.SetActive(menuIsShowing);
        cancelConnectButton.gameObject.SetActive(false);
        joinButton.gameObject.SetActive(false);
        text.gameObject.SetActive(false);
        hostButton.gameObject.SetActive(false);
    }
}
