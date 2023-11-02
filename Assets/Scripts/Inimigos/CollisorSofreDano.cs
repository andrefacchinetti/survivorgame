using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(Rigidbody))]
public class CollisorSofreDano : MonoBehaviourPunCallbacks
{

    [SerializeField] List<Item.NomeItem> nomeItemFerramentasRecomendadas;
    [SerializeField] public bool isApenasFerramentaRecomendadaCausaDano = false;
    [HideInInspector] public StatsGeral statsGeral;
    public PhotonView PV;

    private void Awake()
    {
        statsGeral = GetComponentInParent<StatsGeral>();
        PV = GetComponentInParent<PhotonView>();
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

        if (other.transform.tag == "ItemDrop") //Qdo toca em objeto que causa dano em velocidade (lan�a ou flecha)
        {
            if (other.transform.GetComponent<ItemDrop>().nomeItem.Equals(Item.NomeItem.LancaSimples)
                || other.transform.GetComponent<ItemDrop>().nomeItem.Equals(Item.NomeItem.LancaAvancada)
                || other.transform.GetComponent<ItemDrop>().nomeItem.Equals(Item.NomeItem.FlechaDeMadeira)
                || other.transform.GetComponent<ItemDrop>().nomeItem.Equals(Item.NomeItem.FlechaDeOsso)
                || other.transform.GetComponent<ItemDrop>().nomeItem.Equals(Item.NomeItem.FlechaDeMetal)
                || other.transform.GetComponent<ItemDrop>().nomeItem.Equals(Item.NomeItem.MunicaoPistola))
            {
                if (other.transform.GetComponent<Rigidbody>().velocity.magnitude > 1f)
                {
                    float damage = other.transform.GetComponent<ItemDrop>().damageQuandoColide;
                    statsGeral.TakeDamage(damage);
                }
                if (other.transform.GetComponent<ItemDrop>().nomeItem.Equals(Item.NomeItem.MunicaoPistola))
                {
                    Destroy(other.gameObject);
                }
            }
        }
    }

}
