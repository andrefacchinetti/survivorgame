using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class StatsGeral : MonoBehaviour
{
    [SerializeField] public float vidaMaxima = 100, damage = 20;
    [SerializeField] public float vidaAtual = 100f; // pontos de vida

    [SerializeField] public bool isDead = false;
    [HideInInspector] public bool isAttacking;

    [SerializeField] public List<Item.ItemDropStruct> dropsItems;

    StatsJogador jogadorStats;
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
        jogadorStats = GetComponentInParent<StatsJogador>();
    }

    private void Start()
    {
        vidaAtual = vidaMaxima;
    }

    public void TakeDamage(float damage)
    {
        if(jogadorStats != null) jogadorStats.setarVidaAtual(vidaAtual - damage);
        else vidaAtual -= damage;

        if (vidaAtual > 0)
        {
            if (animalStats != null) animalStats.AcoesTomouDano();
            else if (lobisomemStats != null) lobisomemStats.AcoesTomouDano();
            else if (dropaRecursosStats != null) dropaRecursosStats.AcoesTomouDano();
            else if (jogadorStats != null) jogadorStats.AcoesTomouDano();
        }
        else
        {
            if (animalStats != null) animalStats.AcoesMorreu();
            else if (lobisomemStats != null) lobisomemStats.AcoesMorreu();
            else if (dropaRecursosStats != null) dropaRecursosStats.AcoesMorreu();
            else if (jogadorStats != null) jogadorStats.AcoesMorreu();
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

    public Transform obterTransformPositionDoCollider()
    {
        if(lobisomemStats != null)
        {
            return GetComponent<LobisomemController>().obterGameObjectFormaAtiva().transform;
        }
        else
        {
            return transform;
        }
    }

}
