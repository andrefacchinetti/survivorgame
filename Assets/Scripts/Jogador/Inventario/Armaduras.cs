using System.Collections;
using Opsive.Shared.Inventory;
using System.Collections.Generic;
using UnityEngine;

public class Armaduras : MonoBehaviour
{

    [SerializeField] public Inventario inventario;
    [SerializeField] public List<ItemArmadura> slotsItemArmadura;
    [SerializeField] [HideInInspector] public ItemArmadura slotItemArmaduraSelecionada;
    [SerializeField] public bool estaSelecionandoSlotArmadura;

    [SerializeField] public List<ArmaduraStats> armadurasStats;

    [System.Serializable]
    public struct ArmaduraStats
    {
        public ItemDefinitionBase itemBase;
        public TipoSlotArmadura TipoSlotArmadura;
        public GameObject visualObj;
        public int armor;
        public int moveSpeed;
        public int calor;
    }

    public enum TipoSlotArmadura
    {
        cabeca,
        peitoral,
        pernas,
        pes,
        maos
    }

    public void EquiparArmadura(ItemDefinitionBase itemBase)
    {
        Debug.Log("EquiparArmadura");
        foreach (ArmaduraStats armorStats in armadurasStats)
        {
            if (itemBase.name == armorStats.itemBase.name)
            {
                armorStats.visualObj.SetActive(true);
                Debug.Log("EquiparArmadura ACHOU");
                break;
            }
        }
        Debug.Log("EquiparArmadura NAO ACHOU");
    }

    public void DesequiparArmaduraDoTipoSlot(TipoSlotArmadura tipoSlotArmadura)
    {
        Debug.Log("DesequiparArmaduraDoTipoSlot");
        foreach (ArmaduraStats armorStats in armadurasStats)
        {
            if (tipoSlotArmadura == armorStats.TipoSlotArmadura && armorStats.visualObj.activeSelf)
            {
                armorStats.visualObj.SetActive(false);
                Debug.Log("DesequiparArmaduraDoTipoSlot ACHOU");
                break;
            }
        }
        Debug.Log("DesequiparArmaduraDoTipoSlot NAO ACHOU");
    }

    public void SelecionouItemParaSlotArmadura(Item item)
    {
        if (!slotItemArmaduraSelecionada.ColocarItemNoSlot(item))
        {
            Debug.Log("O item não pertence a essa categoria de slot");
        }
        estaSelecionandoSlotArmadura = false;
    }

}
