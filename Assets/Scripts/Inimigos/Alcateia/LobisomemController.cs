using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobisomemController : MonoBehaviour
{

    [SerializeField] public Categoria categoria;
    [SerializeField] public Forma forma;
    [SerializeField] public LobisomemController alfa;
    [SerializeField] public List<LobisomemController> betas;
    [SerializeField] public List<Item.ItemDropStruct> dropsItems;

    [SerializeField][HideInInspector] public LobisomemStats lobisomemStats;
    [SerializeField] [HideInInspector] public LobisomemMovimentacao lobisomemMovimentacao;

    private void Awake()
    {
        lobisomemStats = GetComponent<LobisomemStats>();
        lobisomemMovimentacao = GetComponent<LobisomemMovimentacao>();
    }

    public enum Categoria
    {
        Alfa, Beta, Omega
    }

    public enum Forma
    {
        Humano, Lobisomem
    }

   

}
