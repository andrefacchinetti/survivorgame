using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using System;

public class SofreDanoAndDropaItem : MonoBehaviourPunCallbacks
{

    [SerializeField] public int vida = 100, qtdMinDrops = 5, qtdMaxDrops = 10;
    [SerializeField] public Item.NomeItem nomeItemFerramentaRecomendada;
    [SerializeField] public bool isApenasFerramentaRecomendadaCausaDano = false, isDropaAlgumItem = true;
    [SerializeField] public Dictionary<Item.NomeItem, Item.TipoItem> dropsItems;
    PhotonView PV;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Ferramenta")
        {
            if (!other.transform.root.gameObject.GetComponent<PlayerController>().isAttacking) return;
            int damage = other.transform.gameObject.GetComponent<ItemObjMao>().damage;
            if (other.transform.gameObject.GetComponent<ItemObjMao>().nomeItem == nomeItemFerramentaRecomendada)
            {
                vida -= damage;
            }
            else
            {
                vida -= damage/2;
            }
            Debug.Log("Vida: " + vida);
            //TODO: mostrar na tela um efeito do dano causado e bonus recebido
            if (vida <= 0)
            {
                if (isDropaAlgumItem)
                {
                    for (int i = 0; i < (UnityEngine.Random.Range(qtdMinDrops, qtdMaxDrops)); i++)
                    {
                        foreach (KeyValuePair<Item.NomeItem, Item.TipoItem> drop in dropsItems)
                        {
                            if (!Item.TipoItem.Nenhum.Equals(drop.Key))
                            {
                                string nomePrefab = drop.Value.GetEnumMemberValue() + "/" + drop.Key.GetEnumMemberValue();
                                ItemDrop.InstanciarPrefabPorPath(nomePrefab, transform.position, transform.rotation, PV.ViewID);
                            }
                        }
                        
                    }
                }
                PhotonNetwork.Destroy(this.gameObject);
            }
            other.transform.root.gameObject.GetComponent<PlayerController>().isAttacking = false;
        }
    }

    

}
