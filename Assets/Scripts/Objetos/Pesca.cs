using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pesca : MonoBehaviour
{

    [SerializeField] GameObject peixes;
    [SerializeField] public bool isAreaDePescaAtiva = true;
    [SerializeField] float tempoPraReativarAreaDePesca = 60 * 2;

    private void Start()
    {
        AtivarAreaDePesca();
    }

    public void DesativarAreaDePesca()
    {
        isAreaDePescaAtiva = false;
        peixes.SetActive(false);
        StartCoroutine(EsperarParaAtivar());
    }

    public void AtivarAreaDePesca()
    {
        isAreaDePescaAtiva = true;
        peixes.SetActive(true);
    }

    IEnumerator EsperarParaAtivar()
    {
        yield return new WaitForSeconds(tempoPraReativarAreaDePesca);
        AtivarAreaDePesca();
    }

}
