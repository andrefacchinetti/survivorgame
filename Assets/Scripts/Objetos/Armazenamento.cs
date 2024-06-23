using UnityEngine;
using System.Collections.Generic;

public class Armazenamento : MonoBehaviour
{

    [SerializeField] List<Item> itensArmazenados;

    private void Awake()
    {
        itensArmazenados = new List<Item>();
    }

    public void GuardarItem(Item item)
    {
        Debug.Log("guardando item");
        itensArmazenados.Add(item);
    }

    public void PegarItem(Item item)
    {
        Debug.Log("pegando item");
        itensArmazenados.Remove(item);
    }

}
