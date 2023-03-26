using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipoFlechaNoArco : MonoBehaviour
{

    [SerializeField] GameObject objFlechaDeMadeira, objFlechaDeOsso, objFlechaDeMetal;
    [SerializeField] Armaduras armaduras;

    public void AtivarTipoFlechaNoArco()
    {
        DesativarTipoFlechaNoArco();
        Item flechaNaAljava = armaduras.ObterItemFlechaNaAljava();
        if (flechaNaAljava != null && flechaNaAljava.quantidade > 0)
        {
            if (Item.NomeItem.FlechaDeMadeira.Equals(flechaNaAljava.nomeItem)) objFlechaDeMadeira.SetActive(true);
            if (Item.NomeItem.FlechaDeOsso.Equals(flechaNaAljava.nomeItem)) objFlechaDeOsso.SetActive(true);
            if (Item.NomeItem.FlechaDeMetal.Equals(flechaNaAljava.nomeItem)) objFlechaDeMetal.SetActive(true);
        }
    }

    public void DesativarTipoFlechaNoArco()
    {
        objFlechaDeMadeira.SetActive(false);
        objFlechaDeOsso.SetActive(false);
        objFlechaDeMetal.SetActive(false);
    }

}
