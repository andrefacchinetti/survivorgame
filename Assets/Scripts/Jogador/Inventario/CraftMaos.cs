using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftMaos : MonoBehaviour
{

    [SerializeField] public Inventario inventario;
    [SerializeField] GameObject contentCanvasReceitas, contentReceitas, prefabItemReceita;
    [SerializeField] public List<SlotHotbar> slots = new List<SlotHotbar>();
    [SerializeField] public SlotHotbar slotResultado;

    // Defina suas receitas de craft aqui
    [SerializeField] public List<ReceitaCraft> receitasCraft = new List<ReceitaCraft>();

    [System.Serializable]
    public class ReceitaCraft
    {
        public Item.NomeItem nomeItemResultado;
        public List<Ingrediente> ingredientes = new List<Ingrediente>();
    }
    [System.Serializable]
    public class Ingrediente
    {
        public Item.NomeItem nomeItem;
        public int quantidade;
        public Item.ItemStruct itemStruct;
    }

    private void Start()
    {
        InstanciarReceitasNoContent();
    }

    public void LimparTodosSlotsCraft()
    {
        foreach (SlotHotbar slot in slots)
        {
            slot.ResetSlotHotbar();
        }
        slotResultado.ResetSlotHotbar();
    }

    public void CraftarItens()
    {
        ReceitaCraft resultadoCraft = EncontrarReceitaCraft();
        if (resultadoCraft != null)
        {
            removerItensReceitaDoJogador(resultadoCraft);
            adicionarItemCraftadoAoJogador(resultadoCraft.nomeItemResultado);
            limparSlots();
            Debug.Log("Item craftado: " + resultadoCraft.nomeItemResultado);
        }
        else
        {
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
        foreach (Ingrediente ingrediente in resultadoCraft.ingredientes)
        {
            if(!ingrediente.nomeItem.Equals(Item.NomeItem.FacaSimples) && !ingrediente.nomeItem.Equals(Item.NomeItem.FacaAvancada)) //Itens que não perdem após craft
            {
                inventario.RemoverItemDoInventarioPorNome(ingrediente.nomeItem, ingrediente.quantidade);
            }
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
        // Cria uma cópia dos slots para manipulação
        List<SlotHotbar> slotsCopy = new List<SlotHotbar>(slots);

        // Verifica se os ingredientes da receita correspondem aos itens nos slots
        foreach (var ingrediente in receita.ingredientes)
        {
            bool ingredienteEncontrado = false;

            foreach (var slot in slotsCopy)
            {
                if (slot.item != null && slot.item.nomeItem == ingrediente.nomeItem && slot.qtdItemNoSlot == ingrediente.quantidade)
                {
                    ingredienteEncontrado = true;
                    break;
                }
            }

            if (!ingredienteEncontrado)
            {
                return false; // Se um ingrediente não for encontrado ou a quantidade for insuficiente, a receita não corresponde
            }
        }

        return true; // Todos os ingredientes foram encontrados nos slots com as quantidades adequadas
    }

    public void ToggleVerReceitas()
    {
        contentCanvasReceitas.SetActive(!contentCanvasReceitas.activeSelf);
    }

    private void InstanciarReceitasNoContent()
    {
        foreach(ReceitaCraft receitaCraft in receitasCraft)
        {
            foreach(Ingrediente ingrediente in receitaCraft.ingredientes)
            {
                ingrediente.itemStruct = inventario.ObterItemStructPeloNome(ingrediente.nomeItem);
            }
            GameObject novaReceita = Instantiate(prefabItemReceita, new Vector3(), new Quaternion(), contentReceitas.transform);
            Item.ItemStruct itemStruct = inventario.ObterItemStructPeloNome(receitaCraft.nomeItemResultado);
            novaReceita.GetComponent<ItemReceitaView>().SetupReceitaView(itemStruct, receitaCraft.ingredientes);
        }
    }

}
