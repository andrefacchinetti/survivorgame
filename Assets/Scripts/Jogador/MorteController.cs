using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MorteController : MonoBehaviour
{
    private StatsGeral statsGeral;
    private PlayerController playerController;
    [SerializeField] GameObject hudMorte;
    [SerializeField] float tempoRespawn = 60.0f; // Tempo em segundos para o respawn
    [SerializeField] TMP_Text textoTempoRestante;
    [SerializeField] Image barraTempo;

    private float tempoAtual;

    private void Awake()
    {
        statsGeral = GetComponent<StatsGeral>();
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        if (!playerController.characterHealth.IsAlive()) // Checa se o jogador está morto e inicia o temporizador
        {
            if (!hudMorte.activeSelf)
            {
                AttHudJogadorMorreu();
            }
            else
            {
                if (Input.GetButtonDown("Use"))
                {
                    tempoAtual = 0;
                    CancelInvoke("DecrementarTempo");
                    RespawnarJogador();
                }
            }
        }
    }

    private void AttHudJogadorMorreu()
    {
        hudMorte.SetActive(true);
        tempoAtual = tempoRespawn;
        AtualizarTempo(tempoAtual);
        InvokeRepeating("DecrementarTempo", 1.0f, 1.0f);
    }

    private void DecrementarTempo()
    {
        if (tempoAtual > 0)
        {
            tempoAtual--;
            AtualizarTempo(tempoAtual);
        }
        else
        {
            CancelInvoke("DecrementarTempo");
            RespawnarJogador();
        }
    }

    public void RespawnarJogador()
    {
        statsGeral.DroparItensDaMochila();

        if (playerController != null && playerController.gameController != null && playerController.gameController.respawnPointJogador != null)
        {
            Transform destinoRespawn = playerController.gameController.respawnPointJogador.transform;
            playerController.characterLocomotion.SetPositionAndRotation(destinoRespawn.position, destinoRespawn.rotation, false, false);
            reviverJogador();
        }
        else
        {
            Debug.LogError("Erro: Ponto de respawn não está configurado corretamente.");
        }
    }

    public void ReanimarJogador()
    {
        CancelInvoke("DecrementarTempo");
        reviverJogador();
    }

    private void reviverJogador()
    {
        hudMorte.SetActive(false);
        statsGeral.jogadorStats.AcoesReviveu();
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
