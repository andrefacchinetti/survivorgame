using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;


public class SpawnDropaRecursosManager : MonoBehaviour
{

    [SerializeField] int diasPraRespawnarArvores = 5, diasPraRespawnarDropaRecursos = 5;
    int countDiasArvore = 0, countDiasDropaRecursos = 0;

    [SerializeField] List<DropaRecursosSpawn> arvoresSpawn;
    [SerializeField] List<DropaRecursosSpawn> dropaRecursosSpawn;

    [System.Serializable]
    public struct DropaRecursosSpawn
    {
        public GameObject objeto;
        public string prefabPath;
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scaleLossy;
    }

    private void Start()
    {
        countDiasArvore = 0;
        countDiasDropaRecursos = 0;
        arvoresSpawn = new List<DropaRecursosSpawn>();
        dropaRecursosSpawn = new List<DropaRecursosSpawn>();
        foreach (GameObject arvore in GameObject.FindGameObjectsWithTag("Arvore"))
        {
            DropaRecursosStats dropaRecursosStats = arvore.GetComponent<DropaRecursosStats>();
            DropaRecursosSpawn arvoreSpawn = new DropaRecursosSpawn();
            arvoreSpawn.objeto = arvore;
            arvoreSpawn.prefabPath = dropaRecursosStats.pathPrefab;
            arvoreSpawn.position = arvore.transform.position;
            arvoreSpawn.rotation = arvore.transform.rotation;
            arvoreSpawn.scaleLossy = arvore.transform.lossyScale;
            arvoresSpawn.Add(arvoreSpawn);
        }
        foreach (GameObject dropaRecurso in GameObject.FindGameObjectsWithTag("DropaRecursos"))
        {
            DropaRecursosStats dropaRecursosStats = dropaRecurso.GetComponent<DropaRecursosStats>();
            DropaRecursosSpawn arvoreSpawn = new DropaRecursosSpawn();
            arvoreSpawn.objeto = dropaRecurso;
            arvoreSpawn.prefabPath = dropaRecursosStats.pathPrefab;
            arvoreSpawn.position = dropaRecurso.transform.position;
            arvoreSpawn.rotation = dropaRecurso.transform.rotation;
            arvoreSpawn.scaleLossy = dropaRecurso.transform.lossyScale;
            dropaRecursosSpawn.Add(arvoreSpawn);
        }
    }

    public void respawnarDropaRecursos(int viewID)
    {
        if(countDiasArvore >= diasPraRespawnarArvores)
        {
            foreach (DropaRecursosSpawn arvoreSpawn in arvoresSpawn)
            {
                TrySpawnarDropaRecursos(arvoreSpawn, viewID);
            }
            countDiasArvore = 0;
        }
        else
        {
            countDiasArvore++;
        }
        if (countDiasDropaRecursos >= diasPraRespawnarArvores)
        {
            foreach (DropaRecursosSpawn dropaRecursoSpawn in dropaRecursosSpawn)
            {
                TrySpawnarDropaRecursos(dropaRecursoSpawn, viewID);
            }
            countDiasDropaRecursos = 0;
        }
        else
        {
            countDiasDropaRecursos++;
        }
    }

    public void TrySpawnarDropaRecursos(DropaRecursosSpawn arvoreSpawn, int viewID)
    {
        if (arvoreSpawn.objeto == null)
        {
            if (PhotonNetwork.IsConnected)
            {
                arvoreSpawn.objeto = PhotonNetwork.Instantiate(arvoreSpawn.prefabPath, arvoreSpawn.position, arvoreSpawn.rotation, 0, new object[] { viewID });
            }
            else
            {
                GameObject prefab = Resources.Load<GameObject>(arvoreSpawn.prefabPath);
                arvoreSpawn.objeto = Instantiate(prefab, arvoreSpawn.position, arvoreSpawn.rotation);
            }
            arvoreSpawn.objeto.transform.localScale = arvoreSpawn.scaleLossy;
        }
    }

}
