using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerListItem : MonoBehaviourPunCallbacks
{
	[SerializeField] TMP_Text text;
	[SerializeField] TMP_Text textScore;
	Player player;

	public void SetUp(Player _player)
	{
		player = _player;
		text.text = player.NickName;
		if(textScore!=null) textScore.text = player.CustomProperties["score"] + " pts";
	}

	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		if(player == otherPlayer)
		{
			Destroy(gameObject);
		}
	}

	public override void OnLeftRoom()
	{
		Destroy(gameObject);
	}
}