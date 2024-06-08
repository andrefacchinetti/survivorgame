using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OverlayController : MonoBehaviour
{
    [HideInInspector] StatsJogador statsJogador;

    [SerializeField] RawImage damageOverlayImg;
    [SerializeField] GameObject bloodOverlayObj, abstinenciaOverlayObj;
    [SerializeField] float durationFading = 5f;

    private Coroutine damageCoroutine;

    private void Awake()
    {
        statsJogador = GetComponent<StatsJogador>();
    }

    public void TakeDamageOverlay(float vidaAtual, float vidaMaxima)
    {
        if (damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine);
        }
        damageCoroutine = StartCoroutine(ShowDamageOverlay(vidaAtual, vidaMaxima));
    }

    private IEnumerator ShowDamageOverlay(float vidaAtual, float vidaMaxima)
    {
        Color splatterAlpha = damageOverlayImg.color;
        splatterAlpha.a = 1 - (vidaAtual / vidaMaxima);
        damageOverlayImg.color = splatterAlpha;

        
        float elapsedTime = 0f;

        while (elapsedTime < durationFading)
        {
            elapsedTime += Time.deltaTime;
            splatterAlpha.a = Mathf.Lerp(splatterAlpha.a, 0, elapsedTime / durationFading);
            damageOverlayImg.color = splatterAlpha;
            yield return null;
        }

        splatterAlpha.a = 0;
        damageOverlayImg.color = splatterAlpha;
        damageCoroutine = null;
    }

    public void AtualizarBloodOverlay(bool isSangrando)
    {
        bloodOverlayObj.SetActive(isSangrando);
    }

    public void AtualizarAbstinenciaOverlay(bool isAbstinencia)
    {
        abstinenciaOverlayObj.SetActive(isAbstinencia);
    }
}
