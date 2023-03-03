using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobisomemStats : MonoBehaviour
{
    // STATS MAXIMO
    [SerializeField] public float vidaMaxima = 100, energiaMaxima = 100, damage = 20, nivelSelvageriaMax = 100, nivelSelvageriaAtual = 50;

    //STATS CURRENT
    [SerializeField] [HideInInspector] public float vidaAtual, energiaAtual;

    public float consumoEnergiaPorSegundo = 5.0f;
    public float recuperacaoEnergiaPorSegundo = 2.0f;
    public bool isDead = false, isEstadoAgressivo = false;

    LobisomemMovimentacao lobisomemMovimentacao;


    private void Awake()
    {
        vidaAtual = vidaMaxima;
        lobisomemMovimentacao = GetComponent<LobisomemMovimentacao>();
    }

    private void Start()
    {
        setarEnergiaAtual(energiaMaxima);
    }

    public void setarEnergiaAtual(float valor)
    {
        if (valor > energiaMaxima) valor = energiaMaxima;
        if (valor < 0) valor = 0;
        energiaAtual = valor;
    }

	public void TakeDamage(float damage)
	{
        vidaAtual -= damage;
        nivelSelvageriaAtual += 20;
        lobisomemMovimentacao.setarEstadoAgressividade();
        lobisomemMovimentacao.ComandosAlfaParaBetas();
        Debug.Log("Vida enemy: " + vidaAtual + " Selvageria: "+nivelSelvageriaAtual);
        if(vidaAtual <= 0)
        {
            lobisomemMovimentacao.animator.SetBool("isDead", true);
            lobisomemMovimentacao.agent.isStopped = true;
            isDead = true;
        }
        else
        {
            lobisomemMovimentacao.animator.SetTrigger("hit");
        }
	}

}
