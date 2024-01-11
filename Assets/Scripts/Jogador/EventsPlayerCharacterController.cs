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

public class EventsPlayerCharacterController : MonoBehaviour
{

    [SerializeField] [HideInInspector] public PlayerController playerController;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();

        EventHandler.RegisterEvent<IItemIdentifier, int, int>(gameObject, "OnInventoryAdjustItemIdentifierAmount", OnAdjustItemIdentifierAmount);
    }

    public void OnDestroy()
    {
        EventHandler.UnregisterEvent<IItemIdentifier, int, int>(gameObject, "OnInventoryAdjustItemIdentifierAmount", OnAdjustItemIdentifierAmount);
    }

    private void OnAdjustItemIdentifierAmount(IItemIdentifier itemIdentifier, int qtdAnterior, int qtdAtual)
    {
        Debug.Log("The inventory used " + itemIdentifier + ",  previousAmount: " + qtdAnterior + " newAmount: " + qtdAtual);
        if (qtdAnterior > qtdAtual)
        {
            playerController.inventario.removendoItemDoInventarioPorNome(itemIdentifier.GetItemDefinition(), qtdAnterior - qtdAtual);
        }
    }

}
