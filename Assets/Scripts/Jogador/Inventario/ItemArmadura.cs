using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ItemArmadura : MonoBehaviour
{

    [SerializeField] public List<Item.NomeItem> itensPermitidosNoSlot;
    [SerializeField] public Item item;
    [SerializeField] public TMP_Text txQuantidade, txNomeItem;
    [SerializeField] public RawImage imagemItem;
    [SerializeField] public Texture texturaInvisivel;
    [SerializeField] public GameObject bordaSelecionado;
    [SerializeField] public Armaduras armaduras;


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

}
