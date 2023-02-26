using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armaduras : MonoBehaviour
{

    [SerializeField] public Inventario inventario;
    [SerializeField] public List<ItemArmadura> slotsItemArmadura;
    [SerializeField] [HideInInspector] public ItemArmadura slotItemArmaduraSelecionada;
    [SerializeField] public ItemArmadura slotAljava;
    [SerializeField] public bool estaSelecionandoSlotArmadura;

    public void SelecionouItemParaSlotArmadura(Item item)
    {
        if (!slotItemArmaduraSelecionada.ColocarItemNoSlot(item))
        {
            Debug.Log("O item não pertence a essa categoria de slot");
        }
        estaSelecionandoSlotArmadura = false;
    }

    public Item ObterItemFlechaNaAljava()
    {
        return slotAljava.item;
    }

}
