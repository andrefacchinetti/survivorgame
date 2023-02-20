using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hotbar : MonoBehaviour
{

    [SerializeField] public List<SlotHotbar> slots = new List<SlotHotbar>();
    [SerializeField] [HideInInspector] public SlotHotbar slotHotbarSelecionada;
    [HideInInspector] public bool estaSelecionandoSlotHotbar = false;

    // Start is called before the first frame update
    void Start()
    {
        estaSelecionandoSlotHotbar = false;
        slotHotbarSelecionada = null;
    }

    public void SelecionouItemParaSlotHotbar(Item item)
    {
        slotHotbarSelecionada.SetupSlotHotbar(item);
        estaSelecionandoSlotHotbar = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if(slots[0].item != null && slots[0].item.quantidade > 0) slots[0].item.SelecionarItem();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (slots[1].item != null && slots[1].item.quantidade > 0) slots[1].item.SelecionarItem();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (slots[2].item != null && slots[2].item.quantidade > 0) slots[2].item.SelecionarItem();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (slots[3].item != null && slots[3].item.quantidade > 0) slots[3].item.SelecionarItem();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            if (slots[4].item != null && slots[4].item.quantidade > 0) slots[4].item.SelecionarItem();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            if (slots[5].item != null && slots[5].item.quantidade > 0) slots[5].item.SelecionarItem();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            if (slots[6].item != null && slots[6].item.quantidade > 0) slots[6].item.SelecionarItem();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            if (slots[7].item != null && slots[7].item.quantidade > 0) slots[7].item.SelecionarItem();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            if (slots[8].item != null && slots[8].item.quantidade > 0) slots[8].item.SelecionarItem();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            if (slots[9].item != null && slots[9].item.quantidade > 0) slots[9].item.SelecionarItem();
        }
    }

}
