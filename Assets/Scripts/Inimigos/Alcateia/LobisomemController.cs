using UnityEngine;

public class LobisomemController : MonoBehaviour
{

    [SerializeField] public GameObject objFormaLobo;
    [SerializeField] [HideInInspector] public GameController gameController;

    [SerializeField] [HideInInspector] public LobisomemStats lobisomemStats;
    [SerializeField] [HideInInspector] public StatsGeral statsGeral;
    [SerializeField] public LobisomemMovimentacao lobisomemMovimentacao;

    [SerializeField] float limiteDistanciaAteJogadores = 80;

    private GameObject[] jogadores;

    private void Awake()
    {
        lobisomemStats = GetComponent<LobisomemStats>();
        statsGeral = GetComponent<StatsGeral>();
    }

    private void Start()
    {
        jogadores = GameObject.FindGameObjectsWithTag("Player");
    }

    public bool estouLongeDeAlgumJogador()
    {
        float distanciaLimiteAoQuadrado = limiteDistanciaAteJogadores * limiteDistanciaAteJogadores;
        foreach (GameObject jogador in jogadores)
        {
            if(jogador != null)
            {
                float distanciaAoQuadrado = (lobisomemMovimentacao.transform.position - jogador.transform.position).sqrMagnitude;
                if (distanciaAoQuadrado > distanciaLimiteAoQuadrado)  // Os objetos NÃO estão dentro da distância limite
                {
                    return true;
                }
            }
        }
        return false;
    }

    public GameObject obterGameObjectFormaAtiva()
    {
        return objFormaLobo;
    }

}
