using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AnimalPassivoStats : MonoBehaviourPunCallbacks
{

    StatsGeral statsGeral;
    AnimalPassivoController animalPassivoController;
    PhotonView PV;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        animalPassivoController = GetComponent<AnimalPassivoController>();
        statsGeral = GetComponent<StatsGeral>();
    }

    public void AcoesTomouDano()
    {
        animalPassivoController.anim.SetTrigger("isHit");
        animalPassivoController.AcoesTomouDano();
        Debug.Log("vida animal: " + statsGeral.vidaAtual);
    }

    public void AcoesMorreu()
    {
        Debug.Log("animal morreu");
    }

}
