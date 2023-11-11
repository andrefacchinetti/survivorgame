using UnityEngine;

public static class EnumMensagens
{
    // 0 = INGLES,  1 = PORTUGUES

    public static string ObterAlertaNaoPossuiMartelo()
    {
        if(PlayerPrefs.GetInt("INDEXIDIOMA") == 1) //PORTUGUES
        {
            return "Você precisa de um Martelo Reparador";
        }
        else //OUTROS
        {
            return "You need a Repair Hammer";
        }
    }

}
