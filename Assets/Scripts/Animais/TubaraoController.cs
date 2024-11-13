using System.Collections;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(PhotonView), typeof(StatsGeral))]
[RequireComponent(typeof(AnimalStats))]
public class TubaraoController : MonoBehaviourPunCallbacks
{
    public bool isSharkAggressive;
    public float detectionRadius = 20f;
    public float swimSpeed = 3f;
    public float chaseSpeed = 7f;
    public float rotationSpeed = 2f;
    public float attackDistance = 2f;
    public float patrolWaitTime = 3f;

    [SerializeField] private StatsGeral playerTarget;
    [SerializeField] private GameObject playerObject;

    private bool isChasing = false;
    private bool isPatrolling = true;
    private PhotonView PV;
    private StatsGeral statsGeral;
    private AnimalStats animalStats;
    public Animator animator;

    private Vector3 patrolTarget;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        statsGeral = GetComponent<StatsGeral>();
        animalStats = GetComponent<AnimalStats>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        StartCoroutine(DetectionCoroutine());
        StartPatrolling();
    }

    void Update()
    {
        if (statsGeral.health.IsAlive())
        {
            if (isChasing && playerTarget != null)
            {
                ChasePlayer();
            }
            else
            {
                Patrol();
            }
            UpdateAnimation();
            CheckAttack();
        }
        else
        {
            animator.SetBool("isDead", true);
            playerTarget = null;
        }
    }

    private IEnumerator DetectionCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            DetectPlayer();
        }
    }

    private void DetectPlayer()
    {
        int layerMask = LayerMask.GetMask("SubCharacter");
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius, layerMask);

        foreach (var hitCollider in hitColliders)
        {
            if (playerTarget != null || !statsGeral.health.IsAlive() || animalStats.estaFugindo) return;
            playerTarget = hitCollider.GetComponentInParent<StatsGeral>();
            playerObject = hitCollider.gameObject;
            isChasing = true;
            isPatrolling = false;
            break;
        }
    }

    private void ChasePlayer()
    {
        if (playerTarget == null || !playerTarget.health.IsAlive())
        {
            isChasing = false;
            isPatrolling = true;
            return;
        }

        Vector3 direction = (playerObject.transform.position - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, playerObject.transform.position);

        // Rotaciona o tubarão em direção ao jogador
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Define a velocidade do tubarão com base na distância do jogador
        float currentSpeed = (distance <= attackDistance) ? 0 : chaseSpeed;
        transform.position += direction * currentSpeed * Time.deltaTime;
    }

    private void Patrol()
    {
        if (!isPatrolling) return;

        float distanceToTarget = Vector3.Distance(transform.position, patrolTarget);

        if (distanceToTarget <= 1f)
        {
            StartCoroutine(WaitAndSetNewPatrolTarget());
        }
        else
        {
            Vector3 direction = (patrolTarget - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            transform.position += direction * swimSpeed * Time.deltaTime;
            animator.SetBool("isMoving", true);
        }
    }

    private IEnumerator WaitAndSetNewPatrolTarget()
    {
        isPatrolling = false;
        animator.SetBool("isMoving", false);
        yield return new WaitForSeconds(patrolWaitTime);
        SetNewPatrolTarget();
        isPatrolling = true;
    }

    private void SetNewPatrolTarget()
    {
        Vector3 randomDirection = Random.insideUnitSphere * detectionRadius;
        randomDirection += transform.position;
        randomDirection.y = transform.position.y; // Mantém o tubarão na mesma profundidade
        patrolTarget = randomDirection;
    }

    private void StartPatrolling()
    {
        SetNewPatrolTarget();
        isPatrolling = true;
    }

    private void UpdateAnimation()
    {
        float currentSpeed = (isChasing && playerTarget != null) ? chaseSpeed : swimSpeed;

        if (currentSpeed >= chaseSpeed - animalStats.speedVariation)
        {
            animator.SetBool("isMoving", false);
            animator.SetBool("run", true);
        }
        else if (currentSpeed > 0.05f)
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

    private void CheckAttack()
    {
        if (playerTarget != null && Vector3.Distance(transform.position, playerObject.transform.position) <= attackDistance)
        {
            animator.SetTrigger("isAttacking");
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
