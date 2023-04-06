using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaDeColeta : MonoBehaviour
{
    [SerializeField] GameObject objColetavel;
    [SerializeField] public bool isAreaAtiva = true;
    [SerializeField] float tempoPraReativarArea = 60 * 2;
    [SerializeField] public Item.NomeItem itemColetavel;

    private void Start()
    {
        AtivarArea();
    }

    public void DesativarArea()
    {
        isAreaAtiva = false;
        objColetavel.SetActive(false);
        StartCoroutine(EsperarParaAtivar());
    }

    public void AtivarArea()
    {
        isAreaAtiva = true;
        objColetavel.SetActive(true);
    }

    IEnumerator EsperarParaAtivar()
    {
        yield return new WaitForSeconds(tempoPraReativarArea);
        AtivarArea();
    }
}
