using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisorDamage : MonoBehaviour
{

    [SerializeField] [HideInInspector] EnemyMovementZombie enemySelvagemController;
    [SerializeField] [HideInInspector] EnemyStats enemyStats;

    private void Awake()
    {
        enemySelvagemController = GetComponentInParent<EnemyMovementZombie>();
        enemyStats = GetComponentInParent<EnemyStats>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) //Dar dano no player qdo collide
        {
            if (enemySelvagemController.isAttacking)
            {
                other.gameObject.GetComponent<PlayerController>().TakeDamage(enemyStats.damage);
            }
            // IA colidiu com o jogador e deve interromper o ataque
            enemySelvagemController.isAttacking = false;
            enemySelvagemController.animator.ResetTrigger("Attacking");
        }
        else if (other.transform.tag == "Ferramenta" || other.transform.tag == "Arma") //recebe dano qdo player ataca com ferramenta ou arma
        {
            if (!other.transform.root.gameObject.GetComponent<PlayerController>().isAttacking) return;
            int damage = other.transform.gameObject.GetComponent<ItemObjMao>().damage;
            enemyStats.TakeDamage(damage);
        }
    }

}
