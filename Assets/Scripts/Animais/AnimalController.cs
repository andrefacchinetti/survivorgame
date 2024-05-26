using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

[RequireComponent(typeof(PhotonView), typeof(NavMeshAgent), typeof(StatsGeral))]
[RequireComponent(typeof(AnimalStats))]
public class AnimalController : MonoBehaviourPunCallbacks
{

    public bool isAnimalAgressivo, isAnimalCarnivoro, isAnimalHerbivoro, isPredador, isPequenoPorte;
    public float eatTime = 5f; // tempo de alimentação
    public float walkSpeed = 1, runSpeed = 2;
    public bool isProcuraComida = true, isCapturado;
    public GameObject objRopePivot, objColeiraRope;

    public float eatDistance = 2f; // distância para detectar comida
    public float tempoCorridaFugindo = 5f; // tempo que o animal corre após tomar dano
    public float restTime = 20f; // tempo que o animal descansa após tomar dano
    public float raioDeDistanciaMinParaAndarAleatoriamente = 10f, raioDeDistanciaMaxParaAndarAleatoriamente = 40f;
    [SerializeField] public float timerParaAndarAleatoriamente = 5f;
    [SerializeField] public float pathUpdateDistanceThreshold = 10;
    [SerializeField] private float detectionInterval = 0.5f;

    private float detectionTimer;
    private float timer;
    private bool isEating = false;

    [SerializeField] [HideInInspector] public GameController gameController;
    [SerializeField] [HideInInspector] StatsGeral statsGeral;
    [SerializeField] [HideInInspector] AnimalStats animalStats;
    [HideInInspector] public Animator animator;
    [HideInInspector] public NavMeshAgent agent;
    [SerializeField] private StatsGeral targetInimigo;
    [SerializeField] private GameObject targetComida;
    [HideInInspector] public Transform targetObstaculo;
    [HideInInspector] public GameObject targetCapturador;
    public PhotonView PV;

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
        if (statsGeral.health.IsAlive())
        {
            detectionTimer += Time.deltaTime;

            if (detectionTimer >= detectionInterval)
            {
                detectionTimer = 0;
                DetectNearbyObjects();
            }
            if (animalStats.estaFugindo)
            {
                //Debug.Log("animal ta fugindo...");
            }else if(isCapturado && targetCapturador != null)
            {
                seguirCapturador();
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
            verificarAtaque();
        }
        else
        {
            animator.SetBool("isDead", true);
            targetInimigo = null;
            targetComida = null;
            if(targetCapturador != null)
            {
                targetCapturador.GetComponent<PlayerController>().inventario.UngrabAnimalCapturado(false);
                targetCapturador = null;
            }
            agent.ResetPath();
        }
    }

    public float detectionRadius = 10f;
    private void DetectNearbyObjects()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (targetInimigo != null || !statsGeral.health.IsAlive() || animalStats.estaFugindo) return;

            if (hitCollider.CompareTag("ItemDrop") && hitCollider.GetComponent<Consumivel>() != null)
            {
                FindFood(hitCollider.gameObject);
            }

            if (!isAnimalAgressivo) return;

            if (hitCollider.CompareTag("Player"))
            {
                targetInimigo = hitCollider.GetComponent<StatsGeral>();
                targetComida = null;
            }

            if (!isPredador) return;

