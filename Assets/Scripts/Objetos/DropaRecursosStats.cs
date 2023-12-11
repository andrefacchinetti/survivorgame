using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropaRecursosStats : MonoBehaviour
{

    StatsGeral statsGeral;

    //Opcoes da arvore
    [SerializeField] bool isParteQuebravel = false;
    [SerializeField] DropaRecursosStats arvorePrincipal;
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
            arvorePrincipal.GetComponent<StatsGeral>().isInvulneravel = true;
        }
    }

    public void AcoesTomouDano()
    {
        Debug.Log("dropa recursos tomou dano");
        //TODO: Mostrar dano visual
    }

    public void AcoesMorreu()
    {
        if (isParteQuebravel)
        {
            Debug.Log("parte arvore quebrou");
            isPedacoQuebrado = true;
            if (verificarTodasPartesQuebraram())
            {
                arvorePrincipal.GetComponent<StatsGeral>().isInvulneravel = false;
                arvorePrincipal.GetComponent<Rigidbody>().isKinematic = false;
                arvorePrincipal.GetComponent<Rigidbody>().AddForce(this.transform.forward, ForceMode.Impulse);
            }
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
        foreach(DropaRecursosStats parteArvore in partesArvore)
        {
            if (!parteArvore.isPedacoQuebrado)
            {
                return false;
            }
        }
        return true;
    }

}
