using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class EmotesController : MonoBehaviour
{

    [SerializeField] Animator animator;
    [SerializeField] PlayerController playerController;

    private void Update()
    {
        if (!playerController.podeSeMexer()) return;

        /*if (Input.GetKeyDown(KeyCode.F1)) { desativarAnimacoesDeBraco("animationF1"); animator.SetBool("animationF1", !animator.GetBool("animationF1")); }
        else if (Input.GetKeyDown(KeyCode.F2)) { desativarAnimacoesDeBraco("animationF2"); animator.SetBool("animationF2", !animator.GetBool("animationF2")); }
        else if (Input.GetKeyDown(KeyCode.F3)) { desativarAnimacoesDePerna("animationF3"); animator.SetBool("animationF3", !animator.GetBool("animationF3")); }
        else if (Input.GetKeyDown(KeyCode.F4)) { desativarAnimacoesDeBraco("animationF4"); animator.SetBool("animationF4", !animator.GetBool("animationF4")); }
        else if (Input.GetKeyDown(KeyCode.F5)) { desativarAnimacoesDeBraco("animationF5"); animator.SetBool("animationF5", !animator.GetBool("animationF5")); }
        else if (Input.GetKeyDown(KeyCode.F6)) { desativarAnimacoesDeBraco("animationF6"); animator.SetBool("animationF6", !animator.GetBool("animationF6")); }*/

        if (!animator.GetBool("isPlayerParado"))
        {
            desativarAnimacoesDePerna("");
        }
        if (animator.GetBool("isPlayerArmado"))
        {
            desativarAnimacoesDeBraco("");
        }
        
    }

    private void desativarAnimacoesDePerna(string animacaoAtivando)
    {
        if (animacaoAtivando != "animationF3") animator.SetBool("animationF3", false);
    }

    private void desativarAnimacoesDeBraco(string animacaoAtivando)
    {
        if(animacaoAtivando != "animationF1") animator.SetBool("animationF1", false);
        if (animacaoAtivando != "animationF2") animator.SetBool("animationF2", false);
        if (animacaoAtivando != "animationF4") animator.SetBool("animationF4", false);
        if (animacaoAtivando != "animationF5") animator.SetBool("animationF5", false);
        if (animacaoAtivando != "animationF6") animator.SetBool("animationF6", false);
    }

}
