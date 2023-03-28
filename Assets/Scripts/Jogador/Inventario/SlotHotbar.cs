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
    [SerializeField] public Inventario inventario;

    [SerializeField] public TMP_Text txTeclaAtalho, txNomeItem ,txQuantidade;
    [SerializeField] public RawImage imagemItem;
    [SerializeField] public GameObject objEmbacarImg, bordaSelecionado;
    public ArrastarItensInventario arrastarItensInventario;


    private void Start()
    {
        ResetSlotHotbar();
        EventTrigger trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.Drop;
        entry.callback.AddListener((data) => { OnDropDelegate((PointerEventData)data); });
        trigger.triggers.Add(entry);
    }

    public void SetupSlotHotbar(Item itemResponse)
    {
        foreach(SlotHotbar hb in hotbar.slots){
            //Debug.Log("Slot: " + hb.name + " item: " + hb.item.nomePortugues);
            if(hb.item == itemResponse){
                Debug.Log("Item = item");
                hb.ResetSlotHotbar();
            }
        }
        item = itemResponse;
        txNomeItem.text = PlayerPrefs.GetInt("INDEXIDIOMA") == 1 ? item.nomePortugues : item.nomeIngles; ;
        txQuantidade.text = item.quantidade + "";
        imagemItem.gameObject.SetActive(true);
        imagemItem.texture = item.imagemItem.texture;
        objEmbacarImg.SetActive(item.quantidade <= 0);
        bordaSelecionado.SetActive(false);
    }

    public void ResetSlotHotbar(){
        item = null;
        txNomeItem.text = "";
        txQuantidade.text = "";
        imagemItem.gameObject.SetActive(false);
        objEmbacarImg.SetActive(false);
    }

    public void SelecionarSlotHotbar()
    {
        foreach(SlotHotbar slot in hotbar.slots)
        {
            slot.bordaSelecionado.SetActive(false);
        }
        hotbar.slotHotbarSelecionada = this;
        hotbar.estaSelecionandoSlotHotbar = true;
        bordaSelecionado.SetActive(true);
    }

    public void OnDropDelegate(PointerEventData data){
        arrastarItensInventario.DragEndItemInventario(this);
    }

}

