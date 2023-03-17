using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

[RequireComponent(typeof(PhotonView), typeof(NavMeshAgent), typeof(StatsGeral))]
[RequireComponent(typeof(AnimalStats))]
public class AnimalController : MonoBehaviourPunCallbacks
{

    public bool isAnimalAgressivo, isAnimalCarnivoro, isAnimalHerbivoro;
    public float walkSpeed = 5f, runSpeed = 10f; // velocidade de corrida
    public float eatTime = 5f; // tempo de alimentação
    public bool isProcuraComida = true;
    
    public float eatDistance = 2f; // distância para detectar comida
    public float runTime = 5f; // tempo que o animal corre após tomar dano
    public float restTime = 20f; // tempo que o animal descansa após tomar dano
    public float raioDeDistanciaParaAndarAleatoriamente = 20f;

    StatsGeral statsGeral;
    [HideInInspector] public Animator anim;
    private NavMeshAgent navAgent;
    private bool isRunning = false;
    private bool isEating = false;
    private GameObject foodTarget, enemyTarget;
    private float lastDamageTime = 0f;
    private float lastRunTime = 0f;
    PhotonView PV;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        statsGeral = GetComponent<StatsGeral>();
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (!statsGeral.isDead)
        {
            // verificar se o animal está atualmente em uma rota definida pelo NavMeshAgent
            if (!navAgent.pathPending && navAgent.remainingDistance < 0.1f)
            {
                // o animal chegou ao seu destino, pare de se mover
                navAgent.ResetPath();
                navAgent.speed = 0;
            }

            if (foodTarget == null)
            {
               if (!navAgent.hasPath)
                {
                    MoveToRandomPosition(); // Não há comida e não há destino definido, mover-se para uma posição aleatória
                }
            }
            else
            {
                // se já temos um alvo de comida, verifique se estamos perto o suficiente para comer
                if (Vector3.Distance(transform.position, foodTarget.transform.position) <= eatDistance)
                {
                    if (!isEating)
                    {
                        isEating = true;
                        anim.SetBool("isEating", true);
                        Invoke("FinishEating", eatTime);
                    }
                }
                else
                {
                    if (!navAgent.hasPath)
                    {
                        MoveToPosition(foodTarget.transform.position);
                    }
                }
            }

            if (navAgent.speed > (walkSpeed + 0.5f))
            {
                anim.SetBool("run", true);
                anim.SetBool("isMoving", true);
            }
            else if (navAgent.speed > 0.1f)
            {
                anim.SetBool("run", false);
                anim.SetBool("isMoving", true);
            }
            else
            {
                anim.SetBool("run", false);
                anim.SetBool("isMoving", false);
            }

        }
    }

    public void AcoesTomouDano()
    {
        foodTarget = null;
        isRunning = true;
        lastRunTime = Time.time;
        lastDamageTime = Time.time;
        MoveToRandomPosition();
        Invoke("StopRunning", runTime);
    }

    void FinishEating()
    {
        isEating = false;
        anim.SetBool("isEating", false);
        Destroy(foodTarget);
        foodTarget = null;
    }

    void StopRunning()
    {
        isRunning = false;
        Debug.Log("parotu de correr");
    }

    public void MoveToPosition(Vector3 position)
    {
       
        transform.LookAt(position);
        // Definir destino para o NavMeshAgent
        navAgent.SetDestination(position);
        if (isRunning)
        {
            navAgent.speed = runSpeed;
            Debug.Log("movendo para position correndo");
        }
        else
        {
            navAgent.speed = walkSpeed;
            Debug.Log("movendo para position andando");
        }
    }

    private void MoveToRandomPosition()
    {
        Vector3 randomDirection = Random.insideUnitSphere * raioDeDistanciaParaAndarAleatoriamente;
        randomDirection += transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, raioDeDistanciaParaAndarAleatoriamente, NavMesh.AllAreas))
        {
            MoveToPosition(hit.position);
        }
    }

    private void FindFood(GameObject food)
    {
        if (!isProcuraComida || enemyTarget != null) return;
        if (isAnimalCarnivoro)
        {
            if ((food.GetComponent<Consumivel>().tipoConsumivel.Equals(Consumivel.TipoConsumivel.Carne)))
            {
                foodTarget = food;
            }
        }
        if (isAnimalHerbivoro)
        {
            if ((food.GetComponent<Consumivel>().tipoConsumivel.Equals(Consumivel.TipoConsumivel.Fruta) || food.GetComponent<Consumivel>().tipoConsumivel.Equals(Consumivel.TipoConsumivel.Vegetal)))
            {
                foodTarget = food;
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "ItemDrop" && other.gameObject.GetComponent<Consumivel>() != null)
        {
            FindFood(other.gameObject);
        }
        if (!isAnimalAgressivo) return;
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Lobisomem")
        {
            //atacar
        }
    }

}
