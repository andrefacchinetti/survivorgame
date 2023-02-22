using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsJogador : MonoBehaviour
{

    [SerializeField][HideInInspector] PlayerController playerController;
    [SerializeField] HudJogador hudJogador;

    // STATS MAXIMO
    [SerializeField] public float vidaMaxima = 100, fomeMaxima = 100, sedeMaxima = 100, energiaMaxima = 100;

    //STATS CURRENT
    [SerializeField] [HideInInspector] public float vidaAtual, fomeAtual, sedeAtual, energiaAtual;

    [SerializeField] float tempoPraDiminuirStatsFomeSedePorSegundos = 30, valorDiminuiFomeSedePorTempo = 30;


    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        setarVidaAtual(vidaMaxima);
        setarFomeAtual(fomeMaxima);
        setarSedeAtual(sedeMaxima);
        setarEnergiaAtual(energiaMaxima);

        InvokeRepeating("DiminuirStatsPorTempo", 0, tempoPraDiminuirStatsFomeSedePorSegundos);
    }

    void DiminuirStatsPorTempo()
    {
        setarFomeAtual(fomeAtual - valorDiminuiFomeSedePorTempo);
        setarSedeAtual(sedeAtual - valorDiminuiFomeSedePorTempo);
        if(sedeAtual <= 0 || fomeAtual <= 0)
        {
            playerController.TakeDamage(10);
        }
        Debug.Log("Diminuiu fome: "+ fomeAtual + " Sede: "+sedeAtual);
    }


    public void setarVidaAtual(float valor)
    {
        if (valor > vidaMaxima) valor = vidaMaxima;
        if (valor < 0) valor = 0;
        vidaAtual = valor;
        hudJogador.atualizarImgVida(vidaAtual, vidaMaxima);
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
        Debug.Log(energiaAtual);
        hudJogador.atualizarImgEnergia(energiaAtual, energiaMaxima);
    }

}
