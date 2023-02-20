using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class Settings : MonoBehaviour
{

    [SerializeField] TraducoesMenu traducoesMenu;
    [SerializeField] TraducoesGame traducoesGame;
    [SerializeField] GameObject panelSettingsGame;
    public RawImage imageIdioma;
    public Texture[] imagensIdioma; // 0 = Ingles, 1 = Portugues-BR
    public Slider volumeSlider;
    public AudioMixer audioMixer;

    void Start()
    {
       if (PlayerPrefs.GetInt("INDEXIDIOMA") == 1)
       {
           imageIdioma.texture = imagensIdioma[1];
       }
       else
       {
           imageIdioma.texture = imagensIdioma[0];
       }
        volumeSlider.value = PlayerPrefs.GetFloat("volume");
    }

    public void ChangeImageIdioma()
    {
        int indexIdioma = PlayerPrefs.GetInt("INDEXIDIOMA") + 1;
        if (indexIdioma > imagensIdioma.Length - 1) indexIdioma = 0;
        PlayerPrefs.SetInt("INDEXIDIOMA", indexIdioma);
        atualizarImagemIdioma();
    }

    private void atualizarImagemIdioma()
    {
        if (PlayerPrefs.GetInt("INDEXIDIOMA") == 0)
        {
            imageIdioma.texture = imagensIdioma[0];
        }
        else if (PlayerPrefs.GetInt("INDEXIDIOMA") == 1)
        {
            imageIdioma.texture = imagensIdioma[1];
        }
        if(traducoesMenu != null) traducoesMenu.CarregarTraducoes();
        if(traducoesGame != null) traducoesGame.CarregarTraducoes();
    }

    public void setVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
        PlayerPrefs.SetFloat("volume", volume);
        volumeSlider.value = volume;
    }

    public void AbrirPanelSettingsGame()
    {
        panelSettingsGame.SetActive(true);
    }

    public void FecharPanelSettingsGame()
    {
        panelSettingsGame.SetActive(false);
    }

}
