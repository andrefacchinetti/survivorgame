using UnityEngine;
using UnityEngine.UI;

public class TooltipItemHolder : MonoBehaviour
{
    public Text txNome, txDescricao, txTipo;
    public Text txArmor, txCalor, txMoveSpeed; 

    public void setupItem(Item item)
    {
        txNome.text = "<color=#a335eeff>" + item.obterNomeItemTraduzido() + "</color>";
        txDescricao.text = item.obterDescricaoItemTraduzida();
        txTipo.text = item.obterTipoItemTraduzido();
        txArmor.text = item.tipoItem == Item.TiposItems.Armadura ? pintarVermelhoOuVerde(10, "Armor") : "";
        txMoveSpeed.text = item.tipoItem == Item.TiposItems.Armadura ? pintarVermelhoOuVerde(-5, "Move Speed") : "";
        txCalor.text = item.tipoItem == Item.TiposItems.Armadura ? pintarVermelhoOuVerde(20, "Temperature") : "";
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
