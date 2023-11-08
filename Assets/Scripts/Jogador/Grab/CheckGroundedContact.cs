using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CheckGroundedContact : MonoBehaviourPunCallbacks
{

    [SerializeField] GrabObjects grabObjects;
    public CharacterController characterController;
    public float heightBeforeFall = 3;
    public float fallDamageMultiplier = 10f;
    private Vector3 startPosition;

    void Start()
    {
        startPosition = characterController.transform.position;
    }

    void Update()
    {
        if ( characterController.isGrounded)
        {
            // Se o jogador estiver no chão, atualize a posição inicial
            startPosition = characterController.transform.position;
        }
    }

    void LateUpdate()
    {
        // Detecta a altura da queda quando o jogador toca o chão novamente
        if (characterController.isGrounded)
        {
            float fallenHeight = startPosition.y - characterController.transform.position.y;

            if (fallenHeight > heightBeforeFall)
            {
                int damage = Mathf.RoundToInt((fallenHeight - heightBeforeFall) * fallDamageMultiplier);

                // Aplica dano ao jogador
                characterController.GetComponent<StatsGeral>().TakeDamage(damage);
            }
        }
    }

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
