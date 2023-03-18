using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class StatsGeral : MonoBehaviour
{
    [SerializeField] public float vidaMaxima = 100, damage = 20;
    [HideInInspector] public float vidaAtual = 100f; // pontos de vida
    [HideInInspector] public bool isDead = false, isAttacking;

    //ATAQUE
    [SerializeField] public float minimumDistanceAtaque = 5f, distanciaDePerseguicao = 10f, distanciaDeAtaque = 2f;
    [SerializeField] public float attackInterval = 1f; // Intervalo de tempo entre ataques
    [HideInInspector] public float lastAttackTime; // Tempo do �ltimo ataque
    [SerializeField] public float destinationOffset = 1f;
    [SerializeField] public float speedVariation = 0.5f;
    [SerializeField] public float leadTime = 1.2f, leadDistance = 2;

    [SerializeField] public List<Item.ItemDropStruct> dropsItems;

    LobisomemStats lobisomemStats;
    AnimalStats animalStats;
    DropaRecursosStats dropaRecursosStats;
    PhotonView PV;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        lobisomemStats = GetComponentInParent<LobisomemStats>();
        animalStats = GetComponentInParent<AnimalStats>();
        dropaRecursosStats = GetComponentInParent<DropaRecursosStats>();
    }

    private void Start()
    {
        vidaAtual = vidaMaxima;
    }

    public void TakeDamage(float damage)
    {
        vidaAtual -= damage;

        if (vidaAtual > 0)
        {
            if (animalStats != null) animalStats.AcoesTomouDano();
            else if (lobisomemStats != null) lobisomemStats.AcoesTomouDano();
            else if (dropaRecursosStats != null) dropaRecursosStats.AcoesTomouDano();
        }
        else
        {
            DroparItensAoMorrer();
            if (animalStats != null) animalStats.AcoesMorreu();
            else if (lobisomemStats != null) lobisomemStats.AcoesMorreu();
            else if (dropaRecursosStats != null) dropaRecursosStats.AcoesMorreu();
        }
    }

    public void DroparItensAoMorrer()
    {
        foreach (Item.ItemDropStruct drop in dropsItems)
        {
            if (!Item.TiposItems.Nenhum.ToString().Equals(drop.nomeItemEnum.GetTipoItemEnum()))
            {
                int quantidade = Random.Range(drop.qtdMinDrops, drop.qtdMaxDrops);
                string nomePrefab = drop.nomeItemEnum.GetTipoItemEnum() + "/" + drop.nomeItemEnum.ToString();
                ItemDrop.InstanciarPrefabPorPath(nomePrefab, quantidade, transform.position, transform.rotation, PV.ViewID);
            }
        }
    }

    public void DestruirGameObject()
    {
        if (PhotonNetwork.IsConnected) PhotonNetwork.Destroy(this.gameObject);
        else GameObject.Destroy(this.gameObject);
    }

}
