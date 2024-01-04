using UnityEngine;

public static class EnumMensagens
{
    // 0 = INGLES,  1 = PORTUGUES

    public static string ObterAlertaNaoPossuiMartelo()
    {
        if(PlayerPrefs.GetInt("INDEXIDIOMA") == 1) return "Voc� precisa de um Martelo Reparador";
        return "You need a Repair Hammer";
    }

    public static string ObterAlertaNaoPossuiMaterialSuficiente(string nomeItem, int qtdItemAtual, int qtdNecessaria)
    {
        if (PlayerPrefs.GetInt("INDEXIDIOMA") == 1) return "Voc� n�o possui "+nomeItem+" suficiente ("+qtdItemAtual+"/"+qtdNecessaria+")";
        return "You do not have enough " + nomeItem + " (" + qtdItemAtual + "/" + qtdNecessaria + ")"; ;
    }

    public static string ObterAlertaInteracaoNaoDisponivelAgora()
    {
        if (PlayerPrefs.GetInt("INDEXIDIOMA") == 1) return "Voc� n�o pode fazer isso agora";
        return "You can't do this now";
    }

    public static string ObterAlertaGarrafaVazia()
    {
        if (PlayerPrefs.GetInt("INDEXIDIOMA") == 1) return "A garrafa est� vazia";
        return "The bottle is empty";
    }

}
