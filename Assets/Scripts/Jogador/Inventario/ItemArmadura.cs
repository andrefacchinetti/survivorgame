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


    public void ColocarItemNoSlot(Item itemResponse)
    {
        foreach(Item.NomeItem nomeItem in itensPermitidosNoSlot)
        {
            if (nomeItem.Equals(item.nomeItem))
            {
                SetupItemNoSlot(itemResponse);
                return;
            }
        }
    }

    public void RetirarItemDoSlot()
    {

    }

    private void SetupItemNoSlot(Item itemResponse)
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
