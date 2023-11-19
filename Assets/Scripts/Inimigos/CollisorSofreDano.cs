using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Opsive.UltimateCharacterController.Inventory;
using Opsive.Shared.Inventory;
using Opsive.Shared.Events;
using Opsive.UltimateCharacterController.Items.Actions.Impact;

[RequireComponent(typeof(Rigidbody))]
public class CollisorSofreDano : MonoBehaviourPunCallbacks
{

    [SerializeField] ItemIdentifierAmount[] itemIdentifierAmountFerramentasRecomendadas;
    [SerializeField] public bool isApenasFerramentaRecomendadaCausaDano = false;
    [HideInInspector] public StatsGeral statsGeral;
    [SerializeField] public bool isConstrucao;
    public PhotonView PV;

    private void Awake()
    {
        EventHandler.RegisterEvent<ImpactCallbackContext>(gameObject, "OnObjectImpact", OnImpact);
        statsGeral = GetComponentInParent<StatsGeral>();
        PV = GetComponentInParent<PhotonView>();
    }

    private void OnImpact(ImpactCallbackContext ctx)
    {
        Debug.Log("Event received " + name + " impacted by " + ctx.ImpactCollisionData.SourceGameObject + " on collider " + ctx.ImpactCollisionData.ImpactCollider + ".");
    }

    public float CalcularDanoPorArmaCausandoDano(ItemObjMao itemNaMao, float damage)
    {
        if (itemNaMao == null) return damage;
        if (isApenasFerramentaRecomendadaCausaDano)
        {
            if (estaNaListaDeFerramentas(itemNaMao.itemDefinition))
            {
                damage += itemNaMao.damage;
            }
            else
            {
                damage = 0;
            }
        }
        else
        {
            damage += itemNaMao.damage;
        }
        return damage;
    }

    private bool estaNaListaDeFerramentas(ItemDefinitionBase itemDefinition)
    {
        foreach(ItemIdentifierAmount itemIdentifierAmount in itemIdentifierAmountFerramentasRecomendadas)
        {
            if (itemIdentifierAmount.ItemDefinition.name.Equals(itemDefinition.name))
            {
                return true;
            }
        }
        return false;
    }

}
