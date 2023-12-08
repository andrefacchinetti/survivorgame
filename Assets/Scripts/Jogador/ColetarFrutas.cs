using UnityEngine;
using Opsive.UltimateCharacterController.Character.Abilities;

public class ColetarFrutas : DetectObjectAbilityBase
{

    public override void OnTriggerExit(Collider other)
    {
        // The detected object will be set when the ability starts and contains a reference to the object that allowed the ability to start.
        if (other.gameObject == m_DetectedObject)
        {
            Debug.Log("parando de coletar frutas pq esta longe");
            StopAbility();
        }
    }

}
