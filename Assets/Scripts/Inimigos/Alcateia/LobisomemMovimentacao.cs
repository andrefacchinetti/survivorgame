using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class LobisomemMovimentacao : MonoBehaviour
{

    public float raioDeDistanciaMinParaAndarAleatoriamente = 10f, raioDeDistanciaMaxParaAndarAleatoriamente = 40f, raioDeDistanciaMaxParaPerseguirAlvo = 40f;
    public float timerParaAndarAleatoriamente = 5f;
    public float preditorMultiplicador = 1.5f;

    [HideInInspector] public StatsGeral targetInimigo;
    [HideInInspector] public GameObject targetComida;
    [HideInInspector] public Transform targetObstaculo;

    private float timer;
    private float caminhoCooldown = 0.5f;
    private float proximaAtualizacaoCaminho;
    public NavMeshAgent agent;
    public Animator animator;
    public StatsGeral statsGeral;
    public LobisomemStats lobisomemStats;

    private float detectionInterval = 0.5f;
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
            if (isPodeAtacarAlvo(transform, targetInimigo.obterTransformPositionDoCollider().position))
            {
                atacarAlvo(targetInimigo.obterTransformPositionDoCollider().position);
            }
            else
            {
                perseguirInimigo();
            }
        }
        else
        {
            movimentarAleatoriamentePeloMapa();
        }

        verificarProximoComida();
        verificarAtaque();
        verificarCorrerAndar();
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

        if(timer >= timerParaAndarAleatoriamente)
        {
            if (targetInimigo == null)
            {
                timer = 0;
                MoveToRandomPosition(raioDeDistanciaMinParaAndarAleatoriamente, raioDeDistanciaMaxParaAndarAleatoriamente);
                Debug.Log("nao tenho alvo, andando aleatoriamente");
            }
            else if (Vector3.Distance(transform.position, targetInimigo.obterTransformPositionDoCollider().position) > raioDeDistanciaMaxParaPerseguirAlvo)
            {
                targetInimigo = null;
                timer = 0;
                Uivar();
                Debug.Log("alvo ta muito distante, parando de perseguir e uivando");
            }
        }
    }

    private void MoveToPosition(Vector3 position)
    {
        if (Time.time < proximaAtualizacaoCaminho) return;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(position, out hit, raioDeDistanciaMaxParaAndarAleatoriamente, NavMesh.AllAreas))
        {
            // Verifica se a posição projetada está dentro do NavMesh
            if (hit.hit)
            {
                agent.SetDestination(hit.position);
            }
            else
            {
                MoveToRandomPosition(raioDeDistanciaMinParaAndarAleatoriamente, raioDeDistanciaMinParaAndarAleatoriamente);
                Debug.Log("move to position: hit else");
            }
        }
        else
        {
            MoveToRandomPosition(raioDeDistanciaMinParaAndarAleatoriamente, raioDeDistanciaMinParaAndarAleatoriamente);
            Debug.Log("move to position: sampleposition else");
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
        }
    }

    public void perseguirInimigo()
    {
        if (targetInimigo == null) return;
        Vector3 alvoPosition = targetInimigo.obterTransformPositionDoCollider().position;
        Vector3 alvoVelocity = (alvoPosition - targetInimigo.transform.position) / detectionInterval;
        Vector3 predictedPosition = alvoPosition + alvoVelocity * preditorMultiplicador;

        if (Time.time >= proximaAtualizacaoCaminho && agent.isOnNavMesh)
        {
            if (!agent.hasPath || agent.remainingDistance > lobisomemStats.pathUpdateDistanceThreshold)
            {
                MoveToPosition(predictedPosition);
                Debug.Log("perseguindo inimigo: move to " + predictedPosition);
            }
            else if (agent.remainingDistance <= agent.stoppingDistance)
            {
                // Se o agente estiver muito perto do destino, pare de perseguir
                agent.ResetPath();
                Debug.Log("perseguindo inimigo: parou de perseguir, muito perto do destino");
            }
            else
            {
                Debug.Log("perseguindo inimigo: nao consegui mover");
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
