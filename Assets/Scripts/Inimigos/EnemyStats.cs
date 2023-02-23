using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStats : MonoBehaviour
{

    public float vida = 100, damage = 10;
    public bool isDead = false;
    [SerializeField] public List<Item.ItemDropStruct> dropsItems;

    [SerializeField] [HideInInspector] Animator animator;
    [SerializeField] [HideInInspector] NavMeshAgent navMeshAgent;

    private void Start()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    public void TakeDamage(float damage)
    {
        vida -= damage;
        Debug.Log("Vida enemy: " + vida);
        if (vida <= 0)
        {
            animator.SetBool("isDead", true);
            animator.SetBool("Attacking", false);
            navMeshAgent.isStopped = true;
            isDead = true;
        }
        else
        {
            animator.SetTrigger("Hit");
        }
    }

}
