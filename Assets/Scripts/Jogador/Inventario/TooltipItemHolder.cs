using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TooltipItemHolder : MonoBehaviour
{
    public TMP_Text txNome, txDescricao, txTipo;
    public TMP_Text txArmor, txCalor, txMoveSpeed; 

    public void setupItem(Item item)
    {
        txNome.text = "<color=#a335eeff>" + item.obterNomeItemTraduzido() + "</color>";
        txDescricao.text = item.obterDescricaoItemTraduzida();
        txTipo.text = item.obterTipoItemTraduzido();
        if(item.tipoItem == Item.TiposItems.Armadura)
        {
            txArmor.text = pintarVermelhoOuVerde(10, "Armor");
            txMoveSpeed.text = pintarVermelhoOuVerde(10, "Armor");
            txCalor.text = pintarVermelhoOuVerde(10, "Armor");
        }
        txArmor.gameObject.SetActive(item.tipoItem == Item.TiposItems.Armadura);
        txMoveSpeed.gameObject.SetActive(item.tipoItem == Item.TiposItems.Armadura);
        txMoveSpeed.gameObject.SetActive(item.tipoItem == Item.TiposItems.Armadura);
    }

    private string pintarVermelhoOuVerde(float valor, string txBonus)
    {
        string texto = "";
        if(valor == 0) return "";
        if (valor > 0)
        {
            texto += "<color=#539047> +";
        }
        else
        {
            texto += "<color=#90474D> -";
        }
        texto += valor + " " + txBonus + "</color>";
        return texto;
    }

}
