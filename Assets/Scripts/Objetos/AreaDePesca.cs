using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class AreaDePesca : MonoBehaviour
{

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "PlayerCharacterCollider")
        {
            PlayerController pc = other.GetComponentInParent<PlayerController>();
            if(pc != null)
            {
                pc.PararAbility(pc.pescarAbility);
            }
        }
    }

}
