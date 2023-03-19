using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CollisorSofreDano : MonoBehaviour
{

    [SerializeField] List<Item.NomeItem> nomeItemFerramentasRecomendadas;
    [SerializeField] public bool isApenasFerramentaRecomendadaCausaDano = false;
    [HideInInspector] public StatsGeral statsGeral;

    private void Awake()
    {
        statsGeral = GetComponentInParent<StatsGeral>();
    }

    public float CalcularDanoPorArmaCausandoDano(ItemObjMao itemNaMao, float damage)
    {
        if (itemNaMao == null) return damage;
        if (isApenasFerramentaRecomendadaCausaDano)
        {
            if (nomeItemFerramentasRecomendadas.Contains(itemNaMao.nomeItem))
            {
                damage += itemNaMao.damage;
            }
            else
            {
                damage = 0;
            }
        }
        else
        {
            damage += itemNaMao.damage;
        }
        return damage;
    }

    void OnCollisionEnter(Collision other)
    {
        if (statsGeral.vidaAtual <= 0) return;
       /* if (other.transform.tag == "Ferramenta" || other.transform.tag == "Arma") //recebe dano qdo player ataca com ferramenta ou arma da mao
        {
            if (!other.transform.root.gameObject.GetComponent<StatsGeral>().isAttacking) return;
            float damage = other.transform.gameObject.GetComponent<ItemObjMao>().damage;
            if (isApenasFerramentaRecomendadaCausaDano)
            {
                if (nomeItemFerramentasRecomendadas.Contains(other.transform.gameObject.GetComponent<ItemObjMao>().nomeItem))
                {
                    statsGeral.TakeDamage(damage / 2);
                }
            }
            else
            {
                statsGeral.TakeDamage(damage);
            }
           
            other.transform.gameObject.GetComponentInParent<StatsGeral>().isAttacking = false;
            other.transform.gameObject.GetComponentInParent<StatsGeral>().gameObject.GetComponent<Animator>().SetTrigger("ferramentaFrenteExit");
        }*/

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
                    statsGeral.TakeDamage(damage);
                }
            }
        }
    }

}
