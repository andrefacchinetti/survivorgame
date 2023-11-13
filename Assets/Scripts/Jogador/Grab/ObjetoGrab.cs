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
    [HideInInspector] public GameObject objFollowed;

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
        objPontaCorda.GetComponent<CapsuleCollider>().isTrigger = true;
        objPontaCorda.GetComponent<Rigidbody>().isKinematic = true;
        objPontaCorda.SetActive(true);
    }

    public void DesativarCordaGrab()
    {
        objFollowed = null;
        objPontaCorda.transform.SetParent(this.transform);
        objPontaCorda.GetComponent<CapsuleCollider>().isTrigger = false;
        objPontaCorda.GetComponent<Rigidbody>().isKinematic = false;
        objPontaCorda.SetActive(false);
    }


}
