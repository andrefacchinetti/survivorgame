using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DuloGames.UI;

public class HudJogador : MonoBehaviour
{

    public Image imgVida, imgFome, imgSede, imgEnergia; //Stats Principais
    public UIProgressBar barraFolego;
    public GameObject hudFolego;
    public GameObject imgAbstinencia, imgFraturado, imgSangrando;
    public GameObject imgIndigestao, imgInfeccionado;
    public GameObject objArmor, objHipertermia, objHipotermia;
    
    public TMP_Text txtArmor, txtTemperatura;
    
    public void atualizarImgVida(float vidaAtual, float vidaMaxima)
    {
        imgVida.fillAmount = vidaAtual / vidaMaxima;
    }

    public void atualizarImgFolego(float atual, float maximo)
    {
        barraFolego.fillAmount = atual / maximo;
    }

    public void atualizarImgArmor(float atual)
    {
        objArmor.SetActive(atual > 0);
        txtArmor.text = atual + "";
    }
    public void atualizarImgTemperatura(bool isHipotermia, bool isHipertermia, float atual)
    {
        objHipotermia.SetActive(isHipotermia);
        objHipertermia.SetActive(isHipertermia);
        txtTemperatura.text = atual + "";
    }

    public void atualizarImgFome(float atual, float maxima)
    {
        imgFome.fillAmount = atual / maxima;
    }

    public void atualizarImgSede(float atual, float maxima)
    {
        imgSede.fillAmount = atual / maxima;
    }

    public void atualizarImgEnergia(float atual, float maxima)
    {
        imgEnergia.fillAmount = atual / maxima;
    }

    public void atualizarImgAbstinencia(bool isAbstinencia)
    {
        imgAbstinencia.SetActive(isAbstinencia);
    }
    public void atualizarImgFraturado(bool isFraturado)
    {
        imgFraturado.SetActive(isFraturado);
    }
    public void atualizarImgSangrando(bool isSangrando)
    {
        imgSangrando.SetActive(isSangrando);
    }

    public void atualizarImgIndigestao(bool isIndigestao)
    {
        imgIndigestao.SetActive(isIndigestao);
    }

    public void atualizarImgInfeccionado(bool isInfeccionado)
    {
        imgInfeccionado.SetActive(isInfeccionado);
    }

}
