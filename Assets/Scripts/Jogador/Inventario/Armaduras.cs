using System.Collections;
using Opsive.Shared.Inventory;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Armaduras : MonoBehaviour
{

    [SerializeField] public Inventario inventario;
    [SerializeField] public List<ItemArmadura> slotsItemArmadura;
    [HideInInspector] public ItemArmadura slotItemArmaduraSelecionada;
    [HideInInspector] public bool estaSelecionandoSlotArmadura;

    [SerializeField] TMP_Text txSpeedBonus, txArmorBonus, txCalorBonus;

    [SerializeField] public List<ArmaduraStats> armadurasStats;
    public Dictionary<string, ArmaduraStats> mapArmaduraStats;

    [HideInInspector] public float moveSpeedBonus = 0;
    [HideInInspector] public int calorBonus = 0;

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

    private void Awake()
    {
        mapArmaduraStats = new Dictionary<string, ArmaduraStats>();
        foreach(ArmaduraStats armadura in armadurasStats)
        {
            mapArmaduraStats.Add(armadura.itemBase.name, armadura);
        }
    }

    private void Start()
    {
        atualizarTextoArmaduraStats();
    }

    private void atualizarTextoArmaduraStats()
    {
        txSpeedBonus.text = "Move Speed: " + (moveSpeedBonus >= 0 ? "+" : "-") + moveSpeedBonus;
        txArmorBonus.text = "Armor: " + (inventario.playerController.characterAttributeManager.GetAttribute("Armor").Value >= 0 ? "+" : "-") + inventario.playerController.characterAttributeManager.GetAttribute("Armor").Value;
        txCalorBonus.text = "Heat: " + (calorBonus >= 0 ? "+" : "-") + calorBonus;
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
                atualizarTextoArmaduraStats();
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
                atualizarTextoArmaduraStats();
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
