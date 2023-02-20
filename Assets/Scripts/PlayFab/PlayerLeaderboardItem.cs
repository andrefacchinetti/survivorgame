using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class PlayerLeaderboardItem : MonoBehaviour
{
	[SerializeField] TMP_Text txNickname, txScore, txPosition;
	[SerializeField] GameObject imgTrofeu, imgRelogio, imgCoroa;

	public void SetUp(string name, string score, int position, string keyImg)
	{
		txNickname.text = name;
		txScore.text = score;
		txPosition.text = position + "º";
		if(keyImg == "trofeu")
        {
			imgTrofeu.SetActive(true);
			imgRelogio.SetActive(false);
			imgCoroa.SetActive(false);
		} else if (keyImg == "relogio")
		{
			imgTrofeu.SetActive(false);
			imgRelogio.SetActive(true);
			imgCoroa.SetActive(false);
		} else if (keyImg == "coroa")
		{
			imgTrofeu.SetActive(false);
			imgRelogio.SetActive(false);
			imgCoroa.SetActive(true);
		}

	}

}
