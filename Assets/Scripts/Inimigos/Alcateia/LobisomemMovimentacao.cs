using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class LobisomemMovimentacao : MonoBehaviour
{

    [SerializeField] public GameObject pontoBaseTerritorio;
    [SerializeField] public float distanciaMaximaPontoBase = 50, distanciaMaximaDoSeuAlfa = 10;
    [SerializeField] public float raioDeDistanciaParaAndarAleatoriamente = 20f;
    [SerializeField] public float timerParaAndarAleatoriamente = 5f, timerParaAlfaDecidirComandosParaSeusBetas = 10;


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
            agent.speed = 2;
            agent.SetDestination(targetArvore.position);
            if (lobisomemStats.isSubindoNaArvore)
            {
                if(agent.velocity.magnitude <= 0.001)
                {
                    animator.SetBool("paradoArvore", true);
                }
                else
                {
                    animator.SetBool("paradoArvore", false);
                }
            }
            animator.SetBool("subindoArvore", lobisomemStats.isSubindoNaArvore);
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
                    transform.LookAt(targetComida.transform);
                    agent.SetDestination(targetComida.transform.position);
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
        if (targetInimigo.isDead || distanceToTarget > statsGeral.distanciaDePerseguicao)
        {
            //targetComida = target;
            targetInimigo = null;
        }
        else if (distanceToTarget < statsGeral.distanciaDeAtaque) // Ataca o alvo
        {
            if (!statsGeral.isAttacking && Time.time > statsGeral.lastAttackTime + statsGeral.attackInterval)
            {
                transform.LookAt(targetInimigo.obterTransformPositionDoCollider().position);
                statsGeral.lastAttackTime = Time.time;
                animator.SetTrigger("attack" + Random.Range(1, 3));
            }
        }
        else // Persegue o alvo
        {
            Vector3 targetOffset = Random.insideUnitSphere * statsGeral.destinationOffset;
            Vector3 leadTarget;
            // Calcula a posi��o futura do jogador com base na sua velocidade atual
            if (targetInimigo.GetComponent<CharacterController>() != null)
            {
                leadTarget = targetInimigo.obterTransformPositionDoCollider().position + (targetInimigo.GetComponent<CharacterController>().velocity.normalized * statsGeral.leadTime);
            }
            else
            {
                leadTarget = targetInimigo.obterTransformPositionDoCollider().position + (targetInimigo.obterTransformPositionDoCollider().GetComponent<NavMeshAgent>().velocity.normalized * statsGeral.leadTime);
            }
            // Calcula o offset da posi��o futura do jogador
            Vector3 leadTargetOffset = (leadTarget - targetInimigo.obterTransformPositionDoCollider().position).normalized * statsGeral.leadDistance;
            // Soma o offset da posi��o futura do jogador com o offset aleat�rio do destino
            Vector3 destination = leadTarget + leadTargetOffset + targetOffset;
            // Define a posi��o de destino para o inimigo
            agent.SetDestination(destination);
           
            // Aplica uma varia��o de velocidade aleat�ria
            agent.speed += Random.Range(-statsGeral.speedVariation, statsGeral.speedVariation);
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
                agent.speed = 1.5f;
            }
            else if(targetInimigo == null)
            {
                agent.speed = 0.1f;
            }
        }
        setarAnimacaoPorVelocidade();
    }

    private void setarAnimacaoPorVelocidade()
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
            agent.SetDestination(pontoBaseTerritorio.transform.position);
        }
        else
        {
            if (targetInimigo == null)
            {
                andarAleatoriamente();
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
                agent.SetDestination(lobisomemController.alfa.transform.position);
            }
            else
            {
                if(targetInimigo != null)
                {
                    perseguirJogador();
                }
                else
                {
                    andarAleatoriamente();
                }
            }
        }
    }


    private void andarAleatoriamente()
    {
        timer += Time.deltaTime;

        if (timer >= timerParaAndarAleatoriamente)
        {
            timer = 0;
            Vector3 randomDirection = Random.insideUnitSphere * raioDeDistanciaParaAndarAleatoriamente;
            randomDirection += transform.position;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, raioDeDistanciaParaAndarAleatoriamente, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }
        }
    }

    private void perseguirJogador()
    {
        agent.SetDestination(targetInimigo.obterTransformPositionDoCollider().position);
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
            agent.speed = 1.5f;
            targetArvore = other.GetComponent<JumpToTree>().treeDestination;
            agent.SetDestination(targetArvore.position);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (targetInimigo != null || statsGeral.isDead) return;
        if (other.gameObject.tag == "Player")
        {
            if (targetInimigo == null && lobisomemStats.isEstadoAgressivo && !other.GetComponent<StatsGeral>().isDead)
            {
                targetInimigo = other.GetComponent<StatsGeral>();
                targetComida = null;
            }
        }
        if (other.gameObject.GetComponent<CollisorSofreDano>() != null)
        {
            StatsGeral objPai = other.gameObject.GetComponent<CollisorSofreDano>().GetComponentInParent<StatsGeral>();
            if (objPai.gameObject.GetComponent<AnimalController>() != null && !objPai.isDead)
            {
                Debug.Log("LOBISOMEM ACHOU ANIMAL");
                targetInimigo = objPai;
                targetComida = null;
            }
        }
        if (targetInimigo == null && other.tag == "ItemDrop")
        {
            if (other.GetComponent<Consumivel>() != null && other.GetComponent<Consumivel>().tipoConsumivel.Equals(Consumivel.TipoConsumivel.Carne))
            {
                targetComida = other.gameObject; //gostou da comida
            }
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
