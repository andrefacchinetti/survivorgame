using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArvoreQuebravel : MonoBehaviour
{

    public FaseArvore faseArvore;
    public DropaRecursosStats arvorePrincipal;
    public GameObject objArvoreInteira, objArvoreRachada, objArvorePedacos;
    [HideInInspector] List<DropaRecursosStats> partesArvore;
    public enum FaseArvore
    {
        Inteira,
        Rachada,
        Pedacos
    }

    private void Awake()
    {
        SetarFaseArvore(faseArvore);

        partesArvore = new List<DropaRecursosStats>();
        foreach (DropaRecursosStats dr in objArvorePedacos.GetComponentsInChildren<DropaRecursosStats>())
        {
            partesArvore.Add(dr);
        }
    }

    public void SetarFaseArvore(FaseArvore fase)
    {
        objArvoreInteira.SetActive(FaseArvore.Inteira.Equals(fase));
        objArvoreRachada.SetActive(FaseArvore.Rachada.Equals(fase));
        objArvorePedacos.SetActive(FaseArvore.Pedacos.Equals(fase));
    }

    private bool verificarPercentualPartesQuebraram()
    {
        int qtdQuebrados = 0;
        foreach (DropaRecursosStats parteArvore in partesArvore)
        {
            if (parteArvore.isPedacoQuebrado)
            {
                qtdQuebrados++;
                if (qtdQuebrados >= partesArvore.Count / 2) return true;
            }
        }
        return false;
    }

}
