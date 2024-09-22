using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using System.IO;
using UnityEngine;
using UnityEngine.AI;

public class SpawnController : MonoBehaviour
{

    [SerializeField] GameController gameController;

    [SerializeField] int qtdBasePorNoiteLobos = 3, qtdAMaisPorNoiteLobos = 1, qtdMaxLobos = 50, qtdMaxAnimaisAgressivos = 10, qtdMaxAnimaisPassivos = 15;
    [SerializeField] public float minDistanceSpawnDosPlayers = 15f, maxDistanceSpawnDosPlayers = 50f;
    [SerializeField] float distanciaMaxEntreSpawnPointLoboxJogadores = 100;
    [SerializeField] SpawnArea spawnLobos, spawnAnimaisAquaticos;
    [SerializeField] SpawnArea[] spawnAnimaisAgressivos, spawnAnimaisPassivos;

    [SerializeField] [HideInInspector] List<StatsGeral> lobosInGame, animaisAgressivosInGame, animaisPassivosInGame, animaisAquaticosInGame;

    PhotonView PV;

    [System.Serializable]
    public struct SpawnArea
    {
        public string[] nomesPrefab;
        public GameObject contentSpawnPoints;
        [HideInInspector] public Transform[] spawnPoints;
    }

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        lobosInGame = new List<StatsGeral>();
        animaisAgressivosInGame = new List<StatsGeral>();
        animaisPassivosInGame = new List<StatsGeral>();
        animaisAquaticosInGame = new List<StatsGeral>();

