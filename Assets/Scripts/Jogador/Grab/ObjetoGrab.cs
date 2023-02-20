using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

[RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(PhotonView)), RequireComponent(typeof(PhotonRigidbodyView)), RequireComponent(typeof(NavMeshObstacle))]
public class ObjetoGrab : MonoBehaviourPunCallbacks
{
    [HideInInspector] private AudioSource audioS;
    [HideInInspector] public PhotonView PV;
    [HideInInspector] public float posAlturaInicial = 0;


    void Start()
    {
        audioS = GetComponent<AudioSource>();
        PV = GetComponent<PhotonView>();
        posAlturaInicial = transform.position.y;
    }
   
}
