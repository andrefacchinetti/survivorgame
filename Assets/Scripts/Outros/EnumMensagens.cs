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

    public static string ObterAlertaInteracaoNaoDisponivelAgora()
    {
        if (PlayerPrefs.GetInt("INDEXIDIOMA") == 1) return "Você não pode fazer isso agora";
        return "You can't do this now";
    }

    public static string ObterAlertaGarrafaVazia()
    {
        if (PlayerPrefs.GetInt("INDEXIDIOMA") == 1) return "A garrafa está vazia";
        return "The bottle is empty";
    }

    public static string ObterAlertaPesoMochilaExcedido()
    {
        if (PlayerPrefs.GetInt("INDEXIDIOMA") == 1) return "A mochila está muito pesada";
        return "The backpack is very heavy";
    }

    public static string ObterNomeTipoItemFormatado(Item.TiposItems tipo)
    {
        if(tipo == Item.TiposItems.Arma)
        {
            if (PlayerPrefs.GetInt("INDEXIDIOMA") == 1) return "Arma";
            return "Weapon";
        }
        else if (tipo == Item.TiposItems.Armadura)
        {
            if (PlayerPrefs.GetInt("INDEXIDIOMA") == 1) return "Armadura";
            return "Armor";
        }
        else if (tipo == Item.TiposItems.Consumivel)
        {
            if (PlayerPrefs.GetInt("INDEXIDIOMA") == 1) return "Consumível";
            return "Consumable";
        }
        else if (tipo == Item.TiposItems.ConsumivelCozinha)
        {
            if (PlayerPrefs.GetInt("INDEXIDIOMA") == 1) return "Cozinha";
            return "Kitchen";
        }
        else if (tipo == Item.TiposItems.Ferramenta)
        {
            if (PlayerPrefs.GetInt("INDEXIDIOMA") == 1) return "Ferramenta";
            return "Tool";
        }
        else if (tipo == Item.TiposItems.Municao)
        {
            if (PlayerPrefs.GetInt("INDEXIDIOMA") == 1) return "Munição";
            return "Ammunition";
        }
        else if (tipo == Item.TiposItems.Objeto)
        {
            if (PlayerPrefs.GetInt("INDEXIDIOMA") == 1) return "Objeto";
            return "Object";
        }
        else if (tipo == Item.TiposItems.Recurso)
        {
            if (PlayerPrefs.GetInt("INDEXIDIOMA") == 1) return "Recurso";
            return "Resource";
        }
        return "";

    }

}
