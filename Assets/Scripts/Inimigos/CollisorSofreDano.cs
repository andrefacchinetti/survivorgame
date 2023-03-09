using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisorSofreDano : MonoBehaviour
{

    LobisomemStats lobisomemStats;

    private void Awake()
    {
        lobisomemStats = GetComponentInParent<LobisomemStats>();
    }

    void OnCollisionEnter(Collision other)
    {
        Debug.Log("colidiu com: " + other.transform.tag);
        if (other.transform.tag == "Ferramenta" || other.transform.tag == "Arma") //recebe dano qdo player ataca com ferramenta ou arma da mao
        {
            if (!other.transform.root.gameObject.GetComponent<PlayerController>().isAttacking) return;
            float damage = other.transform.gameObject.GetComponent<ItemObjMao>().damage;
            lobisomemStats.TakeDamage(damage);
            other.transform.root.gameObject.GetComponent<PlayerController>().isAttacking = false;
            other.transform.root.gameObject.GetComponent<Animator>().SetTrigger("ferramentaFrenteExit");
        }

        if (other.transform.tag == "ItemDrop") //Qdo toca em objeto que causa dano em velocidade (lança ou flecha)
        {
            if (other.transform.GetComponent<ItemDrop>().nomeItem.Equals(Item.NomeItem.LancaSimples)
                || other.transform.GetComponent<ItemDrop>().nomeItem.Equals(Item.NomeItem.LancaAvancada)
                || other.transform.GetComponent<ItemDrop>().nomeItem.Equals(Item.NomeItem.FlechaDeMadeira)
                || other.transform.GetComponent<ItemDrop>().nomeItem.Equals(Item.NomeItem.FlechaDeOsso)
                || other.transform.GetComponent<ItemDrop>().nomeItem.Equals(Item.NomeItem.FlechaDeMetal))
            {
                if (other.transform.GetComponent<Rigidbody>().velocity.magnitude > 1f)
                {
                    float damage = other.transform.GetComponent<ItemDrop>().damageQuandoColide;
                    lobisomemStats.TakeDamage(damage);
                }
            }
        }

    }
}
