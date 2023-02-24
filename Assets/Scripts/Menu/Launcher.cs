using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System.Linq;
using UnityEngine.SceneManagement;
using ExitGames.Client.Photon;
using UnityEngine.UI;

public class Launcher : MonoBehaviourPunCallbacks
{
	public static Launcher Instance;

	[SerializeField] bool MODODEBUG = false;
	[SerializeField] TMP_InputField roomNameInputField, roomNameFiltro, roomPasswordInputField;
	[SerializeField] TMP_Text errorText, txQtdPlayersMatchmaking;
	[SerializeField] TMP_Text roomNameText, roomMaxPlayersText;
	[SerializeField] Transform roomListContent;
	[SerializeField] GameObject roomListItemPrefab;
	[SerializeField] Transform playerListContent, playerListContentInGame;
	[SerializeField] GameObject PlayerListItemPrefab, PlayerListItemPrefabInGame;
	[SerializeField] GameObject startGameButton, playCompetitiveGameButton, unavailableCompetitiveGameButton, voltarPraSalaButton, objMenuFindRoom;
	private bool isStartou = false, modoCompetitivo = false;
	private byte maxPlayers = 6;

	private List<RoomInfo> roomListCache;
	public bool isSalaPrivada;
	[HideInInspector] public SaveManager sm;
	private static string GAME_MODE = "GAMEMODE", MODE_FUN = "FUN", MODE_COMPETITIVE = "COMPETITIVE";

	void Awake()
	{
		Instance = this;
	}
	void Start()
	{
		if (SceneManager.GetActiveScene().name.Contains("Game"))
		{
			return;
		}
		Debug.Log("Start - Offline mode: " + PhotonNetwork.OfflineMode);
		if (!PhotonNetwork.IsConnected)
		{
			string chaveRegion = ObterChaveRegionPorIndex();
			Debug.Log("Connecting to Master: " + chaveRegion);
			AppSettings settings = new AppSettings();
			settings.AppIdRealtime = "38d0d89f-1b27-49fc-8b57-947e6cdd6148";
			settings.AppVersion = "1.0";
			settings.Protocol = ConnectionProtocol.Udp;
			settings.FixedRegion = chaveRegion;
			settings.UseNameServer = true;
			PhotonNetwork.ConnectUsingSettings(settings); //sem setar as settings o photon usa o arquivo ServerPhotonSettings como padrao, e setando as settings, precisa preencher a entidade
        }
        else
        {
			if (PhotonNetwork.InRoom)
			{
				MenuManager.Instance.OpenMenu("room");
				AtualizarPlayersInRoom();
			}
			else
			{
				MenuManager.Instance.OpenMenu("title");
			}
		}
		isStartou = false;
	}

    private void LateUpdate()
    {
		if (voltarPraSalaButton != null && PhotonNetwork.CurrentLobby.Name == null)
		{
			voltarPraSalaButton.GetComponent<Button>().interactable = PhotonNetwork.IsMasterClient;
		}
	}

