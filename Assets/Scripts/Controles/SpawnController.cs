using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using System.IO;
using UnityEngine;

public class SpawnController : MonoBehaviour
{

    [SerializeField] int qtdBasePorNoiteLobos = 3, qtdAMaisPorNoiteLobos = 1, qtdMaxLobos = 50, qtdMaxAnimaisAgressivos = 20, qtdMaxAnimaisPassivos = 10;
    [SerializeField] SpawnArea spawnLobos;
    [SerializeField] SpawnArea[] spawnAnimaisAgressivos, spawnAnimaisPassivos;

    [SerializeField][HideInInspector] List<StatsGeral> lobosInGame, animaisAgressivosInGame, animaisPassivosInGame;

    PhotonView PV;

    [System.Serializable]
    public struct SpawnArea
    {
        public string[] nomesPrefab;
        public Transform[] spawnPoints;
    }

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        lobosInGame = new List<StatsGeral>();
        animaisAgressivosInGame = new List<StatsGeral>();
        animaisPassivosInGame = new List<StatsGeral>();
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
        
        InstanciarPrefabPorPathLobos(diaAtual, PV.ViewID);
    }

    public void SpawnarAnimaisAgressivos()
    {
        animaisAgressivosInGame.RemoveAll(animal => {
            if (animal.isDead)
            {
                Destroy(animal.gameObject);
                return true;
            }
            return false;
        });
       
        string path = "Animais/AnimaisAgressivos/Prontos/";
        InstanciarPrefabPorPathAnimais(path, spawnAnimaisAgressivos, true, PV.ViewID);
    }

    public void SpawnarAnimaisPassivos()
    {
        animaisPassivosInGame.RemoveAll(animal => {
            if (animal.isDead)
            {
                Destroy(animal.gameObject);
                return true;
            }
            return false;
        });

        string path = "Animais/AnimaisPassivos/Prontos/";
        InstanciarPrefabPorPathAnimais(path, spawnAnimaisPassivos, false, PV.ViewID);
    }

    public void InstanciarPrefabPorPathLobos(int diaAtual, int viewID)
    {
        int quantidade = qtdBasePorNoiteLobos + (qtdAMaisPorNoiteLobos * diaAtual);

        bool isPhotonConnected = PhotonNetwork.IsConnected;
        string animalRandom = spawnLobos.nomesPrefab[Random.Range(0, spawnLobos.nomesPrefab.Length)];
        string prefabPath = Path.Combine("Inimigos/Lobisomens/", animalRandom);
        GameObject prefab = Resources.Load<GameObject>(prefabPath);

        int spawnPointCount = spawnLobos.spawnPoints.Length;
        int maxLobosToSpawn = Mathf.Min(qtdMaxLobos - lobosInGame.Count, quantidade);

        for (int i = 0; i < maxLobosToSpawn; i++)
        {
            Vector3 position = spawnLobos.spawnPoints[i % spawnPointCount].position;
            Quaternion rotation = spawnLobos.spawnPoints[i % spawnPointCount].rotation;

            GameObject objInstanciado = isPhotonConnected ? PhotonNetwork.Instantiate(prefabPath, position, rotation, 0, new object[] { viewID }) : Instantiate(prefab, position, rotation);

            lobosInGame.Add(objInstanciado.GetComponent<StatsGeral>());
        }
    }

    public void InstanciarPrefabPorPathAnimais(string path, SpawnArea[] spawnsArea, bool isAnimaisAgressivos, int viewID)
    {
        int qtdMaxToSpawn = isAnimaisAgressivos ? qtdMaxAnimaisAgressivos - animaisAgressivosInGame.Count : qtdMaxAnimaisPassivos - animaisPassivosInGame.Count;

        bool isPhotonConnected = PhotonNetwork.IsConnected;

        for (int i = 0; i < qtdMaxToSpawn; i++)
        {
            int randomAreaIndex = Random.Range(0, spawnsArea.Length);
            SpawnArea randomSpawnArea = spawnsArea[randomAreaIndex];

            int randomSpawnPointIndex = Random.Range(0, randomSpawnArea.spawnPoints.Length);
            Vector3 position = randomSpawnArea.spawnPoints[randomSpawnPointIndex].position;
            Quaternion rotation = randomSpawnArea.spawnPoints[randomSpawnPointIndex].rotation;

            string animalRandom = randomSpawnArea.nomesPrefab[Random.Range(0, randomSpawnArea.nomesPrefab.Length)];
            string prefabPath = Path.Combine(path, animalRandom);
            GameObject prefab = Resources.Load<GameObject>(prefabPath);

            GameObject objInstanciado = isPhotonConnected ? PhotonNetwork.Instantiate(prefabPath, position, rotation, 0, new object[] { viewID }) : Instantiate(prefab, position, rotation);

            if (isAnimaisAgressivos) animaisAgressivosInGame.Add(objInstanciado.GetComponent<StatsGeral>());
            else animaisPassivosInGame.Add(objInstanciado.GetComponent<StatsGeral>());
        }
    }

}
