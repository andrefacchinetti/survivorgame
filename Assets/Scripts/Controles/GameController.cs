using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GameController : MonoBehaviour
{

    [SerializeField] Light luzDoSol;
    [SerializeField] GameObject objMoon, pivotDoSol;
    [SerializeField] float moonRotationSpeed = 10f;
    public float multiplicadorVelocidade = 10f;

    public int gameHour = 12;  // Define a hora atual do jogo
    public int gameMinute = 0;  // Define o minuto atual do jogo
    public int gameSecond = 0;  // Define o minuto atual do jogo
    public int gameDay = 1;  // Define o dia atual do jogo
    public float gameSpeed = 1f;  // Define a velocidade do tempo do jogo (Valor padrão = 1. Diminua para ficar mais rapido)
    private float elapsedTime = 0f;  // Tempo que passou desde o in�cio do jogo

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

    // Hor�rios personalizados
    public float amanhecerHorario = 6f;
    public float meioDiaHorario = 12f;
    public float entardecerHorario = 17f;
    public float noiteHorario = 00f;

    [SerializeField] [HideInInspector] SpawnController spawnController;
    float deltaTime = 0.0f;
    private float lastGameDayLobos = -1, lastGameDayAnimais = -1;

    [SerializeField] public GameObject respawnPointJogador;
    [SerializeField] public List<SpawnLoots> listaSpawnLoots;

    [SerializeField] public bool isRespawnarInimigos = true; //DEIXAR TRUE ]
    [HideInInspector] public PhotonView PV;

    private void Awake()
    {
        listaSpawnLoots = new List<SpawnLoots>();
        spawnController = GetComponent<SpawnController>();
        PV = GetComponent<PhotonView>();
    }

    private void Start()
    {
        float mappedHour = Map(gameHour + gameMinute / 60f + gameSecond / 3600f, 4f, 20f, -190f, 20f);
        targetRotation = mappedHour;
        pivotDoSol.transform.rotation = Quaternion.Euler(targetRotation, 0, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

        elapsedTime += Time.deltaTime;

        if (elapsedTime >= gameSpeed)
        {
            gameMinute++;
            if (gameMinute >= 60)
            {
                gameMinute -= 60;
                gameHour++;

                if (gameHour >= 24)
                {
                    gameHour -= 24;
                    gameDay++;
                    spawnarLootsPorDia();
                }
            }
            isNoite = gameHour >= noiteHorario && gameHour <= amanhecerHorario;
            spawnarPorDia();
            elapsedTime = 0;
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

        gameSecond = Mathf.FloorToInt(elapsedTime % 60);
        AtualizarRotacaoDoSol();
        objMoon.transform.Rotate(Vector3.up, moonRotationSpeed * Time.deltaTime);
    }

    private float targetRotation = 0f;
    private float rotationVelocity = 0f;
    void AtualizarRotacaoDoSol()
    {
        // Mapeia gameHour, gameMinute e gameSegundos para o intervalo desejado (-190, -90, 20)
        float mappedHour = Map(gameHour + gameMinute / 60f + gameSecond / 3600f, 4f, 20f, -190f, 20f);

        // Define o novo ângulo de rotação
        float targetAngle = mappedHour + Mathf.Repeat(deltaTime * multiplicadorVelocidade, 360f);

        // Suaviza a transição entre as posições
        float smoothTime = 1.0f; // Ajuste conforme necessário
        targetRotation = Mathf.SmoothDamp(targetRotation, targetAngle, ref rotationVelocity, smoothTime);

        pivotDoSol.transform.rotation = Quaternion.Euler(targetRotation, 0, 0f);
    }

    // Fun��o para mapear valores de um intervalo para outro
    float Map(float value, float fromSource, float toSource, float fromTarget, float toTarget)
    {
        return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
    }

    private void spawnarLootsPorDia()
    {
        foreach(SpawnLoots spawnLoots in listaSpawnLoots)
        {
            spawnLoots.SpawnarLootPorDias();
        }
    }

    private void spawnarPorDia()
    {
        if (!isRespawnarInimigos) return;
        if (lastGameDayLobos != gameDay && gameHour >= entardecerHorario)
        {
            spawnController.SpawnarLobisomens(gameDay);
            lastGameDayLobos = gameDay;
        }
        if (lastGameDayAnimais != gameDay && gameHour >= amanhecerHorario)
        {
            spawnController.SpawnarAnimaisPassivos();
            spawnController.SpawnarAnimaisAgressivos();
            lastGameDayAnimais = gameDay;
        }
    }

    void OnGUI()
    {
        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(0, 0, w, h * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 2 / 50;
        style.normal.textColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);

        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
        GUI.Label(rect, text, style);
    }


}
