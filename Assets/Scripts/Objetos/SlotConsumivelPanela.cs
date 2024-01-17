using Opsive.Shared.Inventory;
using Opsive.UltimateCharacterController.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotConsumivelPanela : MonoBehaviour
{

    [HideInInspector] public ItemDefinitionBase itemDefinitionNoSlot;
    [SerializeField] public GameObject slotCarneCrua, slotCarneCozida, slotCarneEstragada, slotCarneQueimada;
    [SerializeField] public GameObject slotPeixeCru, slotPeixeCozido, slotPeixeEstragado, slotPeixeQueimado;

    [SerializeField] public ItemDefinitionBase itemCarneCrua, itemCarneCozida, itemCarneEstragada, itemCarneQueimada,
                                               itemPeixeCru, itemPeixeCozido, itemPeixeEstragado, itemPeixeQueimado;
  

    public void AtivarSlotPorNomeItem(ItemDefinitionBase itemDefinition)
    {
        itemDefinitionNoSlot = itemDefinition;
        if (itemDefinition.name.Equals(itemCarneCrua.name)) slotCarneCrua.SetActive(true);
        if (itemDefinition.name.Equals(itemCarneCozida.name)) slotCarneCozida.SetActive(true);
        if (itemDefinition.name.Equals(itemCarneEstragada.name)) slotCarneEstragada.SetActive(true);
        if (itemDefinition.name.Equals(itemCarneQueimada.name)) slotCarneQueimada.SetActive(true);

        if (itemDefinition.name.Equals(itemPeixeCru.name)) slotPeixeCru.SetActive(true);
        if (itemDefinition.name.Equals(itemPeixeCozido.name)) slotPeixeCozido.SetActive(true);
        if (itemDefinition.name.Equals(itemPeixeEstragado.name)) slotPeixeEstragado.SetActive(true);
        if (itemDefinition.name.Equals(itemPeixeQueimado.name)) slotPeixeQueimado.SetActive(true);
    }

    public void DesativarSlots()
    {
        itemDefinitionNoSlot = null;
        slotCarneCrua.SetActive(false);
        slotCarneCozida.SetActive(false);
        slotCarneEstragada.SetActive(false);
        slotCarneQueimada.SetActive(false);

        slotPeixeCru.SetActive(false);
        slotPeixeCozido.SetActive(false);
        slotPeixeEstragado.SetActive(false);
        slotPeixeQueimado.SetActive(false);
    }

}
