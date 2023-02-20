using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class MenuManager : MonoBehaviourPunCallbacks
{
	public static MenuManager Instance;

	[SerializeField] Menu[] menus;

	void Awake()
	{
		Instance = this;
	}

	public void OpenMenu(string menuName)
	{
		for(int i = 0; i < menus.Length; i++)
		{
			if(menus[i].menuName == menuName)
			{
				menus[i].Open();
			}
			else if(menus[i].open)
			{
				CloseMenu(menus[i]);
			}
		}
	}

	public void OpenMenu(Menu menu)
	{
		for(int i = 0; i < menus.Length; i++)
		{
			if(menus[i].open)
			{
				CloseMenu(menus[i]);
			}
		}
		menu.Open();
	}

	public void CloseMenu(Menu menu)
	{
		menu.Close();
	}
	public void LoadCenaMenu()
    {
		Destroy(GameObject.FindGameObjectWithTag("Player"));
		Destroy(GameObject.Find("RoomManager"));
		Destroy(GameObject.Find("PhotonVoiceNetwork singleton"));
		SceneManager.LoadScene("Menu");
    }
	public void LoadCenaLoja()
	{
		SceneManager.LoadScene("LojaCash");
	}

}