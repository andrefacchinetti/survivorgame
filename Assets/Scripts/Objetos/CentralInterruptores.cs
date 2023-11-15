using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentralInterruptores : MonoBehaviour
{
    [SerializeField] Interruptor[] interruptores;

    int A = 0, B = 1, C = 2, D = 3;

    public void AplicandoRegrasDoEnigma(int indexQuemAlterou)
    {
        // Regra 1: Ao ativar A (index 0), desativar D (index 3)
        if (indexQuemAlterou == A)
        {
            interruptores[D].DesligarInterruptor();
        }
        // Regra 2: Se B (index 1) estiver ligado, desativar C (index 2)
        else if (indexQuemAlterou == B)
        {
            interruptores[C].DesligarInterruptor();
        }
        // Regra 3: Se C (index 2) estiver ligado, inverter o estado de A (index 0)
        else if (indexQuemAlterou == C)
        {
            if (interruptores[A].isAtivado)
            {
                interruptores[A].DesligarInterruptor();
            }
            else
            {
                interruptores[A].LigarInterruptor();
            }
        }
        // Regra 4: Se D (index 3) estiver desativado, inverter o estado de B (index 1)
        else if (indexQuemAlterou == D && !interruptores[D].isAtivado)
        {
            if (interruptores[B].isAtivado)
            {
                interruptores[B].DesligarInterruptor();
            }
            else
            {
                interruptores[B].LigarInterruptor();
            }
        }
        // Regra 5: Se A (index 0) estiver ligado, desativar B (index 1)
        else if (indexQuemAlterou == A && interruptores[A].isAtivado)
        {
            interruptores[B].DesligarInterruptor();
        }

        // Verifique se todas as condições foram atendidas após cada mudança
        VerificarSolucao();
    }

    private void VerificarSolucao()
    {
        // Verifique se todos os interruptores estão ligados
        bool todosLigados = true;
        foreach (var interruptor in interruptores)
        {
            if (!interruptor.isAtivado)
            {
                todosLigados = false;
                break;
            }
        }

        // Se todos estiverem ligados, a solução foi alcançada
        if (todosLigados)
        {
            Debug.Log("Parabéns! Você resolveu o enigma!");
        }
    }
}
