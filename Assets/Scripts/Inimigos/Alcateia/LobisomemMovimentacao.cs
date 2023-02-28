using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LobisomemMovimentacao : MonoBehaviour
{

    LobisomemController lobisomemController;
    Animator animator;

    public float wanderRadius = 20f;
    public float wanderTimer = 5f;

    private Transform target;
    private NavMeshAgent agent;
    private float timer;

    private void Start()
    {
        lobisomemController = GetComponent<LobisomemController>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        timer = wanderTimer;
    }

    private void Update()
    {
        if (LobisomemController.Categoria.Omega.Equals(lobisomemController.categoria)) movimentacaoOmega();
        if(target == null) agent.speed = 0.15f;
        else agent.speed = 0.25f;

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
        if (target == null)
        {
            Wander();
        }
        else
        {
            Chase();
        }
    }

    void Wander()
    {
        timer += Time.deltaTime;

        if (timer >= wanderTimer)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            timer = 0;
        }
    }

    void Chase()
    {
        agent.SetDestination(target.position);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            target = other.transform;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            target = null;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if(target == null)
            {
                target = other.transform;
            }
        }
    }

    Vector3 RandomNavSphere(Vector3 origin, float distance, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * distance;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, distance, layermask);

        return navHit.position;
    }

}
