using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ArrastarItensInventario : MonoBehaviour
{
    private Item item, itemHover;
    private int qtdItem;
    public GameObject placeHolder;
    public TooltipItemHolder tooltipHolder;
    [HideInInspector] public GameObject slotStart;
    private int slotEnd;
    [SerializeField]
    public SlotHotbar hotbar1,hotbar2,hotbar3,hotbar4,hotbar5;

    [SerializeField]
    public ItemArmadura[] slotsArmaduras = new ItemArmadura[5];

    private Inventario inventario;
    // Start is called before the first frame update
    void Start()
    {
        inventario = GetComponent<Inventario>();
    }

    // Update is called once per frame
    void Update()
    {
        if(itemHover!=null && item==null && inventario.canvasInventario.activeSelf){
            if (Input.GetButtonDown("HotbarButton_1")) hotbar1.SetupSlotHotbar(itemHover, false);
            else if (Input.GetButtonDown("HotbarButton_2")) hotbar2.SetupSlotHotbar(itemHover, false);
            else if (Input.GetButtonDown("HotbarButton_3")) hotbar3.SetupSlotHotbar(itemHover, false);
            else if (Input.GetButtonDown("HotbarButton_4")) hotbar4.SetupSlotHotbar(itemHover, false);
            else if (Input.GetButtonDown("HotbarButton_5")) hotbar5.SetupSlotHotbar(itemHover, false);
        }
    }

    public void DragStartItemInventario(Item itemDrag, GameObject slotResponse){
        if (itemDrag == null) return;
        Debug.Log("setou DragStartItemInventario: "+ itemDrag.quantidade);
        if(slotResponse.GetComponent<SlotHotbar>() != null && slotResponse.GetComponent<SlotHotbar>().isSlotCraft)
        {
            qtdItem = slotResponse.GetComponent<SlotHotbar>().qtdItemNoSlot;
        }
        else
        {
            qtdItem = itemDrag.quantidade;
        }
        item = itemDrag;
        placeHolder.GetComponent<RawImage>().texture = itemDrag.imagemItem.texture;
        placeHolder.SetActive(true);
        slotStart = slotResponse;
    }

    private Item criarNovoItemParaArmazenar(Item.ItemStruct itemStructResponse, int quantidadeResponse)
    {
        GameObject novoItemObjeto = Instantiate(inventario.prefabItem, Vector3.zero, new Quaternion(), inventario.armazenamentoInventario.transform);
        novoItemObjeto.transform.SetParent(inventario.armazenamentoInventario.transform);
        if (item != null && item.Equals(inventario.itemGarrafa))
        {
            novoItemObjeto.GetComponent<Garrafa>().Setup(item.GetComponent<Garrafa>());
        }
        Item novoItem = novoItemObjeto.GetComponent<Item>().setupItemFromItemStruct(itemStructResponse, quantidadeResponse);
        return novoItem;
    }

    public void DragEndItemInventario(SlotHotbar slotChegada){
        if (slotStart == null) return;
        if(slotStart.GetComponent<SlotHotbar>() == null)
        {
            if (slotChegada.isSlotArmazenamento)//Inventario para armazenamento
            {
                Debug.Log("Inventario para armazenamento");
                if (item != null)
                {
                    if (slotChegada.item == null)
                    {
                        Debug.Log("Inventario para armazenamento com sem item no slot: "+item.quantidade);
                        Item.ItemStruct itemStructResponse = inventario.ObterItemStructPeloNome(item.itemIdentifierAmount.ItemDefinition);
                        Item novoItem = criarNovoItemParaArmazenar(itemStructResponse, item.quantidade);
                        slotChegada.SetupSlotHotbar(novoItem, false);
                        inventario.armazenamentoInventario.armazenamentoEmUso.GuardarItem(novoItem, novoItem.quantidade);
                        inventario.RemoverItemDoInventarioPorNome(item.itemIdentifierAmount.ItemDefinition, novoItem.quantidade);
                    }
                    else
                    {
                        if (slotChegada.item.itemIdentifierAmount.ItemDefinition == item)
                        {
                            Debug.Log("Inventario para armazenamento stackando item no slot: " + item.quantidade);
                            inventario.armazenamentoInventario.armazenamentoEmUso.GuardarItem(slotChegada.item, item.quantidade);
                            inventario.RemoverItemDoInventarioPorNome(slotChegada.item.itemIdentifierAmount.ItemDefinition, item.quantidade);
                        }
                        else
                        {
                            //trocar item de slot
                            Debug.Log("trocar item de slot");
                        }
                    }
                }
            }
            else
            {
                Debug.Log("inventario para craft/armamento");
                if (item != null)
                {
                    bool craftParaCraft = slotStart.GetComponent<SlotHotbar>() != null && slotStart.GetComponent<SlotHotbar>().isSlotCraft;
                    slotChegada.SetupSlotHotbar(item, craftParaCraft);
                }
            }
        }
        else //start = craft/armazenamento
        {
            if (slotStart.GetComponent<SlotHotbar>().isSlotCraft) //start = craft
            {
                if (slotChegada.isSlotCraft)
                {
                    Debug.Log("craft para craft");
                    slotChegada.SetupItemNoSlot(item, qtdItem) ;
                    slotStart.GetComponent<SlotHotbar>().ResetSlotHotbar();
                }
                else if(slotChegada.isSlotArmazenamento)
                {
                    Debug.Log("craft para armazenamento");
                }
                else
                {
                    Debug.Log("craft para inventario");
                    slotStart.GetComponent<SlotHotbar>().ResetSlotHotbar();
                }
            }
            else if (slotStart.GetComponent<SlotHotbar>().isSlotArmazenamento) //start = armazenamento
            {
                if (slotChegada.isSlotCraft)
                {
                    Debug.Log("armazenamento para craft");
                    //slotChegada.SetupItemNoSlot(item, qtdItem);
                    //slotStart.GetComponent<SlotHotbar>().ResetSlotHotbar();
                }
                else if (slotChegada.isSlotArmazenamento) //Armazenamento para armazenamento
                {
                    Debug.Log("Armazenamento para armazenamento");
                    slotChegada.SetupItemNoSlot(item, qtdItem);
                    slotStart.GetComponent<SlotHotbar>().ResetSlotHotbar();
                }
                else //Armazenamento para inventario
                {
                    Debug.Log("Armazenamento para inventario");
                    if (item != null)
                    {
                        if (inventario.AdicionarItemAoInventarioPorNome(item.itemIdentifierAmount.ItemDefinition, item.quantidade))
                        {
                            inventario.armazenamentoInventario.armazenamentoEmUso.PegarItem(item, item.quantidade);
                            slotStart.GetComponent<SlotHotbar>().ResetSlotHotbar();
                        }
                    }
                }
            }
        }
        item = null;
        placeHolder.SetActive(false);
    }

    public void DragEndItemMochila()
    {
        if (item != null)
        {
            if (slotStart.GetComponent<SlotHotbar>() == null)
            {
                Debug.Log("inventario para mochila");
            }
            else if (slotStart.GetComponent<SlotHotbar>().isSlotCraft)
            {
                slotStart.GetComponent<SlotHotbar>().ResetSlotHotbar();
            }
            else
            {
                Debug.Log("Armazenamento para mochila: "+ item.quantidade);
                if(inventario.AdicionarItemAoInventarioPorNome(item.itemIdentifierAmount.ItemDefinition, item.quantidade))
                {
                    inventario.armazenamentoInventario.armazenamentoEmUso.PegarItem(item, item.quantidade);
                    slotStart.GetComponent<SlotHotbar>().ResetSlotHotbar();
                }
            }
        }
    }

    public void DragEndItemInventario(ItemArmadura slotArmadura)
    {
        Debug.Log("DragEndItemInventario slotArmadura");
        if (slotStart.GetComponent<SlotHotbar>().isSlotArmazenamento) return;
        slotArmadura.ColocarItemNoSlot(item);
        item = null;
        placeHolder.SetActive(false);
    }

    public void TrocarLugarInventario(GameObject go2){
        Debug.Log("trocou " + go2.name + " por " + slotStart.name);
        slotEnd = go2.transform.GetSiblingIndex();
        go2.transform.SetSiblingIndex(slotStart.transform.GetSiblingIndex());
        slotStart.transform.SetSiblingIndex(slotEnd);
    }

    public void HoverNothing()
    {
        itemHover = null;
        tooltipHolder.gameObject.SetActive(false);
    }

    public void HoverItem(Item responsiveItem){
        itemHover = responsiveItem;
        tooltipHolder.setupItem(responsiveItem);
        tooltipHolder.gameObject.SetActive(true);
    }

    public void SoltarItemNoPlayer(){
        if (item == null) return;
        Debug.Log("SoltarItemNoPlayer");
        if (item.tipoItem.Equals(Item.TiposItems.Armadura.ToString())){
            foreach(ItemArmadura armadura in slotsArmaduras){
                armadura.ColocarItemNoSlot(item);
            }
        } else{
            item.SelecionarItem();
        }
    }

    public void StopDrag()
    {
        Debug.Log("Soltou item: " + (item != null) + (slotStart != null) );
        placeHolder.SetActive(false);
        item = null;
    }

}
