using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotConsumivelPanela : MonoBehaviour
{

    [SerializeField] public Item.NomeItem nomeItemNoSlot;
    [SerializeField] public GameObject slotCarneCrua, slotCarneCozida;
    [SerializeField] public GameObject slotCogumeloCru, slotCogumeloCozido;
    [SerializeField] public GameObject slotPeixeCru, slotPeixeCozido;


    public void AtivarSlotPorNomeItem(Item.NomeItem nomeItem)
    {
        nomeItemNoSlot = nomeItem;
        if (nomeItem.Equals(Item.NomeItem.CarneCrua)) slotCarneCrua.SetActive(true);
        if (nomeItem.Equals(Item.NomeItem.CarneCozida)) slotCarneCozida.SetActive(true);

        if (nomeItem.Equals(Item.NomeItem.CogumeloCru)) slotCogumeloCru.SetActive(true);
        if (nomeItem.Equals(Item.NomeItem.CogumeloCozido)) slotCogumeloCozido.SetActive(true);

        if (nomeItem.Equals(Item.NomeItem.PeixeCru)) slotPeixeCru.SetActive(true);
        if (nomeItem.Equals(Item.NomeItem.PeixeCozido)) slotPeixeCozido.SetActive(true);
    }

    public void DesativarSlots()
    {
        nomeItemNoSlot = Item.NomeItem.Nenhum;
        slotCarneCrua.SetActive(false);
        slotCarneCozida.SetActive(false);
        slotCogumeloCru.SetActive(false);
        slotCogumeloCozido.SetActive(false);
        slotPeixeCru.SetActive(false);
        slotPeixeCozido.SetActive(false);
    }

}
