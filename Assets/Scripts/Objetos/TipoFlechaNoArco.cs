using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipoFlechaNoArco : MonoBehaviour
{

    [SerializeField] GameObject objFlechaDeMadeira, objFlechaDeOsso, objFlechaDeMetal;

    public void AtivarTipoFlechaNoArco(Item itemResponse)
    {
        DesativarTipoFlechaNoArco();
        if (itemResponse != null && itemResponse.quantidade > 0)
        {
            if (Item.NomeItem.FlechaDeMadeira.Equals(itemResponse.nomeItem)) objFlechaDeMadeira.SetActive(true);
            if (Item.NomeItem.FlechaDeOsso.Equals(itemResponse.nomeItem)) objFlechaDeOsso.SetActive(true);
            if (Item.NomeItem.FlechaDeMetal.Equals(itemResponse.nomeItem)) objFlechaDeMetal.SetActive(true);
        }
    }

    public void DesativarTipoFlechaNoArco()
    {
        objFlechaDeMadeira.SetActive(false);
        objFlechaDeOsso.SetActive(false);
        objFlechaDeMetal.SetActive(false);
    }

}
