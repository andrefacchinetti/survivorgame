using UnityEngine;

public class LobisomemStats : MonoBehaviour
{

    //STATS CURRENT
    [SerializeField] [HideInInspector] public float energiaAtual;

    //ATAQUE
    [SerializeField] public float distanciaDePerseguicao = 10f, distanciaDeAtaque = 2f;
    [SerializeField] public float attackInterval = 1f; // Intervalo de tempo entre ataques
    [SerializeField] [HideInInspector] public float lastAttackTime; // Tempo do �ltimo ataque
    [SerializeField] public float walkSpeed = 0.8f, runSpeed = 1.5f;
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
        if(LobisomemController.CaracteristicasLobisomem.Beserker == lobisomemController.caracteristica)
        {
            if (lobisomemController.attributeManager.GetAttribute("Health").Value < lobisomemController.attributeManager.GetAttribute("Health").MaxValue / 2)
            {
                attackInterval = attackInterval / 2;
            }
            else if (lobisomemController.attributeManager.GetAttribute("Health").Value < lobisomemController.attributeManager.GetAttribute("Health").MaxValue / 3)
            {
                attackInterval = attackInterval / 3;
            }
        }
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
