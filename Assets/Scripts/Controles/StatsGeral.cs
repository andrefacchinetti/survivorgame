using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Opsive.Shared.Inventory;
using Opsive.Shared.Events;
using Opsive.UltimateCharacterController.Traits;

public class StatsGeral : MonoBehaviour
{

    [SerializeField] public float damage;
    [SerializeField] public GameObject objPaiParaDestruir;
    [SerializeField] public List<Item.ItemDropStruct> dropsItems;
    [SerializeField] public GameObject dropPosition;
    [HideInInspector] public bool isAttacking;

    [HideInInspector] public Health health;
    [HideInInspector] public AttributeManager attributeManager;
    [HideInInspector] public StatsJogador jogadorStats;
    LobisomemStats lobisomemStats;
    AnimalStats animalStats;
    DropaRecursosStats dropaRecursosStats;
    ReconstruivelStats reconstruivelStats;
    [SerializeField] public ItemDefinitionBase itemRepairHammer, itemDemolitionHammer;

    PhotonView PV;

    private void Awake()
    {
        EventHandler.RegisterEvent<float, Vector3, Vector3, GameObject, Collider>(gameObject, "OnHealthDamage", OnDamage);
        PV = GetComponent<PhotonView>();
        lobisomemStats = GetComponentInParent<LobisomemStats>();
        animalStats = GetComponentInParent<AnimalStats>();
        dropaRecursosStats = GetComponentInParent<DropaRecursosStats>();
        jogadorStats = GetComponentInParent<StatsJogador>();
        reconstruivelStats = GetComponentInParent<ReconstruivelStats>();
        health = GetComponentInParent<Health>();
        attributeManager = GetComponentInParent<AttributeManager>();
        if (dropPosition == null) dropPosition = this.gameObject;
    }

    private void OnDamage(float amount, Vector3 position, Vector3 force, GameObject attacker, Collider hitCollider)
    {
        Debug.Log("Object took " + amount + " damage at position " + position + " with force " + force + " by attacker " + attacker + ". The collider " + hitCollider + " was hit.");
        AcoesTomouDano();
    }

    public void OnDestroy()
    {
        EventHandler.UnregisterEvent<float, Vector3, Vector3, GameObject, Collider>(gameObject, "OnHealthDamage", OnDamage);
    }

    public float ObterVidaMaximaHealth()
    {
        if (jogadorStats != null) return jogadorStats.ObterVidaMaximaHealth();
        else return attributeManager.GetAttribute(health.HealthAttributeName).MaxValue;
    }

    public void TakeCura(float curaValue)
    {
        if (jogadorStats != null) //jogador
        {
            jogadorStats.TakeHealHealth(curaValue);
        }
        else //outros
        {
            GetComponent<Health>().Heal(curaValue);
        }
    }

    public void AcoesTomouDano()
    {
        
        if (GetComponent<Health>().HealthValue > 0) //SOBREVIVEU
        {
            if (animalStats != null) animalStats.AcoesTomouDano();
            else if (lobisomemStats != null) lobisomemStats.AcoesTomouDano();
            else if (dropaRecursosStats != null) dropaRecursosStats.AcoesTomouDano();
            else if (reconstruivelStats != null) reconstruivelStats.AcoesTomouDano();
        }
        else //MORREU
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

    public void DroparItensDaMochila()
    {
        if (jogadorStats == null) return; //NAO É UM JOGADOR

        List<Item> itensParaRemover = new List<Item>(jogadorStats.playerController.inventario.itens);

        foreach (Item item in itensParaRemover)
        {
            int quantidade = item.quantidade;
            string nomePrefab = item.tipoItem + "/" + item.itemIdentifierAmount.ItemDefinition.name;
            ItemDrop.InstanciarPrefabPorPath(nomePrefab, quantidade, dropPosition.transform.position, dropPosition.transform.rotation, PV.ViewID);
            jogadorStats.playerController.inventario.RemoverItemDoInventario(item, quantidade);
        }
    }

    public void DroparItensAoMorrer()
    {
        foreach (Item.ItemDropStruct drop in dropsItems)
        {
            if (!Item.TiposItems.Nenhum.ToString().Equals(drop.tipoItem))
            {
                int quantidade = Random.Range(drop.qtdMinDrops, drop.qtdMaxDrops);
                string nomePrefab = drop.tipoItem + "/" + drop.itemIdentifierAmount.ItemDefinition.name;
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