    public void AtualizarPlayersInRoom()
    {
		Player[] players = PhotonNetwork.PlayerList;
		txQtdPlayersMatchmaking.text = players.Length + "/"+PhotonNetwork.CurrentRoom.MaxPlayers;
		Debug.Log("Instanciando player list item: " + players.Length);
		if (playerListContent != null)
		{
			foreach (Transform child in playerListContent)
			{
				Destroy(child.gameObject);
			}

			if (PlayerListItemPrefab != null)
			{
				for (int i = 0; i < players.Count(); i++)
				{
					Instantiate(PlayerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
				}
			}
		}
		if (roomNameText != null) roomNameText.text = PhotonNetwork.CurrentRoom.Name;
		if (startGameButton != null) {
			startGameButton.SetActive(PhotonNetwork.IsMasterClient);
			startGameButton.GetComponent<Button>().interactable = (PhotonNetwork.CurrentRoom.PlayerCount >= 3 || MODODEBUG);
		}
	}

	private string ObterChaveRegionPorIndex()
    {
		if (PlayerPrefs.GetInt("INDEXSERVERREGION") == 0) //USA, East
		{
			return "us";
		}
		else if (PlayerPrefs.GetInt("INDEXSERVERREGION") == 1) //South America
		{
			return "sa";
		}
		else if (PlayerPrefs.GetInt("INDEXSERVERREGION") == 2) //Europe
		{
			return "eu";
		}
		else if (PlayerPrefs.GetInt("INDEXSERVERREGION") == 3) //Asia
		{
			return "asia";
		}
		else if (PlayerPrefs.GetInt("INDEXSERVERREGION") == 4) //Australia
		{
			return "au";
		}
		else if (PlayerPrefs.GetInt("INDEXSERVERREGION") == 5) //Japan
		{
			return "jp";
		}
		else
		{
			return "us"; ////USA, East
		}
	}

	public override void OnConnectedToMaster()
	{
		Debug.Log("Connected to Master: "+PhotonNetwork.CloudRegion);
		if (!PhotonNetwork.InLobby)
        {
			PhotonNetwork.JoinLobby(TypedLobby.Random);
			PhotonNetwork.AutomaticallySyncScene = true;
		}
	}

    public override void OnJoinedLobby() //Apos conectado no photon, entra no lobby e abre o menu online
	{
		if (SceneManager.GetActiveScene().name.Contains("Game")) return;
		PhotonNetwork.DestroyAll(false);
        if (PhotonNetwork.CurrentLobby.IsDefault)
        {
			MenuManager.Instance.OpenMenu("find room");
		}
        else
        {
			MenuManager.Instance.OpenMenu("title");
		}
		Debug.Log("Joined Lobby "+ PhotonNetwork.CurrentLobby.Name);
	}

	public void CreateFunGameRoom() //CRIANDO SALA FUN
    {
		modoCompetitivo = false;
		RoomOptions roomOptions = CriarRoomOptions(maxPlayers);
		Debug.Log("Sala FUN criada max players: " + maxPlayers);
		ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable();
		customProperties.Add(GAME_MODE, MODE_FUN);
		roomOptions.CustomRoomPropertiesForLobby = new string[] { "GAME_MODE" };
		if (!string.IsNullOrEmpty(roomPasswordInputField.text))
        {
			customProperties.Add("secret", roomPasswordInputField.text);
			roomOptions.CustomRoomPropertiesForLobby = new string[] { GAME_MODE, "secret" };
		}
		roomOptions.CustomRoomProperties = customProperties;

		CreateRoom(roomOptions, TypedLobby.Default);
		MenuManager.Instance.OpenMenu("loading");
	}
	

	public void CreateCompetitiveRoom() //CRIANDO SALA COMPETITIVO
	{
		modoCompetitivo = true;
		RoomOptions roomOptions = CriarRoomOptions(maxPlayers);
		ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable();
		customProperties.Add(GAME_MODE, MODE_COMPETITIVE);
		roomOptions.CustomRoomPropertiesForLobby = new string[] { GAME_MODE };
		roomOptions.CustomRoomProperties = customProperties;
		CreateRoom(roomOptions, TypedLobby.Random);
		MenuManager.Instance.OpenMenu("findmatch");
	}

	public void CreateRoom(RoomOptions roomOptions, TypedLobby typedLobby) //USADO PARA FUN E COMPETITIVO
	{
		if (SceneManager.GetActiveScene().name.Contains("Game")) return;
		if (string.IsNullOrEmpty(roomNameInputField.text))
		{
			roomNameInputField.text = PhotonNetwork.NickName + "'s room - " +Random.Range(0, 100000);
		}
		Debug.Log("Criando room");
		PhotonNetwork.CreateRoom(roomNameInputField.text, roomOptions, typedLobby);
	}

	public void JoinOrCreateRoom() //NAO TA SENDO USADO
	{
		if (SceneManager.GetActiveScene().name.Contains("Game")) return;
		//PhotonNetwork.NickName = PlayerPrefs.GetString("NICKNAME") != null && PlayerPrefs.GetString("NICKNAME") != "" ? PlayerPrefs.GetString("NICKNAME") : "Player " + Random.Range(0, 1000).ToString("0000");
		if (string.IsNullOrEmpty(roomNameInputField.text))
		{
			roomNameInputField.text = PhotonNetwork.NickName + "'s room -" + Random.Range(0, 100000);
		}
		
		PhotonNetwork.JoinOrCreateRoom("Training", CriarRoomOptions(1), TypedLobby.Default);
		MenuManager.Instance.OpenMenu("loading");
	}

	public override void OnCreatedRoom()
    {
		Debug.Log("Sala criada: "+ PhotonNetwork.CurrentRoom.CustomProperties[GAME_MODE] + " Secret: "+ PhotonNetwork.CurrentRoom.CustomProperties["secret"]); 
		if (!modoCompetitivo)
        {
			MenuManager.Instance.OpenMenu("room");
			AtualizarPlayersInRoom();
		}
    }

	public override void OnJoinedRoom()
	{
		if (SceneManager.GetActiveScene().name.Contains("Game")) return;
        if (PhotonNetwork.OfflineMode)
        {
			Debug.Log("Startando game no modo offline");
			StartGame();
        }
        else
        {
			if (!SceneManager.GetActiveScene().name.Contains("Game"))
            {
				txQtdPlayersMatchmaking.text = PhotonNetwork.CurrentRoom.PlayerCount + "/" + PhotonNetwork.CurrentRoom.MaxPlayers;
				if (MODE_FUN.Equals(PhotonNetwork.CurrentRoom.CustomProperties[GAME_MODE]))
				{
					MenuManager.Instance.OpenMenu("room");
					AtualizarPlayersInRoom();
				}
				else
				{
					MenuManager.Instance.OpenMenu("findmatch");
				}
			}
		}
	}

	public override void OnMasterClientSwitched(Player newMasterClient)
	{
		if(startGameButton != null)
        {
			startGameButton.SetActive(PhotonNetwork.IsMasterClient);
		}
		if(voltarPraSalaButton != null && PhotonNetwork.InLobby && PhotonNetwork.CurrentLobby.Name == null)
        {
			voltarPraSalaButton.GetComponent<Button>().interactable = PhotonNetwork.IsMasterClient;
		}
	}

	public override void OnCreateRoomFailed(short returnCode, string message)
	{
		if (SceneManager.GetActiveScene().name.Contains("Game")) return;
		errorText.text = "Room Creation Failed: " + message;
		Debug.LogError("Room Creation Failed: " + message);
		MenuManager.Instance.OpenMenu("error");
	}

	public void StartGame()
	{
		if (!PhotonNetwork.IsMasterClient) return;
		if (SceneManager.GetActiveScene().name.Contains("Game")) return;
		if(!MODODEBUG && PhotonNetwork.CurrentRoom.PlayerCount < 3 )
        {
			Debug.Log("Necessário 3 jogadores");
		}
        else
        {
			Debug.Log("Iniciando partida");
			if (isStartou) return;
			MenuManager.Instance.OpenMenu("loading");
			isStartou = true;
			PhotonNetwork.CurrentRoom.IsVisible = false;
			PhotonNetwork.CurrentRoom.IsOpen = false;
			PhotonNetwork.LoadLevel("Game1");
		}
	}

	public void LeaveRoom()
	{
		if (SceneManager.GetActiveScene().name.Contains("Game"))
		{
			if (PhotonNetwork.InRoom)
			{
				PhotonNetwork.LeaveRoom();
			}
			else
			{
				PhotonNetwork.LoadLevel("Menu");
			}
		}
        else
        {
            if (PhotonNetwork.InRoom)
            {
				PhotonNetwork.LeaveRoom();
				MenuManager.Instance.OpenMenu("loading");
			}
            else
            {
				PhotonNetwork.LoadLevel("Menu");
			}
		}
	}

	public void LeaveRoomFunMode()
	{
		PhotonNetwork.LeaveRoom();
		if (SceneManager.GetActiveScene().name.Contains("Game")) return;
		MenuManager.Instance.OpenMenu("loading");
	}

	public void JoinRoom(RoomInfo info)
	{
		PhotonNetwork.JoinRoom(info.Name);
		if (SceneManager.GetActiveScene().name.Contains("Game")) return;
		MenuManager.Instance.OpenMenu("loading");
	}

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
		MenuManager.Instance.OpenMenu("find room");
		base.OnJoinRoomFailed(returnCode, message);
    }

