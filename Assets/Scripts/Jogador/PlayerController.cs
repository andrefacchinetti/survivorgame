using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Opsive.Shared.Inventory;
using Opsive.UltimateCharacterController.Inventory;
using Opsive.UltimateCharacterController.Traits;
using Opsive.UltimateCharacterController.Character;
using Opsive.UltimateCharacterController.Character.Abilities;
using Opsive.UltimateCharacterController.AddOns.Swimming;
using Opsive.Shared.Events;
using Crest.Examples;
using Crest;

public class PlayerController : MonoBehaviourPunCallbacks
{
    public float fireRate = 1.0f; // Taxa de tiro por segundo
    private float nextFireTime = 0.0f; // Tempo do próximo tiro

    [SerializeField] public Inventario inventario;
    [SerializeField] public Armaduras armaduras;
    [SerializeField] public GrabObjects grabObjects;
    [SerializeField] public ControleConstruir controleConstruir;
    [SerializeField] public EventsAnimJogador eventsAnimJogador;
    [SerializeField] public Animator animatorVaraDePesca, animatorJogador;
    [SerializeField] public GameObject contentItemsTP, contentItemsFP, objAlerta;
    [SerializeField] public TMP_Text txMsgAlerta;

    [SerializeField] [HideInInspector] public StatsJogador statsJogador;
    [SerializeField] [HideInInspector] public Inventory inventory;
    [SerializeField] [HideInInspector] public UltimateCharacterLocomotion characterLocomotion;
    [SerializeField] [HideInInspector] public CharacterHealth characterHealth;
    [SerializeField] [HideInInspector] public CharacterAttributeManager characterAttributeManager;
    [SerializeField] [HideInInspector] public StatsGeral statsGeral;
    [SerializeField] [HideInInspector] public List<Item.ItemDropStruct> itemsDropsPosDissecar;
    [SerializeField] [HideInInspector] public GameObject corpoDissecando, fogueiraAcendendo, pescaPescando, arvoreColetando, objConsertando, corpoReanimando, objCapturado;
    [SerializeField] [HideInInspector] public AnimalController animalCapturado;
    [SerializeField] [HideInInspector] public Item itemColetando;
    [SerializeField] [HideInInspector] public ItemDefinitionBase itemDefinitionBaseColentando;
    [SerializeField] [HideInInspector] public GameController gameController;

    [SerializeField] [HideInInspector] public Swim swimAbility;
    [SerializeField] [HideInInspector] public ClimbFromWater climbWaterAbility;
    [SerializeField] [HideInInspector] public HeightChange heightChangeAbility;
    [SerializeField] [HideInInspector] public SpeedChange speedChangeAbility;
    [SerializeField] [HideInInspector] public Pescar pescarAbility;
    [SerializeField] [HideInInspector] public AcenderFogueira acenderFogueiraAbility;
    [SerializeField] [HideInInspector] public ApagarFogueira apagarFogueiraAbility;
    [SerializeField] [HideInInspector] public Capturar capturarAbility;
    [SerializeField] [HideInInspector] public Dissecar dissecarAbility;
    [SerializeField] [HideInInspector] public BeberAguaRio beberAguaRioAbility;
    [SerializeField] [HideInInspector] public EncherGarrafaRio encherGarrafaRioAbility;
    [SerializeField] [HideInInspector] public Revive reviveAbility;
    [SerializeField] [HideInInspector] public Jump jumpAbility;

    [HideInInspector] public VaraDePesca varaDePescaTP, varaDePescaFP;
    [HideInInspector] public AcendedorFogueira acendedorFogueiraTP, acendedorFogueiraFP;
    [HideInInspector] public CordaWeapon cordaWeaponFP, cordaWeaponTP;

    [HideInInspector] public bool canMove = true, estouPilotando = false;
    [HideInInspector] public BoatAlignNormal barcoPilotando;
    [HideInInspector] public Submarine submarinoPilotando;

    public float pesoGrab = 0.0f;

    public PhotonView PV;

    //PARAMETROS Configurados automaticamente pelas Abilitys
    [HideInInspector] public float jumpForceInicial;
    [HideInInspector] public Vector3 motorAccelerationInicial;

    private Quaternion lastRotation;
    private float abruptRotationThreshold = 45f; // Ajuste conforme necessário

