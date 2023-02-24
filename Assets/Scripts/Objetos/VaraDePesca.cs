using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaraDePesca : MonoBehaviour
{

    [SerializeField] public PlayerController playerController;


	void AnimEventPescou()
	{
		Debug.Log("pescouvara");
		playerController.EventPescou();
	}

}
