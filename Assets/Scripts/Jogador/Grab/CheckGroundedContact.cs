using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CheckGroundedContact : MonoBehaviourPunCallbacks
{

    [SerializeField] GrabObjects grabObjects;

    void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.GetComponent<ObjetoGrab>() != null && grabObjects.grabedObj != null )
        {
            if (collision.gameObject.GetComponent<PhotonView>().ViewID == grabObjects.grabedObj.GetComponent<PhotonView>().ViewID)
            {
                grabObjects.UngrabObject();
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.GetComponent<ObjetoGrab>() != null && grabObjects.grabedObj != null)
        {
            if (collision.gameObject.GetComponent<PhotonView>().ViewID == grabObjects.grabedObj.GetComponent<PhotonView>().ViewID)
            {
                grabObjects.UngrabObject();
            }
        }
    }

}
