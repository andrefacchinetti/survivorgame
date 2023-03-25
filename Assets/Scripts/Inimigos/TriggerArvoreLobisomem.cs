using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerArvoreLobisomem : MonoBehaviour
{

    LobisomemStats lobisomemStats;

    private void Awake()
    {
        lobisomemStats = GetComponentInParent<LobisomemStats>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Arvore" || other.tag == "NavMeshVertical")
        {
            Debug.Log("subindo NavMeshVertical");
            lobisomemStats.isIndoAteArvore = false;
            lobisomemStats.isSubindoNaArvore = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Arvore" || other.tag == "NavMeshVertical")
        {
            Debug.Log("desceu da NavMeshVertical ");
            lobisomemStats.isSubindoNaArvore = false;
            lobisomemStats.isIndoAteArvore = false;
        }
    }

}
