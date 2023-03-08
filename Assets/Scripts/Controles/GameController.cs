using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public float gameHour = 12f;  // Define a hora atual do jogo
    public int gameDay = 1;  // Define o dia atual do jogo
    public float gameSpeed = 60f;  // Define a velocidade do tempo do jogo (em segundos do mundo real)
    private float elapsedTime = 0f;  // Tempo que passou desde o in�cio do jogo

    public bool isNoite = false;

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;  // Adiciona o tempo do mundo real que passou desde o �ltimo frame

        if (elapsedTime >= gameSpeed)
        {
            // Calcula o n�mero de horas fict�cias que passaram
            int hoursPassed = Mathf.FloorToInt(elapsedTime / gameSpeed);
            gameHour += hoursPassed;

            // Atualiza o tempo restante que n�o foi considerado no �ltimo frame
            elapsedTime -= hoursPassed * gameSpeed;

            // Verifica se o dia do jogo precisa ser atualizado
            if (gameHour >= 24f)
            {
                gameHour -= 24f;
                gameDay++;
            }

            isNoite = gameHour >= 18;

            // Exibe o hor�rio fict�cio atual do jogo
            Debug.Log("Dia " + gameDay + " - Hora " + Mathf.FloorToInt(gameHour) + ":00");
        }
    }

   

}
