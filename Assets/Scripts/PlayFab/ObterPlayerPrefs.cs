using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ObterPlayerPrefs : MonoBehaviour
{

    public TMP_Text[] txPlayerGems, txPlayerTrofeus;

    private void Awake()
    {
        PlayerPrefs.SetInt("PERSONAGEM0", 1); //liberando o personagem 0 Male
        PlayerPrefs.SetInt("PERSONAGEM1", 1); //liberando o personagem 1 Female
    }
   
    private void LateUpdate() //para otimizar, alterar para o metodo a cada chamada de botao
    {
        string qtdGemas = PlayerPrefs.GetInt("GEMS") + "";
        string qtdTrofeus = PlayerPrefs.GetInt("TROFEUS") + "";
        foreach (TMP_Text textGems in txPlayerGems)
        {
            textGems.text = qtdGemas;
        }
        foreach (TMP_Text textTrofeus in txPlayerTrofeus)
        {
            textTrofeus.text = qtdTrofeus;
        }
    }
}
