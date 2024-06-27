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
    [SerializeField] public ArmazenamentoInventario armazenamentoinventario;
    [SerializeField] public bool isSlotCraft = false, isSlotArmazenamento = false;

    [SerializeField] public TMP_Text txTeclaAtalho, txNomeItem ,txQuantidade;
    [SerializeField] public RawImage imagemItem;
    [SerializeField] public Texture texturaInvisivel;
    [SerializeField] public GameObject objEmbacarImg;
    public ArrastarItensInventario arrastarItensInventario;

    [HideInInspector] public int qtdItemNoSlot = 0;


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

    public void SetupSlotHotbar(Item itemResponse, bool craftParaCraft)
    {
        if (itemResponse == null) return;
        if (isSlotCraft) setupSlotHotbarCraft(itemResponse, craftParaCraft);
        else if (isSlotArmazenamento) setupSlotHotbarArmazenamento(itemResponse);
        else setupSlotHotbarAtalhos(itemResponse);
    }

    private void setupSlotHotbarAtalhos(Item itemResponse)
    {
        Debug.LogWarning("setup hotbar");
        foreach (SlotHotbar slot in hotbar.slots)
        {
            if (slot.item == itemResponse)
            {
                slot.ResetSlotHotbar();
                Debug.LogWarning("resetando pq achou igual");
            }
        }
        item = itemResponse;
        txNomeItem.text = item.obterNomeItemTraduzido();
        txQuantidade.text = item.quantidade + "";
        imagemItem.texture = item.imagemItem.texture;
    }

    private void setupSlotHotbarCraft(Item itemResponse, bool craftParaCraft)
    {
        int countItens = craftParaCraft ? qtdItemNoSlot : 1;
        Debug.Log("qtdItemNoSlot: " + qtdItemNoSlot);
        foreach (SlotHotbar slot in craftMaos.slots)
        {
            if (slot.item == itemResponse)
            {
                if(slot.qtdItemNoSlot < itemResponse.quantidade)
                {
                    countItens += slot.qtdItemNoSlot;
                }
                slot.ResetSlotHotbar();
            }
        }
        qtdItemNoSlot = countItens;
        item = itemResponse;
        txNomeItem.text = item.obterNomeItemTraduzido();
        txQuantidade.text = qtdItemNoSlot + "";
        imagemItem.texture = item.imagemItem.texture;
        objEmbacarImg.SetActive(item.quantidade <= 0);
        craftMaos.AtualizarPreviewResultado();
    }

    private void setupSlotHotbarArmazenamento(Item itemResponse)
    {
        Debug.Log("setup hotbar armazenamento");
        qtdItemNoSlot = itemResponse.quantidade;
        item = itemResponse;
        txNomeItem.text = item.obterNomeItemTraduzido();
        txQuantidade.text = qtdItemNoSlot + "";
        imagemItem.texture = item.imagemItem.texture;
    }

    public void ResetSlotHotbar(){
        item = null;
        qtdItemNoSlot = 0;
        txNomeItem.text = "";
        txQuantidade.text = "";
        imagemItem.texture = texturaInvisivel;
        objEmbacarImg.SetActive(false);
        if (isSlotCraft) craftMaos.AtualizarPreviewResultado();
    }

    public void OnDropDelegate(PointerEventData data){
        Debug.Log("OnDropDelegate drag: ");
        arrastarItensInventario.DragEndItemInventario(this);
    }

    public void OnPointerClickDelegate(PointerEventData data){
        if(data.button == PointerEventData.InputButton.Left){
            Debug.Log("OnPointerClickDelegate: Left");
        }
        else if(data.button == PointerEventData.InputButton.Right){
            Debug.Log("Apertou o botão direito sobre: " + name);
            if (!isSlotArmazenamento)
            {
                ResetSlotHotbar();
            }
        }else if(data.button == PointerEventData.InputButton.Middle){
            Debug.Log("OnPointerClickDelegate: Middle");
        }
    }
    
    public void OnBeginDragDelegate(PointerEventData data)
    {
        Debug.Log("OnBeginDragDelegate drag");
        arrastarItensInventario.DragStartItemInventario(this.item, this.gameObject);
    }

    public void OnEndDragDelegate(PointerEventData data)
    {
        Debug.Log("OnEndDragDelegate  drag");
        arrastarItensInventario.StopDrag();
    }

    public void SetupItemNoSlot(Item itemResponse, int quantidadeResponse)
    {
        item = itemResponse;
        qtdItemNoSlot = quantidadeResponse;
        if (itemResponse == null)
        {
            txNomeItem.text = "";
            txQuantidade.text = "";
            imagemItem.texture = texturaInvisivel;
        }
        else
        {
            txNomeItem.text = PlayerPrefs.GetInt("INDEXIDIOMA") == 1 ? item.nomePortugues : item.nomeIngles;
            txQuantidade.text = qtdItemNoSlot + "";
            imagemItem.texture = item.imagemItem.texture;
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
            Item.ItemStruct itemStruct = inventario.ObterItemStructPeloNome(receitaResultado.itemResultado);
            txNomeItem.text = PlayerPrefs.GetInt("INDEXIDIOMA") == 1 ? itemStruct.nomePortugues : itemStruct.nomeIngles;
            txQuantidade.text = "";
            txTeclaAtalho.text = "";
            imagemItem.texture = itemStruct.textureImgItem;
        }
    }

}

