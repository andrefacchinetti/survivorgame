using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class LobisomemMovimentacao : MonoBehaviour
{

    [SerializeField] public float distanciaMaximaPontoBase = 50, distanciaMaximaDoSeuAlfa = 10;
    [SerializeField] public float tempoCorridaFugindo = 5f; // tempo que o animal corre após tomar dano
    [SerializeField] public float raioDeDistanciaMinParaAndarAleatoriamente = 10f, raioDeDistanciaMaxParaAndarAleatoriamente = 40f;
    [SerializeField] public float timerParaAndarAleatoriamente = 5f, timerParaAlfaDecidirComandosParaSeusBetas = 10, tempoParadoNaArvore = 10;


    //MOVIMENTACAO
    public StatsGeral targetInimigo;
    public GameObject targetComida;
    public Transform targetArvore;
    public Transform targetObstaculo;

    private float timer;
    

    [SerializeField] LobisomemController lobisomemController;
    [SerializeField] LobisomemStats lobisomemStats;
    [SerializeField] StatsGeral statsGeral;
    [SerializeField] public Animator animator;
    [SerializeField] public NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        statsGeral = GetComponentInParent<StatsGeral>();
        lobisomemStats = GetComponentInParent<LobisomemStats>();
        timer = timerParaAndarAleatoriamente;
        InvokeRepeating("FinalTurnoProfissao", lobisomemStats.tempoMudancaDeTurnoProfissoes, lobisomemStats.tempoMudancaDeTurnoProfissoes);
    }

    private void Update()
    {
        if (statsGeral.isDead) return;
        if (targetArvore != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, targetArvore.position);
            if (distanceToTarget <= 0.5f)//pousou na arvore
            {
                agent.ResetPath();
                Invoke("SairDaArvore", tempoParadoNaArvore);
            }
            else
            {
                agent.speed = lobisomemStats.walkSpeed;
                MoveToPosition(targetArvore.position);
            }
           
        }
        else
        {
            animator.SetBool("subindoArvore", false);
            animator.SetBool("paradoArvore", false);
            if (LobisomemController.Categoria.Omega.Equals(lobisomemController.categoria)) movimentacaoOmega();
            else if (LobisomemController.Categoria.Alfa.Equals(lobisomemController.categoria)) movimentacaoAlfa();
            else if (LobisomemController.Categoria.Beta.Equals(lobisomemController.categoria)) movimentacaoBeta();
            if (agent.isOnNavMesh && !agent.pathPending && agent.remainingDistance < 0.1f)
            {
                agent.ResetPath(); // o animal chegou ao seu destino, pare de se mover
            }
            verificarProximoComida();
        }
        verificarCorrerAndar();
        verificarAtaque();
    }

    private void LateUpdate()
    {
        AtivarObjetosMaoDasAnimacoes();
    }

    private Transform obterObstaculoNoCaminhoDoInimigo()
    {
        if (targetObstaculo != null) return targetObstaculo;

        Collider[] colliders = Physics.OverlapSphere(transform.position, lobisomemStats.distanciaDeAtaque);
        foreach (Collider col in colliders)
        {
            if (col.tag == "ConstrucaoStats")
            {
                Debug.Log("construcao encontrada..." + col.name);
                agent.ResetPath();
                return col.transform;
            }
        }
        Debug.Log("construcao NAO encontrada ");
        return null;
    }

    private bool procurarObstaculoPerto(Transform targetTransform, float radius)
    {
        Collider[] colliders = Physics.OverlapSphere(targetTransform.position, radius);

        foreach (Collider col in colliders)
        {
            if (col.tag == "ConstrucaoStats")
            {
                // Há um obstáculo perto do alvo
                Debug.Log("Obstáculo encontrado: " + col.name);
                return true;
            }
        }

        // Não há obstáculos perto do alvo
        return false;
    }

    void SairDaArvore()
    {
        targetArvore = null;
    }

    private void verificarProximoComida()
    {
        if(targetComida != null)
        {
            if (targetComida.GetComponent<ItemDrop>() !=null && !targetComida.GetComponent<ItemDrop>().estaSendoComido)
            {
                float distanceToTarget = Vector3.Distance(transform.position, targetComida.transform.position);
                if (distanceToTarget < 1)
                {
                    transform.LookAt(targetComida.transform);
                    animator.SetTrigger("comendo");
                    targetComida.GetComponent<ItemDrop>().estaSendoComido = true;
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
       
        if (targetInimigo.isDead) 
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
        return distanceToInimigo <= 3.1f;
    }

    private void atacarObstaculoOuInimigo()
    {
        targetObstaculo = obterObstaculoNoCaminhoDoInimigo();
        if (targetObstaculo != null)
        {
            if (isPodeAtacarAlvo(transform, targetObstaculo.position))
            {
                atacarAlvo(targetObstaculo.position);
                targetObstaculo = null;
            }
            else
            {
                Debug.Log("indo ate o obstaculo no caminho...");
                MoveToPosition(targetObstaculo.position);
            }
        }
        else
        {
            Debug.Log("Perseguindo inimigo");
            perseguirInimigo();
        }
    }

    private void atacarAlvo(Vector3 positionAlvo)
    {
        if (!statsGeral.isAttacking && Time.time > lobisomemStats.lastAttackTime + lobisomemStats.attackInterval)
        {
            transform.LookAt(positionAlvo);
            lobisomemStats.lastAttackTime = Time.time;
            animator.SetTrigger("attack" + Random.Range(1, 3));
            Debug.Log("Atacando obstaculo");
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
            if(lobisomemStats.isIndoAteArvore || lobisomemStats.isSubindoNaArvore)
            {
                agent.speed = lobisomemStats.walkSpeed;
            }
            else if(targetInimigo == null)
            {
                agent.speed = lobisomemStats.walkSpeed;
            }
        }
        setarAnimacaoPorVelocidade();
    }

    private void setarAnimacaoPorVelocidade()
    {
        animator.SetBool("subindoArvore", lobisomemStats.isSubindoNaArvore);
        if (lobisomemStats.isSubindoNaArvore)
        {
            if (agent.velocity.magnitude <= 0.001)
            {
                animator.SetBool("paradoArvore", true);
            }
            else
            {
                animator.SetBool("paradoArvore", false);
            }
        }
        else
        {
            if (agent.velocity.magnitude > 1f)
            {
                animator.SetBool("run", true);
                animator.SetBool("walk", false);
            }
            else if (agent.velocity.magnitude > 0.05f)
            {
                animator.SetBool("walk", true);
                animator.SetBool("run", false);
            }
            else
            {
                animator.SetBool("walk", false);
                animator.SetBool("run", false);
            }
        }
    }

    private bool estaDistanteDoCentroDaAldeia()
    {
        if (lobisomemController.aldeiaController == null) return false;
        return EstouDistanteDe(lobisomemController.aldeiaController.centroDaAldeia.transform.position, distanciaMaximaPontoBase);
    }

    private bool estaDistanteDoAlfa()
    {
        return EstouDistanteDe(lobisomemController.alfa.transform.position, distanciaMaximaDoSeuAlfa);
    }

    private bool EstouDistanteDe(Vector3 destino, float distanciaMaximaDestino)
    {
        float distance = Vector3.Distance(transform.position, destino);  // Calcula a dist�ncia entre o inimigo e a posicao do territorio base
        if (distance > distanciaMaximaDestino)
        {
            return true;
        }
        return false;
    }

    private void movimentacaoOmega()
    {
        movimentarAleatoriamentePeloMapa();
    }

    private void movimentarAleatoriamentePeloMapa()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("uivar") || targetComida != null || animator.GetCurrentAnimatorStateInfo(0).IsName("comendo")) return;
        if (estaDistanteDoCentroDaAldeia())
        {
            targetInimigo = null;
            targetComida = null;
            MoveToPosition(lobisomemController.aldeiaController.centroDaAldeia.transform.position);
        }
        else
        {
            if (targetInimigo == null)
            {
                if (lobisomemController.forma == LobisomemController.Forma.Humano && !lobisomemStats.isEstadoAgressivo && lobisomemController.categoria != LobisomemController.Categoria.Omega)
                {
                    TrabalharNaProfissao();
                }
                else
                {
                    MoveToRandomPosition(raioDeDistanciaMinParaAndarAleatoriamente, raioDeDistanciaMaxParaAndarAleatoriamente);
                }
            }
        }
    }

    private void movimentacaoAlfa()
    {
        movimentarAleatoriamentePeloMapa();
    }

    public void ComandosAlfaParaBetas()
    {
        if (!LobisomemController.Categoria.Alfa.Equals(lobisomemController.categoria)) return;
        Uivar();
        foreach (LobisomemController beta in lobisomemController.betas)
        {
            if (lobisomemStats.isEstadoAgressivo)
            {
                beta.lobisomemStats.isEstadoAgressivo = true;
                beta.lobisomemMovimentacao.targetInimigo = targetInimigo;
            }
            else
            {
                beta.lobisomemStats.isEstadoAgressivo = false;
                beta.lobisomemMovimentacao.targetInimigo = null;
            }
        }
    }

    public void ComandosBetasParaAlfa()
    {
        lobisomemController.alfa.lobisomemStats.AumentarNivelAgressividade(15);
    }

    private void movimentacaoBeta()
    {
        if (lobisomemController.alfa == null || lobisomemController.alfa.statsGeral.isDead)
        {
            lobisomemStats.isEstadoAgressivo = true;
            movimentarAleatoriamentePeloMapa();
        }
        else
        {
            if (estaDistanteDoAlfa())
            {
                targetInimigo = null;
                MoveToPosition(lobisomemController.alfa.transform.position);
            }
            else
            {
                if(targetInimigo != null)
                {
                    perseguirInimigo();
                }
                else
                {
                    movimentarAleatoriamentePeloMapa();
                }
            }
        }
    }

    private void MoveToPosition(Vector3 position)
    {
        NavMeshHit hit;
        bool hasValidPath = NavMesh.SamplePosition(position, out hit, raioDeDistanciaMaxParaAndarAleatoriamente, NavMesh.AllAreas);
        if (hasValidPath)
        {
            agent.SetDestination(position);
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

    private void perseguirInimigo()
    {
        if (targetInimigo == null) return;
        Debug.Log("perseguir inimigo");
        Vector3 targetOffset = Random.insideUnitSphere * lobisomemStats.destinationOffset;
        Vector3 leadTarget;
        // Calcula a posi��o futura do jogador com base na sua velocidade atual
        if (targetInimigo.GetComponent<CharacterController>() != null)
        {
            leadTarget = targetInimigo.obterTransformPositionDoCollider().position + (targetInimigo.GetComponent<CharacterController>().velocity.normalized * lobisomemStats.leadTime);
        }
        else
        {
            leadTarget = targetInimigo.obterTransformPositionDoCollider().position + (targetInimigo.obterTransformPositionDoCollider().GetComponent<NavMeshAgent>().velocity.normalized * lobisomemStats.leadTime);
        }
        // Calcula o offset da posi��o futura do jogador
        Vector3 leadTargetOffset = (leadTarget - targetInimigo.obterTransformPositionDoCollider().position).normalized * lobisomemStats.leadDistance;
        // Soma o offset da posi��o futura do jogador com o offset aleat�rio do destino
        Vector3 destination = leadTarget + leadTargetOffset + targetOffset;
        // Define a posi��o de destino para o inimigo
        MoveToPosition(destination);

        // Aplica uma varia��o de velocidade aleat�ria
        /*agent.speed = Random.Range(lobisomemStats.walkSpeed, lobisomemStats.runSpeed);
        if (agent.speed > 1.5f) agent.speed = 1.5f;*/
        agent.speed = lobisomemStats.runSpeed;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            lobisomemStats.VerificarSePlayerEstaArmado(other.gameObject);
        }
        if (lobisomemController.categoria == LobisomemController.Categoria.Omega && other.gameObject.tag == "NavMeshVertical" && targetInimigo == null && targetComida == null && !lobisomemStats.isSubindoNaArvore && !lobisomemStats.isIndoAteArvore && other.GetComponent<JumpToTree>() != null)
        {
            Debug.Log("jump to tree");
            lobisomemStats.isIndoAteArvore = true;
            agent.speed = lobisomemStats.walkSpeed;
            targetArvore = other.GetComponent<JumpToTree>().treeDestination;
            MoveToPosition(targetArvore.position);
        }
    }

    private bool VerificarSeAlcancaAlvo(Transform transform)
    {
        NavMeshPath path = new NavMeshPath();

        if (NavMesh.CalculatePath(agent.transform.position, transform.position, NavMesh.AllAreas, path))
        {
            if (path.status == NavMeshPathStatus.PathComplete)
            {
                Debug.Log("O NavMeshAgent pode alcançar o alvo.");
                return true;
            }
            else
            {
                Debug.Log("O NavMeshAgent não pode alcançar o alvo.");
                return false;
            }
        }

        if (!EstouDistanteDe(transform.position, 2))
        {
            return false;
        }
        else
        {
            Debug.Log("O NavMeshAgent esta indo pra perto do alvo.");
            return true;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (targetInimigo != null || statsGeral.isDead) return;
        if (other.gameObject.tag == "Player")
        {
            if (targetInimigo == null && lobisomemStats.isEstadoAgressivo && !other.GetComponent<StatsGeral>().isDead)
            {
                Debug.Log("LOBISOMEM ACHOU player e setou seu alvo");
                targetInimigo = other.GetComponent<StatsGeral>();
                targetComida = null;
                targetArvore = null;
            }
        }
        if (other.gameObject.GetComponent<CollisorSofreDano>() != null)
        {
            StatsGeral objPai = other.gameObject.GetComponent<CollisorSofreDano>().GetComponentInParent<StatsGeral>();
            if (objPai.gameObject.GetComponent<AnimalController>() != null && !objPai.isDead)
            {
                Debug.Log("LOBISOMEM ACHOU ANIMAL");
                targetComida = null;
                targetArvore = null;
                if (objPai.gameObject.GetComponent<AnimalController>().isPequenoPorte)
                {
                    if (objPai.gameObject.GetComponent<AnimalController>().isAnimalAgressivo)
                    {
                        Fugir();
                    }
                }
                else
                {
                    targetInimigo = objPai;
                }
            }
        }
        if (targetInimigo == null && targetArvore == null && other.tag == "ItemDrop")
        {
            if (other.GetComponent<Consumivel>() != null && other.GetComponent<Consumivel>().tipoConsumivel.Equals(Consumivel.TipoConsumivel.Carne))
            {
                targetComida = other.gameObject; //gostou da comida
            }
        }
    }

    private void Fugir()
    {
        Debug.Log("lobisomem fugindo");
        targetInimigo = null;
        targetComida = null;
        Invoke("StopRunning", tempoCorridaFugindo);
        agent.speed = lobisomemStats.runSpeed;
        MoveToRandomPosition(raioDeDistanciaMaxParaAndarAleatoriamente, raioDeDistanciaMaxParaAndarAleatoriamente);
    }

    void GoAtk()
    {
        statsGeral.isAttacking = true;
    }

    void NotAtk()
    {
        statsGeral.isAttacking = false;
    }

    void AnimEventComeu()
    {
        if (targetComida != null && targetComida.tag != "Player") { 
            Destroy(targetComida.gameObject); 
        }
    }

    private void Uivar()
    {
        if (targetInimigo != null || animator.GetCurrentAnimatorStateInfo(0).IsName("uivar")) return;
        animator.SetTrigger("uivar");
    }

    //PROFISSOES

    private void TrabalharNaProfissao()
    {
        if (lobisomemController.profissao == LobisomemController.Profissao.Alfa) trabalharProfissaoAlfa();
        else if (lobisomemController.profissao == LobisomemController.Profissao.Seguranca) trabalharProfissaoSeguranca();
        else if (lobisomemController.profissao == LobisomemController.Profissao.Pescador) trabalharProfissaoPescador();
    }

    private void trabalharProfissaoAlfa()
    {
        MoveToRandomPosition(raioDeDistanciaMinParaAndarAleatoriamente, raioDeDistanciaMaxParaAndarAleatoriamente);
    }

    int indexLocaisSeguranca = 0; //Alterar index por tempo
    private void trabalharProfissaoSeguranca()
    {
        Vector3 positionLocalSeguranca = lobisomemController.aldeiaController.locaisSeguranca[indexLocaisSeguranca].localPosicao.position;
        if (EstouDistanteDe(positionLocalSeguranca, 1))
        {
            MoveToPosition(positionLocalSeguranca);
        }
        else
        {
            if (agent.velocity.magnitude <= 0.001) animator.SetTrigger("poseSeguranca");
            transform.LookAt(lobisomemController.aldeiaController.locaisSeguranca[indexLocaisSeguranca].localOlhando);
        }
    }

    int indexLocaisPesca = 0; //Alterar index por tempo
    private void trabalharProfissaoPescador()
    {
        if (entregandoItemProfissao)
        {
            Vector3 positionLocal = lobisomemController.aldeiaController.armazemPesca.transform.position;
            if (EstouDistanteDe(positionLocal, 1))
            {
                MoveToPosition(lobisomemController.aldeiaController.armazemPesca.transform.position);
            }
            else
            {
                entregandoItemProfissao = false;
            }
        }
        else
        {
            Vector3 positionLocal = lobisomemController.aldeiaController.locaisPesca[indexLocaisPesca].localPosicao.position;
            if (EstouDistanteDe(positionLocal, 1))
            {
                MoveToPosition(positionLocal);
                voltouProLocalDaProfissao = false;
            }
            else
            {
                if (agent.velocity.magnitude <= 0.001) animator.SetTrigger("profissaoPescando");
                transform.LookAt(lobisomemController.aldeiaController.locaisPesca[indexLocaisPesca].localOlhando);
                voltouProLocalDaProfissao = true;
            }
        }
    }

    //FIM PROFISSOES
    bool entregandoItemProfissao = false, voltouProLocalDaProfissao = false;

    void FinalTurnoProfissao()
    {
        if (lobisomemController.aldeiaController == null || entregandoItemProfissao || !voltouProLocalDaProfissao) return;
        Debug.Log("fim turno profissao");
        indexLocaisSeguranca++;
        if (indexLocaisSeguranca >= lobisomemController.aldeiaController.locaisSeguranca.Count) indexLocaisSeguranca = 0;
        entregandoItemProfissao = true;
        voltouProLocalDaProfissao = false;
    }

    //EVENT ANIMACOES

    private void AtivarObjetosMaoDasAnimacoes()
    {
        if(lobisomemController.objProfissaoVaraPesca != null) lobisomemController.objProfissaoVaraPesca.SetActive(animator.GetCurrentAnimatorStateInfo(0).IsName("profissaoPescando"));
    }

}
