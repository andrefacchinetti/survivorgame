using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;
using System.IO;

public class ItemDrop : MonoBehaviourPunCallbacks
{

    [SerializeField] public Item.TipoItem tipoItem;
    [SerializeField] public Item.NomeRecursoItemId nomeRecurso;
    [SerializeField] public Item.NomeFerramentaItemId nomeFerramenta;
    [SerializeField] public Item.NomeConsumivelItemId nomeConsumivel;
    [SerializeField] public Item.NomeArmaItemId nomeArma;
    [SerializeField][HideInInspector] public string nomeId;

    private void Awake()
    {
        nomeId = Item.ObterNomeIdPorTipoItem(tipoItem, nomeRecurso, nomeFerramenta, nomeConsumivel, nomeArma);
    }

    public static void InstanciarPrefabPorPath(string nomePrefab, Vector3 position, Quaternion rotation, int viewID)
    {
        string prefabPath = Path.Combine("Objetos/Prefabs/ItensPegaveis", nomePrefab);
        if (PhotonNetwork.IsConnected) PhotonNetwork.Instantiate(prefabPath, position, new Quaternion(), 0, new object[] { viewID });
        else
        {
            GameObject prefab = Resources.Load<GameObject>(prefabPath);
            GameObject newObject = Instantiate(prefab, position, rotation);
        }
    }

}
