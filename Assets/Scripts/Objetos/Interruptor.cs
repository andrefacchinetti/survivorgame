using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interruptor : MonoBehaviour
{
    [SerializeField] CentralInterruptores centralInterruptores;
    [SerializeField] MeshRenderer renderOn, renderOff;
    [SerializeField] Material materialOn, materialOff, materialInativo;
    [SerializeField] Animation animation;
    public bool isAtivado = false;
    [SerializeField] int index;

    [SerializeField] Light[] luzes;

    private void Start()
    {
        DesligarInterruptor(); // Inicialmente, desligamos o interruptor
    }

    public void ToggleInterruptor()
    {
        if (isAtivado) DesligarInterruptor();
        else LigarInterruptor();
        centralInterruptores.AplicandoRegrasDoEnigma(index);
    }

    public void LigarInterruptor()
    {
        renderOn.material = materialOn;
        renderOff.material = materialInativo;
        isAtivado = true;
        animation.Play("alavancaInterruptorON");
        foreach(Light luz in luzes)
        {
            luz.enabled = true;
        }
    }

    public void DesligarInterruptor()
    {
        renderOn.material = materialInativo;
        renderOff.material = materialOff;
        isAtivado = false;
        animation.Play("alavancaInterruptorOFF");
        foreach (Light luz in luzes)
        {
            luz.enabled = false;
        }
    }

}