        for (int i = 0; i < spawnAnimaisAgressivos.Length; i++)
        {
            spawnAnimaisAgressivos[i].spawnPoints = spawnAnimaisAgressivos[i].contentSpawnPoints.GetComponentsInChildren<Transform>(true);
        }
        for (int i = 0; i < spawnAnimaisPassivos.Length; i++)
        {
            spawnAnimaisPassivos[i].spawnPoints = spawnAnimaisPassivos[i].contentSpawnPoints.GetComponentsInChildren<Transform>(true);
        }
    }

    public void SpawnarLobisomens(int diaAtual)
    {
        Debug.Log("Spawnando lobisomens");
        lobosInGame.RemoveAll(lobo => {
            if (lobo != null && (!lobo.health.IsAlive() || lobo.GetComponent<LobisomemController>().estouLongeDeAlgumJogador()))
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
        Debug.Log("Spawnando animais agressivos");
        animaisAgressivosInGame.RemoveAll(animal => {
            if (animal != null && !animal.health.IsAlive())
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
        Debug.Log("Spawnando animais passivos");
        animaisPassivosInGame.RemoveAll(animal => {
            if (animal != null && !animal.health.IsAlive())
            {
                Destroy(animal.gameObject);
                return true;
            }
            return false;
        });

        string path = "Animais/AnimaisPassivos/Prontos/";
        InstanciarPrefabPorPathAnimais(path, spawnAnimaisPassivos, false, PV.ViewID);
    }

    public void SpawnarAnimaisAquaticos()
    {
        Debug.Log("Spawnando animais agressivos");
        animaisAquaticosInGame.RemoveAll(animal => {
            if (animal != null && !animal.health.IsAlive())
            {
                Destroy(animal.gameObject);
                return true;
            }
            return false;
        });

        InstanciarPrefabPorPathAnimaisAquaticos(PV.ViewID);
    }

    public void InstanciarPrefabPorPathLobos(int diaAtual, int viewID)
    {
        int quantidade = qtdBasePorNoiteLobos + (qtdAMaisPorNoiteLobos * diaAtual);
        bool isPhotonConnected = PhotonNetwork.IsConnected;
        int maxLobosToSpawn = Mathf.Min(qtdMaxLobos - lobosInGame.Count, quantidade);
        int walkableArea = NavMesh.GetAreaFromName("Walkable");

        for (int i = 0; i < maxLobosToSpawn; i++)
        {
            Vector3 spawnPosition = GetRandomNavMeshPositionNearPlayer(walkableArea);

            if (spawnPosition != Vector3.zero)
            {
                string loboRandom;
                if (Random.value <= 0.3f) loboRandom = spawnLobos.nomesPrefab[0]; //Alfa tem 30% de probabilidade
                else loboRandom = spawnLobos.nomesPrefab[Random.Range(1, spawnLobos.nomesPrefab.Length)]; // Selecionar aleatoriamente entre os demais índices
                string prefabPath = Path.Combine("Inimigos/Lobisomens/", loboRandom);
                GameObject prefab = Resources.Load<GameObject>(prefabPath);
                GameObject objInstanciado = isPhotonConnected ? PhotonNetwork.Instantiate(prefabPath, spawnPosition, Quaternion.identity, 0, new object[] { viewID }) : Instantiate(prefab, spawnPosition, Quaternion.identity);
                objInstanciado.GetComponent<LobisomemController>().gameController = gameController;
                lobosInGame.Add(objInstanciado.GetComponent<StatsGeral>());
            }
        }
    }

    public void InstanciarPrefabPorPathAnimais(string path, SpawnArea[] spawnsArea, bool isAnimaisAgressivos, int viewID)
    {
        int qtdMaxToSpawn = isAnimaisAgressivos ? (qtdMaxAnimaisAgressivos) - animaisAgressivosInGame.Count : (qtdMaxAnimaisPassivos) - animaisPassivosInGame.Count;

        bool isPhotonConnected = PhotonNetwork.IsConnected;

        for (int i = 0; i < qtdMaxToSpawn; i++)
        {
            int randomAreaIndex = Random.Range(0, spawnsArea.Length);
            SpawnArea randomSpawnArea = spawnsArea[randomAreaIndex];

            int randomSpawnPointIndex = Random.Range(0, randomSpawnArea.spawnPoints.Length);
            Vector3 spawnPointPosition = randomSpawnArea.spawnPoints[randomSpawnPointIndex].position;

            // Obter uma posição válida no NavMesh perto do spawnPoint
            Vector3 spawnPosition = GetRandomNavMeshPositionNearSpawnPoint(spawnPointPosition, minDistanceSpawnDosPlayers, maxDistanceSpawnDosPlayers);

            if (spawnPosition != Vector3.zero)
            {
                string animalRandom = randomSpawnArea.nomesPrefab[Random.Range(0, randomSpawnArea.nomesPrefab.Length)];
                string prefabPath = Path.Combine(path, animalRandom);
                GameObject prefab = Resources.Load<GameObject>(prefabPath);

                GameObject objInstanciado = isPhotonConnected ? PhotonNetwork.Instantiate(prefabPath, spawnPosition, Quaternion.identity, 0, new object[] { viewID }) : Instantiate(prefab, spawnPosition, Quaternion.identity);
                objInstanciado.GetComponent<AnimalController>().gameController = gameController;
                if (isAnimaisAgressivos) animaisAgressivosInGame.Add(objInstanciado.GetComponent<StatsGeral>());
                else animaisPassivosInGame.Add(objInstanciado.GetComponent<StatsGeral>());
            }
        }
    }

    public void InstanciarPrefabPorPathAnimaisAquaticos(int viewID)
    {
        int quantidade = Random.Range(1, 3);
        bool isPhotonConnected = PhotonNetwork.IsConnected;
        int maskAgentArea = NavMesh.GetAreaFromName("Aquatico");

        for (int i = 0; i < quantidade; i++)
        {
            Vector3 spawnPosition = GetRandomNavMeshPositionNearPlayer(maskAgentArea);

            if (spawnPosition != Vector3.zero)
            {
                string animalRandom = spawnAnimaisAquaticos.nomesPrefab[Random.Range(0, spawnAnimaisAquaticos.nomesPrefab.Length)];
                string prefabPath = Path.Combine("Animais/Aquaticos/", animalRandom);
                GameObject prefab = Resources.Load<GameObject>(prefabPath);
                GameObject objInstanciado = isPhotonConnected ? PhotonNetwork.Instantiate(prefabPath, spawnPosition, Quaternion.identity, 0, new object[] { viewID }) : Instantiate(prefab, spawnPosition, Quaternion.identity);
                objInstanciado.GetComponent<LobisomemController>().gameController = gameController;
                animaisAquaticosInGame.Add(objInstanciado.GetComponent<StatsGeral>());
            }
        }
    }

    private Vector3 GetRandomNavMeshPositionNearPlayer(int agentAreaMask)
    {
        // Verifica se há jogadores online
        if (gameController.playersOnline == null || gameController.playersOnline.Length == 0)
            gameController.playersOnline = GameObject.FindGameObjectsWithTag("Player");
        if (gameController.playersOnline == null || gameController.playersOnline.Length == 0)
            return Vector3.zero;

        // Pega um jogador aleatório para calcular a posição
        int index = Random.Range(0, gameController.playersOnline.Length);
        Vector3 randomDirection = Random.insideUnitSphere * Random.Range(minDistanceSpawnDosPlayers, maxDistanceSpawnDosPlayers);
        randomDirection += gameController.playersOnline[index].transform.position;

        // Encontra a posição correta no NavMesh com a máscara de área específica
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, maxDistanceSpawnDosPlayers, 1 << agentAreaMask))
        {
            return hit.position; // Retorna a posição válida
        }

        return Vector3.zero; // Retorna zero caso nenhuma posição seja válida
    }

    private Vector3 GetRandomNavMeshPositionNearSpawnPoint(Vector3 spawnPointPosition, float minDistance, float maxDistance)
    {
        Vector3 randomDirection = Random.insideUnitSphere * Random.Range(minDistance, maxDistance);
        randomDirection += spawnPointPosition;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, maxDistance, 1 << NavMesh.GetAreaFromName("Walkable")))
        {
            return hit.position;
        }
        return Vector3.zero;
    }

}