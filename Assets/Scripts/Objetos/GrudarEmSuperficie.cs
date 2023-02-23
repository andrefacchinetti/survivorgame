using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrudarEmSuperficie : MonoBehaviour
{

    [SerializeField] GameObject gameObjectPai;

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag != "Player")
        {
            gameObjectPai.GetComponent<Rigidbody>().isKinematic = true;
            gameObjectPai.transform.SetParent(collision.gameObject.transform);
        }
    }

}
