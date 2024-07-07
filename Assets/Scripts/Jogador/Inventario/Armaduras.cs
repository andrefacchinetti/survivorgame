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
    public float moveSpeedBonus = 0;
    public int calorBonus = 0;

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

    public void equiparStatsArmadura(ItemDefinitionBase itemBase)
    {
        foreach (ArmaduraStats armorStats in armadurasStats)
        {
            if (itemBase.name == armorStats.itemBase.name)
            {
                armorStats.visualObj.SetActive(true);
                inventario.statsJogador.AumentarArmorJogador(armorStats.armor);
                calorBonus += armorStats.calor;
                moveSpeedBonus += armorStats.moveSpeed;
                inventario.statsJogador.AtualizarMoveSpeedJogador();
                break;
            }
        }
    }

    public void DesequiparArmaduraDoTipoSlot(TipoSlotArmadura tipoSlotArmadura)
    {
        foreach (ArmaduraStats armorStats in armadurasStats)
        {
            if (tipoSlotArmadura == armorStats.TipoSlotArmadura && armorStats.visualObj.activeSelf)
            {
                armorStats.visualObj.SetActive(false);
                inventario.statsJogador.DiminuirArmorJogador(armorStats.armor);
                calorBonus -= armorStats.calor;
                moveSpeedBonus -= armorStats.moveSpeed;
                inventario.statsJogador.AtualizarMoveSpeedJogador();
                break;
            }
        }
    }

    public void DesequiparArmaduraSeEstiverUsando(Item itemResponse)
    {
        if (itemResponse == null) return;
        foreach (ItemArmadura slotItem in slotsItemArmadura)
        {
            if (slotItem.item != null && itemResponse.itemIdentifierAmount.ItemDefinition == slotItem.item.itemIdentifierAmount.ItemDefinition)
            {
                Debug.Log("DesequiparArmaduraSeEstiverUsando: " + slotItem.item.nomePortugues);
                slotItem.SetupItemNoSlot(null);
                break;
            }
        }
    }

    public void EquiparArmaduraSelecionada(Item itemResponse)
    {
        if (itemResponse == null) return;
        foreach (ItemArmadura slotItem in slotsItemArmadura)
        {
            if (slotItem.ColocarItemNoSlot(itemResponse))
            {
                Debug.Log("EquiparArmaduraSelecionada: " + itemResponse.nomePortugues);
                break;
            }
        }
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
