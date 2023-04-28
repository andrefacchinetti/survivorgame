using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsJogador : MonoBehaviour
{

    [SerializeField][HideInInspector] PlayerController playerController;
    [SerializeField] [HideInInspector] StatsGeral statsGeral;
    [SerializeField] HudJogador hudJogador;

    // STATS MAXIMO
    [SerializeField] public float fomeMaxima = 100, sedeMaxima = 100, energiaMaxima = 100, temperaturaMaxima = 100;

    //STATS CURRENT
    [SerializeField] [HideInInspector] public float fomeAtual, sedeAtual, energiaAtual, temperaturaAtual;

    [SerializeField] float tempoPraDiminuirStatsFomeSedePorSegundos = 60*2, tempoPraVerificarTemperaturaPorSegundos = 60 * 3, valorDiminuiFomePorTempo = 5, valorDiminuiSedePorTempo = 10;

    [SerializeField] int valorMaxHipertermia = 80, valorMaxHipotermia = 10;
    [SerializeField] int damageHipertermia = 10, damageHipotermia = 10;

    [SerializeField] [HideInInspector] public Fogo fogoProximo;
    [SerializeField] [HideInInspector] public float temperaturaAmbiente = 20;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        statsGeral = GetComponent<StatsGeral>();
        temperaturaAtual = 50;
    }

    private void Start()
    {
        setarVidaAtual(statsGeral.vidaMaxima);
        setarFomeAtual(fomeMaxima);
        setarSedeAtual(sedeMaxima);
        setarEnergiaAtual(energiaMaxima);
        InvokeRepeating("DiminuirStatsPorTempo", 0, tempoPraDiminuirStatsFomeSedePorSegundos);
        InvokeRepeating("VerificarTemperaturaJogador", 0, tempoPraVerificarTemperaturaPorSegundos);
    }

    private void LateUpdate()
    {
        if (fogoProximo != null && !fogoProximo.isFogoAceso) fogoProximo = null;
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

    void VerificarTemperaturaJogador()
    {
        Debug.Log("verificando variaveis que alteram a temperatura do jogador: ambiente, armadura, fogo, agua");
        float porcentagemAnterior = temperaturaAtual / temperaturaMaxima * 100;
        bool jaEstavaDoenteHipertermia = false, jaEstavaDoenteHipotermia = false;
        if(porcentagemAnterior > valorMaxHipertermia)
        {
            jaEstavaDoenteHipertermia = true;
        }
        if (porcentagemAnterior < valorMaxHipotermia)
        {
            jaEstavaDoenteHipotermia = true;
        }
        temperaturaAtual = playerController.gameController.isNoite ? playerController.gameController.temperaturaNoite : playerController.gameController.temperaturaDia;
        if(fogoProximo != null && fogoProximo.isFogoAceso)
        {
            temperaturaAtual += fogoProximo.temperaturaAquecimento;
        }
        //TODO: somar com temperatura armadura
        Debug.Log("temperatura atual: " + temperaturaAtual);
        float porcentagem = temperaturaAtual / temperaturaMaxima * 100;
        if (porcentagem > valorMaxHipertermia && jaEstavaDoenteHipertermia)
        {
            statsGeral.TakeDamage(damageHipertermia);
        }
        if (porcentagem < valorMaxHipotermia && jaEstavaDoenteHipotermia)
        {
            statsGeral.TakeDamage(damageHipotermia);
        }
        hudJogador.atualizarImgHipertermia(porcentagem > valorMaxHipertermia);
        hudJogador.atualizarImgHipotermia(porcentagem < valorMaxHipotermia);
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
    }

}
