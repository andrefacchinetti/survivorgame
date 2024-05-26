using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnDropaRecursosManager : MonoBehaviour
{

    List<SpawnPointDropaRecursos> spawnPointsDropaRecursos;

    private void Start()
    {
        spawnPointsDropaRecursos = new List<SpawnPointDropaRecursos>();
        spawnPointsDropaRecursos.AddRange(GetComponentsInChildren<SpawnPointDropaRecursos>());
    }

    public void SpawnarDropaRecursos(int viewID)
    {
        foreach(SpawnPointDropaRecursos spawnPointDropaRecursos in spawnPointsDropaRecursos)
        {
            spawnPointDropaRecursos.TrySpawnarDropaRecursos(viewID);
        }
    }

}
