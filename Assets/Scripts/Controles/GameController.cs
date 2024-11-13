using System.Collections;
using System.Collections.Generic;
using EasySky;
using Photon.Pun;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] Light luzDoSol;
    [SerializeField] GameObject objMoon;
    [SerializeField] float moonRotationSpeed = 10f;

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
    public float noiteHorario = 0f;

    [SerializeField] [HideInInspector] SpawnController spawnController;
    [SerializeField] public TimeController timeController;
    float deltaTime = 0.0f;
    private float lastGameDayLobos = -1, lastGameDayAnimais = -1;

    [SerializeField] public GameObject respawnPointJogador;
    [SerializeField] public SpawnDropaRecursosManager spawnDropaRecursosManager;
    [HideInInspector] public List<SpawnLoots> listaSpawnLoots;

    [SerializeField] public bool isRespawnarInimigos = true, isRespawnarAnimais = true; //DEIXAR TRUE
    [HideInInspector] public PhotonView PV;
    [HideInInspector] public GameObject[] playersOnline;

    //clima
    public float temperaturaClima = 0, temperaturaCalculada = 0;
    //end clima


    private void Awake()
    {
        listaSpawnLoots = new List<SpawnLoots>();
        spawnController = GetComponent<SpawnController>();
        timeController.gameController = this;
        PV = GetComponent<PhotonView>();
        Time.fixedDeltaTime = 0.05f; //Unity roda o FixedUpdate a cada 0,02 segundos (50 vezes por segundo). Você pode ajustar isso para reduzir a carga da CPU:
    }

    private void Start()
    {
        playersOnline = GameObject.FindGameObjectsWithTag("Player");

        SpawnarAnimaisPorDia(); //teste... o spawn de animais é controlado pela hora do dia em TimeController.cs
    }

    private float getGameHour()
    {
        return timeController._globalData.globalTime.Hour;
    }
    private int getGameDay()
    {
        return timeController._globalData.globalTime.Day;
    }

    public bool isNoite()
    {
        return (timeController._globalData.globalTime.Hour >= 0 && timeController._globalData.globalTime.Hour <= 4)
            || (timeController._globalData.globalTime.Hour >= 18 && timeController._globalData.globalTime.Hour < 24);
    }

    // Update is called once per frame
    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        Color currentColor;

        if (getGameHour() >= amanhecerHorario && getGameHour() < meioDiaHorario)
        {
            currentColor = Color.Lerp(amanhecer, meioDia, Mathf.SmoothStep(0f, 1f, (getGameHour() - amanhecerHorario) / (meioDiaHorario - amanhecerHorario)));
            luzDoSol.intensity = Mathf.Lerp(intensidadeAmanhecer, intensidadeMeioDia, Mathf.SmoothStep(0f, 1f, (getGameHour() - amanhecerHorario) / (meioDiaHorario - amanhecerHorario)));
        }
        else if (getGameHour() >= meioDiaHorario && getGameHour() < entardecerHorario)
        {
            currentColor = Color.Lerp(meioDia, entardecer, Mathf.SmoothStep(0f, 1f, (getGameHour() - meioDiaHorario) / (entardecerHorario - meioDiaHorario)));
            luzDoSol.intensity = Mathf.Lerp(intensidadeMeioDia, intensidadeEntardecer, Mathf.SmoothStep(0f, 1f, (getGameHour() - meioDiaHorario) / (entardecerHorario - meioDiaHorario)));
        }
        else if (getGameHour() >= entardecerHorario && getGameHour() < noiteHorario)
        {
            currentColor = Color.Lerp(entardecer, noite, Mathf.SmoothStep(0f, 1f, (getGameHour() - entardecerHorario) / (noiteHorario - entardecerHorario)));
            luzDoSol.intensity = Mathf.Lerp(intensidadeEntardecer, intensidadeNoite, Mathf.SmoothStep(0f, 1f, (getGameHour() - entardecerHorario) / (noiteHorario - entardecerHorario)));
        }
        else
        {
            currentColor = noite;
            luzDoSol.intensity = intensidadeNoite;
        }

        luzDoSol.color = currentColor;

        objMoon.transform.Rotate(Vector3.forward, moonRotationSpeed * Time.deltaTime);
    }

    private void LateUpdate()
    {
        if (playersOnline == null || playersOnline.Length <= 0 )
        {
            if (!PhotonNetwork.IsConnected || playersOnline.Length != PhotonNetwork.CurrentRoom.PlayerCount)
            {
                playersOnline = GameObject.FindGameObjectsWithTag("Player");
            }
        }
    }

    public void SpawnarLootsPorDia()
    {
        spawnarLootsPorDia();
        spawnDropaRecursosManager.respawnarDropaRecursos(PV.ViewID);
    }

    private void spawnarLootsPorDia()
    {
        foreach (SpawnLoots spawnLoots in listaSpawnLoots)
        {
            spawnLoots.SpawnarLootPorDias();
        }
    }

    public void SpawnarLobisomensPorDia()
    {
        if (!isRespawnarInimigos) return;
        spawnController.SpawnarLobisomens(getGameDay());
    }

    public void SpawnarAnimaisPorDia()
    {
        if (!isRespawnarAnimais) return;
        spawnController.SpawnarAnimaisPassivos();
        spawnController.SpawnarAnimaisAgressivos();
    }

    void OnGUI()
    {
        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();

        // Ajusta as coordenadas para o lado direito superior
        Rect rect = new Rect(w - (w * 2 / 100), 0, w * 2 / 100, h * 2 / 100);
        style.alignment = TextAnchor.UpperRight; // Alinha o texto à direita
        style.fontSize = h * 2 / 50;
        style.normal.textColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);

        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
        GUI.Label(rect, text, style);
    }
}
