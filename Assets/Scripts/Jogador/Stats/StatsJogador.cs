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


    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        setarVidaAtual(vidaMaxima);
        setarFomeAtual(fomeMaxima);
        setarSedeAtual(sedeMaxima);
        setarEnergiaAtual(energiaMaxima);
    }

    public void setarVidaAtual(float valor)
    {
        vidaAtual = valor;
        hudJogador.atualizarImgVida(vidaAtual, vidaMaxima);
    }

    public void setarFomeAtual(float valor)
    {
        fomeAtual = valor;
        hudJogador.atualizarImgFome(fomeAtual, fomeMaxima);
    }

    public void setarSedeAtual(float valor)
    {
        sedeAtual = valor;
        hudJogador.atualizarImgSede(sedeAtual, sedeMaxima);
    }

    public void setarEnergiaAtual(float valor)
    {
        energiaAtual = valor;
        hudJogador.atualizarImgEnergia(energiaAtual, energiaMaxima);
    }

}
