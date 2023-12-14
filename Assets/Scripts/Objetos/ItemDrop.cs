using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;
using System.IO;
using Opsive.Shared.Inventory;
using Opsive.UltimateCharacterController.Inventory;

public class ItemDrop : MonoBehaviourPunCallbacks
{

    [SerializeField] public ItemDefinitionBase item;
    public bool estaSendoComido = false;

    public static GameObject InstanciarPrefabPorPath(string nomePrefab, int quantidade, Vector3 position, Quaternion rotation, Material materialPersonalizado, int viewID)
    {
        GameObject objInstanciado = null;
        string prefabPath = Path.Combine("Prefabs/ItensInventario/", nomePrefab);
        Debug.Log(prefabPath);

        for (int i = 0; i < quantidade; i++)
        {
            float alturaObjetoExistente = objInstanciado != null ? objInstanciado.GetComponent<Collider>().bounds.size.y : 0;

            position += rotation * new Vector3(0, alturaObjetoExistente, 0);

            if (PhotonNetwork.IsConnected)
            {
                objInstanciado = PhotonNetwork.Instantiate(prefabPath, position, rotation, 0, new object[] { viewID });
            }
            else
            {
                GameObject prefab = Resources.Load<GameObject>(prefabPath);
                objInstanciado = Instantiate(prefab, position, rotation);
            }
            if (materialPersonalizado != null)
            {
                objInstanciado.GetComponent<MeshRenderer>().material = materialPersonalizado;
            }
        }

        return objInstanciado;
    }

}
