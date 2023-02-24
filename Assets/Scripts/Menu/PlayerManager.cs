using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class PlayerManager : MonoBehaviour
{
	PhotonView PV;

	GameObject controller;
	[HideInInspector] public Transform checkpointAtual;

	void Awake()
	{
		PV = GetComponent<PhotonView>();
	}

	void Start()
	{
		if(PV.IsMine)
		{
			CreateController();
		}
	}

	void CreateController()
	{
		controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"), new Vector3(418.9f, 8.43f, 599.2f), new Quaternion(), 0, new object[] { PV.ViewID });
	}

	public void Die()
	{
		
	}
	
}