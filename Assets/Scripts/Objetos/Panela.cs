using Opsive.Shared.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panela : MonoBehaviour
{

    [SerializeField] public Fogueira fogueira;
    [SerializeField] public SlotConsumivelPanela[] slotsConsumiveis;
    [SerializeField] public ItemDefinitionBase[] itensPodemAssar;

    public bool ColocarConsumivelNaPanela(Item item)
    {
        foreach(SlotConsumivelPanela slot in slotsConsumiveis)
        {
            if (!slot.gameObject.activeSelf)
            {
                if (this.podeAssar(item.itemIdentifierAmount.ItemDefinition))
                {
                    slot.AtivarSlotPorNomeItem(item.itemIdentifierAmount.ItemDefinition);
                    slot.gameObject.SetActive(true);
                    return true;
                }
                else{
                    return false;
                }
            }
        }
        return false;
    }

    private bool podeAssar(ItemDefinitionBase itemSelecionado)
    {
        foreach(ItemDefinitionBase itemSlot in itensPodemAssar)
        {
            if(itemSlot.name.Equals(itemSelecionado.name))
            {
                return true;
            }
        }
        Debug.Log("NAO PODE ASSAR ESSE ITEM"); //TODO: MOSTRAR MSG: "NAO PODE ASSAR ESSE ITEM"
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
