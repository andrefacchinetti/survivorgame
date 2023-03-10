using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;
using System.IO;

public class ItemDrop : MonoBehaviourPunCallbacks
{

    [SerializeField] public Item.NomeItem nomeItem;
    [SerializeField][HideInInspector] public string nomeId;
    [SerializeField] public float damageQuandoColide;
    public bool estaSendoComido = false;

    private void Awake()
    {
        nomeId = Item.ObterNomeIdPorTipoItem(nomeItem);
    }

    public static GameObject InstanciarPrefabPorPath(string nomePrefab, int quantidade, Vector3 position, Quaternion rotation, int viewID)
    {
        GameObject objInstanciado = null;
        string prefabPath = Path.Combine("Objetos/Prefabs/ItensInventario/", nomePrefab);
        Debug.Log(prefabPath);
        for(int i=0; i < quantidade; i++)
        {
            if (PhotonNetwork.IsConnected)
            {
                objInstanciado = PhotonNetwork.Instantiate(prefabPath, position, new Quaternion(), 0, new object[] { viewID });
            }
            else
            {
                GameObject prefab = Resources.Load<GameObject>(prefabPath);
                objInstanciado = Instantiate(prefab, position, rotation);
            }
        }
        return objInstanciado;
    }

}
