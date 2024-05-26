using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class LobisomemMovimentacao : MonoBehaviour
{
    public float distanciaMaximaPontoBase = 50f;
    public float distanciaMaximaDoSeuAlfa = 10f;
    public float tempoCorridaFugindo = 5f;
    public float raioDeDistanciaMinParaAndarAleatoriamente = 10f;
    public float raioDeDistanciaMaxParaAndarAleatoriamente = 40f;
    public float timerParaAndarAleatoriamente = 5f;
    public float timerParaAlfaDecidirComandosParaSeusBetas = 10f;
    public float tempoParadoNaArvore = 10f;

    [HideInInspector] public StatsGeral targetInimigo;
    [HideInInspector] public GameObject targetComida;
    [HideInInspector] public Transform targetObstaculo;

    private float timer;
    private float caminhoCooldown = 1f; // Adicione um cooldown para atualizações de caminho
    private float proximaAtualizacaoCaminho;
    public NavMeshAgent agent;
    public Animator animator;
    public StatsGeral statsGeral;
    public LobisomemStats lobisomemStats;

    private float detectionInterval = 0.5f; // Checa a cada 0.5 segundos
    private float detectionTimer;

    private void Awake()
    {
        timer = timerParaAndarAleatoriamente;
        proximaAtualizacaoCaminho = Time.time;
    }

    private void Update()
    {
        if (!statsGeral.health.IsAlive()) return;
        timer += Time.deltaTime;
        detectionTimer += Time.deltaTime;

        if (detectionTimer >= detectionInterval)
        {
            detectionTimer = 0;
            DetectNearbyObjects();
        }

        if (targetInimigo != null)
        {
            perseguirInimigo();
        }
        else
        {
            movimentarAleatoriamentePeloMapa();
        }

        resetAgentDestination();
        verificarProximoComida();
        verificarAtaque();
        verificarCorrerAndar();
    }

    private void resetAgentDestination()
    {
        if (agent.isOnNavMesh && !agent.pathPending && agent.remainingDistance < 0.1f)
        {
            agent.ResetPath();
        }
    }

    private void verificarProximoComida()
    {
        if (targetComida != null)
        {
            var itemDrop = targetComida.GetComponent<ItemDrop>();
            if (itemDrop != null && !itemDrop.estaSendoComido)
            {
                float distanceToTarget = Vector3.Distance(transform.position, targetComida.transform.position);
                if (distanceToTarget < 1f)
                {
                    transform.LookAt(targetComida.transform);
                    animator.SetTrigger("comendo");
                    itemDrop.estaSendoComido = true;
                }
                else
                {
                    MoveToPosition(targetComida.transform.position);
                }
            }
            else
            {
                targetComida = null;
            }
        }
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
            if (isPodeAtacarAlvo(transform, targetInimigo.obterTransformPositionDoCollider().position))
            {
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
        return distanceToInimigo <= lobisomemStats.distanciaDeAtaque;
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
                if (isPodeAtacarAlvo(transform, targetObstaculo.position))
                {
                    atacarAlvo(targetObstaculo.position);
                }
                else
                {
                    MoveToPosition(targetObstaculo.position);
                }
            }
        }
    }

    private void atacarAlvo(Vector3 positionAlvo)
    {
        if (!statsGeral.isAttacking && Time.time > lobisomemStats.lastAttackTime + lobisomemStats.attackInterval)
        {
            transform.LookAt(positionAlvo);
            lobisomemStats.lastAttackTime = Time.time;
            animator.SetTrigger("attack" + Random.Range(1, 3));
        }
        else
        {
            perseguirInimigo();
        }
    }

    private void verificarCorrerAndar()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("uivar") || animator.GetCurrentAnimatorStateInfo(0).IsName("comendo"))
        {
            agent.speed = 0;
        }
        else
        {
            agent.speed = targetInimigo == null ? lobisomemStats.walkSpeed : lobisomemStats.runSpeed;
        }
        setarAnimacaoPorVelocidade();
    }

    private void setarAnimacaoPorVelocidade()
    {
        float speed = agent.velocity.magnitude;
        animator.SetBool("run", speed > 1f);
        animator.SetBool("walk", speed > 0.05f && speed <= 1f);
    }

    private void movimentarAleatoriamentePeloMapa()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("uivar") || targetComida != null || animator.GetCurrentAnimatorStateInfo(0).IsName("comendo")) return;
        if (targetInimigo == null && timer >= timerParaAndarAleatoriamente)
        {
            timer = 0;
            MoveToRandomPosition(raioDeDistanciaMinParaAndarAleatoriamente, raioDeDistanciaMaxParaAndarAleatoriamente);
        }
    }

    private void MoveToPosition(Vector3 position)
    {
        if (Time.time < proximaAtualizacaoCaminho) return;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(position, out hit, raioDeDistanciaMaxParaAndarAleatoriamente, NavMesh.AllAreas))
        {
            agent.SetDestination(position);
        }
        else
        {
            MoveToRandomPosition(raioDeDistanciaMinParaAndarAleatoriamente, raioDeDistanciaMinParaAndarAleatoriamente);
        }

        proximaAtualizacaoCaminho = Time.time + caminhoCooldown;
    }

    private void MoveToRandomPosition(float minDistance, float maxDistance)
    {
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
                MoveToRandomPosition(minDistance, maxDistance);
            }
        }
    }

    public void perseguirInimigo()
    {
        if (targetInimigo == null) return;

        Vector3 targetPosition = targetInimigo.obterTransformPositionDoCollider().position;
        if (Time.time >= proximaAtualizacaoCaminho && agent.isOnNavMesh)
        {
            if (!agent.hasPath || agent.remainingDistance > lobisomemStats.pathUpdateDistanceThreshold)
            {
                MoveToPosition(targetPosition);
            }
        }

        agent.speed = lobisomemStats.runSpeed;
    }

    public float detectionRadius = 10f;

    private void DetectNearbyObjects()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("PlayerCollider") || hitCollider.CompareTag("AnimalCollider"))
            {
                StatsGeral statsGeralEnemy = hitCollider.transform.GetComponentInParent<StatsGeral>();
                if (statsGeralEnemy != null && statsGeralEnemy.health.IsAlive())
                {
                    targetInimigo = statsGeralEnemy;
                    targetComida = null;
                }
            }
            else if (hitCollider.CompareTag("ItemDrop"))
            {
                Consumivel consumivel = hitCollider.GetComponent<Consumivel>();
                if (consumivel != null && consumivel.tipoConsumivel.Equals(Consumivel.TipoConsumivel.Carne))
                {
                    targetComida = hitCollider.gameObject;
                }
            }
        }
    }

    public void Fugir()
    {
        targetInimigo = null;
        targetComida = null;
        Invoke("StopRunning", tempoCorridaFugindo);
        agent.speed = lobisomemStats.runSpeed;
        MoveToRandomPosition(raioDeDistanciaMaxParaAndarAleatoriamente, raioDeDistanciaMaxParaAndarAleatoriamente);
    }

    private void StopRunning()
    {
        agent.speed = lobisomemStats.walkSpeed;
    }

    private void Uivar()
    {
        if (targetInimigo == null && !animator.GetCurrentAnimatorStateInfo(0).IsName("uivar"))
        {
            animator.SetTrigger("uivar");
        }
    }

    void GoAtk() => statsGeral.isAttacking = true;
    void NotAtk() => statsGeral.isAttacking = false;
    void AnimEventComeu() => Destroy(targetComida);

}
