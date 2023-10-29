using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerificaObstaculo : MonoBehaviour
{
   
    [SerializeField] LobisomemMovimentacao lobisomemMovimentacao;

    void OnTriggerStay(Collider other)
    {
        if (lobisomemMovimentacao.statsGeral.isDead) return;
        if (other.gameObject.tag == "ConstrucaoStats" && other.gameObject.name != "Fundação")
        {
            Debug.Log("Achou obstaculo");
            lobisomemMovimentacao.agent.ResetPath();
            lobisomemMovimentacao.targetObstaculo = other.transform;
        }
    }

}
