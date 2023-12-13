using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileiraQuebravel : MonoBehaviour
{

    [SerializeField] public int meuIndex;
    [SerializeField] ArvoreQuebravel arvoreQuebravelBase;
    [SerializeField] FileiraQuebravel fileiraAcima;

    [SerializeField] List<DropaRecursosStats> partesArvore;
    public bool jaAtivou = false;

    private void Awake()
    {
        partesArvore = new List<DropaRecursosStats>();
        foreach (DropaRecursosStats dr in this.gameObject.GetComponentsInChildren<DropaRecursosStats>(false))
        {
            if (dr.transform.parent == this.transform)
            {
                partesArvore.Add(dr); 
            }
        }
    }

    public void VerificarSeAtivaGravidadeFileiraDeCima()
    {
        if (maioriaPartesDaFileiraQuebraram())
        {
            if (fileiraAcima != null) arvoreQuebravelBase.ativarFileirasGravidadeApartirDeIndex(meuIndex);
            else arvoreQuebravelBase.arvorePrincipal.rb.isKinematic = false;
        }
    }

    private bool maioriaPartesDaFileiraQuebraram()
    {
        int qtdQuebrados = 0;
        foreach (DropaRecursosStats parteArvore in partesArvore)
        {
            qtdQuebrados++;
            Debug.LogWarning("qtdQuebrados: " + qtdQuebrados);
            if (qtdQuebrados >= partesArvore.Count / 2) return true;
        }
        return false;
    }

    public void AtivarGravidadePartes()
    {
        foreach (DropaRecursosStats parteArvore in partesArvore)
        {
            parteArvore.rb.isKinematic = false;
            parteArvore.isPedacoQuebrado = true;
        }
        jaAtivou = true;
    }

}
