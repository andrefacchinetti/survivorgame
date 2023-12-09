using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaraDePesca : MonoBehaviour
{

	[SerializeField] public Animator animator;
	[SerializeField] public GameObject peixeDaVara;
	public EventsAnimJogador eventsAnimJogador;

	void AnimEventPescou()
	{
		peixeDaVara.SetActive(false);
		if (eventsAnimJogador != null)
        {
			eventsAnimJogador.TerminouDePescarComSucesso();
		}
	}

	public void IniciarPesca()
    {
		animator.SetTrigger("pescando");
		peixeDaVara.SetActive(false);
	}

	public void FinalizarPesca()
    {
		animator.SetTrigger("parouDePescar");
	}

}
