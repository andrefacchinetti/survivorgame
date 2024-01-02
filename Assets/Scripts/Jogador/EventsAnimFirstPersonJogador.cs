using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsAnimFirstPersonJogador : MonoBehaviour
{

    public PlayerController playerController;

    void AnimEventAcenderIsqueiro()
    {
        if(playerController.acendedorFogueiraFP != null)
        {
            playerController.acendedorFogueiraFP.AcenderFogo();
            playerController.acendedorFogueiraTP.AcenderFogo();
        }
    }

}
