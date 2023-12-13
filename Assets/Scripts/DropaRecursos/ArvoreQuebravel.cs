using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArvoreQuebravel : MonoBehaviour
{

    public FaseArvore faseArvore;
    public DropaRecursosStats arvorePrincipal;
    public GameObject objArvoreInteira, objArvoreRachada, objArvorePedacos;
    public FileiraQuebravel[] fileirasQuebraveis;
    
    public enum FaseArvore
    {
        Inteira,
        Rachada,
        Pedacos
    }

    private void Awake()
    {
        SetarFaseArvore(faseArvore);
    }

    public void SetarFaseArvore(FaseArvore fase)
    {
        objArvoreInteira.SetActive(FaseArvore.Inteira.Equals(fase));
        objArvoreRachada.SetActive(FaseArvore.Rachada.Equals(fase));
        objArvorePedacos.SetActive(FaseArvore.Pedacos.Equals(fase));
    }

    public float forcaDeQueda = 1000f;
    public void ativarFileirasGravidadeApartirDeIndex(int index)
    {
        for(int i=0;i < fileirasQuebraveis.Length; i++)
        {
            if (fileirasQuebraveis[i].meuIndex >= index)
            {
                if (!fileirasQuebraveis[i].jaAtivou)
                {
                    fileirasQuebraveis[i].AtivarGravidadePartes();
                }
            }
        }
        arvorePrincipal.rb.isKinematic = false;
        arvorePrincipal.rb.AddForce(Vector3.forward * forcaDeQueda);
    }

}
