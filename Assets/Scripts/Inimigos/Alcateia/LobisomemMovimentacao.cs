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
    [SerializeField][HideInInspector] public StatsGeral targetInimigo;
    [SerializeField] [HideInInspector] public GameObject targetComida;
    [SerializeField] [HideInInspector] public Transform targetObstaculo;

    private float timer;

    [SerializeField] [HideInInspector] LobisomemController lobisomemController;
    [SerializeField] [HideInInspector] LobisomemStats lobisomemStats;
    [SerializeField] [HideInInspector] public StatsGeral statsGeral;
    [SerializeField] [HideInInspector] public Animator animator;
    [SerializeField] [HideInInspector] public NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        statsGeral = GetComponentInParent<StatsGeral>();
        lobisomemStats = GetComponentInParent<LobisomemStats>();
        lobisomemController = GetComponentInParent<LobisomemController>();
        timer = timerParaAndarAleatoriamente;
    }

    private void Update()
    {
        if (statsGeral.isDead) return;
        animator.SetBool("subindoArvore", false);
        animator.SetBool("paradoArvore", false);
        movimentacaoAlfa();
        if (agent.isOnNavMesh && !agent.pathPending && agent.remainingDistance < 0.1f)
        {
            agent.ResetPath(); // o animal chegou ao seu destino, pare de se mover
        }
        verificarProximoComida();
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
           if(targetInimigo == null)
           {
               agent.speed = lobisomemStats.walkSpeed;
           }
        }
        setarAnimacaoPorVelocidade();
    }

    private void setarAnimacaoPorVelocidade()
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

    private bool estaDistanteDoCentroDaAldeia()
    {
        if (lobisomemController.aldeiaController == null) return false;
        return EstouDistanteDe(lobisomemController.aldeiaController.centroDaAldeia.transform.position, distanciaMaximaPontoBase);
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
                MoveToRandomPosition(raioDeDistanciaMinParaAndarAleatoriamente, raioDeDistanciaMaxParaAndarAleatoriamente);
            }
        }
    }

    private void movimentacaoAlfa()
    {
        movimentarAleatoriamentePeloMapa();
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

       

        Vector3 targetVelocity = GetTargetVelocity(targetInimigo);

        if (targetVelocity != Vector3.zero)
        {
            Vector3 predictedPosition = PredictTargetPosition(targetInimigo, targetVelocity);

            Vector3 randomOffset = Random.insideUnitSphere * lobisomemStats.destinationOffset;

            Vector3 destination = predictedPosition + (predictedPosition - targetInimigo.obterTransformPositionDoCollider().position).normalized * lobisomemStats.leadDistance + randomOffset;

            // Verifica se o agent já tem um path e se ele está próximo do destino
            if (!agent.hasPath || (agent.hasPath && agent.remainingDistance > lobisomemStats.pathUpdateDistanceThreshold))
            {
                Debug.Log("Perseguindo inimigo 1");
                MoveToPosition(destination);
            }

            agent.speed = lobisomemStats.runSpeed;
        }
        else
        {
            if(agent.velocity == Vector3.zero)
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
        return target.obterTransformPositionDoCollider().position + velocity * lobisomemStats.leadTime;
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
            if (targetInimigo == null && !other.GetComponent<StatsGeral>().isDead)
            {
                Debug.Log("LOBISOMEM ACHOU player e setou seu alvo");
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
                targetComida = null;
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
        if (targetInimigo == null && other.tag == "ItemDrop")
        {
            if (other.GetComponent<Consumivel>() != null && other.GetComponent<Consumivel>().tipoConsumivel.Equals(Consumivel.TipoConsumivel.Carne))
            {
                targetComida = other.gameObject; //gostou da comida
            }
        }
    }

    public void Fugir()
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

}
