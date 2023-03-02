using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobisomemController : MonoBehaviour
{

    [SerializeField] public Categoria categoria;
    [SerializeField] public Forma forma;
    [SerializeField] public float nivelSelvageria = 50, nivelSelvageriaMax = 100;
    [SerializeField] public LobisomemController alfa;
    [SerializeField] public List<LobisomemController> betas;
    [SerializeField] public List<Item.ItemDropStruct> dropsItems;

    public enum Forma
    {
        Humano, Lobisomem
    }

    public enum Categoria
    {
        Alfa, Beta, Omega
    }

}
