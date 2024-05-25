using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using System.IO;
using UnityEngine;
using Opsive.UltimateCharacterController.Inventory;
using Opsive.Shared.Inventory;

public class SpawnLoots : MonoBehaviour
{

    [SerializeField] Item.ItemDropStruct[] itemsDropStruct;
    private GameObject itemSpawnado;
    [SerializeField] GameController gameController;

    public int qtdDiasParaRespawnar = 2;
    private float qtdDias = 0;


    private void Start()
    {
        SpawnarRandomLoot();
        gameController.listaSpawnLoots.Add(this);
    }

    public void SpawnarLootPorDias()
    {
        if (qtdDias >= qtdDiasParaRespawnar)
        {
            SpawnarRandomLoot();
            qtdDias = 0;
        }
        else
        {
            qtdDias++;
        }
    }

    public void SpawnarRandomLoot()
    {
        if(itemSpawnado != null)
        {
            Destroy(itemSpawnado);
        }

        InstanciarPrefabPorPathRandomItem();
    }

    public void InstanciarPrefabPorPathRandomItem()
    {
        if (itemsDropStruct == null || itemsDropStruct.Length == 0) return;
        int randomIndex = Random.Range(0, itemsDropStruct.Length);
        if (itemsDropStruct[randomIndex].itemDefinition == null) return;

        Vector3 position = this.transform.position;
        Quaternion rotation = this.transform.rotation;


        string nomePrefab = itemsDropStruct[randomIndex].itemDefinition.name;
        string prefabPath = itemsDropStruct[randomIndex].tipoItem + "/" + nomePrefab;
        this.itemSpawnado = ItemDrop.InstanciarPrefabPorPath(prefabPath, 1, position, rotation, itemsDropStruct[randomIndex].materialPersonalizado, gameController.PV.ViewID);
    }


}
