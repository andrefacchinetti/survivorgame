using System.Collections;
using System.Collections.Generic;
using Opsive.UltimateCharacterController.Traits;
using UnityEngine;

public class DropaRecursosStats : MonoBehaviour
{

    StatsGeral statsGeral;

    //Opcoes da arvore
    [SerializeField] bool isParteQuebravel = false, souObjInteiro = false, souObjRachado = false;
    [SerializeField] ArvoreQuebravel arvoreQuebravelBase;
    [HideInInspector] public bool isPedacoQuebrado = false;

    private void Awake()
    {
        statsGeral = GetComponent<StatsGeral>();
    }

    private void Start()
    {
        if(arvoreQuebravelBase != null)
        {
            arvoreQuebravelBase.arvorePrincipal.GetComponent<Health>().Invincible = true;
        }
       
    }

    public void AcoesTomouDano()
    {
        Debug.Log("dropa recursos tomou dano");
        AcoesPartesArvoreTomamDano();
        //TODO: Mostrar dano visual
    }

    public float forcaEmpurraArvore = 2;
    public void AcoesMorreu()
    {
        AcoesPartesArvoreTomamDano();
        if (isParteQuebravel)
        {
            Debug.Log("parte arvore quebrou");
            isPedacoQuebrado = true;
            //TODO: SUMIR PEDACO DEPOIS DE UM TEMPO
            /*if (verificarTodasPartesQuebraram())
            {
                arvorePrincipal.GetComponent<Health>().Invincible = false;
                Rigidbody rbArvore = arvorePrincipal.GetComponent<Rigidbody>();
                rbArvore.isKinematic = false;
                Vector3 direcao = (rbArvore.transform.position - transform.position).normalized;
                rbArvore.AddForce(direcao * forcaEmpurraArvore, ForceMode.Impulse);
            }*/
            //this.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("dropa recursos morreu");
            statsGeral.DroparItensAoMorrer();
            statsGeral.DestruirGameObject();
        }
    }

    private void AcoesPartesArvoreTomamDano()
    {
        if (isParteQuebravel)
        {
            Rigidbody rbParte = GetComponent<Rigidbody>();
            rbParte.isKinematic = false;
            rbParte.drag = 5; 
            //rbParte.AddForce(transform.forward * forcaEmpurraArvore, ForceMode.Impulse);
            Rigidbody rbArvore = arvoreQuebravelBase.arvorePrincipal.GetComponent<Rigidbody>();
            rbArvore.isKinematic = false;
        }
        else if (souObjInteiro)
        {
            arvoreQuebravelBase.SetarFaseArvore(ArvoreQuebravel.FaseArvore.Rachada);
        }
        else if (souObjRachado)
        {
            arvoreQuebravelBase.SetarFaseArvore(ArvoreQuebravel.FaseArvore.Pedacos);
        }
    }

}
