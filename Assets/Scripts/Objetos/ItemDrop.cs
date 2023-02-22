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

    private void Awake()
    {
        nomeId = Item.ObterNomeIdPorTipoItem(nomeItem);
    }

    public static void InstanciarPrefabPorPath(string nomePrefab, int quantidade, Vector3 position, Quaternion rotation, int viewID)
    {
        string prefabPath = Path.Combine("Objetos/Prefabs/ItensInventario/", nomePrefab);
        Debug.Log(prefabPath);
        for(int i=0; i < quantidade; i++)
        {
            if (PhotonNetwork.IsConnected) PhotonNetwork.Instantiate(prefabPath, position, new Quaternion(), 0, new object[] { viewID });
            else
            {
                GameObject prefab = Resources.Load<GameObject>(prefabPath);
                GameObject newObject = Instantiate(prefab, position, rotation);
            }
        }
    }

}
