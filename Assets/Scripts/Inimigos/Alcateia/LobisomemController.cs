using UnityEngine;

public class LobisomemController : MonoBehaviour
{

    [SerializeField] public GameObject objFormaLobo;
    [SerializeField] public AldeiaController aldeiaController;

    [SerializeField] [HideInInspector] public LobisomemStats lobisomemStats;
    [SerializeField] [HideInInspector] public StatsGeral statsGeral;
    [SerializeField] public LobisomemMovimentacao lobisomemMovimentacao;

    private void Awake()
    {
        lobisomemStats = GetComponent<LobisomemStats>();
        statsGeral = GetComponent<StatsGeral>();
    }
   
    public GameObject obterGameObjectFormaAtiva()
    {
        return objFormaLobo;
    }

}
