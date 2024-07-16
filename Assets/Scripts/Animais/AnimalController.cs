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
    public float eatTime = 5f;
    public float walkSpeed = 1, runSpeed = 2;
    public bool isProcuraComida = true, isCapturado;
    public GameObject objRopePivot, objColeiraRope;

    public float eatDistance = 2f;
    public float tempoCorridaFugindo = 5f;
    public float restTime = 20f;
    public float raioDeDistanciaMinParaAndarAleatoriamente = 10f, raioDeDistanciaMaxParaAndarAleatoriamente = 40f;
    [SerializeField] public float timerParaAndarAleatoriamente = 5f;
    [SerializeField] public float pathUpdateDistanceThreshold = 10;
    [SerializeField] private float detectionInterval = 2f;

    private float detectionTimer = 0;
    private float timer = 0;
    private bool isEating = false;

    [SerializeField] [HideInInspector] public GameController gameController;
    [SerializeField] [HideInInspector] StatsGeral statsGeral;
    [SerializeField] [HideInInspector] AnimalStats animalStats;
    [HideInInspector] public Animator animator;
    [HideInInspector] public NavMeshAgent agent;
    [SerializeField] private StatsGeral targetInimigo;
    [SerializeField] private GameObject targetComida;
    [HideInInspector] public Transform targetObstaculo;
    [HideInInspector] public PlayerController targetCapturador;
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
        InvokeRepeating(nameof(CorrerOuAndarEmPerseguicao), 0f, tempoCorridaFugindo);
        StartCoroutine(DetectionCoroutine());
    }

    void Update()
    {
        if (statsGeral.health.IsAlive())
        {
            if (animalStats.estaFugindo)
            {
                // Animal is fleeing
            }
            else if (isCapturado && targetCapturador != null)
            {
                SeguirCapturador();
            }
            else if (targetInimigo != null)
            {
                PerseguirInimigo();
            }
            else if (targetComida != null)
            {
                PerseguirComida();
            }
            else
            {
                AndarAleatoriamentePeloMapa();
            }
            VerificarCorrerAndar();
            VerificarAtaque();
        }
        else
        {
            animator.SetBool("isDead", true);
            targetInimigo = null;
            targetComida = null;
            if (targetCapturador != null)
            {
                targetCapturador.inventario.UngrabCoisasCapturadasComCorda(false);
                targetCapturador = null;
            }
            agent.ResetPath();
        }
    }

    private IEnumerator DetectionCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(detectionInterval);
            DetectNearbyObjects();
        }
    }

    public float detectionRadius = 10f;
    private void DetectNearbyObjects()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (targetInimigo != null || !statsGeral.health.IsAlive() || animalStats.estaFugindo) return;

            if (hitCollider.CompareTag("ItemDrop") && hitCollider.TryGetComponent(out Consumivel consumivel))
            {
                FindFood(hitCollider.gameObject);
            }

            if (!isAnimalAgressivo) continue;

            if (hitCollider.CompareTag("Player"))
            {
                targetInimigo = hitCollider.GetComponent<StatsGeral>();
                targetComida = null;
            }

            if (!isPredador) continue;

            if (hitCollider.TryGetComponent(out CollisorSofreDano collisorSofreDano) && collisorSofreDano.PV.ViewID != PV.ViewID)
            {
                var objPai = collisorSofreDano.statsGeral;
                var animalController = objPai.GetComponent<AnimalController>();
                var lobisomemController = objPai.GetComponent<LobisomemController>();

                if (animalController != null && statsGeral.health.IsAlive())
                {
                    if (!animalController.isPredador && (isPequenoPorte || (!isPequenoPorte && !animalController.isPequenoPorte)))
                    {
                        targetInimigo = objPai;
                        targetComida = null;
                    }
                    else if (animalController.isPredador)
                    {
                        Fugir();
                    }
                }
                else if (lobisomemController != null && statsGeral.health.IsAlive())
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

    private void PerseguirComida()
    {
        if (targetComida == null) return;

        if (Vector3.Distance(transform.position, targetComida.transform.position) <= eatDistance)
        {
            if (!isEating)
            {
                isEating = true;
                animator.SetBool("isEating", true);
                Invoke(nameof(FinishEating), eatTime);
            }
        }
        else if (!agent.hasPath)
        {
            MoveToPosition(targetComida.transform.position);
        }
    }
    private float distanciaMinima = 2.0f;
    private void SeguirCapturador()
    {
        float distancia = Vector3.Distance(transform.position, targetCapturador.transform.position);

        if (distancia > distanciaMinima)
        {
            Vector3 posicaoDestino = targetCapturador.transform.position - (targetCapturador.transform.forward * distanciaMinima);
            agent.SetDestination(posicaoDestino);
        }
        else
        {
            agent.ResetPath();
        }
    }

    private bool AgentEstaIndoParaDestino(Vector3 destino, float margemErro = 0.1f)
    {
        return !agent.hasPath || Vector3.Distance(agent.destination, destino) > margemErro;
    }

    private void AndarAleatoriamentePeloMapa()
    {
        if (animalStats.estaFugindo) return;

        if (AgentEstaIndoParaDestino(agent.destination))
        {
            agent.speed = walkSpeed;
            MoveToRandomPosition(raioDeDistanciaMinParaAndarAleatoriamente, raioDeDistanciaMaxParaAndarAleatoriamente);
        }
    }

    private void VerificarCorrerAndar()
    {
        SetarAnimacaoPorVelocidade();
    }

    private void SetarAnimacaoPorVelocidade()
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
        if (NavMesh.SamplePosition(position, out NavMeshHit hit, raioDeDistanciaMaxParaAndarAleatoriamente, NavMesh.AllAreas))
        {
            agent.SetDestination(position);
        }
        else
        {
            MoveToRandomPosition(raioDeDistanciaMinParaAndarAleatoriamente, raioDeDistanciaMaxParaAndarAleatoriamente);
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

            if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, maxDistance, NavMesh.AllAreas))
            {
                float distanceToNewPosition = Vector3.Distance(transform.position, hit.position);
                if (distanceToNewPosition >= minDistance)
                {
                    MoveToPosition(hit.position);
                }
                else
                {
                    MoveToRandomPosition(minDistance, maxDistance);
                }
            }
        }
    }

    private void FindFood(GameObject food)
    {
        if (!isProcuraComida || targetInimigo != null) return;
        var consumivel = food.GetComponent<Consumivel>();
        if (isAnimalCarnivoro && consumivel.tipoConsumivel.Equals(Consumivel.TipoConsumivel.Carne))
        {
            targetComida = food;
        }
        if (isAnimalHerbivoro && (consumivel.tipoConsumivel.Equals(Consumivel.TipoConsumivel.Fruta) || consumivel.tipoConsumivel.Equals(Consumivel.TipoConsumivel.Vegetal)))
        {
            targetComida = food;
        }
    }

    private void Fugir()
    {
        if (animalStats.estaFugindo) return;
        animalStats.estaFugindo = true;
        targetInimigo = null;
        targetComida = null;
        timer = timerParaAndarAleatoriamente;
        Invoke(nameof(PararDeFugir), tempoCorridaFugindo);
        agent.speed = runSpeed;
        MoveToRandomPosition(raioDeDistanciaMaxParaAndarAleatoriamente, raioDeDistanciaMaxParaAndarAleatoriamente);
    }

    private void VerificarAtaque()
    {
        if (targetInimigo == null)
        {
            AtacarObstaculoOuInimigo();
            return;
        }

        if (!targetInimigo.health.IsAlive())
        {
            targetInimigo = null;
        }
        else
        {
            if (PodeAtacarAlvo(transform, targetInimigo.transform.position))
            {
                AtacarAlvo(targetInimigo.transform.position);
            }
            else
            {
                AtacarObstaculoOuInimigo();
            }
        }
    }

    private bool PodeAtacarAlvo(Transform transformInicial, Vector3 positionTarget)
    {
        float distanceToInimigo = Vector3.Distance(transformInicial.position, positionTarget);
        return distanceToInimigo <= animalStats.distanciaDeAtaque;
    }

    private void AtacarObstaculoOuInimigo()
    {
        if (targetObstaculo != null)
        {
            if (!targetObstaculo.gameObject.activeSelf)
            {
                targetObstaculo = null;
            }
            else
            {
                AtacarAlvo(targetObstaculo.position);
                targetObstaculo = null;
            }
        }
        else
        {
            PerseguirInimigo();
        }
    }

    private void AtacarAlvo(Vector3 positionAlvo)
    {
        if (!statsGeral.isAttacking && Time.time > animalStats.lastAttackTime + animalStats.attackInterval)
        {
            transform.LookAt(positionAlvo);
            animalStats.lastAttackTime = Time.time;
            animator.SetTrigger("isAttacking");
        }
        else
        {
            PerseguirInimigo();
        }
    }

    public void PerseguirInimigo()
    {
        if (targetInimigo == null) return;
        Vector3 targetVelocity = GetTargetVelocity(targetInimigo);

        if (targetVelocity != Vector3.zero)
        {
            Vector3 predictedPosition = PredictTargetPosition(targetInimigo, targetVelocity);

            Vector3 randomOffset = Random.insideUnitSphere * animalStats.destinationOffset;

            Vector3 destination = predictedPosition + (predictedPosition - targetInimigo.transform.position).normalized * animalStats.leadDistance + randomOffset;

            if (!agent.hasPath || (agent.hasPath && agent.remainingDistance > pathUpdateDistanceThreshold))
            {
                MoveToPosition(destination);
            }

            agent.speed = runSpeed;
        }
        else
        {
            if (agent.velocity == Vector3.zero)
            {
                MoveToPosition(targetInimigo.transform.position);
            }
        }
    }

    private Vector3 GetTargetVelocity(StatsGeral target)
    {
        if (target.TryGetComponent(out CharacterController characterController))
        {
            return characterController.velocity.normalized;
        }
        else if (target.transform.TryGetComponent(out NavMeshAgent navMeshAgent))
        {
            return navMeshAgent.velocity.normalized;
        }
        return Vector3.zero;
    }

    private Vector3 PredictTargetPosition(StatsGeral target, Vector3 velocity)
    {
        return target.transform.position + velocity * animalStats.leadTime;
    }

    void CorrerOuAndarEmPerseguicao()
    {
        if (targetInimigo == null) return;

        agent.speed = animator.GetBool("run") ? walkSpeed : runSpeed;
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
