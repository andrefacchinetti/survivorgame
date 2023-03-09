using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using System;

public class SofreDanoAndDropaItem : MonoBehaviourPunCallbacks
{

    [SerializeField] public float vida = 100;
    [SerializeField] public List<Item.NomeItem> nomeItemFerramentasRecomendadas;
    [SerializeField] public bool isApenasFerramentaRecomendadaCausaDano = false;
    [SerializeField] public List<Item.ItemDropStruct> dropsItems;
    PhotonView PV;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Ferramenta")
        {
            if (!other.transform.gameObject.GetComponentInParent<PlayerController>().isAttacking) return;
            float damage = other.transform.gameObject.GetComponent<ItemObjMao>().damage;
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
                foreach (Item.ItemDropStruct drop in dropsItems)
                {
                    if (!Item.TiposItems.Nenhum.ToString().Equals(drop.nomeItemEnum.GetTipoItemEnum()))
                    {
                        int quantidade = UnityEngine.Random.Range(drop.qtdMinDrops, drop.qtdMaxDrops);
                        string nomePrefab = drop.nomeItemEnum.GetTipoItemEnum() + "/" + drop.nomeItemEnum.ToString();
                        ItemDrop.InstanciarPrefabPorPath(nomePrefab, quantidade, transform.position, transform.rotation, PV.ViewID);
                    }
                } 
                if (PhotonNetwork.IsConnected) PhotonNetwork.Destroy(this.gameObject);
                else GameObject.Destroy(this.gameObject);
            }
            other.transform.root.gameObject.GetComponent<PlayerController>().isAttacking = false;
            other.transform.root.gameObject.GetComponent<Animator>().SetTrigger("ferramentaFrenteExit");
        }
    }

    

}
