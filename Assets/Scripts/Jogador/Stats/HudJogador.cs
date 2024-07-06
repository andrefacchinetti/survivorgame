using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HudJogador : MonoBehaviour
{

    public RawImage imgVida, imgFome, imgSede, imgEnergia; //Stats Principais
    public GameObject imgAbstinencia, imgFraturado, imgSangrando;
    public GameObject imgIndigestao, imgInfeccionado;
    public GameObject objArmor, objHipertermia, objHipotermia;
    public TMP_Text txtArmor, txtTemperatura;

    public Texture[] listImgsVida, listImgsFome, listImgsSede, listImgsEnergia;

    private int obterIndexPorPorcentagem(float porcentagem)
    {
        if (porcentagem <= 10) return 0;
        else if (porcentagem <= 20) return 1;
        else if (porcentagem <= 50) return 2;
        else if (porcentagem <= 90) return 3;
        else return 4;
    }
    
    public void atualizarImgVida(float vidaAtual, float vidaMaxima)
    {
        float porcentagemVida =  vidaAtual / vidaMaxima * 100;
        int index = obterIndexPorPorcentagem(porcentagemVida);
        imgVida.texture = listImgsVida[index];
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
        float porcentagem = atual / maxima * 100;
        int index = obterIndexPorPorcentagem(porcentagem);
        imgFome.texture = listImgsFome[index];
    }

    public void atualizarImgSede(float atual, float maxima)
    {
        float porcentagem = atual / maxima * 100;
        int index = obterIndexPorPorcentagem(porcentagem);
        imgSede.texture = listImgsSede[index];
    }

    public void atualizarImgEnergia(float atual, float maxima)
    {
        float porcentagem = atual / maxima * 100;
        int index = obterIndexPorPorcentagem(porcentagem);
        imgEnergia.texture = listImgsEnergia[index];
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
