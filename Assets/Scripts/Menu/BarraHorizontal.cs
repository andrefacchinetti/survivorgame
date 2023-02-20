using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarraHorizontal : MonoBehaviour
{
    [SerializeField] private RectTransform barra;

    [SerializeField] private float tamanhoMaximo;
    private float tamanhoAtual;

    private float valorMaximo;

    public void DefinirValorMaximo(float _valorMaximo)
    {
        valorMaximo = _valorMaximo;
    }

    public void AtualizarBarra(float _valorAtual)
    {
        tamanhoAtual = _valorAtual * tamanhoMaximo / valorMaximo;
        barra.gameObject.GetComponent<Image>().fillAmount = tamanhoAtual;
    }
}
