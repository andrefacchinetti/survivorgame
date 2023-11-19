using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Opsive.UltimateCharacterController.Inventory;
using Opsive.Shared.Inventory;

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
        statsGeral = GetComponentInParent<StatsGeral>();
        PV = GetComponentInParent<PhotonView>();
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
