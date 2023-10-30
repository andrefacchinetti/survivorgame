using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using System.IO;
using UnityEngine;

public class SpawnController : MonoBehaviour
{

    [SerializeField] int qtdMaximaLobos = 50;
    [SerializeField] int lobosBasePorNoite = 3, lobosAMaisPorNoite = 1;
    [SerializeField] Transform[] spawnPointsLobos;
    [SerializeField] List<StatsGeral> lobosInGame;

    PhotonView PV;
    

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        lobosInGame = new List<StatsGeral>();
    }

    public void SpawnarLobisomens(int diaAtual)
    {
        lobosInGame.RemoveAll(lobo => {
            if (lobo.isDead)
            {
                Destroy(lobo.gameObject);
                return true;
            }
            return false;
        });
        InstanciarPrefabLobosPorPath("BarukAlfa", lobosBasePorNoite + (lobosAMaisPorNoite * diaAtual), PV.ViewID);
    }

    public void InstanciarPrefabLobosPorPath(string nomePrefab, int quantidade, int viewID)
    {
        Debug.Log("Spawnando " + quantidade + " Lobos");
        string prefabPath = Path.Combine("Inimigos/Lobisomens/", nomePrefab);
        bool isPhotonConnected = PhotonNetwork.IsConnected;

        int spawnPointCount = spawnPointsLobos.Length;

        int totalLobos = lobosInGame.Count + quantidade;
        int maxLobosToSpawn = Mathf.Min(qtdMaximaLobos - lobosInGame.Count, quantidade);

        for (int i = 0; i < maxLobosToSpawn; i++)
        {
            Vector3 position = spawnPointsLobos[i % spawnPointCount].position;
            Quaternion rotation = spawnPointsLobos[i % spawnPointCount].rotation;

            GameObject objInstanciado = null;

            if (isPhotonConnected)
            {
                objInstanciado = PhotonNetwork.Instantiate(prefabPath, position, rotation, 0, new object[] { viewID });
            }
            else
            {
                GameObject prefab = Resources.Load<GameObject>(prefabPath);
                objInstanciado = Instantiate(prefab, position, rotation);
            }

            lobosInGame.Add(objInstanciado.GetComponent<StatsGeral>());
        }
    }

}
