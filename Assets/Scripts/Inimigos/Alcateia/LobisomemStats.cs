using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobisomemStats : MonoBehaviour
{
    // STATS MAXIMO
    [SerializeField] public float vidaMaxima = 100, damage = 20, nivelAgressividadeMax = 100, nivelAgressividadeAtual = 50;

    //STATS CURRENT
    [SerializeField] [HideInInspector] public float vidaAtual, energiaAtual;

    public bool isDead = false, isEstadoAgressivo = false;
    [HideInInspector] public bool isAttacking; // Flag para controlar se a IA est� atacando

    [SerializeField] LobisomemMovimentacao lobisomemMovimentacao;
    [SerializeField] LobisomemHumanoMovimentacao lobisomemHumanoMovimentacao;
    LobisomemController lobisomemController;


    private void Awake()
    {
        lobisomemController = GetComponent<LobisomemController>();
    }

    private void Start()
    {
        vidaAtual = vidaMaxima;
    }

	public void TakeDamage(float damage)
	{
        vidaAtual -= damage;
        AumentarNivelAgressividade(20);
        Debug.Log("Vida enemy: " + vidaAtual + " Selvageria: " + nivelAgressividadeAtual);
        if(vidaAtual <= 0)
        {
            if (lobisomemController.forma.Equals(LobisomemController.Forma.Lobo))
            {
                if (lobisomemController.categoria.Equals(LobisomemController.Categoria.Beta)) lobisomemMovimentacao.ComandosBetasParaAlfa();
                lobisomemMovimentacao.animator.SetBool("isDead", true);
                lobisomemMovimentacao.agent.isStopped = true;
            }
            else
            {
                if (lobisomemController.categoria.Equals(LobisomemController.Categoria.Beta)) lobisomemHumanoMovimentacao.ComandosBetasParaAlfa();
                lobisomemHumanoMovimentacao.animator.SetBool("isDead", true);
                lobisomemHumanoMovimentacao.agent.isStopped = true;
            }
            isDead = true;
        }
        else
        {
            if (lobisomemController.forma.Equals(LobisomemController.Forma.Lobo))
            {
                lobisomemMovimentacao.animator.SetTrigger("hit");
            }
            else
            {
                lobisomemHumanoMovimentacao.animator.SetTrigger("hit");
            }
        }
	}

    public void AumentarNivelAgressividade(float valor)
    {
        nivelAgressividadeAtual += valor;
        if (nivelAgressividadeAtual > nivelAgressividadeMax) nivelAgressividadeAtual = nivelAgressividadeMax;
        if (nivelAgressividadeAtual < 0) nivelAgressividadeAtual = 0;
        Debug.Log("Lobisomem ficou mais agressivo: " + nivelAgressividadeAtual);
        setarEstadoAgressividade();
        if (lobisomemController.categoria.Equals(LobisomemController.Categoria.Alfa)) lobisomemMovimentacao.ComandosAlfaParaBetas();
        else if (lobisomemController.categoria.Equals(LobisomemController.Categoria.Beta)) lobisomemMovimentacao.ComandosBetasParaAlfa();
    }

    public void DiminuirNivelAgressividade(float valor)
    {
        nivelAgressividadeAtual -= valor;
        if (nivelAgressividadeAtual > nivelAgressividadeMax) nivelAgressividadeAtual = nivelAgressividadeMax;
        if (nivelAgressividadeAtual < 0) nivelAgressividadeAtual = 0;
        Debug.Log("Lobisomem ficou menos agressivo: " + nivelAgressividadeAtual);
        setarEstadoAgressividade();
    }

    private void setarEstadoAgressividade()
    {
        if (!LobisomemController.Categoria.Beta.Equals(lobisomemController.categoria))
        {
            if (nivelAgressividadeAtual > 50)
            {
                isEstadoAgressivo = true;
            }
            else
            {
                isEstadoAgressivo = false;
                lobisomemMovimentacao.target = null;
            }
        }
    }

    public void VerificarSePlayerEstaArmado(GameObject playerObj)
    {
        if(playerObj.GetComponent<PlayerController>().inventario.itemNaMao != null && playerObj.GetComponent<PlayerController>().inventario.itemNaMao.nomeItem.GetTipoItemEnum().Equals(Item.TiposItems.Arma.ToString()))
        {
            AumentarNivelAgressividade(10);
        }
    }

}
