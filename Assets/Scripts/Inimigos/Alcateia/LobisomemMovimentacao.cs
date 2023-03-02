using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class LobisomemMovimentacao : MonoBehaviour
{

   

    [SerializeField] public GameObject pontoBaseTerritorio;
    [SerializeField] public float distanciaMaximaPontoBase = 50;
    [SerializeField] public float wanderRadius = 20f;
    [SerializeField] public float wanderTimer = 5f;

    private Transform target;
    private NavMeshAgent agent;
    private float timer;

    LobisomemController lobisomemController;
    LobisomemStats lobisomemStats;
    Animator animator;

    private void Start()
    {
        lobisomemController = GetComponent<LobisomemController>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        lobisomemStats = GetComponent<LobisomemStats>();
        timer = wanderTimer;
    }

    private void Update()
    {
        if (LobisomemController.Categoria.Omega.Equals(lobisomemController.categoria)) movimentacaoOmega();
        verificarCorrerAndar();
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

        if (timer >= wanderTimer)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
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

}
