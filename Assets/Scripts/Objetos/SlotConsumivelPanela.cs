using Opsive.Shared.Inventory;
using Opsive.UltimateCharacterController.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotConsumivelPanela : MonoBehaviour
{

    [HideInInspector] public ItemDefinitionBase itemDefinitionNoSlot;
    [SerializeField] public GameObject slotCarneCrua, slotCarneCozida;
    [SerializeField] public GameObject slotCogumeloCru, slotCogumeloCozido;
    [SerializeField] public GameObject slotPeixeCru, slotPeixeCozido;

    [SerializeField] public ItemDefinitionBase itemCarneCrua, itemCarneCozida, itemCogumeloCru, itemCogumeloCozido, itemPeixeCru, itemPeixeCozido;

    public void AtivarSlotPorNomeItem(ItemDefinitionBase itemDefinition)
    {
        itemDefinitionNoSlot = itemDefinition;
        if (itemDefinition.name.Equals(itemCarneCrua.name)) slotCarneCrua.SetActive(true);
        if (itemDefinition.name.Equals(itemCarneCozida.name)) slotCarneCozida.SetActive(true);

        if (itemDefinition.name.Equals(itemCogumeloCru.name)) slotCogumeloCru.SetActive(true);
        if (itemDefinition.name.Equals(itemCogumeloCozido.name)) slotCogumeloCozido.SetActive(true);

        if (itemDefinition.name.Equals(itemPeixeCru.name)) slotPeixeCru.SetActive(true);
        if (itemDefinition.name.Equals(itemPeixeCozido.name)) slotPeixeCozido.SetActive(true);
    }

    public void DesativarSlots()
    {
        itemDefinitionNoSlot = null;
        slotCarneCrua.SetActive(false);
        slotCarneCozida.SetActive(false);
        slotCogumeloCru.SetActive(false);
        slotCogumeloCozido.SetActive(false);
        slotPeixeCru.SetActive(false);
        slotPeixeCozido.SetActive(false);
    }

}
