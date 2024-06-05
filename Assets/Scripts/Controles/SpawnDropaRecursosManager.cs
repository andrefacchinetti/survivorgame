using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;


public class SpawnDropaRecursosManager : MonoBehaviour
{

    [SerializeField] int diasPraRespawnarArvores = 5, diasPraRespawnarDropaRecursos = 5, diasPraRespawnarRecursos = 2;
    [SerializeField] GameObject contentItensDropPadraoDoCenario; //GameObject contendo todos os itens spawnados por padrao no cenario do jogo (gravetos, pedras, lanternas, etc...)

    int countDiasArvore = 0, countDiasDropaRecursos = 0, countDiasRecursos = 0;
    [SerializeField] List<DropaRecursosSpawn> arvoresSpawn;
    [SerializeField] List<DropaRecursosSpawn> dropaRecursosSpawn;
    [SerializeField] List<DropaRecursosSpawn> recursosSpawn;

    [System.Serializable]
    public struct DropaRecursosSpawn
    {
        public GameObject objeto;
        public string prefabPath;
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scaleLossy;
    }

    private void Awake()
    {
        recursosSpawn = new List<DropaRecursosSpawn>();
        foreach (ItemDrop recurso in contentItensDropPadraoDoCenario.GetComponentsInChildren<ItemDrop>())
        {
            if(dropaRecursosSpawn != null)
            {
                DropaRecursosSpawn recursoSpawn = new DropaRecursosSpawn();
                recursoSpawn.objeto = recurso.gameObject;
                recursoSpawn.prefabPath = recurso.pathPrefab;
                recursoSpawn.position = recurso.transform.position;
                recursoSpawn.rotation = recurso.transform.rotation;
                recursoSpawn.scaleLossy = recurso.transform.lossyScale;
                recursosSpawn.Add(recursoSpawn);
            }
        }
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
            if (dropaRecursosSpawn != null)
            {
                DropaRecursosSpawn arvoreSpawn = new DropaRecursosSpawn();
                arvoreSpawn.objeto = arvore;
                arvoreSpawn.prefabPath = dropaRecursosStats.pathPrefab;
                arvoreSpawn.position = arvore.transform.position;
                arvoreSpawn.rotation = arvore.transform.rotation;
                arvoreSpawn.scaleLossy = arvore.transform.lossyScale;
                arvoresSpawn.Add(arvoreSpawn);
            }
        }
        foreach (GameObject dropaRecurso in GameObject.FindGameObjectsWithTag("DropaRecursos"))
        {
            DropaRecursosStats dropaRecursosStats = dropaRecurso.GetComponent<DropaRecursosStats>();
            if (dropaRecursosSpawn != null)
            {
                DropaRecursosSpawn arvoreSpawn = new DropaRecursosSpawn();
                arvoreSpawn.objeto = dropaRecurso;
                arvoreSpawn.prefabPath = dropaRecursosStats.pathPrefab;
                arvoreSpawn.position = dropaRecurso.transform.position;
                arvoreSpawn.rotation = dropaRecurso.transform.rotation;
                arvoreSpawn.scaleLossy = dropaRecurso.transform.lossyScale;
                dropaRecursosSpawn.Add(arvoreSpawn);
            }
        }
    }

    public void respawnarDropaRecursos(int viewID)
    {
        if(countDiasArvore >= diasPraRespawnarArvores)
        {
            foreach (DropaRecursosSpawn arvoreSpawn in arvoresSpawn)
            {
                TrySpawnarDropaRecursosSpawn(arvoreSpawn, viewID);
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
                TrySpawnarDropaRecursosSpawn(dropaRecursoSpawn, viewID);
            }
            countDiasDropaRecursos = 0;
        }
        else
        {
            countDiasDropaRecursos++;
        }
        if (countDiasRecursos >= diasPraRespawnarRecursos)
        {
            foreach (DropaRecursosSpawn recursoSpawn in recursosSpawn)
            {
                TrySpawnarDropaRecursosSpawn(recursoSpawn, viewID);
            }
            countDiasRecursos = 0;
        }
        else
        {
            countDiasRecursos++;
        }
    }

    public void TrySpawnarDropaRecursosSpawn(DropaRecursosSpawn arvoreSpawn, int viewID)
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
