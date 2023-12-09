using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaraDePesca : MonoBehaviour
{

	[SerializeField] public Animator animator;
	[SerializeField] public GameObject peixeDaVara;

	void AnimEventPescou()
	{
		Debug.Log("pescouvara");
		GetComponentInParent<EventsAnimJogador>().EventPescou();
	}

}
