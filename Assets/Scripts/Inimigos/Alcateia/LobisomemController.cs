using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobisomemController : MonoBehaviour
{

    [SerializeField] public Categoria categoria;
    [SerializeField] public Forma forma;
    [SerializeField] public Profissao profissao;
    [SerializeField] public GameObject objFormaHumano, objFormaLobo;
    [SerializeField] public LobisomemController alfa;
    [SerializeField] public AldeiaController aldeiaController;
    [SerializeField] public List<LobisomemController> betas;
    [SerializeField] public GameObject objProfissaoVaraPesca;

    [SerializeField] [HideInInspector] public LobisomemStats lobisomemStats;
    [SerializeField] [HideInInspector] public StatsGeral statsGeral;
    [SerializeField] public LobisomemMovimentacao lobisomemMovimentacao, lobisomemHumanoMovimentacao;
    GameController gameController;

    private void Awake()
    {
        lobisomemStats = GetComponent<LobisomemStats>();
        statsGeral = GetComponent<StatsGeral>();
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

    public enum Profissao
    {
        Alfa, Seguranca, Cacador, Pescador, ColetorAgua, Fazendeiro, ColhedorFrutas, Rezador
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
        if (gameController.isNoite)
        {
            objFormaHumano.transform.position = objFormaLobo.transform.position;
            objFormaHumano.transform.rotation = objFormaLobo.transform.rotation;
            forma = Forma.Lobo;
        }
        else
        {
            objFormaLobo.transform.position = objFormaHumano.transform.position;
            objFormaLobo.transform.rotation = objFormaHumano.transform.rotation;
            forma = Forma.Humano;
        }
        objFormaHumano.SetActive(!gameController.isNoite);
        objFormaLobo.SetActive(gameController.isNoite);
    }

    public GameObject obterGameObjectFormaAtiva()
    {
        if (forma.Equals(Forma.Humano)) return objFormaHumano;
        else return objFormaLobo;
    }

}
