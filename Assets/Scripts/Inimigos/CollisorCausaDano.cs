using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisorCausaDano : MonoBehaviour
{

    StatsGeral statsGeral;

    private void Awake()
    {
        statsGeral = GetComponentInParent<StatsGeral>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) //Dar dano no player qdo collide
        {
            if (statsGeral.isAttacking)
            {
                other.gameObject.GetComponent<PlayerController>().TakeDamage(statsGeral.damage);
                statsGeral.isAttacking = false;
            }
        }
    }

}
