using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobisomemStats : MonoBehaviour
{
    // STATS MAXIMO
    [SerializeField] public float nivelAgressividadeMax = 100, nivelAgressividadeAtual = 50;

    //STATS CURRENT
    [SerializeField] [HideInInspector] public float energiaAtual;

    public bool isEstadoAgressivo = false, isSubindoNaArvore = false, isIndoAteArvore = false;

    //ATAQUE
    [SerializeField] public float distanciaDePerseguicao = 10f, distanciaDeAtaque = 2f;
    [SerializeField] public float attackInterval = 1f; // Intervalo de tempo entre ataques
    [HideInInspector] public float lastAttackTime; // Tempo do �ltimo ataque
    [SerializeField] public float destinationOffset = 1f;
    [SerializeField] public float walkSpeed = 0.8f, runSpeed = 1.5f, speedVariation = 0.5f;
    [SerializeField] public float leadTime = 1.2f, leadDistance = 2, tempoMudancaDeTurnoProfissoes = 300;
    LobisomemController lobisomemController;
    StatsGeral statsGeral;


    private void Awake()
    {
        lobisomemController = GetComponentInParent<LobisomemController>();
        statsGeral = GetComponent<StatsGeral>();
    }

	public void AcoesTomouDano()
	{
        AumentarNivelAgressividade(20);
        lobisomemController.lobisomemMovimentacao.animator.SetTrigger("hit");
        lobisomemController.lobisomemHumanoMovimentacao.animator.SetTrigger("hit");
        Debug.Log("Vida enemy: " + statsGeral.vidaAtual + " Selvageria: " + nivelAgressividadeAtual);
    }

    public void AcoesMorreu()
    {
        if (lobisomemController.statsGeral.isDead) return;
        AumentarNivelAgressividade(20);
        if (lobisomemController.categoria.Equals(LobisomemController.Categoria.Beta))
        {
            lobisomemController.lobisomemMovimentacao.ComandosBetasParaAlfa();
            lobisomemController.lobisomemHumanoMovimentacao.ComandosBetasParaAlfa();
        }
        if (lobisomemController.lobisomemMovimentacao.isActiveAndEnabled)
        {
            lobisomemController.lobisomemMovimentacao.animator.SetBool("isDead", true);
            lobisomemController.lobisomemMovimentacao.agent.isStopped = true;
            lobisomemController.lobisomemMovimentacao.agent.speed = 0;
        }
        if (lobisomemController.lobisomemHumanoMovimentacao.isActiveAndEnabled)
        {
            lobisomemController.lobisomemHumanoMovimentacao.animator.SetBool("isDead", true);
            lobisomemController.lobisomemHumanoMovimentacao.agent.isStopped = true;
            lobisomemController.lobisomemHumanoMovimentacao.agent.speed = 0;
        }
        
        statsGeral.isDead = true;
        Debug.Log("Lobisomen morreu");
    }

    public void AumentarNivelAgressividade(float valor)
    {
        nivelAgressividadeAtual += valor;
        if (nivelAgressividadeAtual > nivelAgressividadeMax) nivelAgressividadeAtual = nivelAgressividadeMax;
        if (nivelAgressividadeAtual < 0) nivelAgressividadeAtual = 0;
        Debug.Log("Lobisomem ficou mais agressivo: " + nivelAgressividadeAtual);
        setarEstadoAgressividade();
        if (lobisomemController.categoria.Equals(LobisomemController.Categoria.Alfa))
        {
            lobisomemController.lobisomemMovimentacao.ComandosAlfaParaBetas();
            lobisomemController.lobisomemHumanoMovimentacao.ComandosAlfaParaBetas();
        }
        else if (lobisomemController.categoria.Equals(LobisomemController.Categoria.Beta))
        {
            lobisomemController.lobisomemMovimentacao.ComandosBetasParaAlfa();
            lobisomemController.lobisomemHumanoMovimentacao.ComandosBetasParaAlfa();
        }
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
                lobisomemController.lobisomemMovimentacao.targetInimigo = null;
                lobisomemController.lobisomemHumanoMovimentacao.targetInimigo = null;
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
