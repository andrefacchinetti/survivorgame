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
using Opsive.UltimateCharacterController.Camera.ViewTypes;
using Opsive.Shared.Events;
using Crest.Examples;
using Crest;
using Opsive.UltimateCharacterController.Camera;

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
    [SerializeField] public CameraController cameraController;
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
    [SerializeField] [HideInInspector] public Fall fallAbility;
    [SerializeField] [HideInInspector] public Pilotar pilotarAbility;

    [HideInInspector] public VaraDePesca varaDePescaTP, varaDePescaFP;
    [HideInInspector] public AcendedorFogueira acendedorFogueiraTP, acendedorFogueiraFP;
    [HideInInspector] public CordaWeapon cordaWeaponFP, cordaWeaponTP;

    [HideInInspector] public bool canMove = true, estouPilotando = false;
    [HideInInspector] public BoatProbes barcoPilotando;

    public float pesoGrab = 0.0f;

    public PhotonView PV;

    //PARAMETROS Configurados automaticamente pelas Abilitys
    [HideInInspector] public float jumpForceInicial;
    [HideInInspector] public Vector3 motorAccelerationInicial;

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
        fallAbility = characterLocomotion.GetAbility<Fall>();
        pilotarAbility = characterLocomotion.GetAbility<Pilotar>();

        sampleHeightHelper = new SampleHeightHelper();

        motorAccelerationInicial = characterLocomotion.MotorAcceleration;
        jumpForceInicial = jumpAbility.Force;
        EventHandler.RegisterEvent<Ability, bool>(gameObject, "OnCharacterAbilityActive", OnAbilityActive);
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
                if (barcoPilotando != null)
                {
                    if (Input.GetButtonDown("Action") || BarcoCapotou() || distanteDoBarco()) //PARANDO DE PILOTAR BARCO
                    {
                        PararAbility(pilotarAbility);
                    }
                }
                else
                {
                    estouPilotando = false;
                }
            }

            if (swimAbility.IsActive || climbWaterAbility.IsActive)
            {
                if(pilotarAbility.IsActive) PararAbility(pilotarAbility);
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
            toggleOceanTrailsPes(true);
        }
        else
        {
            toggleOceanTrailsPes(false);
        }

        verificarSwimCrestOcean();
    }

    [SerializeField] GameObject[] oceanTrailPes;

    private void toggleOceanTrailsPes(bool ativando)
    {
        foreach (GameObject oceanTrailPe in oceanTrailPes)
        {
            
            if (ativando)
            {
                if (oceanTrailPe.transform.position.y < waterHeight)
                {
                    if (!oceanTrailPe.activeSelf) oceanTrailPe.SetActive(true);
                }
                else
                {
                    if (oceanTrailPe.activeSelf) oceanTrailPe.SetActive(false);
                }
            }
            else
            {
                if (oceanTrailPe.activeSelf) oceanTrailPe.SetActive(false);
            }
        }
    }

    public void PilotarBarco(BoatProbes barcoResponse)
    {
        estouPilotando = true;
        barcoPilotando = barcoResponse;
        cameraController.SetPerspective(false);
        PararAbilitys();
        StartarAbility(pilotarAbility);
    }

    public void SubirNoBarco(BoatProbes barcoController)
    {
        setarPosicaoNoBarco(barcoController);
    }

    public void PararDePilotarBarco()
    {
        cameraController.SetPerspective(true, true);
        if (barcoPilotando != null)
        {
            barcoPilotando.PararDePilotarBarco();
        }
        estouPilotando = false;
        barcoPilotando = null;
    }

    private void setarPosicaoNoBarco(BoatProbes barcoResponse)
    {
        if (barcoResponse != null)
        {
            characterLocomotion.SetPosition(barcoResponse.obterPosicaoPassageiro()); // Ajusta a posição do jogador
        }
    }

    private SampleHeightHelper sampleHeightHelper;
    private float swimAntiBug = 4.0f, waterHeight;
    public Collider colliderNatacao; //TODO: Setar dinamico no awake
    private void verificarSwimCrestOcean()
    {
        // Verificar se as referências necessárias estão disponíveis
        if (sampleHeightHelper == null || characterLocomotion == null || swimAbility == null) return;
        // Obter a posição atual do jogador
        Vector3 playerPosition = characterLocomotion.transform.position;
        // Inicializar o sampleHeightHelper com a posição do jogador
        sampleHeightHelper.Init(playerPosition);
        // Definir a posição da superfície da água na habilidade de nadar
        if(sampleHeightHelper.Sample(out waterHeight))  // Consultar a altura da água na posição do jogador >> 
        {
            swimAbility.SetWaterSurfacePosition(waterHeight);
            if (playerPosition.y < waterHeight - swimAntiBug)
            {
                if (estouPilotando)
                {
                    PararAbility(pilotarAbility);
                }
                if (fallAbility.IsActive)
                {
                    swimAbility.acoesOnTriggerEnter(colliderNatacao);
                    fallAbility.StopAbility(true);
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

    public void PararAbilitys() //Parar todas as habilidades executadas manualmente. Obs: Parar habilidades que podem devem ser paradas atraves de uma ação maior;
    {
        PararAbility(pescarAbility);
        PararAbility(acenderFogueiraAbility);
        PararAbility(apagarFogueiraAbility);
        PararAbility(beberAguaRioAbility);
        PararAbility(encherGarrafaRioAbility);
        PararAbility(capturarAbility);
        PararAbility(dissecarAbility);
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
        else if (ability.AbilityIndexParameter == pilotarAbility.AbilityIndexParameter)
        {
            if (!activated)
            {
                PararDePilotarBarco();
            }
        }
    }

    // Método para verificar se o jogador capotou
    public float paramTimeCapotar = 0.5f;
    public float paramMargemBarcoAfundou = 1.5f;
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

    private bool distanteDoBarco()
    {
        if (barcoPilotando == null) return true;
        if (Vector3.Distance(characterLocomotion.transform.position, barcoPilotando.posicaoVolante.transform.position) > 5)
        {
            return true;
        }
        return false;
    }
}
