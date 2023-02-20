using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using Steamworks;
using System;
using System.Text;
using Hashtable = ExitGames.Client.Photon.Hashtable;



public class SaveManager : MonoBehaviour
{
    public GetPlayerCombinedInfoRequestParams info;
    [HideInInspector] public int gems;
    public TMP_Text loginStatus;
    [HideInInspector] public string myPlayerFabId;
    //private string catalogVersion = "Catalogo";
    [SerializeField] string versao = "1.0.0"; //Versao daqui deve ser igual a do playfab title data
    [HideInInspector] public bool verificouVersao = false, versoesIguais = false;
    

    protected static SaveManager s_instance;
    protected static SaveManager Instance
    {
        get
        {
            if (s_instance == null)
            {
                return new GameObject("SaveManager").AddComponent<SaveManager>();
            }
            else
            {
                return s_instance;
            }
        }
    }
    public string GetSteamAuthTicket()
    {
        byte[] ticketBlob = new byte[1024];
        uint ticketSize;

        // Retrieve ticket; hTicket should be a field in the class so you can use it to cancel the ticket later
        // When you pass an object, the object can be modified by the callee. This function modifies the byte array you've passed to it.
        HAuthTicket hTicket = SteamUser.GetAuthSessionTicket(ticketBlob, ticketBlob.Length, out ticketSize);

        // Resize the buffer to actual length
        Array.Resize(ref ticketBlob, (int)ticketSize);

        // Convert bytes to string
        StringBuilder sb = new StringBuilder();
        foreach (byte b in ticketBlob)
        {
            sb.AppendFormat("{0:x2}", b);
        }
        return sb.ToString();
    }


    private void Awake()
    {
        if (s_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        s_instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        verificouVersao = false;
        versoesIguais = false;
        if (SteamManager.Initialized)
        {
            conectarPlayFabLogin();
        }
        else
        {
            loginStatus.text = "You must start the game from steam";
        }
    }

    private void conectarPlayFabLogin()
    {
        PlayFabClientAPI.LoginWithSteam(new LoginWithSteamRequest
        {
            TitleId = "91DF9",
            CreateAccount = true,
            SteamTicket = GetSteamAuthTicket(),
            InfoRequestParameters = info
        }, OnSuccess, OnErrorToConnect);
    }

    void OnSuccess(LoginResult result) //Logged In
    {
       Debug.Log("Login Successful");
       myPlayerFabId = result.PlayFabId;
       PlayerPrefs.SetString("NICKNAME", SteamFriends.GetPersonaName());
       GetContentPlayfabTitleDataVersao();
       atualizarDisplayname(SteamFriends.GetPersonaName());
       loginStatus.text = "Connected";
    }
    void OnErrorToConnect(PlayFabError error)
    {
        if (loginStatus != null) loginStatus.text = "Login Failed: Reconnecting...";
        Debug.Log(error.ErrorMessage);
        conectarPlayFabLogin();
    }

    void OnError(PlayFabError error)
    {
        Debug.Log(error.ErrorMessage);
    }

    public void GetContentPlayfabTitleDataVersao()
    {
        PlayFabClientAPI.GetTitleData(new GetTitleDataRequest(),
        result => {
            if (result.Data == null || !result.Data.ContainsKey("versao")) Debug.Log("No versao");
            else
            {
                verificouVersao = true;
                string versaoAtual = result.Data["versao"];
                if (versao.Equals(versaoAtual))
                {
                    versoesIguais = true;
                    Debug.Log("O cliente esta com a versao atual");
                }
                else
                {
                    versoesIguais = false;
                    if (loginStatus != null) loginStatus.text = "Incorrect Version. Restart Steam to get the latest version of Domination";
                    Debug.Log("O cliente esta com uma vers�o incorreta do game");
                }
                Debug.Log("Versao: " + result.Data["versao"]);
            }
        },
        error => {
            Debug.Log("Got error getting titleData:");
            Debug.Log(error.GenerateErrorReport());
        });
    }

    public void SaveProgress()
    {
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                {"Nickname", PlayerPrefs.GetString("NICKNAME").ToString() },
            }
        };
       
        PlayFabClientAPI.UpdateUserData(request, OnDataSend, OnError);
    }

    public void GetProgress()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnDataRecieved, OnError);
    }

    void OnDataRecieved(GetUserDataResult result) //get progress ok
    {
        Invoke(nameof(HidePanel), 2f);
    }

    void OnDataSend(UpdateUserDataResult result) //save progress ok
    {
        Debug.Log("Data sending successful");
        if(loginStatus!=null) loginStatus.text = "";
        HidePanel();
    }

    void HidePanel()
    {
        if (loginStatus != null) loginStatus.text = "";
    }
   
    void atualizarDisplayname(string nome)
    {
        PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = nome
        },
        OnDisplayName => {
            Debug.Log(OnDisplayName.DisplayName + " is your display name");
        },
        errorCallback => {
            Debug.Log(errorCallback.ErrorMessage + " error with display name, possibly in use");
        });
    }

}