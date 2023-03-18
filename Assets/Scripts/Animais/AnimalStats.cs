using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AnimalStats : MonoBehaviourPunCallbacks
{

    StatsGeral statsGeral;
    AnimalController animalController;

    private void Awake()
    {
        animalController = GetComponent<AnimalController>();
        statsGeral = GetComponent<StatsGeral>();
    }

    public void AcoesTomouDano()
    {
        animalController.animator.SetTrigger("isHit");
        animalController.AcoesTomouDano();
        Debug.Log("vida animal: " + statsGeral.vidaAtual);
    }

    public void AcoesMorreu()
    {
        Debug.Log("animal morreu");
        animalController.animator.SetBool("isDead", true);
        animalController.agent.speed = 0;
        animalController.agent.isStopped = true;
    }

}
