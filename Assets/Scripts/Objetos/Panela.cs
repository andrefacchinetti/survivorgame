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
                slot.AtivarSlotPorNomeItem(item.nomeItem);
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

    public Item.NomeItem ObterConsumivelDaPanela()
    {
        foreach (SlotConsumivelPanela slot in slotsConsumiveis)
        {
            if (slot.gameObject.activeSelf)
            {
                return slot.nomeItemNoSlot;
            }
        }
        return Item.NomeItem.Nenhum;
    }

}
