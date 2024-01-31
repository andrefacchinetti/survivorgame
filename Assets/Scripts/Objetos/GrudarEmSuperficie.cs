using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrudarEmSuperficie : MonoBehaviour
{

    [SerializeField] GameObject gameObjectPai;

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.GetComponent<CollisorSofreDano>() != null || collision.gameObject.tag == "Terreno")
        {
            // A colis�o ocorreu com um objeto da camada "Construcao"
            if (gameObjectPai.GetComponent<Rigidbody>().linearVelocity.magnitude > 1f)
            {
                gameObjectPai.GetComponent<Rigidbody>().isKinematic = true;
                gameObjectPai.transform.SetParent(collision.gameObject.transform);
            }
        }
    }

}