    void Awake()
    {
        PV = GetComponent<PhotonView>();
        GameObject gc = GameObject.FindGameObjectWithTag("GameController");
        if (gc != null) gameController = gc.GetComponent<GameController>();
        statsJogador = GetComponent<StatsJogador>();
        statsGeral = GetComponent<StatsGeral>();
        characterHealth = GetComponent<CharacterHealth>();
        characterAttributeManager = GetComponent<CharacterAttributeManager>();
        objAlerta.SetActive(false);
        txMsgAlerta.text = "";

        inventory = GetComponent<Inventory>();
        characterLocomotion = GetComponent<UltimateCharacterLocomotion>();
        swimAbility = characterLocomotion.GetAbility<Swim>();
        climbWaterAbility = characterLocomotion.GetAbility<ClimbFromWater>();
        heightChangeAbility = characterLocomotion.GetAbility<HeightChange>();
        speedChangeAbility = characterLocomotion.GetAbility<SpeedChange>();
        pescarAbility = characterLocomotion.GetAbility<Pescar>();
        acenderFogueiraAbility = characterLocomotion.GetAbility<AcenderFogueira>();
        apagarFogueiraAbility = characterLocomotion.GetAbility<ApagarFogueira>();
        capturarAbility = characterLocomotion.GetAbility<Capturar>();
        dissecarAbility = characterLocomotion.GetAbility<Dissecar>();
        beberAguaRioAbility = characterLocomotion.GetAbility<BeberAguaRio>();
        encherGarrafaRioAbility = characterLocomotion.GetAbility<EncherGarrafaRio>();
        reviveAbility = characterLocomotion.GetAbility<Revive>();
        jumpAbility = characterLocomotion.GetAbility<Jump>();

        sampleHeightHelper = new SampleHeightHelper();

        motorAccelerationInicial = characterLocomotion.MotorAcceleration;
        jumpForceInicial = jumpAbility.Force;
        EventHandler.RegisterEvent<Ability, bool>(gameObject, "OnCharacterAbilityActive", OnAbilityActive);

        lastRotation = characterLocomotion.transform.rotation;
    }

    public void OnDestroy()
    {
        EventHandler.UnregisterEvent<Ability, bool>(gameObject, "OnCharacterAbilityActive", OnAbilityActive);
    }

    void Start()
    {
        if (PV == null) return;
        swimAbility.SetWaterSurfacePosition(5f);
    }

    bool jaSaiuDaAgua = true, tt = false;
    bool recarregandoEnergia = false;
    void Update()
    {
        if (PV == null || !PV.IsMine) return;

        if (!inventario.canvasInventario.activeSelf && podeSeMexer())
        {
            if (!estouPilotando)
            {
                if (Input.GetButtonDown("Crouch"))
                {
                    if (heightChangeAbility.IsActive) heightChangeAbility.StopAbility(true);
                    else heightChangeAbility.StartAbility();
                }
            }

            bool isRunning = Input.GetButton("Change Speeds") && pesoGrab == 0 && !recarregandoEnergia;
            if (isRunning && !statsJogador.isFraturado)
            {
                statsJogador.setarEnergiaAtual(statsJogador.energiaAtual - statsJogador.consumoEnergiaPorSegundo * Time.deltaTime);
                statsJogador.setarSedeAtual(statsJogador.sedeAtual - (statsJogador.valorDaSedeReduzidaPorTempo / statsJogador.consumoEnergiaPorSegundo) * Time.deltaTime);
                speedChangeAbility.StartAbility();
            }
            else
            {
                statsJogador.setarEnergiaAtual(statsJogador.energiaAtual + statsJogador.recuperacaoEnergiaPorSegundo * Time.deltaTime);
                if (statsJogador.energiaAtual > 10) recarregandoEnergia = false;
                if (speedChangeAbility.IsActive) speedChangeAbility.StopAbility(true);
            }
            if (statsJogador.energiaAtual <= 0 && !recarregandoEnergia)
            {
                recarregandoEnergia = true;
            }

            if (inventario.itemNaMao != null)
            {
                if (Input.GetButtonDown("Dropar"))
                {
                    inventario.itemNaMao.DroparItem(1);
                }
            }

            if (estouPilotando)
            {
                if (barcoPilotando != null || submarinoPilotando != null)
                {
                    if (Input.GetButtonDown("Action") || BarcoCapotou()) //PARANDO DE PILOTAR BARCO
                    {
                        PararDePilotarBarco();
                    }
                    atualizacaoPilotandoBarco();
                }
                else
                {
                    estouPilotando = false;
                }
            }

            if (swimAbility.IsActive || climbWaterAbility.IsActive)
            {
                // Se o personagem está nadando, desequipe todos os itens
                if (jaSaiuDaAgua)
                {
                    GetComponent<ItemSetManagerBase>().UnEquipAllItems(true, true);
                    jaSaiuDaAgua = false;
                    statsJogador.ToggleHudFolego(true);
                }
                else
                {
                    statsJogador.AtualizarBarraDeFolego();
                }
            }
            else
            {
                // Se o personagem não está nadando, equipe o item apenas uma vez quando ele sair da água
                if (!jaSaiuDaAgua)
                {
                    IItemIdentifier itemIdBody = inventario.inventory.DefaultLoadout[0].ItemIdentifier;
                    GetComponent<ItemSetManagerBase>().EquipItem(itemIdBody, -1, true, true);
                    jaSaiuDaAgua = true;
                    statsJogador.ToggleHudFolego(false);
                }
            }

        }
        if (characterLocomotion.Moving)
        {
            PararAbilitys();
        }

        verificarSwimCrestOcean();

        // Atualiza a rotação do jogador
        lastRotation = characterLocomotion.transform.rotation;
    }

