using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.IO;

public class RoomManager : MonoBehaviourPunCallbacks
{

	protected static RoomManager s_instance;

	protected static RoomManager Instance
	{
		get
		{
			if (s_instance == null)
			{
				return new GameObject("RoomManager").AddComponent<RoomManager>();
			}
			else
			{
				return s_instance;
			}
		}
	}

	void Awake()
	{
		if (s_instance != null)
		{
			Destroy(gameObject);
			return;
		}
		s_instance = this;
		DontDestroyOnLoad(gameObject);
	}

	public override void OnEnable()
	{
		base.OnEnable();
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	public override void OnDisable()
	{
		base.OnDisable();
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
	{
		Cursor.lockState = CursorLockMode.Confined;
		Cursor.visible = true;
		if (scene.name.Contains("Game")) // We're in the game scene
		{
			Destroy(GameObject.Find("PlayerManager"));
			PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager"), Vector3.zero, Quaternion.identity);
		}
        else
        {
			if (scene.name == "Menu")
            {
				Debug.Log("Chegou no menu");
				if (PhotonNetwork.InRoom && PhotonNetwork.IsMasterClient)
                {
					PhotonNetwork.CurrentRoom.IsVisible = true;
					PhotonNetwork.CurrentRoom.IsOpen = true;
					Debug.Log("Abriu a sala para os players entrar");
				}
			}
		}
	}
}