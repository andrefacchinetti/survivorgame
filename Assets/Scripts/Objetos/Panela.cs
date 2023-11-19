using Opsive.Shared.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panela : MonoBehaviour
{

    [SerializeField] public Fogueira fogueira;
    [SerializeField] public SlotConsumivelPanela[] slotsConsumiveis;

    public bool ColocarConsumivelNaPanela(Item item)
    {
        foreach(SlotConsumivelPanela slot in slotsConsumiveis)
        {
            if (!slot.gameObject.activeSelf)
            {
                slot.AtivarSlotPorNomeItem(item.itemIdentifierAmount.ItemDefinition);
                slot.gameObject.SetActive(true);
                return true;
            }
        }
        return false;
    }

    public bool RetirarConsumivelDaPanela()
    {
        foreach (SlotConsumivelPanela slot in slotsConsumiveis)
        {
            if (slot.gameObject.activeSelf)
            {
                slot.DesativarSlots();
                slot.gameObject.SetActive(false);
                return true;
            }
        }
        return false;
    }

    public void RetirarConsumivelDoSlot(SlotConsumivelPanela slot)
    {
        slot.DesativarSlots();
        slot.gameObject.SetActive(false);
    }

    public ItemDefinitionBase ObterConsumivelDaPanela()
    {
        foreach (SlotConsumivelPanela slot in slotsConsumiveis)
        {
            if (slot.gameObject.activeSelf)
            {
                return slot.itemDefinitionNoSlot;
            }
        }
        return null;
    }

}
