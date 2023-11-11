using UnityEngine;

public static class EnumMensagens
{
    // 0 = INGLES,  1 = PORTUGUES

    public static string ObterAlertaNaoPossuiMartelo()
    {
        if(PlayerPrefs.GetInt("INDEXIDIOMA") == 1) return "Você precisa de um Martelo Reparador";
        return "You need a Repair Hammer";
    }

    public static string ObterAlertaNaoPossuiMaterialSuficiente(string nomeItem, int qtdItemAtual, int qtdNecessaria)
    {
        if (PlayerPrefs.GetInt("INDEXIDIOMA") == 1) return "Você não possui "+nomeItem+" suficiente ("+qtdItemAtual+"/"+qtdNecessaria+")";
        return "You do not have enough " + nomeItem + " (" + qtdItemAtual + "/" + qtdNecessaria + ")"; ;
    }

}
