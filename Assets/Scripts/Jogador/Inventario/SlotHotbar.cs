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
    [SerializeField] public GameObject objEmbacarImg;
    public ArrastarItensInventario arrastarItensInventario;


    private void Start()
    {
        ResetSlotHotbar();
        EventTrigger trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        EventTrigger.Entry entry2 = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.Drop;
        entry2.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((data) => { OnDropDelegate((PointerEventData)data); });
        entry2.callback.AddListener((data) => { OnPointerClickDelegate((PointerEventData)data);});
        trigger.triggers.Add(entry);
        trigger.triggers.Add(entry2);
    }

    public void SetupSlotHotbar(Item itemResponse)
    {
        foreach(SlotHotbar slot in hotbar.slots){
            //Debug.Log("Slot: " + slot.name + " item: " + slot.item.nomePortugues);
            if(slot.item == itemResponse){
                //Debug.Log("Item = item");
                slot.ResetSlotHotbar();
            }
        }
        item = itemResponse;
        txNomeItem.text = PlayerPrefs.GetInt("INDEXIDIOMA") == 1 ? item.nomePortugues : item.nomeIngles; ;
        txQuantidade.text = item.quantidade + "";
        imagemItem.gameObject.SetActive(true);
        imagemItem.texture = item.imagemItem.texture;
        objEmbacarImg.SetActive(item.quantidade <= 0);
    }

    public void ResetSlotHotbar(){
        item = null;
        txNomeItem.text = "";
        txQuantidade.text = "";
        imagemItem.gameObject.SetActive(false);
        objEmbacarImg.SetActive(false);
    }


    public void OnDropDelegate(PointerEventData data){
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
}

