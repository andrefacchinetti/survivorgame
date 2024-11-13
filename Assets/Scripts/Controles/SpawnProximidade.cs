using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class SpawnProximidade : MonoBehaviour
{
    [SerializeField] GameController gameController;
    PhotonView PV;

    [SerializeField] public SpawnAreaProximidade[] spawnsAreas;
    [SerializeField] float detectionRadius = 80f; // Distância para detectar jogadores
    [SerializeField] float checkInterval = 5f; // Intervalo de tempo para verificar proximidade

    private Dictionary<SpawnAreaProximidade, GameObject> activeEnemies = new Dictionary<SpawnAreaProximidade, GameObject>();

    [System.Serializable]
    public struct SpawnAreaProximidade
    {
        public string pathPrefab;
        public Transform spawnPoint;
    }

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    void Start()
    {
        StartCoroutine(CheckProximityRoutine());
    }

    private IEnumerator CheckProximityRoutine()
    {
        while (true)
        {
            CheckProximityAndSpawnOrDestroy();
            yield return new WaitForSeconds(checkInterval); // Espera o intervalo antes de verificar novamente
        }
    }

    private void CheckProximityAndSpawnOrDestroy()
    {
        foreach (SpawnAreaProximidade spawnArea in spawnsAreas)
        {
            bool playerNearby = IsPlayerNearby(spawnArea.spawnPoint.position, detectionRadius);
            bool enemySpawned = activeEnemies.ContainsKey(spawnArea) && activeEnemies[spawnArea] != null;

            if (playerNearby && !enemySpawned)
            {
                // Spawn do inimigo se o jogador está por perto e o inimigo ainda não foi instanciado
                SpawnEnemy(spawnArea);
            }
            else if (!playerNearby && enemySpawned)
            {
                // Destruição do inimigo se não houver jogador por perto
                DestroyEnemy(spawnArea);
            }
        }
    }

    private bool IsPlayerNearby(Vector3 position, float radius)
    {
        int playerLayer = LayerMask.GetMask("SubCharacter");
        Collider[] hitColliders = Physics.OverlapSphere(position, radius, playerLayer);

        return hitColliders.Length > 0; // Retorna true se encontrar pelo menos um jogador
    }

    private void SpawnEnemy(SpawnAreaProximidade spawnArea)
    {
        bool isPhotonConnected = PhotonNetwork.IsConnected;
        GameObject prefab = Resources.Load<GameObject>(spawnArea.pathPrefab);

        GameObject objInstanciado = isPhotonConnected
            ? PhotonNetwork.Instantiate(spawnArea.pathPrefab, spawnArea.spawnPoint.position, Quaternion.identity, 0, new object[] { PV.ViewID })
            : Instantiate(prefab, spawnArea.spawnPoint.position, Quaternion.identity);

        objInstanciado.GetComponent<TubaraoController>().gameController = gameController;
        objInstanciado.GetComponent<TubaraoController>().patrolArea = this.GetComponent<BoxCollider>();
        activeEnemies[spawnArea] = objInstanciado; // Armazena o inimigo instanciado para controle de despawn
    }

    private void DestroyEnemy(SpawnAreaProximidade spawnArea)
    {
        if (activeEnemies.ContainsKey(spawnArea) && activeEnemies[spawnArea] != null)
        {
            // Verifica se o inimigo ainda existe na cena
            if (activeEnemies[spawnArea] && activeEnemies[spawnArea].activeInHierarchy)
            {
                if (PhotonNetwork.IsConnected)
                {
                    PhotonNetwork.Destroy(activeEnemies[spawnArea]);
                }
                else
                {
                    Destroy(activeEnemies[spawnArea]);
                }
            }

            activeEnemies[spawnArea] = null; // Remove o registro do inimigo destruído
        }
    }
}
