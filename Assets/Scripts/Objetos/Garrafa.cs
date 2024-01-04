using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Garrafa : MonoBehaviour
{

    public int qtdPorGole = 50;
    public int qtdMaxima = 100, qtdAtual = 0;

    public void EncherRepositorioComAgua()
    {
        qtdAtual = 100;
    }

    public int BeberAgua()
    {
        if(qtdAtual >= qtdPorGole)
        {
            qtdAtual -= qtdPorGole;
            if (qtdAtual < 0) qtdAtual = 0;
            return qtdPorGole;
        }
        else
        {
            int qtd = qtdAtual;
            qtdAtual = 0;
            return qtd;
        }
    }

    public void Setup(Garrafa garrafa)
    {
        qtdAtual = garrafa.qtdAtual;
    }

}
