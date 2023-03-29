using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ArrastarItensInventario : MonoBehaviour
{
    private Item item, itemHover;
    public GameObject placeHolder, slot1, nameHolder;
    private int slot2;
    [SerializeField]
    public SlotHotbar hotbar1,hotbar2,hotbar3,hotbar4,hotbar5,hotbar6,hotbar7,hotbar8,hotbar9,hotbar0;

    [SerializeField]
    public ItemArmadura[] slotsArmaduras = new ItemArmadura[4];
    [SerializeField]
    public ItemArmadura slotMunicoes;

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
            //Debug.Log("Mouse sobre: " + itemHover);
            if(Input.GetButtonDown("HotbarButton_1")){
                hotbar1.SetupSlotHotbar(itemHover);
            }else if (Input.GetButtonDown("HotbarButton_2"))
            {
                    hotbar2.SetupSlotHotbar(itemHover);
            }else if (Input.GetButtonDown("HotbarButton_3"))
            {
                    hotbar3.SetupSlotHotbar(itemHover);
            }else if (Input.GetButtonDown("HotbarButton_4"))
            {
                    hotbar4.SetupSlotHotbar(itemHover);
            }else if (Input.GetButtonDown("HotbarButton_5"))
            {
                    hotbar5.SetupSlotHotbar(itemHover);
            }else if (Input.GetButtonDown("HotbarButton_6"))
            {
                    hotbar6.SetupSlotHotbar(itemHover);
            }else if (Input.GetButtonDown("HotbarButton_7"))
            {
                    hotbar7.SetupSlotHotbar(itemHover);
            }else if (Input.GetButtonDown("HotbarButton_8"))
            {
                    hotbar8.SetupSlotHotbar(itemHover);
            }else if (Input.GetButtonDown("HotbarButton_9"))
            {
                    hotbar9.SetupSlotHotbar(itemHover);
            }else if (Input.GetButtonDown("HotbarButton_0"))
            {
                    hotbar0.SetupSlotHotbar(itemHover);
            }
        }
    }

    public void DragStartItemInventario(Item itemDrag, GameObject go1){
        item = itemDrag;
        
        placeHolder.GetComponent<RawImage>().texture = itemDrag.imagemItem.texture;
        placeHolder.SetActive(true);
        slot1 = go1;
    }

    public void DragEndItemInventario(SlotHotbar slotHotbar){
        slotHotbar.SetupSlotHotbar(item);
        item = null;
    }

    public void DragEndItemInventario(ItemArmadura slotArmadura)
    {
        slotArmadura.ColocarItemNoSlot(item);
        item = null;
    }

    public void TrocarLugarInventario(GameObject go2){
        //Debug.Log("trocou " + go2.name + " por " + slot1.name);
        slot2 = go2.transform.GetSiblingIndex();
        go2.transform.SetSiblingIndex(slot1.transform.GetSiblingIndex());
        slot1.transform.SetSiblingIndex(slot2);
    }

    public void HoverNothing()
    {
        itemHover = null;
        nameHolder.SetActive(false);
    }
    public void HoverItem(Item responsiveItem){
        itemHover = responsiveItem;
        nameHolder.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetInt("INDEXIDIOMA") == 1 ? responsiveItem.nomePortugues : responsiveItem.nomeIngles;
        nameHolder.SetActive(true);

    }

    public void SoltarItemNoPlayer(){
        if(item.nomeItem.GetTipoItemEnum().Equals(Item.TiposItems.Armadura.ToString())){
            foreach(ItemArmadura armadura in slotsArmaduras){
                armadura.ColocarItemNoSlot(item);
            }
        } else if(item.nomeItem.GetTipoItemEnum().Equals(Item.TiposItems.Municao.ToString())){
            slotMunicoes.ColocarItemNoSlot(item);
        } else{
            item.SelecionarItem();
        }
    }

    public void StopDrag()
    {
        Debug.Log("Soltou item");
        placeHolder.SetActive(false);
        item = null;
    }
}
