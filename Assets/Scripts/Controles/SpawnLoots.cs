using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using System.IO;
using UnityEngine;

public class SpawnLoots : MonoBehaviour
{

    public Item.NomeItem[] itensPossiveis;
    private GameObject itemSpawnado;
    [SerializeField] GameController gameController;

    PhotonView PV;

    private void Start()
    {
        SpawnarRandomLoot();
    }

    private void Update()
    {
        if(gameController.gameHour == 5)
        {
            SpawnarRandomLoot();
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

        int randomIndex = Random.Range(0, itensPossiveis.Length);

        Vector3 position = this.transform.position;
        Quaternion rotation = this.transform.rotation;
        
        string nomePrefab = itensPossiveis[randomIndex].GetTipoItemEnum() + "/" + itensPossiveis[randomIndex].ToString();
        string prefabPath = Path.Combine("Prefabs/ItensInventario/", nomePrefab);
        GameObject prefab = Resources.Load<GameObject>(prefabPath);

        this.itemSpawnado = isPhotonConnected ? PhotonNetwork.Instantiate(prefabPath, position, rotation, 0, new object[] { PV.ViewID }) : Instantiate(prefab, position, rotation);
    }


}
