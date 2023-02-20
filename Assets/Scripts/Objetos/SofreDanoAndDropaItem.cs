using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using System;

public class SofreDanoAndDropaItem : MonoBehaviourPunCallbacks
{

    [SerializeField] public int vida = 100, qtdMinDrops = 5, qtdMaxDrops = 10;
    [SerializeField] public Item.NomeFerramentaItemId ferramentaRecomendada;
    [SerializeField] public bool isApenasFerramentaRecomendadaCausaDano = false, isDropaAlgumItem = true;
    [SerializeField] public Item.NomeConsumivelItemId dropConsumivel;
    [SerializeField] public Item.NomeRecursoItemId dropRecurso;
    [SerializeField] public Item.NomeArmaItemId dropArma;
    [SerializeField] public Item.NomeFerramentaItemId dropFerramenta;
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
            if (other.transform.gameObject.GetComponent<ItemObjMao>().nomeFerramenta == ferramentaRecomendada)
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
                        string nomePrefab = "";
                        if (!Item.NomeConsumivelItemId.Nenhum.Equals(dropConsumivel))
                        {
                            nomePrefab = "Consumivel" + dropConsumivel.GetEnumMemberValue();
                            ItemDrop.InstanciarPrefabPorPath(nomePrefab, transform.position, transform.rotation, PV.ViewID);
                        }
                        if (!Item.NomeRecursoItemId.Nenhum.Equals(dropRecurso))
                        {
                            nomePrefab = "Recurso" + dropRecurso.GetEnumMemberValue();
                            ItemDrop.InstanciarPrefabPorPath(nomePrefab, transform.position, transform.rotation, PV.ViewID);
                        }
                        if (!Item.NomeArmaItemId.Nenhum.Equals(dropArma))
                        {
                            nomePrefab = "Arma" + dropRecurso.GetEnumMemberValue();
                            ItemDrop.InstanciarPrefabPorPath(nomePrefab, transform.position, transform.rotation, PV.ViewID);
                        }
                        if (!Item.NomeFerramentaItemId.Nenhum.Equals(dropFerramenta))
                        {
                            nomePrefab = "Ferramenta" + dropRecurso.GetEnumMemberValue();
                            ItemDrop.InstanciarPrefabPorPath(nomePrefab, transform.position, transform.rotation, PV.ViewID);
                        }
                    }
                }
                PhotonNetwork.Destroy(this.gameObject);
            }
            other.transform.root.gameObject.GetComponent<PlayerController>().isAttacking = false;
        }
    }

    

}
