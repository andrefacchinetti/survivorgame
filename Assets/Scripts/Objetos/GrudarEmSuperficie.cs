using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrudarEmSuperficie : MonoBehaviour
{

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag != "Player")
        {
            transform.root.GetComponent<Rigidbody>().isKinematic = true;
            transform.root.SetParent(collision.gameObject.transform);
        }
    }

}
