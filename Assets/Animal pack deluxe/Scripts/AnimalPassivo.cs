using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimalPassivo : MonoBehaviour
{
    public float walkSpeed = 5f, runSpeed = 10f; // velocidade de corrida
    public float eatTime = 5f; // tempo de alimentação
    public float hitPoints = 100f; // pontos de vida
    public float eatDistance = 2f; // distância para detectar comida
    public float runTime = 5f; // tempo que o animal corre após tomar dano
    public float restTime = 20f; // tempo que o animal descansa após tomar dano
    public float raioDeDistanciaParaAndarAleatoriamente = 20f;

    private Animator anim;
    private NavMeshAgent navAgent;
    private bool isRunning = false;
    private bool isEating = false;
    private bool isDead = false;
    private GameObject foodTarget;
    private float lastDamageTime = 0f;
    private float lastRunTime = 0f;

    void Start()
    {
        anim = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (!isDead)
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
                foodTarget = FindFood();
                if (foodTarget != null && !isRunning)
                {
                    // se encontrarmos comida, defina o destino do NavMeshAgent para a posição da comida
                    MoveToPosition(foodTarget.transform.position);
                }
                else if (!navAgent.hasPath)
                {
                    // Não há comida e não há destino definido, mover-se para uma posição aleatória
                    MoveToRandomPosition();
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
                    // ainda estamos longe demais, continue se movendo para o alvo de comida
                    MoveToPosition(foodTarget.transform.position);
                }
            }

            if (navAgent.speed > 2)
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

    void FinishEating()
    {
        isEating = false;
        anim.SetBool("isEating", false);
        Destroy(foodTarget);
        foodTarget = null;
    }

    public void TakeDamage(float damageAmount)
    {
        hitPoints -= damageAmount;
        anim.SetTrigger("isHit");

        if (hitPoints > 0)
        {
            foodTarget = null;
            isRunning = true;
            lastRunTime = Time.time;
            // o animal tomou dano, então ele deve correr por um tempo
            MoveToRandomPosition();
            Invoke("StopRunning", runTime);
        }
        Debug.Log("vida animal: " + hitPoints);
        // definir o tempo do último dano
        lastDamageTime = Time.time;
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
        Vector3 randomPosition = transform.position + Random.insideUnitSphere * raioDeDistanciaParaAndarAleatoriamente;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPosition, out hit, raioDeDistanciaParaAndarAleatoriamente, NavMesh.AllAreas))
        {
            MoveToPosition(hit.position);
        }
    }

    GameObject FindFood()
    {
        // encontrar todos os objetos com a tag "Food"
        GameObject[] foodObjects = GameObject.FindGameObjectsWithTag("ItemDrop");

        // encontrar o objeto de comida mais próximo
        GameObject closestObject = null;
        float closestDistance = Mathf.Infinity;
        foreach (GameObject food in foodObjects)
        {
            if (food.GetComponent<ItemDrop>().nomeItem.GetTipoItemEnum().Equals(Item.TiposItems.Consumivel.ToString()))
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
