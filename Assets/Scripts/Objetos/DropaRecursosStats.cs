using System.Collections;
using System.Collections.Generic;
using Opsive.UltimateCharacterController.Traits;
using UnityEngine;

public class DropaRecursosStats : MonoBehaviour
{

    StatsGeral statsGeral;

    //Opcoes da arvore
    [SerializeField] bool isParteQuebravel = false;
    [SerializeField] DropaRecursosStats arvorePrincipal;
    [SerializeField] GameObject contentQuebraveis;
    [SerializeField] List<DropaRecursosStats> partesArvore;
    [HideInInspector] public bool isPedacoQuebrado = false;

    private void Awake()
    {
        statsGeral = GetComponent<StatsGeral>();
    }

    private void Start()
    {
        if(arvorePrincipal != null)
        {
            arvorePrincipal.GetComponent<Health>().Invincible = true;
        }
        partesArvore = new List<DropaRecursosStats>();
        foreach (DropaRecursosStats gameObject in contentQuebraveis.GetComponentsInChildren<DropaRecursosStats>())
        {
            partesArvore.Add(gameObject);
        }
    }

    public void AcoesTomouDano()
    {
        Debug.Log("dropa recursos tomou dano");
        if (isParteQuebravel)
        {
            Rigidbody rbParte = GetComponent<Rigidbody>();
            rbParte.isKinematic = false;
            //rbParte.AddForce(transform.forward * forcaEmpurraArvore, ForceMode.Impulse);
            Rigidbody rbArvore = arvorePrincipal.GetComponent<Rigidbody>();
            rbArvore.isKinematic = false;
        }
        //TODO: Mostrar dano visual
    }

    public float forcaEmpurraArvore = 2;
    public void AcoesMorreu()
    {
        if (isParteQuebravel)
        {
            Debug.Log("parte arvore quebrou");
            isPedacoQuebrado = true;
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

    private bool verificarTodasPartesQuebraram()
    {
        int qtdQuebrados = 0;
        foreach(DropaRecursosStats parteArvore in arvorePrincipal.partesArvore)
        {
            if (parteArvore.isPedacoQuebrado)
            {
                qtdQuebrados++;
                if (qtdQuebrados >= arvorePrincipal.partesArvore.Count / 2) return true;
            }
        }
        return false;
    }

}
