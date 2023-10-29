using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using System.IO;
using UnityEngine;

public class SpawnController : MonoBehaviour
{

    [SerializeField] int qtdMaximaLobos = 50;
    [SerializeField] int lobosAMaisPorNoite = 3; //A cada noite, será spawnado (qtdMaximaLobos * multiplicadorLobosPorNoite)
    [SerializeField] Transform[] spawnPointsLobos;
    [SerializeField] List<GameObject> lobosInGame;

    PhotonView PV;
    

    private void Awake()
    {
        PV = GetComponentInParent<PhotonView>();
        lobosInGame = new List<GameObject>();
    }

    public void ReiniciarSpawnPorDiaNoite(bool isNoite, int diaAtual)
    {
        if (isNoite)
        {
            //lobosInGame.Add(InstanciarPrefabLobosPorPath("BarukAlfa", lobosAMaisPorNoite * diaAtual, PV.ViewID));
        }
        else
        {
            //TODO: Spawnar Animais
        }
    }

    public GameObject InstanciarPrefabLobosPorPath(string nomePrefab, int quantidade, int viewID)
    {
        GameObject objInstanciado = null;
        string prefabPath = Path.Combine("Resources/Inimigos/Lobisomens/", nomePrefab);
        for (int i = 0; i < quantidade; i++)
        {
            float alturaObjetoExistente = objInstanciado != null ? objInstanciado.GetComponent<Renderer>().bounds.size.y : 0;
            int indexSpawnPoint = Random.Range(0, spawnPointsLobos.Length);
            Vector3 position = spawnPointsLobos[indexSpawnPoint].position;
            Quaternion rotation = spawnPointsLobos[indexSpawnPoint].rotation;

            position = objInstanciado != null ? objInstanciado.transform.position : position;
            position = position + new Vector3(0, alturaObjetoExistente, 0);
            if (PhotonNetwork.IsConnected)
            {
                objInstanciado = PhotonNetwork.Instantiate(prefabPath, position, rotation, 0, new object[] { viewID });
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
