using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class EmotesController : MonoBehaviour
{

    [SerializeField] [HideInInspector] Animator animator;
    [SerializeField] [HideInInspector] PlayerController playerController;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (!playerController.podeSeMexer()) return;

        if (Input.GetKeyDown(KeyCode.F1)) animator.SetTrigger("animationF1");
        else if (Input.GetKeyDown(KeyCode.F2)) animator.SetTrigger("animationF2");
        else if (Input.GetKeyDown(KeyCode.F3)) animator.SetTrigger("animationF3");
        else if (Input.GetKeyDown(KeyCode.F4)) animator.SetTrigger("animationF4");
        else if (Input.GetKeyDown(KeyCode.F5)) animator.SetTrigger("animationF5");
        else if (Input.GetKeyDown(KeyCode.F9))
        {
            Destroy(this.gameObject);
            SceneManager.LoadScene(0);
        }
    }

}
