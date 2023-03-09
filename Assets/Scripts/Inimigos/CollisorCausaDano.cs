using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisorCausaDano : MonoBehaviour
{

    LobisomemStats lobisomemStats;

    private void Awake()
    {
        lobisomemStats = GetComponentInParent<LobisomemStats>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) //Dar dano no player qdo collide
        {
            if (lobisomemStats.isAttacking)
            {
                other.gameObject.GetComponent<PlayerController>().TakeDamage(lobisomemStats.damage);
                lobisomemStats.isAttacking = false;
            }
        }
    }

}
