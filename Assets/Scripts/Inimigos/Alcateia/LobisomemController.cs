using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobisomemController : MonoBehaviour
{

    [SerializeField] public Categoria categoria;
    [SerializeField] public Forma forma;
    [SerializeField] public GameObject objFormaHumano, objFormaLobo;
    [SerializeField] public LobisomemController alfa;
    [SerializeField] public List<LobisomemController> betas;
    [SerializeField] public List<Item.ItemDropStruct> dropsItems;

    [SerializeField][HideInInspector] public LobisomemStats lobisomemStats;
    [SerializeField] public LobisomemMovimentacao lobisomemMovimentacao;
    [SerializeField] public LobisomemHumanoMovimentacao lobisomemHumanoMovimentacao;
    GameController gameController;

    private void Awake()
    {
        lobisomemStats = GetComponent<LobisomemStats>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }


    public enum Categoria
    {
        Alfa, Beta, Omega, Outro
    }

    public enum Forma
    {
        Humano, Lobo
    }

    private void Start()
    {
        setarFormaLobisomem();
    }

    private void LateUpdate()
    {
        setarFormaLobisomem();
    }

    private void setarFormaLobisomem()
    {
        if (Categoria.Outro.Equals(categoria)) return;
        objFormaHumano.SetActive(!gameController.isNoite);
        objFormaLobo.SetActive(gameController.isNoite);
    }

}
