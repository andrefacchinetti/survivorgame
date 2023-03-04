using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisorSofreDano : MonoBehaviour
{
    [SerializeField] LobisomemMovimentacao lobisomemMovimentacao;
    [SerializeField] LobisomemStats lobisomemStats;

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Ferramenta" || other.transform.tag == "Arma") //recebe dano qdo player ataca com ferramenta ou arma da mao
        {
            if (!other.transform.root.gameObject.GetComponent<PlayerController>().isAttacking) return;
            float damage = other.transform.gameObject.GetComponent<ItemObjMao>().damage;
            lobisomemStats.TakeDamage(damage);
            other.transform.root.gameObject.GetComponent<PlayerController>().isAttacking = false;
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
