using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotConsumivelPanela : MonoBehaviour
{

    [SerializeField] public Item itemNoSlot;
    [SerializeField] public GameObject slotCarneCrua, slotCarneCozida;
    [SerializeField] public GameObject slotCogumeloCru, slotCogumeloCozido;
    [SerializeField] public GameObject slotPeixeCru, slotPeixeCozido;


    public void AtivarSlotPorTipoItem(Item item)
    {
        itemNoSlot = item;
        if (item.nomeItem.Equals(Item.NomeItem.CarneCrua)) slotCarneCrua.SetActive(true);
        if (item.nomeItem.Equals(Item.NomeItem.CarneCozida)) slotCarneCozida.SetActive(true);

        if (item.nomeItem.Equals(Item.NomeItem.CogumeloCru)) slotCogumeloCru.SetActive(true);
        if (item.nomeItem.Equals(Item.NomeItem.CogumeloCozido)) slotCogumeloCozido.SetActive(true);

        if (item.nomeItem.Equals(Item.NomeItem.PeixeCru)) slotPeixeCru.SetActive(true);
        if (item.nomeItem.Equals(Item.NomeItem.PeixeCozido)) slotPeixeCozido.SetActive(true);
    }

    public void DesativarSlots()
    {
        slotCarneCrua.SetActive(false);
        slotCarneCozida.SetActive(false);
        slotCogumeloCru.SetActive(false);
        slotCogumeloCozido.SetActive(false);
        slotPeixeCru.SetActive(false);
        slotPeixeCozido.SetActive(false);
        itemNoSlot = null;
    }

}
