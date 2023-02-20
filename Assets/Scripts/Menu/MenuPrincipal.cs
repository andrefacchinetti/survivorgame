using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using PlayFab;
using TMPro;
using UnityEngine.UI;

public class MenuPrincipal : MonoBehaviourPunCallbacks
{
    private bool startou = false;
    public TMP_Text txPressStart, txQuitGame;
    public RawImage imageServerRegion;
    public Texture[] imagensServersRegion; // 0 = us, 1 = sa, 2 = eu, 3 = asia, 4 = au, 5 = jp

    void Awake()
    {
        startou = false;
        Destroy(GameObject.FindGameObjectWithTag("Player"));
        Destroy(GameObject.Find("RoomManager"));
        Destroy(GameObject.Find("PhotonVoiceNetwork singleton"));
        atualizarImagemServerRegion();
        if (PlayerPrefs.GetInt("INDEXIDIOMA") == 1) //PORTUGUES
        {
            txPressStart.text = "Iniciar Jogo";
            txQuitGame.text = "Sair";
        }
        else //ingles
        {
            txPressStart.text = "Start Game";
            txQuitGame.text = "Quit";
        }
    }

    private void Update()
    {
        if (Input.GetButton("Start") || Input.GetKey(KeyCode.KeypadEnter))
        {
           startOnline();
        }
    }
  
    public void startOnline()
    {
        PlayerPrefs.SetString("gamemode", "competitive");
        if (startou || !PlayFabClientAPI.IsClientLoggedIn()) return;
        GameObject objSaveManager = GameObject.FindGameObjectWithTag("SaveManager");
        SaveManager sm = objSaveManager.GetComponent<SaveManager>();
        if (sm.verificouVersao)
        {
            if (sm.versoesIguais)
            {
                startou = true;
                Debug.Log("Iniciando game versao igual");
                sm.GetProgress();
                PhotonNetwork.OfflineMode = false;
                PhotonNetwork.LoadLevel(1);
            }
            else
            {
                sm.loginStatus.text = "Incorrect Version. Restart Steam to get the latest version of Deathrun Guys";
            }
        }
    }

    public void startOffline()
    {
        if (startou || !PlayFabClientAPI.IsClientLoggedIn()) return;
        startou = true;
        GameObject sm = GameObject.FindGameObjectWithTag("SaveManager");
        sm.GetComponent<SaveManager>().GetProgress();
        PhotonNetwork.OfflineMode = true;
        PhotonNetwork.LoadLevel(1);
    }

    public void quitGame()
    {
        Application.Quit();
    }

    public void ChangeServerRegionPhoton()
    {
        int indexServer = PlayerPrefs.GetInt("INDEXSERVERREGION") + 1;
        if (indexServer > imagensServersRegion.Length-1) indexServer = 0;
        PlayerPrefs.SetInt("INDEXSERVERREGION", indexServer);
        atualizarImagemServerRegion();
    }

    private void atualizarImagemServerRegion()
    {
        if(PlayerPrefs.GetInt("INDEXSERVERREGION") == 0)
        {
            imageServerRegion.texture = imagensServersRegion[0];
        }
        else if (PlayerPrefs.GetInt("INDEXSERVERREGION") == 1)
        {
            imageServerRegion.texture = imagensServersRegion[1];
        }
        else if (PlayerPrefs.GetInt("INDEXSERVERREGION") == 2)
        {
            imageServerRegion.texture = imagensServersRegion[2];
        }
        else if (PlayerPrefs.GetInt("INDEXSERVERREGION") == 3)
        {
            imageServerRegion.texture = imagensServersRegion[3];
        }
        else if (PlayerPrefs.GetInt("INDEXSERVERREGION") == 4)
        {
            imageServerRegion.texture = imagensServersRegion[4];
        }
        else if (PlayerPrefs.GetInt("INDEXSERVERREGION") == 5)
        {
            imageServerRegion.texture = imagensServersRegion[5];
        }
        else
        {
            PlayerPrefs.GetInt("INDEXSERVERREGION", 0);
            imageServerRegion.texture = imagensServersRegion[0];
        }
    }

}
