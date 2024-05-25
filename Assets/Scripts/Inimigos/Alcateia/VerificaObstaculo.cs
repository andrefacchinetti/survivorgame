using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerificaObstaculo : MonoBehaviour
{
   
    [SerializeField] LobisomemMovimentacao lobisomemMovimentacao;

    void OnTriggerStay(Collider other)
    {
        if (lobisomemMovimentacao.targetObstaculo != null || !lobisomemMovimentacao.statsGeral.health.IsAlive()) return;
        if (other.gameObject.tag == "ConstrucaoStats" && other.gameObject.name != "Fundação")
        {
            lobisomemMovimentacao.agent.ResetPath();
            lobisomemMovimentacao.targetObstaculo = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (lobisomemMovimentacao.targetObstaculo == null) return;
        if(other.GetInstanceID() == lobisomemMovimentacao.targetObstaculo.GetInstanceID())
        {
            lobisomemMovimentacao.targetObstaculo = null;
        }
    }

}
