using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

[RequireComponent(typeof(PhotonView), typeof(NavMeshAgent), typeof(StatsGeral))]
[RequireComponent(typeof(AnimalPassivoStats))]
public class AnimalPassivoController : MonoBehaviourPunCallbacks
{

    public float walkSpeed = 5f, runSpeed = 10f; // velocidade de corrida
    public float eatTime = 5f; // tempo de alimenta??o
    public bool isProcuraComida = true;
    
    public float eatDistance = 2f; // dist?ncia para detectar comida
    public float runTime = 5f; // tempo que o animal corre ap?s tomar dano
    public float restTime = 20f; // tempo que o animal descansa ap?s tomar dano
    public float raioDeDistanciaParaAndarAleatoriamente = 20f;

    StatsGeral statsGeral;
    [HideInInspector] public Animator anim;
    private NavMeshAgent navAgent;
    private bool isRunning = false;
    private bool isEating = false;
    private GameObject foodTarget;
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
            // verificar se o animal est? atualmente em uma rota definida pelo NavMeshAgent
            if (!navAgent.pathPending && navAgent.remainingDistance < 0.1f)
            {
                // o animal chegou ao seu destino, pare de se mover
                navAgent.ResetPath();
                navAgent.speed = 0;
            }

            if (foodTarget == null)
            {
                foodTarget = FindFood();
                if (foodTarget != null && !isRunning)
                {
                    // se encontrarmos comida, defina o destino do NavMeshAgent para a posi??o da comida
                    MoveToPosition(foodTarget.transform.position);
                }
                else if (!navAgent.hasPath)
                {
                    // N?o h? comida e n?o h? destino definido, mover-se para uma posi??o aleat?ria
                    MoveToRandomPosition();
                }
            }
            else
            {
                // se j? temos um alvo de comida, verifique se estamos perto o suficiente para comer
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
                    // ainda estamos longe demais, continue se movendo para o alvo de comida
                    MoveToPosition(foodTarget.transform.position);
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

    GameObject FindFood()
    {
        if (!isProcuraComida) return null;
        // encontrar todos os objetos com a tag "Food"
        GameObject[] foodObjects = GameObject.FindGameObjectsWithTag("ItemDrop");

        // encontrar o objeto de comida mais pr?ximo
        GameObject closestObject = null;
        float closestDistance = Mathf.Infinity;
        foreach (GameObject food in foodObjects)
        {
            if (food.GetComponent<Consumivel>() != null && (food.GetComponent<Consumivel>().tipoConsumivel.Equals(Consumivel.TipoConsumivel.Fruta) || food.GetComponent<Consumivel>().tipoConsumivel.Equals(Consumivel.TipoConsumivel.Vegetal)))
            {
                float distance = Vector3.Distance(transform.position, food.transform.position);
                if (distance < closestDistance)
                {
                    closestObject = food;
                    closestDistance = distance;
                }
            }
        }

        return closestObject;
    }

}
