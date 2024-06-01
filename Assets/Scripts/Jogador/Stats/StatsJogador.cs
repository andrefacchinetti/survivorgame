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
    [SerializeField] public float fomeAtual, sedeAtual, energiaAtual;

    [SerializeField] public float tempoPraDiminuirStatsFomeSedePorSegundos = 60 * 2, tempoPraDiminuirStatsFeridasInternasPorSegundos = 60 * 2;
    [SerializeField] public float valorDaFomeReduzidaPorTempo = 5, valorDaSedeReduzidaPorTempo = 10;
    public float consumoEnergiaPorSegundo = 5.0f;
    public float recuperacaoEnergiaPorSegundo = 2.0f;

    //Feridas internas
    public bool isFraturado = false, isAbstinencia = false, isSangrando = false;

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
        InvokeRepeating("VerificarStatsFeridasInternas", 0, tempoPraDiminuirStatsFeridasInternasPorSegundos);
    }

    void DiminuirStatsPorTempo()
    {
        setarFomeAtual(fomeAtual - valorDaFomeReduzidaPorTempo);
        setarSedeAtual(sedeAtual - valorDaSedeReduzidaPorTempo);
        if(sedeAtual <= 0 || fomeAtual <= 0)
        {
            TakeDamageHealth(10);
        }
        if (isAbstinencia)
        {
            TakeDamageHealth(10);
        }
        if (isFraturado)
        {
            TakeDamageHealth(10);
        }
        if (isSangrando)
        {
            TakeDamageHealth(10);
        }
    }
    
    void VerificarStatsFeridasInternas()
    {
        verificarAbstinencia();
    }

    int countAbstinencia = 0;
    private void verificarAbstinencia()
    {
        if (isAbstinencia) return; //Ja esta com abstinencia
        if (fomeAtual <= fomeMaxima * 0.15f || sedeAtual <= sedeMaxima * 0.15f)
        {
            countAbstinencia++;
        }
        if(countAbstinencia >= 3) //Causa abstinencia
        {
            isAbstinencia = true;
            countAbstinencia = 0;
            AtualizarImgAbstinencia();
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

    public void AtualizarImgAbstinencia()
    {
        hudJogador.atualizarImgAbstinencia(isAbstinencia);
    }

    public void AtualizarImgSangrando()
    {
        hudJogador.atualizarImgSangrando(isSangrando);
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
        playerController.StartarAbility(playerController.reviveAbility);
        playerController.canMove = true;
        playerController.corpoDissecando = null;
        playerController.animalCapturado = null;
        TakeHealHealth(playerController.characterAttributeManager.GetAttribute(playerController.characterHealth.HealthAttributeName).MaxValue * 0.20f); //CURA 20% DA VIDA MAXIMA
        setarFomeAtual(fomeMaxima*0.25f);
        setarSedeAtual(sedeMaxima * 0.25f);
        setarEnergiaAtual(energiaMaxima);
    }

}
