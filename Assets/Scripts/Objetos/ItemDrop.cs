using UnityEngine;
using Photon.Pun;
using System.IO;
using Opsive.Shared.Inventory;

public class ItemDrop : MonoBehaviourPunCallbacks
{

    [SerializeField] public ItemDefinitionBase item;
    [SerializeField] public string pathPrefab;
    public bool estaSendoComido = false;

    public static GameObject InstanciarPrefabPorPath(string prefabPath, int quantidade, Vector3 position, Quaternion rotation, Material materialPersonalizado, int viewID)
    {
        GameObject objInstanciado = null;

        for (int i = 0; i < quantidade; i++)
        {            

            if (PhotonNetwork.IsConnected)
            {
                objInstanciado = PhotonNetwork.Instantiate(prefabPath, position, rotation, 0, new object[] { viewID });
            }
            else
            {
                GameObject prefab = Resources.Load<GameObject>(prefabPath);
                if (prefab != null)
                {
                    objInstanciado = Instantiate(prefab, position, rotation);
                }
            }
            if (materialPersonalizado != null)
            {
                objInstanciado.GetComponent<MeshRenderer>().material = materialPersonalizado;
            }
        }

        return objInstanciado;
    }

    public static GameObject InstanciarPrefabPorPrefabMark(string prefabPath, GameObject[] prefabMarks, Vector3 force, int viewID)
    {
        GameObject objInstanciado = null;
        Debug.Log(prefabPath);

        for (int i = 0; i < prefabMarks.Length; i++)
        {
            if (PhotonNetwork.IsConnected)
            {
                objInstanciado = PhotonNetwork.Instantiate(prefabPath, prefabMarks[i].transform.position, prefabMarks[i].transform.rotation, 0, new object[] { viewID });
            }
            else
            {
                GameObject prefab = Resources.Load<GameObject>(prefabPath);
                objInstanciado = Instantiate(prefab, prefabMarks[i].transform.position, prefabMarks[i].transform.rotation);
            }
            objInstanciado.GetComponent<MeshRenderer>().material = prefabMarks[i].GetComponent<MeshRenderer>().material;
            objInstanciado.transform.localScale = prefabMarks[i].transform.lossyScale;
            if(force != Vector3.zero)
            {
                Rigidbody rigid = objInstanciado.GetComponent<Rigidbody>();
                if (rigid != null)
                {
                    rigid.AddForce(force, ForceMode.Impulse);
                }
            }
        }

        return objInstanciado;
    }

}
