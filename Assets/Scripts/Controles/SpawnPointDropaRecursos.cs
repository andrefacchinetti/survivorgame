using UnityEngine;
using Photon.Pun;

public class SpawnPointDropaRecursos : MonoBehaviour
{

    [SerializeField] public bool podeRespawnar = true;
    [SerializeField] string prefabPath; //EX: Prefabs/DropaRecursos/Frutiferas/Laranjeira
    [SerializeField] GameObject objReferencia;
    private GameObject dropaRecursosObj;

    private Vector3 position, lossyScale;
    private Quaternion rotation;
    private bool spawnouPrimeiraVez = false;

    private void Awake()
    {
        position = objReferencia.transform.position;
        rotation = objReferencia.transform.rotation;
        lossyScale = objReferencia.transform.lossyScale;
        PhotonNetwork.Destroy(objReferencia);
    }

    public void TrySpawnarDropaRecursos(int viewID)
    {
        if(dropaRecursosObj == null && (podeRespawnar || !spawnouPrimeiraVez))
        {
            if (PhotonNetwork.IsConnected)
            {
                dropaRecursosObj = PhotonNetwork.Instantiate(prefabPath, position, rotation, 0, new object[] { viewID });
            }
            else
            {
                GameObject prefab = Resources.Load<GameObject>(prefabPath);
                dropaRecursosObj = Instantiate(prefab, position, rotation);
            }
            dropaRecursosObj.transform.localScale = lossyScale;
            spawnouPrimeiraVez = true;
        }
    }

}
