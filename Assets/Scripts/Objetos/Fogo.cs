using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fogo : MonoBehaviour
{

    [SerializeField] public float temperaturaAquecimento = 20;
    [SerializeField] public bool isFogoAceso;

    private void OnTriggerStay(Collider other)
    {
        if (!isFogoAceso) return;
        if(other.GetComponent<StatsJogador>() != null)
        {
            Debug.Log("fogo aquecendo player");
            other.GetComponent<StatsJogador>().fogoProximo = this;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isFogoAceso) return;
        if (other.GetComponent<StatsJogador>() != null)
        {
            Debug.Log("fogo parou de aquecer player");
            other.GetComponent<StatsJogador>().fogoProximo = null;
        }
    }

}
