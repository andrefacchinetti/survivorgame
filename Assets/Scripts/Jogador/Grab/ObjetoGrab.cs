using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class ObjetoGrab : MonoBehaviourPunCallbacks
{
    [HideInInspector] private AudioSource audioS;
    [HideInInspector] public PhotonView PV;


    void Start()
    {
        audioS = GetComponent<AudioSource>();
        PV = GetComponent<PhotonView>();
    }
   
}
