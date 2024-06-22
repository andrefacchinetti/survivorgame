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
        
    }

    public void PegarItem(Item item)
    {

    }

}
