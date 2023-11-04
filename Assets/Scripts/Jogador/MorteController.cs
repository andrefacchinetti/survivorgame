using System.Collections;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class MorteController : MonoBehaviour
{
    private StatsGeral statsGeral;
    [SerializeField] GameObject hudMorte;
    [SerializeField] float tempoRespawn = 60.0f; // Tempo em segundos para o respawn
    [SerializeField] TMP_Text textoTempoRestante;
    [SerializeField] Image barraTempo;

    private void Awake()
    {
        statsGeral = GetComponent<StatsGeral>();
    }

    void Update()
    {
        if (statsGeral.isDead && !hudMorte.activeSelf) // Checa se o jogador está morto e inicia o temporizador
        {
            AttHudJogadorMorreu();
        }
    }

    private void AttHudJogadorMorreu()
    {
        hudMorte.SetActive(true);
        StartCoroutine(TemporizadorRespawn());
    }

    public void RespawnarJogador()
    {
        // Implemente as ações de respawn do jogador
        reviverJogador();
    }

    public void ReanimarJogador()
    {
        reviverJogador();
    }

    private void reviverJogador()
    {
        hudMorte.SetActive(false);
        statsGeral.jogadorStats.AcoesReviveu();
    }

    IEnumerator TemporizadorRespawn()
    {
        float tempoAtual = tempoRespawn;
        while (tempoAtual > 0)
        {
            AtualizarTempo(tempoAtual);
            yield return new WaitForSeconds(1.0f);
            tempoAtual--;
        }
        RespawnarJogador();
    }

    public void AtualizarTempo(float tempoRestante) // Atualiza a HUD com o tempo restante
    {
        if (textoTempoRestante != null)
        {
            AtualizarBarraTempo(tempoRestante);
            textoTempoRestante.text = "Tempo restante: " + tempoRestante.ToString("F0"); // Atualiza o texto UI
        }
    }

    void AtualizarBarraTempo(float tempoRestante)
    {
        float fillAmount = tempoRestante / tempoRespawn; // Calcula a porcentagem preenchida da barra
        barraTempo.fillAmount = fillAmount; // Atualiza a largura da barra de acordo com a porcentagem de preenchimento
    }

}