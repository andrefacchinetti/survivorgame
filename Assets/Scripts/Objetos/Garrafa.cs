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
            return qtdPorGole;
        }
        else
        {
            return qtdAtual;
        }
    }

    public void Setup(Garrafa garrafa)
    {
        qtdAtual = garrafa.qtdAtual;
    }

}
