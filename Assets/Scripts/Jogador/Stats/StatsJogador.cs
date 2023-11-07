using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsJogador : MonoBehaviour
{

    [SerializeField][HideInInspector] public PlayerController playerController;
    [SerializeField] [HideInInspector] public StatsGeral statsGeral;
    [SerializeField] HudJogador hudJogador;

    // STATS MAXIMO
    [SerializeField] public float fomeMaxima = 100, sedeMaxima = 100, energiaMaxima = 100;

    //STATS CURRENT
    [SerializeField] [HideInInspector] public float fomeAtual, sedeAtual, energiaAtual;

    [SerializeField] float tempoPraDiminuirStatsFomeSedePorSegundos = 60*2, valorDiminuiFomePorTempo = 5, valorDiminuiSedePorTempo = 10;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        statsGeral = GetComponent<StatsGeral>();
    }

    private void Start()
    {
        setarVidaAtual(statsGeral.vidaMaxima);
        setarFomeAtual(fomeMaxima);
        setarSedeAtual(sedeMaxima);
        setarEnergiaAtual(energiaMaxima);
        InvokeRepeating("DiminuirStatsPorTempo", 0, tempoPraDiminuirStatsFomeSedePorSegundos);
    }

    void DiminuirStatsPorTempo()
    {
        setarFomeAtual(fomeAtual - valorDiminuiFomePorTempo);
        setarSedeAtual(sedeAtual - valorDiminuiSedePorTempo);
        if(sedeAtual <= 0 || fomeAtual <= 0)
        {
            statsGeral.TakeDamage(10);
        }
    }


    public void setarVidaAtual(float valor)
    {
        if (valor > statsGeral.vidaMaxima) valor = statsGeral.vidaMaxima;
        if (valor < 0) valor = 0;
        statsGeral.vidaAtual = valor;
        hudJogador.atualizarImgVida(statsGeral.vidaAtual, statsGeral.vidaMaxima);
    }

    public void setarFomeAtual(float valor)
    {
        if (valor > fomeMaxima) valor = fomeMaxima;
        if (valor < 0) valor = 0;
        fomeAtual = valor;
        hudJogador.atualizarImgFome(fomeAtual, fomeMaxima);
    }

    public void setarSedeAtual(float valor)
    {
        if (valor > sedeMaxima) valor = sedeMaxima;
        if (valor < 0) valor = 0;
        sedeAtual = valor;
        hudJogador.atualizarImgSede(sedeAtual, sedeMaxima);
    }

    public void setarEnergiaAtual(float valor)
    {
        if (valor > energiaMaxima) valor = energiaMaxima;
        if (valor < 0) valor = 0;
        energiaAtual = valor;
        hudJogador.atualizarImgEnergia(energiaAtual, energiaMaxima);
    }

    public void AcoesTomouDano()
    {
        playerController.animator.SetTrigger("Hit");
        Debug.Log("player tomou hit. Vida: " + statsGeral.vidaAtual);
    }

    public void AcoesMorreu()
    {
        //TODO: DROPAR MOCHILA
        playerController.animator.SetBool("isDead", true);
        statsGeral.isDead = true;
        playerController.playerMovement.canMove = false;
        setarVidaAtual(0);
    }

    public void AcoesReviveu()
    {
        playerController.animator.SetBool("isDead", false);
        statsGeral.isDead = false;
        playerController.playerMovement.canMove = true;
        playerController.corpoDissecando = null;
        playerController.animalCapturado = null;
        setarVidaAtual(statsGeral.vidaMaxima * 0.20f);
    }

}
