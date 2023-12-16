using Opsive.UltimateCharacterController.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
	public static SpawnManager Instance;

	SpawnPoint[] spawnpoints; //NAO PRECISA USAR ESSE SCRIPT, PODEMOS USAR O SPAWN DO OPSIVE

	void Awake()
	{
		Instance = this;
		spawnpoints = GetComponentsInChildren<SpawnPoint>();
	}

	public Transform GetSpawnpoint()
	{
		return spawnpoints[Random.Range(0, spawnpoints.Length)].transform;
	}
}
