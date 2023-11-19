using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using System.IO;
using UnityEngine;
using Opsive.UltimateCharacterController.Inventory;
using Opsive.Shared.Inventory;

public class SpawnLoots : MonoBehaviour
{

    [SerializeField] Item.TiposItems tipoItem;
    public ItemDefinitionBase[] itens;
    private GameObject itemSpawnado;
    [SerializeField] GameController gameController;

    private float lastGameDay = -1;

    PhotonView PV;

    private void Start()
    {
        SpawnarRandomLoot();
    }

    private void LateUpdate()
    {
        if(lastGameDay != gameController.gameDay)
        {
            SpawnarRandomLoot();
            lastGameDay = gameController.gameDay;
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
        bool isPhotonConnected = PhotonNetwork.IsConnected;

        int randomIndex = Random.Range(0, itens.Length);

        Vector3 position = this.transform.position;
        Quaternion rotation = this.transform.rotation;
        
        string nomePrefab = tipoItem + "/" + itens[randomIndex].name;
        string prefabPath = Path.Combine("Prefabs/ItensInventario/", nomePrefab);
        GameObject prefab = Resources.Load<GameObject>(prefabPath);

        this.itemSpawnado = isPhotonConnected ? PhotonNetwork.Instantiate(prefabPath, position, rotation, 0, new object[] { PV.ViewID }) : Instantiate(prefab, position, rotation);
        Debug.Log("Spawnou loot: " + nomePrefab);
    }


}
