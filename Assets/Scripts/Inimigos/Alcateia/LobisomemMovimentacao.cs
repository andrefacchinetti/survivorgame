using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class LobisomemMovimentacao : MonoBehaviour
{

    [SerializeField] public GameObject pontoBaseTerritorio;
    [SerializeField] public float distanciaMaximaPontoBase = 50, distanciaMaximaDoSeuAlfa = 10;
    [SerializeField] public float tempoCorridaFugindo = 5f; // tempo que o animal corre após tomar dano
    [SerializeField] public float raioDeDistanciaMinParaAndarAleatoriamente = 10f, raioDeDistanciaMaxParaAndarAleatoriamente = 40f;
    [SerializeField] public float timerParaAndarAleatoriamente = 5f, timerParaAlfaDecidirComandosParaSeusBetas = 10, tempoParadoNaArvore = 10;


    //MOVIMENTACAO
    public StatsGeral targetInimigo;
    public GameObject targetComida;
    public Transform targetArvore;

    [HideInInspector] public NavMeshAgent agent;
    private float timer;
    

    [SerializeField] LobisomemController lobisomemController;
    [SerializeField] LobisomemStats lobisomemStats;
    [SerializeField] StatsGeral statsGeral;
    [HideInInspector] public Animator animator;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        statsGeral = GetComponentInParent<StatsGeral>();
        lobisomemStats = GetComponentInParent<LobisomemStats>();
        timer = timerParaAndarAleatoriamente;
    }

    private void Update()
    {
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
            if (!agent.pathPending && agent.remainingDistance < 0.1f)
            {
                agent.ResetPath(); // o animal chegou ao seu destino, pare de se mover
            }
            verificarProximoComida();
        }
        verificarCorrerAndar();
        verificarAtaque();
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
        if (targetInimigo == null) return;
        float distanceToTarget = Vector3.Distance(transform.position, targetInimigo.obterTransformPositionDoCollider().position);
        if (targetInimigo.isDead || distanceToTarget > lobisomemStats.distanciaDePerseguicao)
        {
            //targetComida = target;
            targetInimigo = null;
        }
        else if (distanceToTarget < lobisomemStats.distanciaDeAtaque) // Ataca o alvo
        {
            if (!statsGeral.isAttacking && Time.time > lobisomemStats.lastAttackTime + lobisomemStats.attackInterval)
            {
                transform.LookAt(targetInimigo.obterTransformPositionDoCollider().position);
                lobisomemStats.lastAttackTime = Time.time;
                animator.SetTrigger("attack" + Random.Range(1, 3));
            }
        }
        else // Persegue o alvo
        {
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
            /*agent.speed += Random.Range(-statsGeral.speedVariation, statsGeral.speedVariation);
            if (agent.speed > 1.5f) agent.speed = 1.5f;*/
            agent.speed = lobisomemStats.walkSpeed;
        }
    }

    private void verificarCorrerAndar()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Uivar") || animator.GetCurrentAnimatorStateInfo(0).IsName("Comendo"))
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
            if (agent.velocity.magnitude > 0.2f)
            {
                animator.SetBool("run", true);
                animator.SetBool("walk", false);
            }
            else if (agent.velocity.magnitude > 0.02f)
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

    private bool estaDistanteDoPontoBaseTerritorio()
    {
        if (pontoBaseTerritorio == null) return false;
        float distance = Vector3.Distance(transform.position, pontoBaseTerritorio.transform.position);  // Calcula a dist�ncia entre o inimigo e a posicao do territorio base
        if (distance > distanciaMaximaPontoBase)
        {
            return true;
        }
        return false;
    }

    private bool estaDistanteDoAlfa()
    {
        float distance = Vector3.Distance(transform.position, lobisomemController.alfa.transform.position);  // Calcula a dist�ncia entre o inimigo e a posicao do territorio base
        if (distance > distanciaMaximaDoSeuAlfa)
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
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Uivar") || targetComida != null || animator.GetCurrentAnimatorStateInfo(0).IsName("Comendo")) return;
        if (estaDistanteDoPontoBaseTerritorio())
        {
            targetInimigo = null;
            targetComida = null;
            MoveToPosition(pontoBaseTerritorio.transform.position);
        }
        else
        {
            if (targetInimigo == null)
            {
                MoveToRandomPosition(raioDeDistanciaMinParaAndarAleatoriamente, raioDeDistanciaMaxParaAndarAleatoriamente);
            }
            else
            {
                perseguirJogador();
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
                    perseguirJogador();
                }
                else
                {
                    MoveToRandomPosition(raioDeDistanciaMinParaAndarAleatoriamente, raioDeDistanciaMaxParaAndarAleatoriamente);
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

    private void perseguirJogador()
    {
        MoveToPosition(targetInimigo.obterTransformPositionDoCollider().position);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            lobisomemStats.VerificarSePlayerEstaArmado(other.gameObject);
        }
        if (other.gameObject.tag == "NavMeshVertical" && targetInimigo == null && targetComida == null && !lobisomemStats.isSubindoNaArvore && !lobisomemStats.isIndoAteArvore)
        {
            Debug.Log("jump to tree");
            lobisomemStats.isIndoAteArvore = true;
            agent.speed = lobisomemStats.walkSpeed;
            targetArvore = other.GetComponent<JumpToTree>().treeDestination;
            MoveToPosition(targetArvore.position);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (targetInimigo != null || statsGeral.isDead) return;
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("LOBISOMEM ACHOU player");
            if (targetInimigo == null && lobisomemStats.isEstadoAgressivo && !other.GetComponent<StatsGeral>().isDead)
            {
                targetInimigo = other.GetComponent<StatsGeral>();
                targetComida = null;
                targetArvore = null;
                Debug.Log("LOBISOMEM indo atrasd do player");
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
        if (targetInimigo != null || animator.GetCurrentAnimatorStateInfo(0).IsName("Uivar")) return;
        animator.SetTrigger("uivar");
    }

}
