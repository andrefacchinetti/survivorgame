using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class ItemReceitaView : MonoBehaviour
{

    [SerializeField] public GameObject contentIngredientes, prefabItemIngrediente;
    [SerializeField] public Text txNomeItem;
    [SerializeField] public RawImage imagemItem;
    List<ItemIngredienteView> ingredientesViews;

    public void SetupReceitaView(Item.ItemStruct itemStruct, List<CraftMaos.Ingrediente> ingredientes)
    {
        txNomeItem.text = PlayerPrefs.GetInt("INDEXIDIOMA") == 1 ? itemStruct.nomePortugues : itemStruct.nomeIngles;
        imagemItem.texture = itemStruct.textureImgItem;
        InstanciarIngredientes(ingredientes);
    }

    private void InstanciarIngredientes(List<CraftMaos.Ingrediente> ingredientes)
    {
        foreach (CraftMaos.Ingrediente ingrediente in ingredientes)
        {
            GameObject novaReceita = Instantiate(prefabItemIngrediente, new Vector3(), new Quaternion(), contentIngredientes.transform);
            novaReceita.GetComponent<ItemIngredienteView>().SetupIngredienteView(ingrediente.itemStruct, ingrediente.quantidade);
        }
    }

}
