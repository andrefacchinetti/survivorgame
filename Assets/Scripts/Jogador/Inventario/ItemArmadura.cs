using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;


public class ItemArmadura : MonoBehaviour
{

    [SerializeField] public List<Item.NomeItem> itensPermitidosNoSlot;
    [SerializeField] public Item item;
    [SerializeField] public TMP_Text txQuantidade, txNomeItem;
    [SerializeField] public RawImage imagemItem;
    [SerializeField] public Texture texturaInvisivel;
    [SerializeField] public GameObject bordaSelecionado;
    [SerializeField] public Armaduras armaduras;
    [SerializeField] public ArrastarItensInventario arrastarItensInventario;

    private void Start() {
        EventTrigger trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        EventTrigger.Entry entry2 = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.Drop;
        entry2.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((data) => { OnDropDelegate((PointerEventData)data); });
        entry2.callback.AddListener((data) => { OnPointerClickDelegate((PointerEventData)data);});
        trigger.triggers.Add(entry);
    }


    public void SelecionarSlotArmadura()
    {
        foreach (ItemArmadura slot in armaduras.slotsItemArmadura)
        {
            slot.bordaSelecionado.SetActive(false);
        }
        armaduras.slotItemArmaduraSelecionada = this;
        armaduras.estaSelecionandoSlotArmadura = true;
        bordaSelecionado.SetActive(true);
    }

    public bool ColocarItemNoSlot(Item itemResponse)
    {
        bordaSelecionado.SetActive(false);
        armaduras.slotItemArmaduraSelecionada = null;
        armaduras.estaSelecionandoSlotArmadura = false;
        foreach (Item.NomeItem nomeItem in itensPermitidosNoSlot)
        {
            if (nomeItem.Equals(itemResponse.nomeItem))
            {
                SetupItemNoSlot(itemResponse);
                return true;
            }
        }
        return false;
    }

    public void RetirarItemDoSlot()
    {

    }

    public void SetupItemNoSlot(Item itemResponse)
    {
        item = itemResponse;
        if(itemResponse == null)
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

    public void OnDropDelegate(PointerEventData data){
        arrastarItensInventario.DragEndItemInventario(this);
    }

    public void OnPointerClickDelegate(PointerEventData data)
    {
        if (data.button == PointerEventData.InputButton.Left)
        {

        }
        else if (data.button == PointerEventData.InputButton.Right)
        {
            Debug.Log("Apertou o botão direito sobre: " + name);
            SetupItemNoSlot(null);
        }
        else if (data.button == PointerEventData.InputButton.Middle)
        {

        }
    }
}
