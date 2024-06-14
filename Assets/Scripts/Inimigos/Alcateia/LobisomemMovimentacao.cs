using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LobisomemMovimentacao : MonoBehaviour
{
    // Variáveis públicas para ajustar o comportamento do lobisomem
    public float raioDeDistanciaMinParaAndarAleatoriamente = 10f;
    public float raioDeDistanciaMaxParaAndarAleatoriamente = 40f;
    public float raioDeDistanciaMaxParaPerseguirAlvo = 40f;
    public float timerParaAndarAleatoriamente = 5f;
    public float preditorMultiplicador = 1.5f;
    public float paramDistanciaProximoDoAlvo = 15f;

    [HideInInspector] public StatsGeral statsGeral;
    [HideInInspector] public LobisomemStats lobisomemStats;
    [HideInInspector] public LobisomemController lobisomemController;
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Animator animator;

    // Referências a outros componentes
     public StatsGeral targetInimigo;
    [HideInInspector] public GameObject targetComida;
    [HideInInspector] public Transform targetObstaculo;

    // Variáveis privadas para controle de tempo
    private float timer;
    private float caminhoCooldown = 0.5f;
    private float proximaAtualizacaoCaminho;
    private float detectionInterval = 0.5f;
    private float detectionTimer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        statsGeral = GetComponent<StatsGeral>();
        lobisomemStats = GetComponent<LobisomemStats>();
        lobisomemController = GetComponent<LobisomemController>();

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
            //Otimizar: detectar em tempos diferentes
            DetectarJogadores();
            DetectarAnimais();
            DetectarArbustos();
            DetectarItensDrop();
        }

        //BEGIN Update movimentacao por Caracteristicas
        UpdateMovimentacaoCaracteristicaNormal();
        UpdateMovimentacaoCaracteristicaStealth();
        UpdateMovimentacaoCaracteristicaMedroso();
        UpdateMovimentacaoCaracteristicaCovarde();
        //END Update movimentacao por Caracteristicas

        verificarProximoComida();
        verificarAtaque();
        verificarCorrerAndar();
    }

    // ------------------- UPDATES POR CARACTERISTICA -------------------------

    private void UpdateMovimentacaoCaracteristicaNormal()
    {
        if (!(LobisomemController.CaracteristicasLobisomem.Normal == lobisomemController.caracteristica
            || LobisomemController.CaracteristicasLobisomem.Veloz == lobisomemController.caracteristica
            || LobisomemController.CaracteristicasLobisomem.Tank == lobisomemController.caracteristica)) return;

        perseguirAndAtacar();
    }

    [SerializeField] GameObject arbustoDestino;
    private void UpdateMovimentacaoCaracteristicaStealth()
    {
        if (!(LobisomemController.CaracteristicasLobisomem.Stealth == lobisomemController.caracteristica)) return;

        if (estaProximoDoAlvo(paramDistanciaProximoDoAlvo)) 
        {
            perseguirAndAtacar(); //Perseguir e atacar
        }
        else
        {
            if (arbustoDestino == null) movimentarAleatoriamentePeloMapa();
            else
            {
                Debug.Log("indo ate arbusto");
                MoveToPosition(arbustoDestino.transform.position);
            }
        }
    }

    bool isEntrouEmCombate = false;
    private void UpdateMovimentacaoCaracteristicaMedroso()
    {
        if (!(LobisomemController.CaracteristicasLobisomem.Medroso == lobisomemController.caracteristica)) return;

        if(targetInimigo == null)
        {
            movimentarAleatoriamentePeloMapa();
            isEntrouEmCombate = false;
        }
        else
        {
            if (estaProximoDoAlvo(paramDistanciaProximoDoAlvo) || isEntrouEmCombate)
            {
                perseguirAndAtacar(); //Perseguir e atacar
            }
            else
            {
                if (isAlvoEstaArmado())
                {
                    MoverParaDistanciaSeguraDoAlvo();
                }
                else
                {
                    isEntrouEmCombate = true;
                    MoveToPosition(targetInimigo.transform.position);
                }
            }
        }
    }

    private void UpdateMovimentacaoCaracteristicaCovarde()
    {
        if (!(LobisomemController.CaracteristicasLobisomem.Covarde == lobisomemController.caracteristica)) return;

        if (targetInimigo == null)
        {
            movimentarAleatoriamentePeloMapa();
        }
        else
        {
            if (estaProximoDoAlvo(paramDistanciaProximoDoAlvo / 2))
            {
                perseguirAndAtacar(); //Perseguir e atacar
            }
            else
            {
                if (isJogadorEstaOlhandoParaLobisomem())
                {
                    MoverPertoDasCostasDoJogador();
                }
                else
                {
                    MoveToPosition(targetInimigo.transform.position);
                }
            }
        }
    }

    // ------------------- FUNCOES ESPECIFICAS POR CARACTERISTICA -------------------------

    private bool isAlvoEstaArmado()
    {
        if(targetInimigo != null && targetInimigo.jogadorStats.playerController.inventario.itemNaMao != null &&
            targetInimigo.jogadorStats.playerController.inventario.itemNaMao.tipoItem == Item.TiposItems.Arma)
        {
            return true;
        }
        return false;
    }

    private bool isJogadorEstaOlhandoParaLobisomem()
    {
        if (targetInimigo == null) return false;
        // Obtém o vetor de direção do jogador
        Vector3 direcaoDoOlharDoJogador = targetInimigo.transform.forward;
        // Obtém o vetor de direção do jogador até o lobisomem
        Vector3 direcaoParaLobisomem = (transform.position - targetInimigo.transform.position).normalized;
        // Calcula o produto escalar entre as duas direções
        float dotProduct = Vector3.Dot(direcaoDoOlharDoJogador, direcaoParaLobisomem);
        // Calcula o ângulo entre as duas direções
        float angulo = Mathf.Acos(dotProduct) * Mathf.Rad2Deg;
        // Verifica se o ângulo está dentro do limite
        return angulo <= 45f;
    }


    // ------------------- FUNCOES BASICAS -------------------------

    private bool estaProximoDoAlvo(float distanciaProx) //Se o lobisomem está a 10m do jogador, é pq ele ta perto
    {
        if (targetInimigo == null) return false;
        float distanceToTarget = Vector3.Distance(transform.position, targetInimigo.transform.position);
        return distanceToTarget < distanciaProx;
    }

    private void perseguirAndAtacar() 
    {
        Debug.Log("perseguindo e atacando");
        if (targetInimigo != null)
        {
            if (isPodeAtacarAlvo(transform, targetInimigo.transform.position))
            {
                atacarAlvo(targetInimigo.transform.position);
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
            if (isPodeAtacarAlvo(transform, targetInimigo.transform.position))
            {
                atacarAlvo(targetInimigo.transform.position);
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
        return distanceToInimigo <= lobisomemStats.distanciaDeAtaque*2;
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
            // Calcula a posição prevista do alvo
            Vector3 predictedPosition = PreverPosicaoAlvo(positionAlvo);

            // Ajusta a direção do lobisomem para a posição prevista
            transform.LookAt(predictedPosition);

            // Ativa o ataque
            lobisomemStats.lastAttackTime = Time.time;
            animator.SetTrigger("attack" + Random.Range(1, 3));
        }
        else
        {
            perseguirInimigo();
        }
    }

    // Função para prever a posição futura do alvo
    private Vector3 PreverPosicaoAlvo(Vector3 positionAlvo)
    {
        if (targetInimigo == null) return positionAlvo;
        Vector3 alvoPosition = targetInimigo.transform.position;
        Vector3 alvoVelocity = (alvoPosition - targetInimigo.transform.position) / detectionInterval;
        Vector3 predictedPosition = alvoPosition + alvoVelocity * preditorMultiplicador;
        return predictedPosition;
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

        Debug.Log("movimentando aleatoriamente pelo mapa");
        if (timer >= timerParaAndarAleatoriamente)
        {
            if (targetInimigo == null)
            {
                timer = 0;
                MoveToRandomPosition(raioDeDistanciaMinParaAndarAleatoriamente, raioDeDistanciaMaxParaAndarAleatoriamente);
            }
            else if (Vector3.Distance(transform.position, targetInimigo.transform.position) > raioDeDistanciaMaxParaPerseguirAlvo)
            {
                targetInimigo = null;
                timer = 0;
                Uivar();
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
        }

        proximaAtualizacaoCaminho = Time.time + caminhoCooldown;
    }

    float distanciaSegura = 20f;
    private void MoverParaDistanciaSeguraDoAlvo()
    {
        if (targetInimigo == null) return;

        // Calcula a direção do lobisomem para o alvo
        Vector3 directionToTarget = (transform.position - targetInimigo.transform.position).normalized;
        // Calcula a posição que está a 'distanciaSegura' do alvo
        Vector3 targetPosition = targetInimigo.transform.position + directionToTarget * distanciaSegura;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetPosition, out hit, distanciaSegura, NavMesh.AllAreas))
        {
            // Verifica se a posição projetada está dentro do NavMesh
            if (hit.hit)
            {
                agent.SetDestination(hit.position);
            }
        }
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
        Vector3 alvoPosition = targetInimigo.transform.position;
        Vector3 alvoVelocity = (alvoPosition - targetInimigo.transform.position) / detectionInterval;
        Vector3 predictedPosition = alvoPosition + alvoVelocity * preditorMultiplicador;

        if (Time.time >= proximaAtualizacaoCaminho && agent.isOnNavMesh)
        {
            if (!agent.hasPath || agent.remainingDistance > lobisomemStats.pathUpdateDistanceThreshold)
            {
                MoveToPosition(predictedPosition);
            }
            else if (agent.remainingDistance <= agent.stoppingDistance)
            {
                agent.ResetPath(); // Se o agente estiver muito perto do destino, pare de perseguir
            }
            else
            {
                targetInimigo = null;
                movimentarAleatoriamentePeloMapa();
            }
        }

        agent.speed = lobisomemStats.runSpeed;
    }

    private void MoverPertoDasCostasDoJogador()
    {
        if (targetInimigo == null) return;
        Debug.Log("movendo para as costas do jogador");

        Vector3 alvoPosition = targetInimigo.transform.position;
        Vector3 alvoDirection = targetInimigo.transform.forward;
        float anguloIncremento = 45f; // Incremento de ângulo para verificar pontos ao redor do jogador

        List<Vector3> potentialPositions = new List<Vector3>();

        // Gera posições ao redor do jogador
        for (float angle = -90; angle <= 90; angle += anguloIncremento)
        {
            Vector3 direction = Quaternion.Euler(0, angle, 0) * alvoDirection;
            Vector3 position = alvoPosition - direction * distanciaSegura;
            potentialPositions.Add(position);
        }

        // Verifica a posição navegável mais próxima
        Vector3 melhorPosicao = alvoPosition;
        float menorDistancia = float.MaxValue;
        NavMeshHit hit;

        foreach (Vector3 pos in potentialPositions)
        {
            if (NavMesh.SamplePosition(pos, out hit, distanciaSegura, NavMesh.AllAreas))
            {
                float distancia = Vector3.Distance(transform.position, hit.position);
                if (distancia < menorDistancia)
                {
                    melhorPosicao = hit.position;
                    menorDistancia = distancia;
                }
            }
        }

        if (melhorPosicao != alvoPosition)
        {
            if (Time.time >= proximaAtualizacaoCaminho && agent.isOnNavMesh)
            {
                if (!agent.hasPath || agent.remainingDistance > lobisomemStats.pathUpdateDistanceThreshold)
                {
                    MoveToPosition(melhorPosicao);
                }
                else if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    agent.ResetPath(); // Se o agente estiver muito perto do destino, pare de perseguir
                }
                else
                {
                    targetInimigo = null;
                    movimentarAleatoriamentePeloMapa();
                }
            }

            agent.speed = lobisomemStats.runSpeed;
        }
    }

    public float detectionRadius = 100f;

    private void DetectarJogadores()
    {
        if (targetInimigo != null) return;
        int layerMask = LayerMask.GetMask("SubCharacter");
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius, layerMask);

        foreach (var hitCollider in hitColliders)
        {
            StatsGeral statsGeralEnemy = hitCollider.transform.GetComponentInParent<StatsGeral>();
            if (statsGeralEnemy != null && statsGeralEnemy.health.IsAlive())
            {
                targetInimigo = statsGeralEnemy;
                targetComida = null;
                break;
            }
        }
    }

    private void DetectarAnimais()
    {
        if (targetInimigo != null) return;
        int layerMask = LayerMask.GetMask("Enemy");
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius, layerMask);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("AnimalCollider"))
            {
                StatsGeral statsGeralEnemy = hitCollider.transform.GetComponentInParent<StatsGeral>();
                if (statsGeralEnemy != null && statsGeralEnemy.health.IsAlive())
                {
                    targetInimigo = statsGeralEnemy;
                    targetComida = null;
                    break;
                }
            }
        }
    }

    private void DetectarArbustos()
    {
        int layerMask = LayerMask.GetMask("ArbustoStealth");
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius, layerMask);

        float shortestDistance = float.MaxValue;
        GameObject nearestArbusto = null;

        foreach (var hitCollider in hitColliders)
        {
            if (targetInimigo != null)
            {
                float distanceToLobisomem = Vector3.Distance(transform.position, hitCollider.transform.position);
                float distanceToPlayer = Vector3.Distance(targetInimigo.transform.position, hitCollider.transform.position);
                float averageDistance = (distanceToLobisomem + distanceToPlayer) / 2;

                if (averageDistance < shortestDistance)
                {
                    shortestDistance = averageDistance;
                    nearestArbusto = hitCollider.gameObject;
                }
            }
        }
        if (nearestArbusto != null)
        {
            arbustoDestino = nearestArbusto;
        }
    }

    private void DetectarItensDrop()
    {
        if (targetInimigo != null) return;
        int layerMask = LayerMask.GetMask("InteracaoComGrab");
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius, layerMask);
        foreach (var hitCollider in hitColliders)
        {
             if (hitCollider.CompareTag("ItemDrop"))
            {
                Consumivel consumivel = hitCollider.GetComponent<Consumivel>();
                if (consumivel != null && consumivel.tipoConsumivel.Equals(Consumivel.TipoConsumivel.Carne))
                {
                    targetComida = hitCollider.gameObject;
                    break;
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

