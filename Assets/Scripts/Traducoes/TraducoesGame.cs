using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TraducoesGame : MonoBehaviour
{

    public TMP_Text aguardesuavez, DISTRIBUINDO, ATACANDO, REMANEJANDO, De, Para, Sair, Room, Digite, BONUS;

    // Start is called before the first frame update
    void Start()
    {
        CarregarTraducoes();
    }

    public void CarregarTraducoes()
    {
        if (PlayerPrefs.GetInt("INDEXIDIOMA") == 1)  //PORTUGUES
        {
            if (aguardesuavez != null) aguardesuavez.text = "Aguarde sua vez de jogar...";
            if (DISTRIBUINDO != null) DISTRIBUINDO.text = "DISTRIBUINDO";
            if (ATACANDO != null) ATACANDO.text = "ATACANDO";
            if (REMANEJANDO != null) REMANEJANDO.text = "REMANEJANDO";
            if (De != null) De.text = "De:";
            if (Para != null) Para.text = "Para:";
            if (Sair != null) Sair.text = "Sair";
            if (Room != null) Room.text = "Sala";
            if (Digite != null) Digite.text = "Digite...";
            if (BONUS != null) BONUS.text = "BÔNUS";
        }
        else //INGLES
        {
            if (aguardesuavez != null) aguardesuavez.text = "Wait your turn to play...";
            if (DISTRIBUINDO != null) DISTRIBUINDO.text = "DISTRIBUTING";
            if (ATACANDO != null) ATACANDO.text = "ATTACKING";
            if (REMANEJANDO != null) REMANEJANDO.text = "REMANAGING";
            if (De != null) De.text = "From:";
            if (Para != null) Para.text = "To:";
            if (Sair != null) Sair.text = "Leave";
            if (Room != null) Room.text = "Room";
            if (Digite != null) Digite.text = "Type it...";
            if (BONUS != null) BONUS.text = "BONUS";
        }
    }
}
