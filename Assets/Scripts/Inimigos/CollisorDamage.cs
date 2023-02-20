using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisorDamage : MonoBehaviour
{

    [SerializeField] EnemySelvagem enemySelvagemController;

    private void Awake()
    {
        if (enemySelvagemController == null)  enemySelvagemController = GetComponentInParent<EnemySelvagem>(); //Procura o controller do inimigo no objetos pais
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) //Dar dano no player qdo collide
        {
            if (enemySelvagemController.isAttacking)
            {
                other.gameObject.GetComponent<PlayerController>().TakeDamage(enemySelvagemController.damage);
            }
            // IA colidiu com o jogador e deve interromper o ataque
            enemySelvagemController.isAttacking = false;
            enemySelvagemController.animator.ResetTrigger("Attacking");
        }
        else if (other.transform.tag == "Ferramenta" || other.transform.tag == "Arma") //recebe dano qdo player ataca com ferramenta ou arma
        {
            if (!other.transform.root.gameObject.GetComponent<PlayerController>().isAttacking) return;
            int damage = other.transform.gameObject.GetComponent<ItemObjMao>().damage;
            enemySelvagemController.TakeDamage(damage);
        }
    }

}
