using Opsive.UltimateCharacterController.Traits;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Opsive.Shared.Events;

public class StatsJogador : MonoBehaviour
{

    [SerializeField] [HideInInspector] public PlayerController playerController;
    [SerializeField] [HideInInspector] public CharacterAttributeManager characterAttributeManager;
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
        characterAttributeManager = GetComponent<CharacterAttributeManager>();
    }

    private void Start()
    {
        AtualizarImgVida();
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
            TakeDamageHealth(10);
        }
    }


    public void TakeDamageHealth(float value)
    {
        playerController.characterHealth.Damage(value);
        AtualizarImgVida();
    }

    public void TakeHealHealth(float value)
    {
        playerController.characterHealth.Heal(value);
        AtualizarImgVida();
    }

    public void AtualizarImgVida()
    {
        hudJogador.atualizarImgVida(playerController.characterHealth.HealthValue, ObterVidaMaximaHealth());
    }

    public float ObterVidaMaximaHealth()
    {
        return playerController.characterAttributeManager.GetAttribute("Health").MaxValue;
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

    public void AcoesMorreu()
    {
        //TODO: DROPAR MOCHILA
        playerController.canMove = false;
    }

    public void AcoesReviveu()
    {
        playerController.canMove = true;
        playerController.corpoDissecando = null;
        playerController.animalCapturado = null;
        TakeHealHealth(playerController.characterAttributeManager.GetAttribute(playerController.characterHealth.HealthAttributeName).MaxValue * 0.20f); //CURA 20% DA VIDA MAXIMA
    }

}
