using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public float gameHour = 12f;  // Define a hora atual do jogo
    public int gameDay = 1;  // Define o dia atual do jogo
    public float gameSpeed = 60f;  // Define a velocidade do tempo do jogo (em segundos do mundo real)
    private float elapsedTime = 0f;  // Tempo que passou desde o início do jogo

    public bool isNoite = false;

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;  // Adiciona o tempo do mundo real que passou desde o último frame

        if (elapsedTime >= gameSpeed)
        {
            // Calcula o número de horas fictícias que passaram
            int hoursPassed = Mathf.FloorToInt(elapsedTime / gameSpeed);
            gameHour += hoursPassed;

            // Atualiza o tempo restante que não foi considerado no último frame
            elapsedTime -= hoursPassed * gameSpeed;

            // Verifica se o dia do jogo precisa ser atualizado
            if (gameHour >= 24f)
            {
                gameHour -= 24f;
                gameDay++;
            }

            isNoite = gameHour >= 18;

            // Exibe o horário fictício atual do jogo
            Debug.Log("Dia " + gameDay + " - Hora " + Mathf.FloorToInt(gameHour) + ":00");
        }
    }

   

}
