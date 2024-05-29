using UnityEngine;
using Photon.Pun;

public class LobisomemController : MonoBehaviour
{

    [SerializeField] [HideInInspector] public GameController gameController;

    [HideInInspector] public LobisomemStats lobisomemStats;
    [HideInInspector] public StatsGeral statsGeral;
    [HideInInspector] public LobisomemMovimentacao lobisomemMovimentacao;

    [SerializeField] float limiteDistanciaAteJogadores = 80;

    private void Awake()
    {
        lobisomemStats = GetComponent<LobisomemStats>();
        statsGeral = GetComponent<StatsGeral>();
        lobisomemMovimentacao = GetComponent<LobisomemMovimentacao>();
    }

    public bool estouLongeDeAlgumJogador()
    {
        float distanciaLimiteAoQuadrado = limiteDistanciaAteJogadores * limiteDistanciaAteJogadores;
        foreach (GameObject jogador in gameController.playersOnline)
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

}
