using Opsive.Shared.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrutaEmArvore : MonoBehaviour
{

    [SerializeField] public StatsGeral statsGeral;

    public void ColetarUmaFruta(ItemDefinitionBase itemDefinitionBase)
    {
        List<Item.ItemDropStruct> itemDrops = new List<Item.ItemDropStruct>();
        foreach (Item.ItemDropStruct itemDropScruct in statsGeral.dropsItems)
        {
            if (itemDropScruct.itemDefinition.Equals(itemDefinitionBase))
            {
                if (itemDropScruct.qtdMaxDrops > 0)
                {
                    Item.ItemDropStruct novo = new Item.ItemDropStruct();
                    novo.itemDefinition = itemDropScruct.itemDefinition;
                    novo.tipoItem = itemDropScruct.tipoItem;
                    novo.materialPersonalizado = itemDropScruct.materialPersonalizado;
                    novo.qtdMinDrops = itemDropScruct.qtdMinDrops - 1;
                    novo.qtdMaxDrops = itemDropScruct.qtdMaxDrops - 1;
                    itemDrops.Add(novo);
                }
            }
            else
            {
                itemDrops.Add(itemDropScruct);
            }
        }
        statsGeral.dropsItems = itemDrops;
    }

}
