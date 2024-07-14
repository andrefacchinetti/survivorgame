using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TooltipItemHolder : MonoBehaviour
{
    public TMP_Text txNome, txDescricao, txTipo;
    public TMP_Text txArmor, txCalor, txMoveSpeed;
    [SerializeField] Armaduras armaduras;

    public void setupItem(Item item)
    {
        txNome.text = "<color=#a335eeff>" + item.obterNomeItemTraduzido() + "</color>";
        txDescricao.text = item.obterDescricaoItem();
        txTipo.text = item.obterTipoItemTraduzido();
        if(item.tipoItem == Item.TiposItems.Armadura)
        {
            Armaduras.ArmaduraStats armaduraStats = armaduras.mapArmaduraStats[item.itemIdentifierAmount.ItemDefinition.name];
            txArmor.text = pintarVermelhoOuVerde(armaduraStats.armor, "Armor");
            txMoveSpeed.text = pintarVermelhoOuVerde(armaduraStats.moveSpeed, "Move Speed");
            txCalor.text = pintarVermelhoOuVerde(armaduraStats.calor, "Temperature");
        }
        txArmor.gameObject.SetActive(item.tipoItem == Item.TiposItems.Armadura);
        txMoveSpeed.gameObject.SetActive(item.tipoItem == Item.TiposItems.Armadura);
        txCalor.gameObject.SetActive(item.tipoItem == Item.TiposItems.Armadura);
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
            texto += "<color=#90474D> ";
        }
        texto += valor + " " + txBonus + "</color>";
        return texto;
    }

}
