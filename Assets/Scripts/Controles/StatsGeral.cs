using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Opsive.Shared.Inventory;
using Opsive.Shared.Events;
using Opsive.UltimateCharacterController.Traits;
using System.IO;

public class StatsGeral : MonoBehaviour
{

    [SerializeField] public float damage;
    [SerializeField] public GameObject objPaiParaDestruir;
    [SerializeField] public List<Item.ItemDropStruct> dropsItems;
    [SerializeField] public List<Item.ObjDropStruct> dropsObjs;
    [SerializeField] public GameObject dropPosition;
    [SerializeField] public char direcaoDrop = 'y';
    [HideInInspector] public bool isAttacking;

    [SerializeField] public Health health;
    [HideInInspector] public AttributeManager attributeManager;
    [HideInInspector] public StatsJogador jogadorStats;
    [HideInInspector] public LobisomemStats lobisomemStats;
    [HideInInspector] public AnimalStats animalStats;
    [HideInInspector] public DropaRecursosStats dropaRecursosStats;
    [HideInInspector] public ReconstruivelStats reconstruivelStats;
    [HideInInspector] public ConstrucaoStats construcaoStats;
    [HideInInspector] public CollisorSofreDano collisorSofreDano;

    public EstilhacoFxController.TipoEstilhaco tipoEstilhaco;
    [HideInInspector] public BFX_DemoTest bloodController;
    EstilhacoFxController estilhacoFxController;

    PhotonView PV;

    private void Awake()
    {
        EventHandler.RegisterEvent<float, Vector3, Vector3, GameObject, Collider>(gameObject, "OnHealthDamage", OnDamage);
        EventHandler.RegisterEvent<float, Vector3, GameObject, Collider>(gameObject, "OnPreHealthDamage", OnPreDamage);
        EventHandler.RegisterEvent<float>(gameObject, "OnFallDamage", OnActionFallDamage);
        PV = GetComponent<PhotonView>();
        lobisomemStats = GetComponentInParent<LobisomemStats>();
        animalStats = GetComponentInParent<AnimalStats>();
        dropaRecursosStats = GetComponentInParent<DropaRecursosStats>();
        jogadorStats = GetComponentInParent<StatsJogador>();
        reconstruivelStats = GetComponentInParent<ReconstruivelStats>();
        construcaoStats = GetComponentInParent<ConstrucaoStats>();
        collisorSofreDano = GetComponentInParent<CollisorSofreDano>();
        if (health == null) health = GetComponentInParent<Health>();
        attributeManager = GetComponentInParent<AttributeManager>();
        if (dropPosition == null) dropPosition = this.gameObject;
        if(EstilhacoFxController.TipoEstilhaco.Sangue.Equals(tipoEstilhaco)) bloodController = GameObject.FindGameObjectWithTag("BloodController").GetComponent<BFX_DemoTest>();
        else if (!EstilhacoFxController.TipoEstilhaco.Nenhum.Equals(tipoEstilhaco)) estilhacoFxController = GameObject.FindGameObjectWithTag("BloodController").GetComponent<EstilhacoFxController>();
    }

    private void LateUpdate()
    {
        if (transform.position.y < -40)
        {
            TakeDamage(9999, false);
        }
    }

    private void OnActionFallDamage(float fallDamageValue)
    {
        if(fallDamageValue >= 20 && jogadorStats != null)
        {
            jogadorStats.FraturarJogador();
        }
    }

    private void OnPreDamage(float dano, Vector3 position, GameObject attacker, Collider hitCollider)
    {
        //Evento que acontece antes de aplicar o damage no Health
        if(attacker != null)
        {
            if (construcaoStats != null) //Verificar se cura construcao com martelo reparador
            {
                PlayerController pc = attacker.GetComponentInParent<PlayerController>();
                if (pc != null)
                {
                    if (pc.inventario.itemNaMao != null && pc.inventario.itemNaMao.itemIdentifierAmount.ItemDefinition.name == construcaoStats.itemMarteloReparador.name)
                    {
                        TakeCura(dano);
                        return;
                    }
                }
            }
            if (collisorSofreDano != null)
            {
                if (collisorSofreDano.isApenasFerramentaRecomendadaCausaDano)
                {
                    PlayerController pc = attacker.GetComponentInParent<PlayerController>();
                    if (pc != null && pc.inventario.itemNaMao != null)
                    {
                        dano = collisorSofreDano.CalcularDanoPorArmaCausandoDano(pc.inventario.itemNaMao.itemIdentifierAmount.ItemDefinition, dano);
                    }
                    else //Else serve para evitar que lobisomens ou outras coisas causem dano em dropa recursos
                    {
                        dano = 0;
                    }
                }
            }
        }
        Debug.Log("aplicando dano");
        health.AplicarDanoNoHealth(dano);
    }

