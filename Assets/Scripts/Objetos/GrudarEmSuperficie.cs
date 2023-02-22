using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrudarEmSuperficie : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag != "Player")
        {
            transform.SetParent(collision.gameObject.transform);
            //GetComponent<Rigidbody>().isKinematic = true;
        }
    }

}
