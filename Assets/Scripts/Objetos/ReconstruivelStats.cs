using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReconstruivelStats : MonoBehaviour
{
    
    [SerializeField] GameObject reconstruivelInteiro, reconstruivelQuebrado;

    public void AcoesTomouDano()
    {
        Debug.Log("reconstruivel tomou dano");
        //TODO: Mostrar dano visual
    }

    public void AcoesMorreu()
    {
        Debug.Log("reconstruivel morreu");
        quebrarReconstruivel();
    }

    private void quebrarReconstruivel()
    {
        reconstruivelInteiro.SetActive(false);
        reconstruivelQuebrado.SetActive(true);
    }

    public void ConsertarReconstruivel()
    {
        reconstruivelInteiro.GetComponent<StatsGeral>().vidaAtual = reconstruivelInteiro.GetComponent<StatsGeral>().vidaMaxima;
        reconstruivelInteiro.SetActive(true);
        reconstruivelQuebrado.SetActive(false);
    }

}
