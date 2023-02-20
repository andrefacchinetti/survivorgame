using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class EnemySelvagem : MonoBehaviourPunCallbacks
{
    public int vida = 100, damage = 10;
    public float minimumDistance = 5f, distanciaDePerseguicao = 10f, distanciaDeAtaque = 2f;
    
    public float attackInterval = 1f; // Intervalo de tempo entre ataques
    private float lastAttackTime; // Tempo do último ataque
    private GameObject[] players;
    private GameObject target;

    public Animator animator;
    [HideInInspector] public bool isAttacking; // Flag para controlar se a IA está atacando
    public NavMeshAgent navMeshAgent;
    public PhotonView PV;

    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        if(navMeshAgent == null) navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if(PhotonNetwork.IsConnected && players.Length < PhotonNetwork.CurrentRoom.PlayerCount)
        {
            players = GameObject.FindGameObjectsWithTag("Player");
        }
        if (target == null) //NAO TEM ALBO
        {
            float closestDistance = Mathf.Infinity;

            foreach (GameObject player in players)
            {
                float distance = Vector3.Distance(transform.position, player.transform.position);
                if (distance < closestDistance && !player.GetComponent<PlayerController>().isMorto)
                {
                    closestDistance = distance;
                    target = player;
                }
            }
        }
        else //JA TEM UM ALVO
        {
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
                    animator.SetTrigger("Attacking");
                }
            }
            else // Persegue o alvo
            {
                navMeshAgent.SetDestination(target.transform.position);
            }
        }

        if(navMeshAgent.velocity.magnitude > 0.1f)
        {
            animator.SetBool("Walking", true);
        }
        else
        {
            animator.SetBool("Walking", false);
        }
    }

    public void TakeDamage(int damage)
    {
        vida -= damage;
        Debug.Log("Vida enemy: " + vida);
        if (vida <= 0)
        {
            animator.SetBool("isDead", true);
            animator.SetBool("Attacking", false);
            navMeshAgent.isStopped = true;
        }
        else
        {
            animator.SetTrigger("Hit");
        }
    }

    void GoAtk()
    {
        isAttacking = true;
    }

    void NotAtk()
    {
        isAttacking = false;
    }

    private void OnDestinationReached(NavMeshAgent navMeshAgent)
    {
        if (navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete)
        {
            // O agente chegou ao destino com sucesso
        }
        else if (navMeshAgent.pathStatus == NavMeshPathStatus.PathPartial)
        {
            // O agente não conseguiu alcançar o destino e parou de se mover
        }
        else if (navMeshAgent.pathStatus == NavMeshPathStatus.PathInvalid)
        {
            // O destino é inválido e o agente não pode se mover
        }
    }

}
