using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropaRecursosStats : MonoBehaviour
{

    StatsGeral statsGeral;

    private void Awake()
    {
        statsGeral.GetComponent<StatsGeral>();
    }

    public void AcoesTomouDano()
    {
        Debug.Log("dropa recursos tomou dano");
        //TODO: Mostrar dano visual
    }

    public void AcoesMorreu()
    {
        Debug.Log("dropa recursos morreu");
        statsGeral.DestruirGameObject();
    }

}
