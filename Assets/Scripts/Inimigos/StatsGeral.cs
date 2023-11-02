using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class StatsGeral : MonoBehaviour
{
    [SerializeField] public float vidaMaxima = 100, damage = 20;
    [SerializeField] public float vidaAtual = 100f; // pontos de vida

    [SerializeField] public bool isDead = false;
    [SerializeField] public GameObject objPaiParaDestruir;

    [SerializeField] public List<Item.ItemDropStruct> dropsItems;
    [SerializeField] public GameObject dropPosition;
    [HideInInspector] public bool isAttacking;

    StatsJogador jogadorStats;
    LobisomemStats lobisomemStats;
    AnimalStats animalStats;
    DropaRecursosStats dropaRecursosStats;
    ReconstruivelStats reconstruivelStats;
    PhotonView PV;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        lobisomemStats = GetComponentInParent<LobisomemStats>();
        animalStats = GetComponentInParent<AnimalStats>();
        dropaRecursosStats = GetComponentInParent<DropaRecursosStats>();
        jogadorStats = GetComponentInParent<StatsJogador>();
        reconstruivelStats = GetComponentInParent<ReconstruivelStats>();
        if (dropPosition == null) dropPosition = this.gameObject;
    }

    private void Start()
    {
        vidaAtual = vidaMaxima;
    }

    public void TakeDamage(float damage)
    {
        Debug.Log("Tomando dano");
        if(jogadorStats != null) jogadorStats.setarVidaAtual(vidaAtual - damage);
        else vidaAtual -= damage;

        if (vidaAtual > 0)
        {
            if (animalStats != null) animalStats.AcoesTomouDano();
            else if (lobisomemStats != null) lobisomemStats.AcoesTomouDano();
            else if (dropaRecursosStats != null) dropaRecursosStats.AcoesTomouDano();
            else if (jogadorStats != null) jogadorStats.AcoesTomouDano();
            else if (reconstruivelStats != null) reconstruivelStats.AcoesTomouDano();
        }
        else
        {
            if (animalStats != null) animalStats.AcoesMorreu();
            else if (lobisomemStats != null) lobisomemStats.AcoesMorreu();
            else if (dropaRecursosStats != null) dropaRecursosStats.AcoesMorreu();
            else if (jogadorStats != null) jogadorStats.AcoesMorreu();
            else if(reconstruivelStats != null)
            {
                reconstruivelStats.AcoesMorreu();
            }
            else
            {
                DestruirGameObject();
            }
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
                ItemDrop.InstanciarPrefabPorPath(nomePrefab, quantidade, dropPosition.transform.position, dropPosition.transform.rotation, PV.ViewID);
            }
        }
    }

    public void DestruirGameObject()
    {
        if(this.gameObject.tag == "ConstrucaoStats")
        {
            ConstrucoesController construcaoController = this.GetComponentInParent<ConstrucoesController>();
            construcaoController.MandarDestruirTodasAsConstrucoesConectadas();
        }
        else
        {
            if (PhotonNetwork.IsConnected) PhotonNetwork.Destroy(objPaiParaDestruir != null ? objPaiParaDestruir : this.gameObject);
            else GameObject.Destroy(objPaiParaDestruir != null ? objPaiParaDestruir : this.gameObject);
        }
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
