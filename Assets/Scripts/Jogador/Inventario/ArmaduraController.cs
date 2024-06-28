using Opsive.Shared.Inventory;
using System.Collections.Generic;
using UnityEngine;

public class ArmaduraController : MonoBehaviour
{

    [SerializeField] Dictionary<ItemDefinitionBase, GameObject> mapArmaduraObjeto;
    [SerializeField] Dictionary<ItemDefinitionBase, ArmaduraStats> mapArmadura;

    public struct ArmaduraStats
    {
        public GameObject objeto;
        public int armorValue;
        public int moveSpeedValue;
    }

}
