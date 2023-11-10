using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animation))]
public class ToggleAnimationObjeto : MonoBehaviour
{

    public bool isAtivado = false, isAtivaApenasUmaVez = false;
    [SerializeField] Animation animation;
    [SerializeField] AnimationClip clipOn, clipOff;

    private void Awake()
    {
        this.gameObject.tag = "ToggleAnimationObjeto";
        this.gameObject.layer = 11;
    }

    public void AtivarToggleAnimacaoObjeto()
    {
        if(!isAtivado || !isAtivaApenasUmaVez)
        {
            if (!isAtivado)
            {
                animation.Play(clipOn.name);
            }
            else
            {
                animation.Play(clipOff.name);
            }
            isAtivado = !isAtivado;
        }
    }

}
