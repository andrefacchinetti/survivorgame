using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Opsive.Shared.Inventory;
using Opsive.UltimateCharacterController.Inventory;
using Opsive.UltimateCharacterController.Traits;
using Opsive.UltimateCharacterController.Character;
using Opsive.UltimateCharacterController.Character.Abilities;
using Opsive.Shared.Events;
using Opsive.UltimateCharacterController.Items;
using Opsive.UltimateCharacterController.Items.Actions.Modules.Shootable;

public class EventsPlayerCharacterController : MonoBehaviour
{

    [SerializeField] [HideInInspector] public PlayerController playerController;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();

        EventHandler.RegisterEvent<IItemIdentifier, int, int>(gameObject, "OnInventoryAdjustItemIdentifierAmount", OnAdjustItemIdentifierAmount);
        EventHandler.RegisterEvent<IItemIdentifier>(gameObject, "OnPreInventoryAdjustItemIdentifierAmount", OnPreAdjustItemIdentifierAmount);
        EventHandler.RegisterEvent<CharacterItem, ShootableClipModule>(gameObject, "OnShootableItemClipChange", OnShootableItemClipChanged);
    }

    public void OnDestroy()
    {
        EventHandler.UnregisterEvent<IItemIdentifier, int, int>(gameObject, "OnInventoryAdjustItemIdentifierAmount", OnAdjustItemIdentifierAmount);
        EventHandler.UnregisterEvent<IItemIdentifier>(gameObject, "OnPreInventoryAdjustItemIdentifierAmount", OnPreAdjustItemIdentifierAmount);
        EventHandler.UnregisterEvent<CharacterItem, ShootableClipModule>(gameObject, "OnShootableItemClipChange", OnShootableItemClipChanged);
    }

    private void OnPreAdjustItemIdentifierAmount(IItemIdentifier itemIdentifier)
    {
        if (playerController.inventario.itemBody.Equals(itemIdentifier.GetItemDefinition())) //Itens não removiveis
        {
            return; 
        }
        playerController.inventario.AdicionarMunicoesDoClipNoInventarioAposDroparArma(itemIdentifier);
    }

    private void OnAdjustItemIdentifierAmount(IItemIdentifier itemIdentifier, int qtdAnterior, int qtdAtual)
    {
        if (playerController.inventario.itemBody.Equals(itemIdentifier.GetItemDefinition())) //Itens não removiveis
        {
            return;
        }
        Debug.Log("The inventory used " + itemIdentifier + ",  previousAmount: " + qtdAnterior + " newAmount: " + qtdAtual);
        if (qtdAnterior > qtdAtual)
        {
            playerController.inventario.removendoItemDoInventarioPorNome(itemIdentifier.GetItemDefinition(), qtdAnterior - qtdAtual);
        }
        playerController.inventario.AtualizarHudMunicoesComArmaAtual();
    }

    private void OnShootableItemClipChanged(CharacterItem characterItem, ShootableClipModule shootableClipModule)
    {
        Debug.Log("Shootable Item Clip Changed" + characterItem.name);
        playerController.inventario.AtualizarHudMunicoesComArmaAtual();
    }

}
