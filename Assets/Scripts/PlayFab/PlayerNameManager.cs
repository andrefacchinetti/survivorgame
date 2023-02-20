using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerNameManager : MonoBehaviour
{
	[SerializeField] TMP_Text usernameText;

	void Start()
	{
		if(PlayerPrefs.HasKey("NICKNAME"))
		{
			usernameText.text = PlayerPrefs.GetString("NICKNAME");
			PhotonNetwork.NickName = PlayerPrefs.GetString("NICKNAME");
		}
		else
		{
			usernameText.text = "Player " + Random.Range(0, 10000).ToString("0000");
		}
	}

}
