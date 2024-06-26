using UnityEngine;
using System.Collections.Generic;

public class Armazenamento : MonoBehaviour
{

    [SerializeField] Dictionary<Item, int> itensArmazenados;

    private void Awake()
    {
        itensArmazenados = new Dictionary<Item, int>();
    }

    public void GuardarItem(Item itemBase, int qtd)
    {
        Debug.Log("guardando item");
        if (itensArmazenados.ContainsKey(itemBase))
        {
            itensArmazenados[itemBase] += qtd;
        }
        else
        {
            itensArmazenados.Add(itemBase, qtd);
        }
    }

    public void PegarItem(Item itemBase, int qtd)
    {
        Debug.Log("pegando item");
        if (itensArmazenados.ContainsKey(itemBase))
        {
            itensArmazenados[itemBase] -= qtd;

            if (itensArmazenados[itemBase] <= 0)
            {
                itensArmazenados.Remove(itemBase);
            }
        }
        else
        {
            Debug.LogWarning("Item não encontrado no baú.");
        }
    }

    public int ObterQuantidadeItem(Item itemBase)
    {
        if (itensArmazenados.ContainsKey(itemBase))
        {
            return itensArmazenados[itemBase];
        }
        else
        {
            return 0; // Retorna 0 se o item não estiver no dicionário
        }
    }

}
