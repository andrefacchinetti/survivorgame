using UnityEngine;
using Photon.Pun;
using Opsive.UltimateCharacterController.Traits;

public class LobisomemController : MonoBehaviour
{

    [HideInInspector] public GameController gameController;
    [HideInInspector] public LobisomemStats lobisomemStats;
    [HideInInspector] public StatsGeral statsGeral;
    [HideInInspector] public LobisomemMovimentacao lobisomemMovimentacao;
    [HideInInspector] public AttributeManager attributeManager;

    [SerializeField] public CaracteristicasLobisomem caracteristica;
    [SerializeField] float limiteDistanciaAteJogadores = 80;

    public enum CaracteristicasLobisomem
    {
        Normal,
        Veloz,
        Tank,
        Stealth,
        Medroso,
        Covarde,
        Protetor,
        Astuto,
        Beserker
    }

    private void Awake()
    {
        lobisomemStats = GetComponent<LobisomemStats>();
        statsGeral = GetComponent<StatsGeral>();
        lobisomemMovimentacao = GetComponent<LobisomemMovimentacao>();
        attributeManager = GetComponent<AttributeManager>();
    }

    private void Start()
    {
        //TODO: INSERIR CARACTERISTICA ALEATORIA
        atualizarStatsPorCaracteristica();
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

    private void atualizarStatsPorCaracteristica()
    {
        if(CaracteristicasLobisomem.Veloz == caracteristica)
        {
            lobisomemStats.attackInterval = lobisomemStats.attackInterval / 2;
        } 
        else if (CaracteristicasLobisomem.Tank == caracteristica)
        {
            attributeManager.GetAttribute("Health").MaxValue *= 2;
            attributeManager.GetAttribute("Health").Value = attributeManager.GetAttribute("Health").MaxValue;
        }
        else if(CaracteristicasLobisomem.Stealth == caracteristica)
        {
            lobisomemMovimentacao.detectionRadius = 100;
            lobisomemMovimentacao.agent.stoppingDistance = 3;
        }
        else if (CaracteristicasLobisomem.Astuto == caracteristica)
        {
            attributeManager.GetAttribute("Health").MaxValue *= 1.5f;
            attributeManager.GetAttribute("Health").Value = attributeManager.GetAttribute("Health").MaxValue;
        }
    }

}
