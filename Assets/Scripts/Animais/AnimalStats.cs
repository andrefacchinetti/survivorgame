using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AnimalStats : MonoBehaviourPunCallbacks
{

    //ATAQUE
    [SerializeField] public bool isTubarao = false;
    [SerializeField] public float distanciaDePerseguicao = 10f, distanciaDeAtaque = 2f;
    [SerializeField] public float attackInterval = 1f; // Intervalo de tempo entre ataques
    [HideInInspector] public float lastAttackTime; // Tempo do �ltimo ataque
    [SerializeField] public float destinationOffset = 1f;
    [SerializeField] public float speedVariation = 0.5f;
    [SerializeField] public float leadTime = 1.2f, leadDistance = 2;
    [HideInInspector] public bool estaFugindo = false;
    StatsGeral statsGeral;
    AnimalController animalController;
    TubaraoController tubaraoController;


    private void Awake()
    {
        animalController = GetComponent<AnimalController>();
        tubaraoController = GetComponent<TubaraoController>();
        statsGeral = GetComponent<StatsGeral>();
        estaFugindo = false;
    }

    public void AcoesTomouDano()
    {
        if (isTubarao)
        {
            tubaraoController.animator.SetTrigger("isHit");
        }
        else
        {
            animalController.animator.SetTrigger("isHit");
            animalController.AcoesTomouDano();
        }
    }

    public void AcoesMorreu()
    {
        if (!isTubarao)
        {
            animalController.agent.speed = 0;
            animalController.agent.isStopped = true;
        }
    }

}
