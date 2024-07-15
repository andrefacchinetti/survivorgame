using UnityEngine;
using UnityEngine.UI;

public class PiscarTransparencia : MonoBehaviour
{
    public RawImage rawImage; // Arraste e solte sua RawImage aqui no inspector.
    public float minAlpha = 0.0f; // Transpar�ncia m�nima.
    public float maxAlpha = 1.0f; // Transpar�ncia m�xima.
    public float speed = 1.0f; // Velocidade da mudan�a.

    private float targetAlpha; // Alpha que estamos tentando alcan�ar.
    private bool increasing = true; // Dire��o da mudan�a.
     
    void Start()
    {
        if (rawImage == null)
        {
            rawImage = GetComponent<RawImage>();
        }

        if (rawImage == null)
        {
            enabled = false;
            return;
        }

        targetAlpha = minAlpha;
    }

    void Update()
    {
        if (!transform.gameObject.activeSelf) return;
        Color color = rawImage.color;
        if (increasing)
        {
            color.a += speed * Time.deltaTime;
            if (color.a >= maxAlpha)
            {
                color.a = maxAlpha;
                increasing = false;
            }
        }
        else
        {
            color.a -= speed * Time.deltaTime;
            if (color.a <= minAlpha)
            {
                color.a = minAlpha;
                increasing = true;
            }
        }
        rawImage.color = color;
    }
}
