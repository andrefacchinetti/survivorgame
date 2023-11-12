using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftMaos : MonoBehaviour
{

    [SerializeField] public Inventario inventario;
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
        ReceitaCraft resultadoCraft = EncontrarReceitaCraft();
        if (resultadoCraft != null)
        {
            removerItensReceitaDoJogador(resultadoCraft);
            adicionarItemCraftadoAoJogador(resultadoCraft.nomeItemResultado);
            limparSlots();
            Debug.Log("Item craftado: " + resultadoCraft);
        }
        else
        {
            // Não foi encontrada uma receita correspondente
            Debug.Log("Nenhuma receita correspondente encontrada.");
        }
    }

    public void AtualizarPreviewResultado()
    {
        ReceitaCraft resultadoCraft = EncontrarReceitaCraft();
        slotResultado.SetupPreviewNoSlot(resultadoCraft);
    }

    private void removerItensReceitaDoJogador(ReceitaCraft resultadoCraft)
    {
        foreach (Item.NomeItem nomeItemIngrediente in resultadoCraft.ingredientes)
        {
            inventario.RemoverItemDoInventarioPorNome(nomeItemIngrediente, 1);
        }
    }

    private void adicionarItemCraftadoAoJogador(Item.NomeItem nomeItem)
    {
        inventario.AdicionarItemAoInventarioPorNome(nomeItem, 1);
    }

    private void limparSlots()
    {
        foreach (var slot in slots)
        {
            slot.ResetSlotHotbar();
        }
        slotResultado.ResetSlotHotbar();
    }

    private ReceitaCraft EncontrarReceitaCraft()
    {
        foreach (ReceitaCraft receita in receitasCraft)
        {
            if (ReceitaCorresponde(receita))
            {
                return receita;
            }
        }
        return null;
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
