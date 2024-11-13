using System.Collections;
using UnityEngine;
using Photon.Pun;
using Crest;

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
    public float timeOutsidePatrolAreaLimit = 5f; // Tempo limite fora da área antes de morrer
    public BoxCollider patrolArea;

    [HideInInspector] public GameController gameController;
    [HideInInspector] private StatsGeral playerTarget;
    [HideInInspector] private GameObject playerObject;
    [HideInInspector] public Animator animator;

    private bool isChasing = false;
    private bool isPatrolling = true;
    private bool isOutsidePatrolArea = false;
    private float timeOutsidePatrolArea = 0f; // Temporizador de permanência fora da área

    private PhotonView PV;
    private StatsGeral statsGeral;
    private AnimalStats animalStats;

    private Vector3 patrolTarget;
    private SampleHeightHelper sampleHeightHelper;
    public float swimAntiBug = 1.0f;
    private float waterHeight;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        statsGeral = GetComponent<StatsGeral>();
        animalStats = GetComponent<AnimalStats>();
        animator = GetComponent<Animator>();
        sampleHeightHelper = new SampleHeightHelper();
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
            CheckPatrolArea();

            // Mantém o tubarão abaixo da altura do oceano
            RestrictSharkBelowWater();

            if (!isOutsidePatrolArea) // Somente permite ações se o tubarão estiver dentro da área
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
        }
        else
        {
            animator.SetBool("isDead", true);
            playerTarget = null;
        }
    }

    private void RestrictSharkBelowWater()
    {
        if (verificarSharkUltrapassouAlturaOceano())
        {
            Vector3 position = transform.position;
            position.y = Mathf.Min(position.y, waterHeight - swimAntiBug);
            transform.position = position;
        }
    }

    private void CheckPatrolArea()
    {
        if (patrolArea != null && !patrolArea.bounds.Contains(transform.position))
        {
            if (!isOutsidePatrolArea)
            {
                isOutsidePatrolArea = true;
                timeOutsidePatrolArea = 0f; // Reinicia o temporizador ao sair da área
            }

            timeOutsidePatrolArea += Time.deltaTime;

            // Se o tubarão ficar fora da área por mais tempo que o limite, ele morre
            if (timeOutsidePatrolArea >= timeOutsidePatrolAreaLimit)
            {
                DieOutsidePatrolArea();
            }
        }
        else
        {
            // Reseta o estado e temporizador se o tubarão estiver dentro da área
            isOutsidePatrolArea = false;
            timeOutsidePatrolArea = 0f;
        }
    }

    private void DieOutsidePatrolArea()
    {
        statsGeral.TakeDamage(9999, false); // Causa dano suficiente para matar o tubarão
        animator.SetBool("isDead", true);
        isChasing = false;
        isPatrolling = false;
        playerTarget = null;
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
        if (isOutsidePatrolArea) return; // Impede a detecção de jogadores se estiver fora da área

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
        if (isOutsidePatrolArea || playerTarget == null || !playerTarget.health.IsAlive())
        {
            isChasing = false;
            isPatrolling = true;
            return;
        }

        Vector3 direction = (playerObject.transform.position - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, playerObject.transform.position);

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        float currentSpeed = (distance <= attackDistance) ? 0 : chaseSpeed;
        transform.position += direction * currentSpeed * Time.deltaTime;
    }

    private void Patrol()
    {
        if (!isPatrolling || isOutsidePatrolArea) return; // Impede a patrulha se estiver fora da área

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
        if (patrolArea != null)
        {
            Bounds bounds = patrolArea.bounds;
            float patrolX = Random.Range(bounds.min.x, bounds.max.x);
            float patrolZ = Random.Range(bounds.min.z, bounds.max.z);

            patrolTarget = new Vector3(patrolX, transform.position.y, patrolZ);
        }
        else
        {
            patrolTarget = transform.position;
        }
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

    private bool verificarSharkUltrapassouAlturaOceano()
    {
        if (sampleHeightHelper == null) return false;

        Vector3 sharkPosition = transform.position;
        sampleHeightHelper.Init(sharkPosition);

        if (sampleHeightHelper.Sample(out waterHeight))
        {
            return sharkPosition.y > waterHeight - swimAntiBug;
        }
        return false;
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
