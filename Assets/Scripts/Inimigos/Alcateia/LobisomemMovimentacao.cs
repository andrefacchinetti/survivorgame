using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class LobisomemMovimentacao : MonoBehaviour
{

   

    [SerializeField] public GameObject pontoBaseTerritorio;
    [SerializeField] public float distanciaMaximaPontoBase = 50, distanciaMaximaDoSeuAlfa = 10;
    [SerializeField] public float raioDeDistanciaParaAndarAleatoriamente = 20f;
    [SerializeField] public float timerParaAndarAleatoriamente = 5f, timerParaAlfaDecidirComandosParaSeusBetas = 10;


    //MOVIMENTACAO
    [HideInInspector] public Transform target, targetComida;
    [HideInInspector] public NavMeshAgent agent;
    private float timer;
    [SerializeField] public float velocidadeWalk = 0.8f, velocidadeRun = 0.12f;

    //ATAQUE
    public float minimumDistanceAtaque = 5f, distanciaDePerseguicao = 10f, distanciaDeAtaque = 2f;
    public float attackInterval = 1f; // Intervalo de tempo entre ataques
    private float lastAttackTime; // Tempo do último ataque
    [HideInInspector] public bool isAttacking; // Flag para controlar se a IA está atacando

    LobisomemController lobisomemController;
    LobisomemStats lobisomemStats;
    [HideInInspector] public Animator animator;

    private void Start()
    {
        lobisomemController = GetComponent<LobisomemController>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        lobisomemStats = GetComponent<LobisomemStats>();
        timer = timerParaAndarAleatoriamente;
        
        if (lobisomemController.categoria.Equals(LobisomemController.Categoria.Alfa)) InvokeRepeating("ComandosAlfaParaBetas", 0, timerParaAlfaDecidirComandosParaSeusBetas);
    }

    private void Update()
    {
        if (LobisomemController.Categoria.Omega.Equals(lobisomemController.categoria)) movimentacaoOmega();
        else if (LobisomemController.Categoria.Alfa.Equals(lobisomemController.categoria)) movimentacaoAlfa();
        else if (LobisomemController.Categoria.Beta.Equals(lobisomemController.categoria)) movimentacaoBeta();
        verificarCorrerAndar();
        verificarAtaque();
        verificarProximoComida();
    }

    private void verificarProximoComida()
    {
        if(targetComida != null)
        {
            if (targetComida.GetComponent<ItemDrop>() !=null && !targetComida.GetComponent<ItemDrop>().estaSendoComido)
            {
                float distanceToTarget = Vector3.Distance(transform.position, targetComida.transform.position);
                if (distanceToTarget < 1)
                {
                    transform.LookAt(targetComida);
                    animator.SetTrigger("comendo");
                    targetComida.GetComponent<ItemDrop>().estaSendoComido = true;
                }
                else
                {
                    transform.LookAt(targetComida);
                    agent.SetDestination(targetComida.position);
                }
            }
            else
            {
                targetComida = null;
            }
        }
    }

    private void verificarAtaque()
    {
        if (target == null) return;
        float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
        if (target.GetComponent<PlayerController>().isMorto || distanceToTarget > minimumDistanceAtaque)
        {
            targetComida = target;
            target = null;
        }
        else if (distanceToTarget < distanciaDeAtaque) // Ataca o alvo
        {
            if (!isAttacking && Time.time > lastAttackTime + attackInterval)
            {
                transform.LookAt(target.transform.position);
                lastAttackTime = Time.time;
                animator.SetTrigger("attack"+Random.Range(1,3));
            }
        }
        else // Persegue o alvo
        {
            agent.SetDestination(target.transform.position);
        }
    }

    bool recarregandoEnergia = false;
    private void verificarCorrerAndar()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Uivar") || animator.GetCurrentAnimatorStateInfo(0).IsName("Comendo"))
        {
            agent.speed = 0;
        }
        else if (!recarregandoEnergia && (target != null || estaDistanteDoPontoBaseTerritorio()))
        {
            agent.speed = velocidadeRun;
            lobisomemStats.setarEnergiaAtual(lobisomemStats.energiaAtual - lobisomemStats.consumoEnergiaPorSegundo * Time.deltaTime);
        }
        else
        {
            agent.speed = velocidadeWalk;
            lobisomemStats.setarEnergiaAtual(lobisomemStats.energiaAtual + lobisomemStats.recuperacaoEnergiaPorSegundo * Time.deltaTime);
            if (lobisomemStats.energiaAtual > 80) recarregandoEnergia = false;
        }
        if (lobisomemStats.energiaAtual <= 0 && !recarregandoEnergia)
        {
            recarregandoEnergia = true;
        }
        setarAnimacaoPorVelocidade();
    }

    private void setarAnimacaoPorVelocidade()
    {
        if (agent.velocity.magnitude > 0.2f)
        {
            animator.SetBool("run", true);
            animator.SetBool("walk", false);
        }
        else if (agent.velocity.magnitude > 0.05f)
        {
            animator.SetBool("walk", true);
            animator.SetBool("run", false);
        }
        else
        {
            animator.SetBool("walk", false);
            animator.SetBool("run", false);
        }
    }

    private bool estaDistanteDoPontoBaseTerritorio()
    {
        float distance = Vector3.Distance(transform.position, pontoBaseTerritorio.transform.position);  // Calcula a distância entre o inimigo e a posicao do territorio base
        if (distance > distanciaMaximaPontoBase)
        {
            return true;
        }
        return false;
    }

    private bool estaDistanteDoAlfa()
    {
        float distance = Vector3.Distance(transform.position, lobisomemController.alfa.transform.position);  // Calcula a distância entre o inimigo e a posicao do territorio base
        if (distance > distanciaMaximaDoSeuAlfa)
        {
            return true;
        }
        return false;
    }

    private void movimentacaoOmega()
    {
        movimentarAleatoriamentePeloMapa();
    }

    private void movimentarAleatoriamentePeloMapa()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Uivar") || targetComida != null || animator.GetCurrentAnimatorStateInfo(0).IsName("Comendo")) return;
        if (estaDistanteDoPontoBaseTerritorio())
        {
            target = null;
            targetComida = null;
            agent.SetDestination(pontoBaseTerritorio.transform.position);
        }
        else
        {
            if (target == null)
            {
                andarAleatoriamente();
            }
            else
            {
                perseguirJogador();
            }
        }
    }

    private void movimentacaoAlfa()
    {
        movimentarAleatoriamentePeloMapa();
    }

    public void ComandosAlfaParaBetas()
    {
        if (!LobisomemController.Categoria.Alfa.Equals(lobisomemController.categoria)) return;
        foreach (LobisomemController beta in lobisomemController.betas)
        {
            if (lobisomemStats.isEstadoAgressivo)
            {
                beta.lobisomemStats.isEstadoAgressivo = true;
                beta.lobisomemMovimentacao.target = target;
            }
            else
            {
                beta.lobisomemStats.isEstadoAgressivo = false;
                beta.lobisomemMovimentacao.target = null;
            }
        }
    }

    public void ComandosBetasParaAlfa()
    {
        lobisomemController.alfa.lobisomemStats.AumentarNivelAgressividade(15);
    }

    private void movimentacaoBeta()
    {
        if (lobisomemController.alfa == null || lobisomemController.alfa.lobisomemStats.isDead)
        {
            lobisomemStats.isEstadoAgressivo = true;
            movimentarAleatoriamentePeloMapa();
        }
        else
        {
            if (estaDistanteDoAlfa())
            {
                lobisomemController.alfa.lobisomemMovimentacao.Uivar();
                target = null;
                agent.SetDestination(lobisomemController.alfa.transform.position);
            }
            else
            {
                if(target != null)
                {
                    perseguirJogador();
                }
                else
                {
                    andarAleatoriamente();
                }
            }
        }
    }


    private void andarAleatoriamente()
    {
        timer += Time.deltaTime;

        if (timer >= timerParaAndarAleatoriamente)
        {

            Vector3 newPos = RandomNavSphere(transform.position, raioDeDistanciaParaAndarAleatoriamente, -1);
            agent.SetDestination(newPos);
            timer = 0;
        }
    }

    private void perseguirJogador()
    {
        agent.SetDestination(target.position);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            lobisomemStats.VerificarSePlayerEstaArmado(other.gameObject);
            if(target == null && lobisomemStats.isEstadoAgressivo)
            {
                if (other.GetComponent<PlayerController>().isMorto)
                {
                    //targetComida = other.transform; //REMOVIDO OPACAO DE PLAYER MORTO VIRAR COMIDA, POIS É NECESSARIO INSTANCIAR UM CORPO MORTO QDO UM PLAYER MORRER PARA SER DESTRUIDO AO SER COMIDO
                }
                else
                {
                    target = other.transform;
                }
            }
        }
        if (other.tag == "ItemDrop" && other.GetComponent<ItemDrop>().nomeItem.GetTipoItemEnum().Equals(Item.TiposItems.Consumivel.ToString()))
        {
            if (other.GetComponent<ItemDrop>().nomeItem.Equals(Item.NomeItem.CarneCrua) || other.GetComponent<ItemDrop>().nomeItem.Equals(Item.NomeItem.CarneCozida)
                || other.GetComponent<ItemDrop>().nomeItem.Equals(Item.NomeItem.PeixeCru) || other.GetComponent<ItemDrop>().nomeItem.Equals(Item.NomeItem.PeixeCozido))
            {
                //gostou da comida
                targetComida = other.transform;
                Debug.Log("achou comida");
            }
            else
            {
                //nao gosta da comida
            }
        }
    }

   

    Vector3 RandomNavSphere(Vector3 origin, float distance, int layermask) //Posicao aleatoria no mapa
    {
        Vector3 randDirection = Random.insideUnitSphere * distance;
        randDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, distance, layermask);
        return navHit.position;
    }

    void GoAtk()
    {
        isAttacking = true;
    }

    void NotAtk()
    {
        isAttacking = false;
    }

    void AnimEventComeu()
    {
        if (targetComida != null && targetComida.tag != "Player") { 
            Destroy(targetComida.gameObject); 
        }
    }

    private void Uivar()
    {
        if (target != null || animator.GetCurrentAnimatorStateInfo(0).IsName("Uivar")) return;
        animator.SetTrigger("uivar");
    }

}
