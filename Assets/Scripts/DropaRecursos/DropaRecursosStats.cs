using System.Collections;
using System.Collections.Generic;
using Opsive.UltimateCharacterController.Traits;
using UnityEngine;

public class DropaRecursosStats : MonoBehaviour
{

    [SerializeField] public string pathPrefab;
    StatsGeral statsGeral;

    [HideInInspector] public Rigidbody rb;

    private void Awake()
    {
        statsGeral = GetComponent<StatsGeral>();
        rb = GetComponent<Rigidbody>();
    }

    public void AcoesTomouDano()
    {
        //TODO: Mostrar dano visual
    }

    public float forcaEmpurraArvore = 2;
    public void AcoesMorreu()
    {
        statsGeral.DroparItensAoMorrer();
        statsGeral.DestruirGameObject();
    }

}
