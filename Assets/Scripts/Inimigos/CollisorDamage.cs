using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisorDamage : MonoBehaviour
{

    [SerializeField] bool isParteDoCorpoCausaDano;
    [SerializeField] [HideInInspector] EnemyMovementZombie enemySelvagemController;
    [SerializeField] [HideInInspector] EnemyStats enemyStats;

    private void Awake()
    {
        enemySelvagemController = GetComponentInParent<EnemyMovementZombie>();
        enemyStats = GetComponentInParent<EnemyStats>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (isParteDoCorpoCausaDano) //CAUSANDO DANO
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
        }

        if (other.transform.tag == "Ferramenta" || other.transform.tag == "Arma") //recebe dano qdo player ataca com ferramenta ou arma da mao
        {
            if (!other.transform.root.gameObject.GetComponent<PlayerController>().isAttacking) return;
            float damage = other.transform.gameObject.GetComponent<ItemObjMao>().damage;
            enemyStats.TakeDamage(damage);
        }

        if(other.transform.tag == "ItemDrop") //Qdo toca em objeto que causa dano
        {
            if (other.transform.GetComponent<ItemDrop>().nomeItem.Equals(Item.NomeItem.LancaSimples)
                || other.transform.GetComponent<ItemDrop>().nomeItem.Equals(Item.NomeItem.LancaAvancada)
                || other.transform.GetComponent<ItemDrop>().nomeItem.Equals(Item.NomeItem.FlechaDeMadeira)
                || other.transform.GetComponent<ItemDrop>().nomeItem.Equals(Item.NomeItem.FlechaDeOsso)
                || other.transform.GetComponent<ItemDrop>().nomeItem.Equals(Item.NomeItem.FlechaDeMetal))
            {
                float damage = other.transform.GetComponent<ItemDrop>().damageQuandoColide;
                enemyStats.TakeDamage(damage);
            }
        }

    }

}
