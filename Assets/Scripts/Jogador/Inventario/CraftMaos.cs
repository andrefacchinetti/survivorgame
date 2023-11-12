using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftMaos : MonoBehaviour
{

    [SerializeField] public List<SlotHotbar> slots = new List<SlotHotbar>();
    [SerializeField] public SlotHotbar slotResultado;

    // Defina suas receitas de craft aqui
    [SerializeField] public List<ReceitaCraft> receitasCraft = new List<ReceitaCraft>();

    [System.Serializable]
    public class ReceitaCraft
    {
        public Item.NomeItem nomeItemResultado;
        public List<Item.NomeItem> ingredientes;
    }

    public void CraftarItens()
    {
        Item.NomeItem resultadoCraft = EncontrarReceitaCraft();
        if (!Item.NomeItem.Nenhum.Equals(resultadoCraft))
        {
            // O item pode ser craftado, faça o que for necessário aqui
            Debug.Log("Item craftado: " + resultadoCraft);
        }
        else
        {
            // Não foi encontrada uma receita correspondente
            Debug.Log("Nenhuma receita correspondente encontrada.");
        }
    }

    private Item.NomeItem EncontrarReceitaCraft()
    {
        foreach (ReceitaCraft receita in receitasCraft)
        {
            if (ReceitaCorresponde(receita))
            {
                return receita.nomeItemResultado;
            }
        }
        return Item.NomeItem.Nenhum;
    }

    private bool ReceitaCorresponde(ReceitaCraft receita)
    {
        // Verifica se os ingredientes da receita correspondem aos itens nos slots
        foreach (var ingrediente in receita.ingredientes)
        {
            bool ingredienteEncontrado = false;

            foreach (var slot in slots)
            {
                if (slot.item != null && slot.item.nomeItem == ingrediente)
                {
                    ingredienteEncontrado = true;
                    break;
                }
            }

            if (!ingredienteEncontrado)
            {
                return false; // Se um ingrediente não for encontrado, a receita não corresponde
            }
        }

        return true; // Todos os ingredientes foram encontrados nos slots
    }

}