            CollisorSofreDano collisorSofreDano = hitCollider.GetComponent<CollisorSofreDano>();
            if (collisorSofreDano != null && collisorSofreDano.PV.ViewID != PV.ViewID)
            {
                StatsGeral objPai = collisorSofreDano.statsGeral;
                if ((objPai.gameObject.GetComponent<AnimalController>() != null || objPai.gameObject.GetComponent<LobisomemController>() != null) && statsGeral.health.IsAlive())
                {
                    if (objPai.gameObject.GetComponent<AnimalController>() != null)
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
                    else if (objPai.gameObject.GetComponent<LobisomemController>() != null)
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

    private float distanciaMinima = 2.0f;
    private void seguirCapturador()
    {
        float distancia = Vector3.Distance(transform.position, targetCapturador.transform.position); // Calcula a distância entre o animal e o targetCapturador

        if (distancia > distanciaMinima)
        {
            // Define a posição de destino para ser a posição do targetCapturador com um deslocamento para trás com base na distância mínima
            Vector3 posicaoDestino = targetCapturador.transform.position - (targetCapturador.transform.forward * distanciaMinima);
            // Move o animal em direção à posição de destino
            agent.SetDestination(posicaoDestino);
        }
        else
        {
            // Se estiver dentro da distância mínima, para o agente
            agent.ResetPath();
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

    private void MoveToPosition(Vector3 position)
    {
        NavMeshHit hit;
        bool hasValidPath = NavMesh.SamplePosition(position, out hit, raioDeDistanciaMaxParaAndarAleatoriamente, NavMesh.AllAreas);
        if (hasValidPath)
        {
            agent.SetDestination(position);
            Debug.Log("animal move to position");
        }
        else
        {
            MoveToRandomPosition(raioDeDistanciaMinParaAndarAleatoriamente, raioDeDistanciaMinParaAndarAleatoriamente);
        }
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
                float distanceToNewPosition = Vector3.Distance(transform.position, hit.position);
                if (distanceToNewPosition >= minDistance)
                {
                    MoveToPosition(hit.position);
                }
                else
                {
                    // Se a nova posição estiver muito próxima, gere uma nova posição aleatória
                    MoveToRandomPosition(minDistance, maxDistance);
                }
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

    private void Fugir()
    {
        if (animalStats.estaFugindo) return;
        Debug.Log("animal fugindo");
        animalStats.estaFugindo = true;
        targetInimigo = null;
        targetComida = null;
        timer = timerParaAndarAleatoriamente;
        Invoke("PararDeFugir", tempoCorridaFugindo);
        agent.speed = runSpeed;
        MoveToRandomPosition(raioDeDistanciaMaxParaAndarAleatoriamente, raioDeDistanciaMaxParaAndarAleatoriamente);
    }

    private void verificarAtaque()
    {
        if (targetInimigo == null)
        {
            atacarObstaculoOuInimigo();
            return;
        }

        if (!targetInimigo.health.IsAlive())
        {
            targetInimigo = null;
        }
        else
        {
            if (isPodeAtacarAlvo(transform, targetInimigo.obterTransformPositionDoCollider().position))  // Ataca o alvo
            {
                Debug.Log("Atacando inimigo");
                atacarAlvo(targetInimigo.obterTransformPositionDoCollider().position);
            }
            else
            {
                atacarObstaculoOuInimigo();
            }
        }
    }

    private bool isPodeAtacarAlvo(Transform transformInicial, Vector3 positionTarget)
    {
        float distanceToInimigo = Vector3.Distance(transformInicial.position, positionTarget);
        return distanceToInimigo <= animalStats.distanciaDeAtaque;
    }

    private void atacarObstaculoOuInimigo()
    {
        if (targetObstaculo != null)
        {
            if (!targetObstaculo.gameObject.activeSelf)
            {
                targetObstaculo = null;
            }
            else
            {
                atacarAlvo(targetObstaculo.position);
                targetObstaculo = null;
            }
        }
        else
        {
            perseguirInimigo();
        }
    }

    private void atacarAlvo(Vector3 positionAlvo)
    {
        if (!statsGeral.isAttacking && Time.time > animalStats.lastAttackTime + animalStats.attackInterval)
        {
            transform.LookAt(positionAlvo);
            animalStats.lastAttackTime = Time.time;
            animator.SetTrigger("isAttacking");
            Debug.Log("Atacando obstaculo");
        }
        else
        {
            perseguirInimigo();
        }
    }

    public void perseguirInimigo()
    {
        if (targetInimigo == null) return;
        Vector3 targetVelocity = GetTargetVelocity(targetInimigo);

        if (targetVelocity != Vector3.zero)
        {
            Vector3 predictedPosition = PredictTargetPosition(targetInimigo, targetVelocity);

            Vector3 randomOffset = Random.insideUnitSphere * animalStats.destinationOffset;

            Vector3 destination = predictedPosition + (predictedPosition - targetInimigo.obterTransformPositionDoCollider().position).normalized * animalStats.leadDistance + randomOffset;

            // Verifica se o agent já tem um path e se ele está próximo do destino
            if (!agent.hasPath || (agent.hasPath && agent.remainingDistance > pathUpdateDistanceThreshold))
            {
                Debug.Log("Perseguindo inimigo 1");
                MoveToPosition(destination);
            }

            agent.speed = runSpeed;
        }
        else
        {
            if (agent.velocity == Vector3.zero)
            {
                Debug.Log("Perseguindo inimigo 2");
                MoveToPosition(targetInimigo.obterTransformPositionDoCollider().position);
            }
        }
    }

    private Vector3 GetTargetVelocity(StatsGeral target)
    {
        if (target.GetComponent<CharacterController>() != null)
        {
            return target.GetComponent<CharacterController>().velocity.normalized;
        }
        else if (target.obterTransformPositionDoCollider().GetComponent<NavMeshAgent>() != null)
        {
            return target.obterTransformPositionDoCollider().GetComponent<NavMeshAgent>().velocity.normalized;
        }
        return Vector3.zero;
    }

    private Vector3 PredictTargetPosition(StatsGeral target, Vector3 velocity)
    {
        return target.obterTransformPositionDoCollider().position + velocity * animalStats.leadTime;
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
