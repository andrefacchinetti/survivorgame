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
    public Transform target, targetComida;
    [HideInInspector] public NavMeshAgent agent;
    private float timer;
    //[SerializeField] public float velocidadeWalk = 0.8f, velocidadeRun = 0.12f;

    //ATAQUE
    public float minimumDistanceAtaque = 5f, distanciaDePerseguicao = 10f, distanciaDeAtaque = 2f;
    public float attackInterval = 1f; // Intervalo de tempo entre ataques
    private float lastAttackTime; // Tempo do �ltimo ataque
    

    [SerializeField] LobisomemController lobisomemController;
    [SerializeField] LobisomemStats lobisomemStats;
    [HideInInspector] public Animator animator;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        timer = timerParaAndarAleatoriamente;
    }

    private void Update()
    {
       /* if (LobisomemController.Categoria.Omega.Equals(lobisomemController.categoria)) movimentacaoOmega();
        else if (LobisomemController.Categoria.Alfa.Equals(lobisomemController.categoria)) movimentacaoAlfa();
        else if (LobisomemController.Categoria.Beta.Equals(lobisomemController.categoria)) movimentacaoBeta();
        verificarCorrerAndar();
        verificarAtaque();
        verificarProximoComida();*/
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

    private float destinationOffset = 1f;
    [SerializeField] float speedVariation = 0.05f;
    [SerializeField] float leadTime = 1.2f, leadDistance = 2;

    private void verificarAtaque()
    {
        if (target == null) return;
        float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
        if (target.GetComponent<PlayerController>().isMorto || distanceToTarget > minimumDistanceAtaque)
        {
            //targetComida = target;
            target = null;
        }
        else if (distanceToTarget < distanciaDeAtaque) // Ataca o alvo
        {
            if (!lobisomemStats.isAttacking && Time.time > lastAttackTime + attackInterval)
            {
                transform.LookAt(target.transform.position);
                lastAttackTime = Time.time;
                animator.SetTrigger("attack" + Random.Range(1, 3));
            }
        }
        else // Persegue o alvo
        {
            Vector3 targetOffset = Random.insideUnitSphere * destinationOffset;
            // Calcula a posi��o futura do jogador com base na sua velocidade atual
            Vector3 leadTarget = target.transform.position + (target.GetComponent<CharacterController>().velocity.normalized * leadTime);
            // Calcula o offset da posi��o futura do jogador
            Vector3 leadTargetOffset = (leadTarget - target.transform.position).normalized * leadDistance;
            // Soma o offset da posi��o futura do jogador com o offset aleat�rio do destino
            Vector3 destination = leadTarget + leadTargetOffset + targetOffset;
            // Define a posi��o de destino para o inimigo
            agent.SetDestination(destination);
           
            // Aplica uma varia��o de velocidade aleat�ria
            agent.speed += Random.Range(-speedVariation, speedVariation);
        }
    }

    private void verificarCorrerAndar()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Uivar") || animator.GetCurrentAnimatorStateInfo(0).IsName("Comendo"))
        {
            agent.speed = 0;
        }
        else
        {
            if(target == null)
            {
                agent.speed = 0.1f;
            }
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
        else if (agent.velocity.magnitude > 0.02f)
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
        if (pontoBaseTerritorio == null) return false;
        float distance = Vector3.Distance(transform.position, pontoBaseTerritorio.transform.position);  // Calcula a dist�ncia entre o inimigo e a posicao do territorio base
        if (distance > distanciaMaximaPontoBase)
        {
            return true;
        }
        return false;
    }

    private bool estaDistanteDoAlfa()
    {
        float distance = Vector3.Distance(transform.position, lobisomemController.alfa.transform.position);  // Calcula a dist�ncia entre o inimigo e a posicao do territorio base
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
        Uivar();
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
                    //targetComida = other.transform; //REMOVIDO OPACAO DE PLAYER MORTO VIRAR COMIDA, POIS � NECESSARIO INSTANCIAR UM CORPO MORTO QDO UM PLAYER MORRER PARA SER DESTRUIDO AO SER COMIDO
                }
                else
                {
                    target = other.transform;
                    targetComida = null;
                }
            }
        }
        if (target == null && other.tag == "ItemDrop")
        {
            if (other.GetComponent<Consumivel>() != null && other.GetComponent<Consumivel>().tipoConsumivel.Equals(Consumivel.TipoConsumivel.Carne))
            {
                //gostou da comida
                targetComida = other.transform;
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (target == null && lobisomemStats.isEstadoAgressivo)
            {
                target = other.transform;
                targetComida = null;
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
        lobisomemStats.isAttacking = true;
    }

    void NotAtk()
    {
        lobisomemStats.isAttacking = false;
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