    public void PararDePilotarBarco()
    {
        if (barcoPilotando != null) barcoPilotando.PararDePilotarBarco();
        if (submarinoPilotando != null) submarinoPilotando.PararDePilotarBarco();
        estouPilotando = false;
        barcoPilotando = null;
        submarinoPilotando = null;
        characterLocomotion.DetectHorizontalCollisions = true;
        characterLocomotion.DetectVerticalCollisions = true;
    }

    private void atualizacaoPilotandoBarco()
    {
        if (barcoPilotando != null)
        {
            // Atualiza a posição e rotação do jogador com base na posição e rotação do barco
            Vector3 posicaoPiloto = barcoPilotando.posicaoPiloto.transform.position;
            Quaternion rotacaoPiloto = barcoPilotando.transform.rotation;
            if (posicaoPiloto != Vector3.zero)
            {
                // Ajusta a posição do jogador
                characterLocomotion.SetPosition(posicaoPiloto);
                // Ajusta a rotação do jogador
                characterLocomotion.transform.rotation = rotacaoPiloto;
            }
        }
        else if (submarinoPilotando != null)
        {
            // Atualiza a posição e rotação do jogador com base na posição e rotação do submarino
            Vector3 posicaoPiloto = submarinoPilotando.posicaoPiloto.transform.position;
            Quaternion rotacaoPiloto = submarinoPilotando.transform.rotation;
            if (posicaoPiloto != Vector3.zero)
            {
                // Ajusta a posição do jogador
                characterLocomotion.SetPosition(posicaoPiloto);
                // Ajusta a rotação do jogador
                characterLocomotion.transform.rotation = rotacaoPiloto;
            }
        }
        // Desativa a detecção de colisões enquanto o jogador está pilotando
        characterLocomotion.DetectHorizontalCollisions = false;
        characterLocomotion.DetectVerticalCollisions = false;
    }

    private SampleHeightHelper sampleHeightHelper;
    private float swimAntiBug = 4.0f;
    private void verificarSwimCrestOcean()
    {
        // Verificar se as referências necessárias estão disponíveis
        if (sampleHeightHelper == null || characterLocomotion == null || swimAbility == null) return;
        // Obter a posição atual do jogador
        Vector3 playerPosition = characterLocomotion.transform.position;
        // Inicializar o sampleHeightHelper com a posição do jogador
        sampleHeightHelper.Init(playerPosition, 0f);
        // Consultar a altura da água na posição do jogador
        if (sampleHeightHelper.Sample(out float waterHeight))
        {
            // Definir a posição da superfície da água na habilidade de nadar
            swimAbility.SetWaterSurfacePosition(waterHeight);
            if (playerPosition.y < waterHeight + swimAntiBug)
            {
                if (!swimAbility.IsActive)
                {
                    swimAbility.TryStartStopSwim(true);
                }
            }
        }
    }

