using UnityEngine;

public class AcendedorFogueira : MonoBehaviour
{
	[SerializeField] public Animator animator;
	public EventsAnimJogador eventsAnimJogador;


	public void IniciarAcendedorFogueira()
	{
		animator.SetTrigger("acendendo"); 
	}

	public void FinalizaAcendedorFogueira()
	{
		animator.SetTrigger("parouAcender");
	}
}
