using UnityEngine;

public class LobisomemStats : MonoBehaviour
{

    //STATS CURRENT
    [SerializeField] [HideInInspector] public float energiaAtual;

    //ATAQUE
    [SerializeField] public float distanciaDePerseguicao = 10f, distanciaDeAtaque = 2f;
    [SerializeField] public float attackInterval = 1f; // Intervalo de tempo entre ataques
    [SerializeField] [HideInInspector] public float lastAttackTime; // Tempo do �ltimo ataque
    [SerializeField] public float destinationOffset = 1f;
    [SerializeField] public float walkSpeed = 0.8f, runSpeed = 1.5f, speedVariation = 0.5f;
    [SerializeField] public float leadTime = 1.2f, leadDistance = 2, tempoMudancaDeTurnoProfissoes = 300;
    [SerializeField] public float pathUpdateDistanceThreshold = 10;

    [SerializeField][HideInInspector] public LobisomemController lobisomemController;
    [SerializeField] StatsGeral statsGeral;


    private void Awake()
    {
        lobisomemController = GetComponentInParent<LobisomemController>();
        statsGeral = GetComponent<StatsGeral>();
    }

	public void AcoesTomouDano()
	{
        lobisomemController.lobisomemMovimentacao.animator.SetTrigger("hit");
    }

    public void AcoesMorreu()
    {
        if (lobisomemController.statsGeral.health.IsAlive()) return;
        lobisomemController.lobisomemMovimentacao.animator.SetBool("isDead", true);
        lobisomemController.lobisomemMovimentacao.agent.isStopped = true;
        lobisomemController.lobisomemMovimentacao.agent.speed = 0;
        Debug.Log("Lobisomen morreu");
    }

}
