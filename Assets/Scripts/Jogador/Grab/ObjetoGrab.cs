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

    [SerializeField] public GameObject objPontaCorda, objNovoPaiPonta;
    [SerializeField] public GameObject objFollowed;

    void LateUpdate()
    {
        if (objFollowed != null)
        {
            objPontaCorda.transform.position = objFollowed.transform.position;
        }
    }
    void Start()
    {
        audioS = GetComponent<AudioSource>();
        PV = GetComponent<PhotonView>();
    }

    public void AtivarCordaGrab(GameObject pivotRopeStart)
    {
        objPontaCorda.transform.SetParent(objNovoPaiPonta.transform);
        objFollowed = pivotRopeStart;
        objPontaCorda.SetActive(true);
        
    }


}
