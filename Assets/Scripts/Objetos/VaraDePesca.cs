using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaraDePesca : MonoBehaviour
{

    [SerializeField] public EventsAnimJogador eventsAnimJogador;


	void AnimEventPescou()
	{
		Debug.Log("pescouvara");
		eventsAnimJogador.EventPescou();
	}

}
