using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class SlotHotbar : MonoBehaviour
{
    [SerializeField] public Item item;
    [SerializeField] public Hotbar hotbar;
    [SerializeField] public CraftMaos craftMaos;
    [SerializeField] public Inventario inventario;
    [SerializeField] public bool isSlotCraft = false;

    [SerializeField] public TMP_Text txTeclaAtalho, txNomeItem ,txQuantidade;
    [SerializeField] public RawImage imagemItem;
    [SerializeField] public Texture texturaInvisivel;
    [SerializeField] public GameObject objEmbacarImg;
    public ArrastarItensInventario arrastarItensInventario;


    private void Start()
    {
        ResetSlotHotbar();
        EventTrigger trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        EventTrigger.Entry entry2 = new EventTrigger.Entry();
        EventTrigger.Entry entry3 = new EventTrigger.Entry();
        EventTrigger.Entry entry4 = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.Drop;
        entry2.eventID = EventTriggerType.PointerClick;
        entry3.eventID = EventTriggerType.BeginDrag;
        entry4.eventID = EventTriggerType.EndDrag;
        entry.callback.AddListener((data) => { OnDropDelegate((PointerEventData)data); });
        entry2.callback.AddListener((data) => { OnPointerClickDelegate((PointerEventData)data); });
        entry3.callback.AddListener((data) => { OnBeginDragDelegate((PointerEventData)data); });
        entry4.callback.AddListener((data) => { OnEndDragDelegate((PointerEventData)data); });
        trigger.triggers.Add(entry);
        trigger.triggers.Add(entry2);
        trigger.triggers.Add(entry3);
        trigger.triggers.Add(entry4);
    }

    public void SetupSlotHotbar(Item itemResponse)
    {
        if (itemResponse == null) return;
        List<SlotHotbar> listaSlots;
        if (isSlotCraft) listaSlots = craftMaos.slots;
        else listaSlots = hotbar.slots;

        foreach (SlotHotbar slot in listaSlots)
        {
            if (slot.item == itemResponse)
            {
                slot.ResetSlotHotbar();
            }
        }

        item = itemResponse;
        txNomeItem.text = item.obterNomeItemTraduzido();
        txQuantidade.text = item.quantidade + "";
        imagemItem.texture = item.imagemItem.texture;
        objEmbacarImg.SetActive(item.quantidade <= 0);
        if(isSlotCraft) craftMaos.AtualizarPreviewResultado();
    }

    public void ResetSlotHotbar(){
        item = null;
        txNomeItem.text = "";
        txQuantidade.text = "";
        imagemItem.texture = texturaInvisivel;
        objEmbacarImg.SetActive(false);
        if (isSlotCraft) craftMaos.AtualizarPreviewResultado();
    }


    public void OnDropDelegate(PointerEventData data){
        Debug.Log("OnDropDelegate drag");
        arrastarItensInventario.DragEndItemInventario(this);
    }

    public void OnPointerClickDelegate(PointerEventData data){
        if(data.button == PointerEventData.InputButton.Left){
            
        }else if(data.button == PointerEventData.InputButton.Right){
            Debug.Log("Apertou o botão direito sobre: " + name);
            ResetSlotHotbar();
        }else if(data.button == PointerEventData.InputButton.Middle){

        }
    }
    
    public void OnBeginDragDelegate(PointerEventData data)
    {
        Debug.Log("OnBeginDragDelegate drag");
        arrastarItensInventario.DragStartItemInventario(this.item, this.gameObject);
        SetupItemNoSlot(null);
    }

    public void OnEndDragDelegate(PointerEventData data)
    {
        Debug.Log("OnEndDragDelegate drag");
        arrastarItensInventario.StopDrag();
        ResetSlotHotbar();
    }

    public void SetupItemNoSlot(Item itemResponse)
    {
        item = itemResponse;
        if (itemResponse == null)
        {
            txNomeItem.text = "";
            txQuantidade.text = "";
            imagemItem.texture = texturaInvisivel;
        }
        else
        {
            txNomeItem.text = PlayerPrefs.GetInt("INDEXIDIOMA") == 1 ? itemResponse.nomePortugues : itemResponse.nomeIngles;
            txQuantidade.text = itemResponse.quantidade + "";
            imagemItem.texture = itemResponse.imagemItem.texture;
        }
    }

    public void SetupPreviewNoSlot(CraftMaos.ReceitaCraft receitaResultado)
    {
        if (receitaResultado == null)
        {
            txNomeItem.text = "";
            txQuantidade.text = "";
            txTeclaAtalho.text = "";
            imagemItem.texture = texturaInvisivel;
        }
        else
        {
            Item.ItemStruct itemStruct = inventario.PegarStructPeloNome(receitaResultado.nomeItemResultado);
            txNomeItem.text = PlayerPrefs.GetInt("INDEXIDIOMA") == 1 ? itemStruct.nomePortugues : itemStruct.nomeIngles;
            txQuantidade.text = "";
            txTeclaAtalho.text = "";
            imagemItem.texture = itemStruct.textureImgItem;
        }
    }

}

