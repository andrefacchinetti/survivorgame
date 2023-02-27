using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotesController : MonoBehaviour
{

    [SerializeField] Animator animator;
    [SerializeField] PlayerMovement playerMoviment;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerMoviment = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (!playerMoviment.canMove || playerMoviment.playerController.isMorto) return;

        if (Input.GetKeyDown(KeyCode.F1)) animator.SetTrigger("animationF1");
        else if (Input.GetKeyDown(KeyCode.F2)) animator.SetTrigger("animationF2");
        else if (Input.GetKeyDown(KeyCode.F3)) animator.SetTrigger("animationF3");
    }

}
