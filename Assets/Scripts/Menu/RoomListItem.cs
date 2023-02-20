using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomListItem : MonoBehaviour
{
	[SerializeField] TMP_Text textNameRoom, txQtdPlayer, txPlaceholderPasswordInput;
	[SerializeField] TMP_InputField inputPassword;
	[SerializeField] Image imgPlay;

	public RoomInfo info;

	public void SetUp(RoomInfo _info)
	{
		info = _info;
		textNameRoom.text = _info.Name;
		txQtdPlayer.text = _info.PlayerCount + "/" + _info.MaxPlayers;
		txPlaceholderPasswordInput.text = PlayerPrefs.GetInt("INDEXIDIOMA") == 1 ? "Insira a senha..." : "Insert Password...";
		if(!info.CustomProperties.ContainsKey("secret"))
        {
			inputPassword.gameObject.SetActive(false);
        }
	}

	public void OnClick()
	{
		if ((info.CustomProperties.ContainsKey("secret") && !info.CustomProperties["secret"].Equals(inputPassword.text)) || info.PlayerCount >= info.MaxPlayers)
		{
			imgPlay.color = Color.red;
			Debug.Log("Senha incorreta ou maximo de players");
        }
        else
        {
			Launcher.Instance.JoinRoom(info);
		}
	}

	public void OnChangePasswordInput()
    {
		imgPlay.color = Color.green;
	}

}