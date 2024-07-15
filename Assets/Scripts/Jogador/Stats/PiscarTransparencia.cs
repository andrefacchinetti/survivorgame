using UnityEngine;
using UnityEngine.UI;

public class PiscarTransparencia : MonoBehaviour
{
    public RawImage rawImage; // Arraste e solte sua RawImage aqui no inspector.
    public float minAlpha = 0.0f; // Transparência mínima.
    public float maxAlpha = 1.0f; // Transparência máxima.
    public float speed = 1.0f; // Velocidade da mudança.

    private float targetAlpha; // Alpha que estamos tentando alcançar.
    private bool increasing = true; // Direção da mudança.
     
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
