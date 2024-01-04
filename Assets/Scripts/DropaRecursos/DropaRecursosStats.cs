using System.Collections;
using System.Collections.Generic;
using Opsive.UltimateCharacterController.Traits;
using UnityEngine;

public class DropaRecursosStats : MonoBehaviour
{

    StatsGeral statsGeral;

    //Opcoes da arvore
    [SerializeField] bool isParteQuebravel = false, souObjInteiro = false, souObjRachado = false, souObjNucleoQuebravel = false;
    [SerializeField] ArvoreQuebravel arvoreQuebravelBase;
    [SerializeField] public FileiraQuebravel minhaFileira;
    [HideInInspector] public bool isPedacoQuebrado = false;
    [HideInInspector] public Rigidbody rb;

    private void Awake()
    {
        statsGeral = GetComponent<StatsGeral>();
        rb = GetComponent<Rigidbody>();
    }

    public void AcoesTomouDano()
    {
        Debug.Log("dropa recursos tomou dano");
        //AcoesPartesArvoreTomamDano();
        //TODO: Mostrar dano visual
    }

    public float forcaEmpurraArvore = 2;
    public void AcoesMorreu()
    {
        Debug.Log("dropa recursos morreu");
        AcoesPartesArvoreTomamDano();

        statsGeral.DroparItensAoMorrer();
        statsGeral.DestruirGameObject();
    }

    private void AcoesPartesArvoreTomamDano()
    {
        if (isParteQuebravel)
        {
            isPedacoQuebrado = true; 
            rb.isKinematic = false;
            minhaFileira.VerificarSeAtivaGravidadeFileiraDeCima();
        }
        else if (souObjInteiro)
        {
            arvoreQuebravelBase.SetarFaseArvore(ArvoreQuebravel.FaseArvore.Rachada);
        }
        else if (souObjRachado)
        {
            arvoreQuebravelBase.SetarFaseArvore(ArvoreQuebravel.FaseArvore.Pedacos);
        }
        else if (souObjNucleoQuebravel)
        {
            Rigidbody rbArvore = arvoreQuebravelBase.arvorePrincipal.GetComponent<Rigidbody>();
            rbArvore.isKinematic = false;
            gameObject.SetActive(false);
        }
    }

}
