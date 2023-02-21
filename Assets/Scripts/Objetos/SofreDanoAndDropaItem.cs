using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using System;

public class SofreDanoAndDropaItem : MonoBehaviourPunCallbacks
{

    [SerializeField] public int vida = 100;
    [SerializeField] public List<Item.NomeItem> nomeItemFerramentasRecomendadas;
    [SerializeField] public bool isApenasFerramentaRecomendadaCausaDano = false;
    [SerializeField] public List<ItemDropStruct> dropsItems;
    PhotonView PV;

    [System.Serializable]
    public struct ItemDropStruct
    {
        public Item.NomeItem nomeItemEnum;
        public Item.TipoItem tipoItemEnum;
        public int qtdMinDrops;
        public int qtdMaxDrops;
    }

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
            if (nomeItemFerramentasRecomendadas.Contains(other.transform.gameObject.GetComponent<ItemObjMao>().nomeItem))
            {
                vida -= damage;
            }
            else
            {
                if(!isApenasFerramentaRecomendadaCausaDano) vida -= damage/2;
            }
            Debug.Log("Vida: " + vida);
            //TODO: mostrar na tela um efeito do dano causado e bonus recebido
            if (vida <= 0)
            {
                foreach (ItemDropStruct drop in dropsItems)
                {
                    if (!Item.TipoItem.Nenhum.Equals(drop.tipoItemEnum))
                    {
                        int quantidade = UnityEngine.Random.Range(drop.qtdMinDrops, drop.qtdMaxDrops);
                        string nomePrefab = drop.nomeItemEnum.GetEnumMemberValue() + "/" + drop.nomeItemEnum.GetEnumMemberValue();
                        ItemDrop.InstanciarPrefabPorPath(nomePrefab, quantidade, transform.position, transform.rotation, PV.ViewID);
                    }
                }
                PhotonNetwork.Destroy(this.gameObject);
            }
            other.transform.root.gameObject.GetComponent<PlayerController>().isAttacking = false;
        }
    }

    

}
