using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class ItemIngredienteView : MonoBehaviour
{

    [SerializeField] public Text txNomeItem, txQuantidadeItem;
    [SerializeField] public RawImage imagemItem;

    public void SetupIngredienteView(Item.ItemStruct itemStruct, int quantidade)
    {
        txNomeItem.text = PlayerPrefs.GetInt("INDEXIDIOMA") == 1 ? itemStruct.nomePortugues : itemStruct.nomeIngles;
        txQuantidadeItem.text = quantidade + "";
        imagemItem.texture = itemStruct.textureImgItem;
    }

}
