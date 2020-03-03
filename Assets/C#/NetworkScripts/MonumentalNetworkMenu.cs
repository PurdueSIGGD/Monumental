using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Mirror;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(NetworkManager))]
public class MonumentalNetworkMenu : MonoBehaviour
{
    bool escapeIsPressed = false;
    bool menuIsShowing = true;
    bool joining = false;
    NetworkManager manager;

    public GameObject lobbyCamera;
    private Vector3 lobbyCameraPosition;
    public GameObject titleCard;
    public GameObject bottomBar;
    public GameObject menu;
    public InputField text;
    public InputField nameField;
    public Button joinButton, hostButton, quitButton1, quitButton2, cancelConnectButton, restartButton;

    private void Start()
    {
        if(lobbyCamera != null)
        {
            lobbyCamera.SetActive(true);
            lobbyCameraPosition = lobbyCamera.GetComponent<Transform>().position;
        }
    }

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
            if (lobbyCamera != null)
            {
                lobbyCamera.SetActive(true);
                lobbyCamera.GetComponent<Transform>().position = lobbyCameraPosition;
            }
        }
        else
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#endif
#if UNITY_STANDALONE
            Application.Quit();
#endif
        }
    }

    public void changeName()
    {
        foreach(GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (player.GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                if (string.IsNullOrEmpty(nameField.text))
                {
                    player.GetComponent<Player>().chooseName("Player");
                }
                else
                {
                    player.GetComponent<Player>().chooseName(nameField.text);
                }
            }
        }
    }

    public void restart()
    {
        GameObject.FindObjectOfType<UI_Control>().clear();
        GameObject.FindObjectOfType<Monuments>().clear();
        Base[] bases = GameObject.FindObjectsOfType<Base>();
        for (int i = 0; i < bases.Length; i++)
        {
            bases[i].clear();
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
        quitButton1.gameObject.SetActive(true);
        bottomBar.SetActive(false);
        titleCard.SetActive(true);
        nameField.gameObject.SetActive(false);
        quitButton2.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
    }

    private void OnJoined()
    {
        GameObject.FindObjectOfType<UI_Control>().clear();
        menuIsShowing = !menuIsShowing;
        menu.SetActive(menuIsShowing);
        cancelConnectButton.gameObject.SetActive(false);
        joinButton.gameObject.SetActive(false);
        text.gameObject.SetActive(false);
        hostButton.gameObject.SetActive(false);
        quitButton1.gameObject.SetActive(false);
        bottomBar.SetActive(true);
        titleCard.SetActive(false);
        nameField.gameObject.SetActive(true);
        quitButton2.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
    }
}
