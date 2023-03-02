using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class LobisomemMovimentacao : MonoBehaviour
{

   

    [SerializeField] public GameObject pontoBaseTerritorio;
    [SerializeField] public float distanciaMaximaPontoBase = 50;
    [SerializeField] public float raioDeDistanciaParaAndarAleatoriamente = 20f;
    [SerializeField] public float timerParaAndarAleatoriamente = 5f;


    //MOVIMENTACAO
    private Transform target;
    [HideInInspector] public NavMeshAgent agent;
    private float timer;

    //ATAQUE
    public float minimumDistance = 5f, distanciaDePerseguicao = 10f, distanciaDeAtaque = 2f;
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
    }

    private void Update()
    {
        if (LobisomemController.Categoria.Omega.Equals(lobisomemController.categoria)) movimentacaoOmega();
        verificarCorrerAndar();
        verificarAtaque();
    }

    private void verificarAtaque()
    {
        if (target == null) return;
        float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
        if (target.GetComponent<PlayerController>().isMorto || distanceToTarget > minimumDistance)
        {
            target = null;
        }
        else if (distanceToTarget < distanciaDeAtaque) // Ataca o alvo
        {
            transform.LookAt(target.transform.position);
            if (!isAttacking && Time.time > lastAttackTime + attackInterval)
            {
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
        if (target != null && !recarregandoEnergia)
        {
            agent.speed = 0.25f;
            lobisomemStats.setarEnergiaAtual(lobisomemStats.energiaAtual - lobisomemStats.consumoEnergiaPorSegundo * Time.deltaTime);
        }
        else
        {
            agent.speed = 0.15f;
            lobisomemStats.setarEnergiaAtual(lobisomemStats.energiaAtual + lobisomemStats.recuperacaoEnergiaPorSegundo * Time.deltaTime);
            if (lobisomemStats.energiaAtual > 10) recarregandoEnergia = false;
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
        else if (agent.velocity.magnitude > 0.1f)
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

    private void movimentacaoOmega()
    {
        float distance = Vector3.Distance(transform.position, pontoBaseTerritorio.transform.position);  // Calcula a distância entre o inimigo e a posicao do territorio base
        if (distance > distanciaMaximaPontoBase)
        {
            target = null;
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
        if (other.tag == "Player" && target == null)
        {
            target = other.transform;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && target.gameObject.GetComponent<PhotonView>().Owner.UserId == other.gameObject.GetComponent<PhotonView>().Owner.UserId)
        {
            target = null;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && target == null)
        {
            target = other.transform;
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

}
