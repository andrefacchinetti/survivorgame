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
    [SerializeField] List<GameObject> lobosInGame;

    PhotonView PV;
    

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        lobosInGame = new List<GameObject>();
    }

    public void ReiniciarSpawnPorDiaNoite(bool isNoite, int diaAtual)
    {
        if (isNoite)
        {
            InstanciarPrefabLobosPorPath("BarukAlfa", lobosBasePorNoite + (lobosAMaisPorNoite * diaAtual), PV.ViewID);
        }
        else
        {
            //TODO: Spawnar Animais
        }
    }

    public void InstanciarPrefabLobosPorPath(string nomePrefab, int quantidade, int viewID)
    {
        GameObject objInstanciado = null;
        string prefabPath = Path.Combine("Inimigos/Lobisomens/", nomePrefab);
        for (int i = 0; i < quantidade; i++)
        {
            if (lobosInGame.Count >= qtdMaximaLobos) return;
            if (i >= spawnPointsLobos.Length) i = 0;
            Vector3 position = spawnPointsLobos[i].position;
            Quaternion rotation = spawnPointsLobos[i].rotation;

            position = objInstanciado != null ? objInstanciado.transform.position : position;
            if (PhotonNetwork.IsConnected)
            {
                objInstanciado = PhotonNetwork.Instantiate(prefabPath, position, rotation, 0, new object[] { viewID });
            }
            else
            {
                GameObject prefab = Resources.Load<GameObject>(prefabPath);
                objInstanciado = Instantiate(prefab, position, rotation);
            }
            lobosInGame.Add(objInstanciado);
        }
    }

}
