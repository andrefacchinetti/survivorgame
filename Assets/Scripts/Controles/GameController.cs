using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    [SerializeField] Light luzDoSol;
    public float gameHour = 12f;  // Define a hora atual do jogo
    public int gameDay = 1;  // Define o dia atual do jogo
    public float gameSpeed = 60f;  // Define a velocidade do tempo do jogo (em segundos do mundo real)
    private float elapsedTime = 0f;  // Tempo que passou desde o in?cio do jogo

    public bool isNoite = false;

    // Define as cores de sol para diferentes horas do dia
    public Color amanhecer;
    public Color meioDia;
    public Color entardecer;
    public Color noite;

    // Define a intensidade da luz do sol para diferentes horas do dia
    public float intensidadeAmanhecer = 0.5f;
    public float intensidadeMeioDia = 1f;
    public float intensidadeEntardecer = 0.5f;
    public float intensidadeNoite = 0.1f;

    // Hor?rios personalizados
    public float amanhecerHorario = 6f;
    public float meioDiaHorario = 12f;
    public float entardecerHorario = 17f;
    public float noiteHorario = 00f;

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;  // Adiciona o tempo do mundo real que passou desde o ?ltimo frame

        if (elapsedTime >= gameSpeed)
        {
            // Calcula o n?mero de horas fict?cias que passaram
            int hoursPassed = Mathf.FloorToInt(elapsedTime / gameSpeed);
            gameHour += hoursPassed;

            // Atualiza o tempo restante que n?o foi considerado no ?ltimo frame
            elapsedTime -= hoursPassed * gameSpeed;

            // Verifica se o dia do jogo precisa ser atualizado
            if (gameHour >= 24f)
            {
                gameHour -= 24f;
                gameDay++;
            }

            isNoite = gameHour >= 18;

            // Exibe o hor?rio fict?cio atual do jogo
            Debug.Log("Dia " + gameDay + " - Hora " + Mathf.FloorToInt(gameHour) + ":00");

        }

        Color currentColor;

        if (gameHour >= amanhecerHorario && gameHour < meioDiaHorario)
        {
            currentColor = Color.Lerp(amanhecer, meioDia, Mathf.SmoothStep(0f, 1f, (gameHour - amanhecerHorario) / (meioDiaHorario - amanhecerHorario)));
            luzDoSol.intensity = Mathf.Lerp(intensidadeAmanhecer, intensidadeMeioDia, Mathf.SmoothStep(0f, 1f, (gameHour - amanhecerHorario) / (meioDiaHorario - amanhecerHorario)));
        }
        else if (gameHour >= meioDiaHorario && gameHour < entardecerHorario)
        {
            currentColor = Color.Lerp(meioDia, entardecer, Mathf.SmoothStep(0f, 1f, (gameHour - meioDiaHorario) / (entardecerHorario - meioDiaHorario)));
            luzDoSol.intensity = Mathf.Lerp(intensidadeMeioDia, intensidadeEntardecer, Mathf.SmoothStep(0f, 1f, (gameHour - meioDiaHorario) / (entardecerHorario - meioDiaHorario)));
        }
        else if (gameHour >= entardecerHorario && gameHour < noiteHorario)
        {
            currentColor = Color.Lerp(entardecer, noite, Mathf.SmoothStep(0f, 1f, (gameHour - entardecerHorario) / (noiteHorario - entardecerHorario)));
            luzDoSol.intensity = Mathf.Lerp(intensidadeEntardecer, intensidadeNoite, Mathf.SmoothStep(0f, 1f, (gameHour - entardecerHorario) / (noiteHorario - entardecerHorario)));
        }
        else
        {
            currentColor = noite;
            luzDoSol.intensity = intensidadeNoite;
        }

        luzDoSol.color = currentColor;

    }


}
