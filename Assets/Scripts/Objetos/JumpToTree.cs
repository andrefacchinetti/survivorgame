using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class JumpToTree : MonoBehaviour
{
    public Transform treeDestination;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Lobisomem")
        {
            Debug.Log("jump to tree");
            other.gameObject.GetComponent<NavMeshAgent>().SetDestination(treeDestination.position);
        }
    }

}