    private void OnDamage(float amount, Vector3 position, Vector3 force, GameObject attacker, Collider hitCollider)
    {
        //Evento que acontece depois de aplicar o damage no Health
        if (hitCollider != null && EstilhacoFxController.TipoEstilhaco.Sangue.Equals(tipoEstilhaco)) bloodController.SangrarAlvo(hitCollider, attacker.transform.position, -1);
        else if (hitCollider != null && !EstilhacoFxController.TipoEstilhaco.Nenhum.Equals(tipoEstilhaco)) estilhacoFxController.GerarEstilhaco(tipoEstilhaco, position, attacker.transform.position);
        if (attacker != null)
        {
            PlayerController pc = attacker.GetComponentInParent<PlayerController>(); //TODO: OTIMIZAR ISSO
            if (pc != null)
            {
                pc.animatorJogador.SetTrigger("acertouAtaque");
            }
        }
        Debug.Log("OnDamage");
        AcoesTomouDano();
    }

    public void OnDestroy()
    {
        EventHandler.UnregisterEvent<float, Vector3, Vector3, GameObject, Collider>(gameObject, "OnHealthDamage", OnDamage);
        EventHandler.UnregisterEvent<float, Vector3, GameObject, Collider>(gameObject, "OnPreHealthDamage", OnPreDamage);
        EventHandler.UnregisterEvent<float>(gameObject, "OnFallDamage", OnActionFallDamage);
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
            health.Heal(curaValue);
        }
    }

    public void TakeDamage(float damageValue, bool isPodeCausarSangramento) // metodo usadno para Dano causado por fatores externos (sem ser por dano de arma do player)
    {
        if (jogadorStats != null) //jogador
        {
            jogadorStats.TakeDamageHealth(damageValue, isPodeCausarSangramento, true);
        }
        else //outros
        {
            health.Damage(damageValue);
        }
    }

    public void AcoesTomouDano() //� chamado no event invocado apos o Health receber o Damage()
    {
        if (jogadorStats != null)
        {
            jogadorStats.AtualizarImgVida();
        }
        if (health.HealthValue > 0) //SOBREVIVEU
        {
            if (animalStats != null) animalStats.AcoesTomouDano();
            else if (lobisomemStats != null) lobisomemStats.AcoesTomouDano();
            else if (dropaRecursosStats != null) dropaRecursosStats.AcoesTomouDano();
            else if (reconstruivelStats != null) reconstruivelStats.AcoesTomouDano();
            else if (construcaoStats != null) construcaoStats.AcoesTomouDano();
        }
        else //MORREU
        {
            if (animalStats != null) animalStats.AcoesMorreu();
            else if (lobisomemStats != null) lobisomemStats.AcoesMorreu();
            else if (dropaRecursosStats != null) dropaRecursosStats.AcoesMorreu();
            else if (jogadorStats != null) jogadorStats.AcoesMorreu();
            else if(reconstruivelStats != null) reconstruivelStats.AcoesMorreu();
            else if (construcaoStats != null) construcaoStats.AcoesMorreu();
            else
            {
                DestruirGameObject();
            }
        }
    }

    public void DroparItensDaMochila()
    {
        if (jogadorStats == null) return; //NAO � UM JOGADOR

        List<Item> itensParaRemover = new List<Item>(jogadorStats.playerController.inventario.itens);

        foreach (Item item in itensParaRemover)
        {
            int quantidade = item.quantidade;
            string nomePrefab = item.tipoItem + "/" + item.itemIdentifierAmount.ItemDefinition.name;
            string prefabPath = Path.Combine("Prefabs/ItensInventario/", nomePrefab);
            ItemDrop.InstanciarPrefabPorPath(prefabPath, quantidade, dropPosition.transform.position, dropPosition.transform.rotation, null, PV.ViewID);
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
                string nomePrefab = drop.tipoItem + "/" + drop.itemDefinition.name;
                string prefabPath = Path.Combine("Prefabs/ItensInventario/", nomePrefab);
                if (drop.prefabDropMarks != null && drop.prefabDropMarks.Length > 0)
                {
                    ItemDrop.InstanciarPrefabPorPrefabMark(prefabPath, drop.prefabDropMarks, Vector3.zero, PV.ViewID);
                }
                else
                {
                    float alturaObjetoExistente = dropPosition != null ? dropPosition.GetComponent<Collider>().bounds.size.y : 0;
                    Vector3 spawnPosition = dropPosition.transform.position + new Vector3(0, alturaObjetoExistente, 0);
                    ItemDrop.InstanciarPrefabPorPath(prefabPath, quantidade, spawnPosition, dropPosition.transform.rotation, drop.materialPersonalizado, PV.ViewID);
                }
            }
        }
    }

    public void DroparObjetosAoSerDestruido()
    {
        Vector3 force = new Vector3(10, 0, 10);
        foreach (Item.ObjDropStruct drop in dropsObjs)
        {
            string prefabPath = drop.prefabPath;
            if (drop.prefabDropMarks != null && drop.prefabDropMarks.Length > 0)
            {
                ItemDrop.InstanciarPrefabPorPrefabMark(prefabPath, drop.prefabDropMarks, force, PV.ViewID);
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
            if (PhotonNetwork.IsConnected) PhotonNetwork.Destroy(objPaiParaDestruir != null ? objPaiParaDestruir.gameObject : this.gameObject);
            else GameObject.Destroy(objPaiParaDestruir != null ? objPaiParaDestruir.gameObject : this.gameObject);
        }
    }

    public bool isVivo()
    {
        if (jogadorStats != null) return jogadorStats.playerController.characterHealth.IsAlive();
        return health.IsAlive();
    }

}
