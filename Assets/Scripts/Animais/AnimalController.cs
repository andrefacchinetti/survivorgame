using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

[RequireComponent(typeof(PhotonView), typeof(NavMeshAgent), typeof(StatsGeral))]
[RequireComponent(typeof(AnimalStats), typeof(BoxCollider))]
public class AnimalController : MonoBehaviourPunCallbacks
{

    public bool isAnimalAgressivo, isAnimalCarnivoro, isAnimalHerbivoro;
    public float eatTime = 5f; // tempo de alimentação
    public float walkSpeed = 1, runSpeed = 2;
    public bool isProcuraComida = true;
    
    public float eatDistance = 2f; // distância para detectar comida
    public float tempoCorridaFugindo = 5f; // tempo que o animal corre após tomar dano
    public float restTime = 20f; // tempo que o animal descansa após tomar dano
    public float raioDeDistanciaParaAndarAleatoriamente = 20f;

    StatsGeral statsGeral;
    [HideInInspector] public Animator animator;
    [HideInInspector] public NavMeshAgent agent;
    private bool isEating = false;
    [SerializeField] private GameObject targetComida, targetInimigo;
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
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        InvokeRepeating("CorrerOuAndarEmPerseguicao", 0f, tempoCorridaFugindo);
    }

    void Update()
    {
        if (!statsGeral.isDead)
        {
            // verificar se o animal está atualmente em uma rota definida pelo NavMeshAgent
            if (!agent.pathPending && agent.remainingDistance < 0.1f)
            {
                // o animal chegou ao seu destino, pare de se mover
                agent.ResetPath();
            }

            if (targetInimigo != null)
            {
                perseguirInimigo(); 
            }
            else if(targetComida != null)
            {
                perseguirComida();
            }
            else
            {
                andarAleatoriamentePeloMapa();
            }
            verificarCorrerAndar();
        }
        else
        {
            animator.SetBool("isDead", true);
            targetInimigo = null;
            targetComida = null;
            agent.ResetPath();
        }
    }

    private void perseguirComida()
    {
        if (targetComida == null) return;
        // se já temos um alvo de comida, verifique se estamos perto o suficiente para comer
        if (Vector3.Distance(transform.position, targetComida.transform.position) <= eatDistance)
        {
            if (!isEating)
            {
                isEating = true;
                animator.SetBool("isEating", true);
                Invoke("FinishEating", eatTime);
            }
        }
        else
        {
            if (!agent.hasPath)
            {
                MoveToPosition(targetComida.transform.position);
            }
        }
    }

    private void andarAleatoriamentePeloMapa()
    {
        if (!agent.hasPath)
        {
            agent.speed = walkSpeed;
            MoveToRandomPosition(); // Não há comida e não há destino definido, mover-se para uma posição aleatória
        }
    }

    private void verificarCorrerAndar()
    {
        /*if (animator.GetCurrentAnimatorStateInfo(0).IsName("isEating") || animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            agent.speed = 0;
        }*/
        setarAnimacaoPorVelocidade();
    }

    private void setarAnimacaoPorVelocidade()
    {
        if (agent.velocity.magnitude > runSpeed - statsGeral.speedVariation)
        {
            animator.SetBool("isMoving", false);
            animator.SetBool("run", true);
        }
        else if (agent.velocity.magnitude > 0.05f)
        {
            animator.SetBool("isMoving", true);
            animator.SetBool("run", false);
        }
        else
        {
            animator.SetBool("isMoving", false);
            animator.SetBool("run", false);
        }
    }

    public void AcoesTomouDano()
    {
        targetComida = null;
        lastRunTime = Time.time;
        lastDamageTime = Time.time;
        if (!isAnimalAgressivo)
        {
            Invoke("StopRunning", tempoCorridaFugindo);
            Debug.Log("tomou dano e setou corrida");
            agent.speed = runSpeed;
            MoveToRandomPosition();
        }
    }

    void StopRunning()
    {
        agent.speed = walkSpeed;
        Debug.Log("walk 2");
    }

    void FinishEating()
    {
        isEating = false;
        animator.SetBool("isEating", false);
        Destroy(targetComida);
        targetComida = null;
    }

    public void MoveToPosition(Vector3 position)
    {
        transform.LookAt(position);
        agent.SetDestination(position);
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
        if (!isProcuraComida || targetInimigo != null) return;
        if (isAnimalCarnivoro)
        {
            if ((food.GetComponent<Consumivel>().tipoConsumivel.Equals(Consumivel.TipoConsumivel.Carne)))
            {
                targetComida = food;
            }
        }
        if (isAnimalHerbivoro)
        {
            if ((food.GetComponent<Consumivel>().tipoConsumivel.Equals(Consumivel.TipoConsumivel.Fruta) || food.GetComponent<Consumivel>().tipoConsumivel.Equals(Consumivel.TipoConsumivel.Vegetal)))
            {
                targetComida = food;
            }
        }
    }

    //AGRESSIVOS

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "ItemDrop" && other.gameObject.GetComponent<Consumivel>() != null)
        {
            FindFood(other.gameObject);
        }
        if (!isAnimalAgressivo) return;
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Lobisomem")
        {
            targetInimigo = other.gameObject;
            targetComida = null;
        }
    }

    private void perseguirInimigo()
    {
        if (targetInimigo == null || !isAnimalAgressivo) return;
        float distanceToTarget = Vector3.Distance(transform.position, targetInimigo.transform.position);
        if (targetInimigo.GetComponent<PlayerController>().isMorto || distanceToTarget > statsGeral.minimumDistanceAtaque)
        {
            //targetComida = target;
            targetInimigo = null;
        }
        else if (distanceToTarget < statsGeral.distanciaDeAtaque) // Ataca o alvo
        {
            if (!statsGeral.isAttacking && Time.time > statsGeral.lastAttackTime + statsGeral.attackInterval)
            {
                transform.LookAt(targetInimigo.transform.position);
                statsGeral.lastAttackTime = Time.time;
                animator.SetTrigger("isAttacking");
            }
        }
        else // Persegue o alvo
        {
            Vector3 targetOffset = Random.insideUnitSphere * statsGeral.destinationOffset;
            // Calcula a posi��o futura do jogador com base na sua velocidade atual
            Vector3 leadTarget = targetInimigo.transform.position + (targetInimigo.GetComponent<CharacterController>().velocity.normalized * statsGeral.leadTime);
            // Calcula o offset da posi��o futura do jogador
            Vector3 leadTargetOffset = (leadTarget - targetInimigo.transform.position).normalized * statsGeral.leadDistance;
            // Soma o offset da posi��o futura do jogador com o offset aleat�rio do destino
            Vector3 destination = leadTarget + leadTargetOffset + targetOffset;
            // Define a posi��o de destino para o inimigo
            agent.SetDestination(destination);
        }
    }

    void CorrerOuAndarEmPerseguicao()
    {
        if (targetInimigo == null) return;

        if (animator.GetBool("run") == true)
        {
            agent.speed = walkSpeed;
            Debug.Log("walk 3");
        }
        else
        {
            agent.speed = runSpeed;
        }
    }

    void GoAtk()
    {
        statsGeral.isAttacking = true;
    }

    void NotAtk()
    {
        statsGeral.isAttacking = false;
    }

}
