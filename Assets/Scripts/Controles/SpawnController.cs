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

    public void SpawnarLobisomens(int diaAtual)
    {
        InstanciarPrefabLobosPorPath("BarukAlfa", lobosBasePorNoite + (lobosAMaisPorNoite * diaAtual), PV.ViewID);
    }

    public void InstanciarPrefabLobosPorPath(string nomePrefab, int quantidade, int viewID)
    {
        Debug.Log("Spawnando " + quantidade + " Lobos");

        GameObject objInstanciado = null;
        string prefabPath = Path.Combine("Inimigos/Lobisomens/", nomePrefab);

        for (int i = 0; i < quantidade; i++)
        {
            if (lobosInGame.Count >= qtdMaximaLobos)
            {
                // Se atingiu a quantidade máxima de lobos, interrompe o loop
                Debug.Log("Quantidade máxima de lobos atingida.");
                break;
            }

            Vector3 position = spawnPointsLobos[i % spawnPointsLobos.Length].position;
            Quaternion rotation = spawnPointsLobos[i % spawnPointsLobos.Length].rotation;

            // Atribui a posição do último objeto instanciado, se existir
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

            // Adiciona o lobo instanciado à lista
            lobosInGame.Add(objInstanciado);
        }
    }

}
