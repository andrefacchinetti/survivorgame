using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

[RequireComponent(typeof(PhotonView), typeof(NavMeshAgent), typeof(StatsGeral))]
[RequireComponent(typeof(AnimalStats), typeof(BoxCollider))]
public class AnimalController : MonoBehaviourPunCallbacks
{

    public bool isAnimalAgressivo, isAnimalCarnivoro, isAnimalHerbivoro, isPredador, isPequenoPorte;
    public float eatTime = 5f; // tempo de alimentação
    public float walkSpeed = 1, runSpeed = 2;
    public bool isProcuraComida = true;
    
    public float eatDistance = 2f; // distância para detectar comida
    public float tempoCorridaFugindo = 5f; // tempo que o animal corre após tomar dano
    public float restTime = 20f; // tempo que o animal descansa após tomar dano
    public float raioDeDistanciaMinParaAndarAleatoriamente = 10f, raioDeDistanciaMaxParaAndarAleatoriamente = 40f;
    [SerializeField] public float timerParaAndarAleatoriamente = 5f;

    private float timer;
    private bool isEating = false;

    [SerializeField] [HideInInspector] public GameController gameController;
    [SerializeField] [HideInInspector] StatsGeral statsGeral;
    [SerializeField] [HideInInspector] AnimalStats animalStats;
    [HideInInspector] public Animator animator;
    [HideInInspector] public NavMeshAgent agent;
    [SerializeField] private StatsGeral targetInimigo;
    [SerializeField] private GameObject targetComida;
    PhotonView PV;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        statsGeral = GetComponent<StatsGeral>();
        animalStats = GetComponent<AnimalStats>();
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
            if (animalStats.estaFugindo)
            {
                //Debug.Log("animal ta fugindo...");
            }
            else if (targetInimigo != null)
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

    private bool agentEstaIndoParaDestino(Vector3 destino, float margemErro = 0.1f)
    {
        return !agent.hasPath || Vector3.Distance(agent.destination, destino) > margemErro;
    }

    private void andarAleatoriamentePeloMapa()
    {
        if (animalStats.estaFugindo) return;

        if (agentEstaIndoParaDestino(agent.destination))
        {
            agent.speed = walkSpeed;
            MoveToRandomPosition(raioDeDistanciaMinParaAndarAleatoriamente, raioDeDistanciaMaxParaAndarAleatoriamente);
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
        if (agent.velocity.magnitude > runSpeed - animalStats.speedVariation)
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
        if (!isAnimalAgressivo)
        {
            Debug.Log("tomou dano e setou corrida");
            Fugir();
        }
    }

    void PararDeFugir()
    {
        agent.speed = walkSpeed;
        animalStats.estaFugindo = false;
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
        Debug.Log("Animal movendo position");
    }

    private void MoveToRandomPosition(float minDistance, float maxDistance)
    {
        timer += Time.deltaTime;

        if (timer >= timerParaAndarAleatoriamente)
        {
            timer = 0;

            Vector3 randomDirection = Random.insideUnitSphere * maxDistance;
            randomDirection += transform.position;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, maxDistance, NavMesh.AllAreas))
            {
                MoveToPosition(hit.position);
            }
            else
            {
                Debug.Log("else random position");
                MoveToRandomPosition(minDistance, maxDistance);
            }
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

    void OnTriggerStay(Collider other)
    {
        if (targetInimigo != null || statsGeral.isDead || animalStats.estaFugindo) return;
        if (other.gameObject.tag == "ItemDrop" && other.gameObject.GetComponent<Consumivel>() != null)
        {
            FindFood(other.gameObject);
        }
        if (!isAnimalAgressivo) return;
        if (other.gameObject.tag == "Player")
        {
            targetInimigo = other.gameObject.GetComponent<StatsGeral>(); ;
            targetComida = null;
        }
        if (!isPredador) return;
        CollisorSofreDano collisorSofreDano = other.gameObject.GetComponent<CollisorSofreDano>();
        if (collisorSofreDano != null && collisorSofreDano.PV.ViewID != PV.ViewID)
        {
            StatsGeral objPai = collisorSofreDano.statsGeral;
            if ((objPai.gameObject.GetComponent<AnimalController>() != null || objPai.gameObject.GetComponent<LobisomemController>() != null) && !objPai.isDead)
            {
                if(objPai.gameObject.GetComponent<AnimalController>() != null)
                {
                    if (!objPai.gameObject.GetComponent<AnimalController>().isPredador && (isPequenoPorte || (!isPequenoPorte && !objPai.gameObject.GetComponent<AnimalController>().isPequenoPorte)))
                    {
                        targetInimigo = objPai;
                        targetComida = null;
                    }
                    else if (objPai.gameObject.GetComponent<AnimalController>().isPredador)
                    {
                        Fugir();
                    }
                }
                else if(objPai.gameObject.GetComponent<LobisomemController>() != null)
                {
                    if (isPredador)
                    {
                        targetInimigo = objPai;
                        targetComida = null;
                    }
                    else
                    {
                        Fugir();
                    }
                }
            }
        }
    }

    private void Fugir()
    {
        Debug.Log("animal fugindo");
        animalStats.estaFugindo = true;
        targetInimigo = null;
        targetComida = null;
        timer = timerParaAndarAleatoriamente;
        Invoke("PararDeFugir", tempoCorridaFugindo);
        agent.speed = runSpeed;
        MoveToRandomPosition(raioDeDistanciaMaxParaAndarAleatoriamente, raioDeDistanciaMaxParaAndarAleatoriamente);
    }

    private void perseguirInimigo()
    {
        if (targetInimigo == null || !isAnimalAgressivo) return;
        float distanceToTarget = Vector3.Distance(transform.position, targetInimigo.obterTransformPositionDoCollider().position);
        if (targetInimigo.isDead || distanceToTarget > animalStats.distanciaDePerseguicao)
        {
            //targetComida = target;
            targetInimigo = null;
        }
        else if (distanceToTarget < animalStats.distanciaDeAtaque) // Ataca o alvo
        {
            if (!statsGeral.isAttacking && Time.time > animalStats.lastAttackTime + animalStats.attackInterval)
            {
                transform.LookAt(targetInimigo.obterTransformPositionDoCollider().position);
                animalStats.lastAttackTime = Time.time;
                animator.SetTrigger("isAttacking");
            }
        }
        else // Persegue o alvo
        {
            Vector3 targetOffset = Random.insideUnitSphere * animalStats.destinationOffset;
            Vector3 leadTarget;
            // Calcula a posi��o futura do jogador com base na sua velocidade atual
            if (targetInimigo.GetComponent<CharacterController>() != null)
            {
                leadTarget = targetInimigo.transform.position + (targetInimigo.GetComponent<CharacterController>().velocity.normalized * animalStats.leadTime);
            }
            else
            {
                leadTarget = targetInimigo.obterTransformPositionDoCollider().position + (targetInimigo.obterTransformPositionDoCollider().GetComponent<NavMeshAgent>().velocity.normalized * animalStats.leadTime);
            }
            
            // Calcula o offset da posi��o futura do jogador
            Vector3 leadTargetOffset = (leadTarget - targetInimigo.obterTransformPositionDoCollider().position).normalized * animalStats.leadDistance;
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
