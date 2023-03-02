using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisorCausaDano : MonoBehaviour
{

    [SerializeField] LobisomemMovimentacao lobisomemMovimentacao;
    [SerializeField] LobisomemStats lobisomemStats;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) //Dar dano no player qdo collide
        {
            Debug.Log("acertou player");
            if (lobisomemMovimentacao.isAttacking)
            {
                other.gameObject.GetComponent<PlayerController>().TakeDamage(lobisomemStats.damage);
                lobisomemMovimentacao.isAttacking = false;
            }
        }
    }

}
