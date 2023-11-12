using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class ItemReceitaView : MonoBehaviour
{

    [SerializeField] public TMP_Text txNomeItem;
    [SerializeField] public RawImage imagemItem;

    public void SetupReceitaView(Item.ItemStruct itemStruct)
    {
        txNomeItem.text = PlayerPrefs.GetInt("INDEXIDIOMA") == 1 ? itemStruct.nomePortugues : itemStruct.nomeIngles;
        imagemItem.texture = itemStruct.textureImgItem;
    }
}
