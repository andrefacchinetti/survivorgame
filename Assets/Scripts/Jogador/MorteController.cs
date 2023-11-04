using System.Collections;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class MorteController : MonoBehaviour
{
    private StatsGeral statsGeral;
    private PlayerController playerController;
    [SerializeField] GameObject hudMorte;
    [SerializeField] float tempoRespawn = 60.0f; // Tempo em segundos para o respawn
    [SerializeField] TMP_Text textoTempoRestante;
    [SerializeField] Image barraTempo;

    private void Awake()
    {
        statsGeral = GetComponent<StatsGeral>();
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        if (statsGeral.isDead && !hudMorte.activeSelf) // Checa se o jogador está morto e inicia o temporizador
        {
            AttHudJogadorMorreu();
        }
        if (hudMorte.activeSelf && statsGeral.isDead)
        {
            if (Input.GetButtonDown("Use"))
            {
                tempoAtual = 0;
            }
        }
    }

    private void AttHudJogadorMorreu()
    {
        hudMorte.SetActive(true);
        StartCoroutine(TemporizadorRespawn());
    }

    public void RespawnarJogador()
    {
        statsGeral.DroparItensDaMochila();

        if (playerController != null && playerController.gameController != null && playerController.gameController.respawnPointJogador != null)
        {
            this.gameObject.transform.position = playerController.gameController.respawnPointJogador.transform.position;
            reviverJogador();
            Debug.Log("Respawnou jogador na posição: " + playerController.gameController.respawnPointJogador.transform.position);
        }
        else
        {
            Debug.Log("Erro: Ponto de respawn não está configurado corretamente.");
        }
    }

    public void ReanimarJogador()
    {
        StopCoroutine(TemporizadorRespawn());
        reviverJogador();
    }

    private void reviverJogador()
    {
        hudMorte.SetActive(false);
        statsGeral.jogadorStats.AcoesReviveu();
    }

    float tempoAtual;
    IEnumerator TemporizadorRespawn()
    {
        tempoAtual = tempoRespawn;
        while (tempoAtual > 0)
        {
            AtualizarTempo(tempoAtual);
            yield return new WaitForSeconds(1.0f);
            tempoAtual--;
        }
        RespawnarJogador();
        StopAllCoroutines();
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