    public override void OnLeftRoom()
	{
		Debug.Log("Saiu da room");
		if (SceneManager.GetActiveScene().name == "Menu")
        {
			MenuManager.Instance.OpenMenu("title");
		}
        else
        {
			PhotonNetwork.LoadLevel("Menu");
		}

		base.OnLeftRoom();
	}

	public override void OnRoomListUpdate(List<RoomInfo> roomList)
	{
		if (SceneManager.GetActiveScene().name.Contains("Game")) return;
		if (roomListContent == null) return;
		Debug.Log("Atualizando lista de servidores");

		foreach (Transform trans in roomListContent)
		{
			Destroy(trans.gameObject);
		}

		for (int i = 0; i < roomList.Count; i++)
		{
			if (roomList[i].RemovedFromList)
				continue;

			Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
		}
		roomListCache = roomList;
	}

	public void filtrarRoomList()
	{
		foreach (Transform trans in roomListContent)
		{
			Destroy(trans.gameObject);
		}

		for (int i = 0; i < roomListCache.Count; i++)
		{
			if (roomNameFiltro.text == null || roomNameFiltro.text == "" || roomListCache[i].Name.Contains(roomNameFiltro.text)) //filtro sala por nome
			{
				Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomListCache[i]);
			}
		}
	}

	public void BackFunGameMode()
    {
		PhotonNetwork.JoinLobby(TypedLobby.Random);
	}
	
	public void StartFunGame() //FUN GAME: Modo de jogar com amigos
    {
		modoCompetitivo = false;
		PhotonNetwork.JoinLobby(TypedLobby.Default);
	}

	public void FindMatchMaking1() //MODO COMPETITIVO: 
    {
		modoCompetitivo = true;
		maxPlayers = 6;
		FindMatchmaking();
	}

	private void FindMatchmaking() //Deixar sala nao visivel
    {
		Debug.Log("Finding game matchmaking");
		MenuManager.Instance.OpenMenu("loading");
		PhotonNetwork.JoinRandomRoom(TypedLobby.Random);
	}

	private RoomOptions CriarRoomOptions(byte maxPlayers)
	{
		RoomOptions roomOptions = new RoomOptions();
		roomOptions.IsOpen = true;
		roomOptions.IsVisible = true; //se botar false nao encontra nem no find do matchmaking
		roomOptions.MaxPlayers = maxPlayers;
		roomOptions.PublishUserId = true;
		roomOptions.CleanupCacheOnLeave = true;
		roomOptions.PlayerTtl = 1000; //verificar isso, qual o melhor pr
		return roomOptions;
	}

	public void StopSearch()
    {
		PhotonNetwork.LeaveRoom();
    }

	public override void OnJoinRandomFailed(short returneCode, string message) //NAO ENCONTROU UMA SALA COMPETITIVE
    {
		Debug.Log("Nao encontrou nenhuma sala, criando uma...");
		CreateCompetitiveRoom();
	}

	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		Debug.Log("O player " + newPlayer.NickName + " entrou na sala");
		if (!SceneManager.GetActiveScene().name.Contains("Game"))
		{
			AtualizarPlayersInRoom();
			if (!PhotonNetwork.CurrentLobby.IsDefault)
			{
				//TODO: colocar contador pro masterclient, e se for maior que 2 min e tiver 3 players, startar
				if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers && PhotonNetwork.IsMasterClient)
				{
					Debug.Log("A sala tem o maximo de players, iniciando game...");
					StartGame();
				}
			}
		}
	}

	public override void OnLeftLobby()
	{
		Debug.Log("Saiu do Lobby");
		foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
		{
			if (player.GetComponent<PhotonView>().IsMine)
			{
				PhotonNetwork.Destroy(player);
			}
		}
		if (PhotonNetwork.IsConnected)
		{
			PhotonNetwork.Disconnect();
		}
		PhotonNetwork.LoadLevel(0);
		base.OnLeftLobby();
	}

	public override void OnPlayerLeftRoom(Player playerLeft)
	{
		Debug.Log("Player Disconnected left room: " + playerLeft.NickName + " in scene " + SceneManager.GetActiveScene().name);
		if (!SceneManager.GetActiveScene().name.Contains("Game"))
		{
			AtualizarPlayersInRoom();
		}
	}

	public void AtualizarListaDeSalas()
    {
		PhotonNetwork.JoinLobby(TypedLobby.Default);
	}

	public void VoltarAoMenuPrincipal()
	{
		if (PhotonNetwork.IsConnected)
		{
			PhotonNetwork.Disconnect();
		}
		SceneManager.LoadScene("MenuPrincipal");
	}

	public void VoltarAoMenu()
	{
		SceneManager.LoadScene("Menu");
	}

	public void SairTelaFimDeJogo()
    {
        if (PhotonNetwork.InRoom)
        {
			LeaveRoom();
        }
        else
        {
			PhotonNetwork.LoadLevel("Menu");
		}
	}

	public void SairDoGame()
    {
		LeaveRoom();
	}

	public void VoltarPraSala()
    {
		SceneManager.LoadScene("Menu");
    }

	public void MasterLevaTodoMundoPraSala()
    {
        if (PhotonNetwork.IsMasterClient)
        {
			PhotonNetwork.LoadLevel("Menu");
		}
	}

	public void PlayAgain()
    {
		if(PhotonNetwork.InRoom) 
        {
			PhotonNetwork.LoadLevel("GAME0");
        }
    }

	public void SairDoJogo()
	{
		Application.Quit();
	}

	public void setMaxPlayersSlider(float value)
	{
		maxPlayers = (byte)((int)value);
		roomMaxPlayersText.text = maxPlayers+"";
		Debug.Log("Maxplayers: " + maxPlayers);
	}

}