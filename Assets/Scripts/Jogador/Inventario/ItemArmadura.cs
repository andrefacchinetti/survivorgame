using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Opsive.UltimateCharacterController.Inventory;
using Opsive.Shared.Inventory;

public class ItemArmadura : MonoBehaviour
{

    [SerializeField] public ItemDefinitionBase[] itemsPermitidosNoSlot;
    [SerializeField] public Armaduras.TipoSlotArmadura tipoSlotArmadura;
    [SerializeField][HideInInspector] public Item item;
    [SerializeField] public TMP_Text txQuantidade, txNomeItem;
    [SerializeField] public RawImage imagemItem;
    [SerializeField] public Texture texturaInvisivel;
    [SerializeField] public GameObject bordaSelecionado;
    [SerializeField] public Armaduras armaduras;
    [SerializeField] public ArrastarItensInventario arrastarItensInventario;

    private void Start() {
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
        entry2.callback.AddListener((data) => { OnPointerClickDelegate((PointerEventData)data);});
        entry3.callback.AddListener((data) => { OnBeginDragDelegate((PointerEventData)data); });
        entry4.callback.AddListener((data) => { OnEndDragDelegate((PointerEventData)data); });
        trigger.triggers.Add(entry);
        trigger.triggers.Add(entry2);
        trigger.triggers.Add(entry3);
        trigger.triggers.Add(entry4);
    }

    public void ResetSlotHotbar()
    {
        item = null;
        txNomeItem.text = "";
        txQuantidade.text = "";
        imagemItem.texture = texturaInvisivel;
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
        if (itemResponse == null) return false;
        bordaSelecionado.SetActive(false);
        armaduras.slotItemArmaduraSelecionada = null;
        armaduras.estaSelecionandoSlotArmadura = false;
        foreach (ItemDefinitionBase itemBase in itemsPermitidosNoSlot)
        {
            if (itemBase.name.Equals(itemResponse.itemIdentifierAmount.ItemDefinition.name))
            {
                SetupItemNoSlot(itemResponse);
                return true;
            }
        }
        return false;
    }

    public void RetirarItemDoSlot()
    {
        Debug.Log("RETIRANDO ITEM SLOT");
    }

    public void SetupItemNoSlot(Item itemResponse)
    {
        string nomeItem = itemResponse != null ? itemResponse.nomePortugues : "null";
        Debug.Log("SetupItemNoSlot slotArmadura: "+ nomeItem);
        armaduras.DesequiparArmaduraDoTipoSlot(tipoSlotArmadura);
        item = itemResponse;
        if(itemResponse == null)
        {
            txNomeItem.text = "";
            txQuantidade.text = "";
            imagemItem.texture = texturaInvisivel;
        }
        else
        {
            txNomeItem.text = PlayerPrefs.GetInt("INDEXIDIOMA") == 1 ? item.nomePortugues : item.nomeIngles;
            txQuantidade.text = item.quantidade + "";
            imagemItem.texture = item.imagemItem.texture;
            armaduras.EquiparArmadura(item.itemIdentifierAmount.ItemDefinition);
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

    public void OnBeginDragDelegate(PointerEventData data)
    {
        Debug.Log("onbegin drag");
        arrastarItensInventario.DragStartItemInventario(this.item, this.gameObject);
        SetupItemNoSlot(null);
    }

    public void OnEndDragDelegate(PointerEventData data)
    {
        arrastarItensInventario.StopDrag();
    }

}
