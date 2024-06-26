using UnityEngine;
using System.Collections.Generic;

public class Armazenamento : MonoBehaviour
{

    [SerializeField] List<Item> itensArmazenados;

    private void Awake()
    {
        itensArmazenados = new List<Item>();
    }

    public void GuardarItem(Item itemBase)
    {
        Debug.Log("guardando item");
        itensArmazenados.Add(itemBase);
    }

    public void PegarItem(Item itemBase)
    {
        Debug.Log("pegando item");
        itensArmazenados.Remove(itemBase);
    }

}