    public void TogglePlayerModoConstrucao(bool construcaoAtiva)
    {
        if (construcaoAtiva)
        {
            inventario.GuardarItemDaMao();
        }
        //kitModoConstrucao.SetActive(construcaoAtiva);
    }

    public bool podeSeMexer()
    {
        return canMove && characterHealth.IsAlive();
    }

    public bool temFogoPerto()
    {
        GameObject[] fireObjects = GameObject.FindGameObjectsWithTag("Fogo");
        Vector3 playerPosition = transform.position;

        foreach (GameObject objeto in fireObjects)
        {
            // Verificar se a distância entre o jogador e o objeto é menor ou igual ao raio de detecção
            if (Vector3.Distance(playerPosition, objeto.transform.position) <= 5)
            {
                if (objeto.GetComponent<Fogo>().isFogoAceso)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private Collider[] collidersNoRaio = new Collider[10];
    public bool estaEmTerrenoGelado()
    {
        Vector3 playerPosition = transform.position;
        // Obter todos os colliders no raio de detecção
        int numColliders = Physics.OverlapSphereNonAlloc(playerPosition, 5f, collidersNoRaio, LayerMask.GetMask("Terreno"));
        for (int i = 0; i < numColliders; i++)
        {
            // Verificar se o collider é de um objeto com a tag correta
            if (collidersNoRaio[i].CompareTag("TerrenoNeve"))
            {
                return true;
            }
        }
        return false;
    }

    public void AlertarJogadorComMensagem(string texto)
    {
        CancelInvoke("SumirAlerta");
        txMsgAlerta.text = texto;
        objAlerta.SetActive(true);
        Invoke("SumirAlerta", 3);
    }

    void SumirAlerta()
    {
        txMsgAlerta.text = "";
        objAlerta.SetActive(false);
    }

    public void PararAbilitys()
    {
        PararAbility(pescarAbility);
        PararAbility(acenderFogueiraAbility);
        PararAbility(apagarFogueiraAbility);
        PararAbility(dissecarAbility);
        PararAbility(beberAguaRioAbility);
        PararAbility(encherGarrafaRioAbility);
        PararAbility(capturarAbility);
    }

    public void PararAbility(Ability habilidade)
    {
        if (habilidade.IsActive)
        {
            habilidade.StopAbility();
        }
    }

    public void StartarAbility(Ability habilidade)
    {
        characterLocomotion.TryStopAbility(heightChangeAbility, true);
        characterLocomotion.TryStartAbility(habilidade);
    }

    private void OnAbilityActive(Ability ability, bool activated)
    {
        if (ability.AbilityIndexParameter == pescarAbility.AbilityIndexParameter)
        {
            if (activated)
            {
                varaDePescaTP.IniciarPesca();
                varaDePescaFP.IniciarPesca();
            }
            else
            {
                if (varaDePescaFP != null && varaDePescaTP != null)
                {
                    varaDePescaTP.FinalizarPesca();
                    varaDePescaFP.FinalizarPesca();
                }
            }
        }
        else if (ability.AbilityIndexParameter == capturarAbility.AbilityIndexParameter)
        {
            if (activated)
            {
                if (cordaWeaponFP != null && cordaWeaponTP != null)
                {
                    cordaWeaponFP.objCordaMaos.SetActive(false);
                    cordaWeaponTP.objCordaMaos.SetActive(false);
                }
            }
            else
            {
                if (cordaWeaponFP != null && cordaWeaponTP != null)
                {
                    cordaWeaponFP.objCordaMaos.SetActive(true);
                    cordaWeaponTP.objCordaMaos.SetActive(true);
                }
            }
        }
    }

    // Método para verificar se o jogador capotou
    public float paramTimeCapotar = 0.5f;
    private bool BarcoCapotou()
    {
        // Verifica se o vetor "up" está apontando para baixo
        if (Mathf.Abs(barcoPilotando.transform.up.y) < paramTimeCapotar)
        {
            // Verifica a rotação do barco para confirmar capotamento
            float pitch = barcoPilotando.transform.rotation.eulerAngles.x;
            float roll = barcoPilotando.transform.rotation.eulerAngles.z;

            if (pitch > 80f && pitch < 100f)  // Verifica se o pitch está próximo de 90 graus
            {
                return true;
            }
            if (roll > 80f && roll < 100f)  // Verifica se o roll está próximo de 90 graus
            {
                return true;
            }
        }
        return false;
    }
}
