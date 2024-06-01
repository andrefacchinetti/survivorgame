using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Opsive.Shared.Inventory;
using Opsive.UltimateCharacterController.Inventory;
using Opsive.UltimateCharacterController.Traits;
using Opsive.UltimateCharacterController.Character;
using Opsive.UltimateCharacterController.Character.Abilities;
using Opsive.UltimateCharacterController.AddOns.Swimming;
using Opsive.Shared.Events;

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
	[SerializeField] public GameObject contentItemsTP, contentItemsFP;
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

	[HideInInspector] public VaraDePesca varaDePescaTP, varaDePescaFP;
	[HideInInspector] public AcendedorFogueira acendedorFogueiraTP, acendedorFogueiraFP;
	[HideInInspector] public CordaWeapon cordaWeaponFP, cordaWeaponTP;

	[HideInInspector] public bool canMove = true;
	public float pesoGrab = 0.0f;

	public PhotonView PV;

	void Awake()
	{
		PV = GetComponent<PhotonView>();
		GameObject gc = GameObject.FindGameObjectWithTag("GameController");
		if (gc != null) gameController = gc.GetComponent<GameController>();
		statsJogador = GetComponent<StatsJogador>();
		statsGeral = GetComponent<StatsGeral>();
		characterHealth = GetComponent<CharacterHealth>();
		characterAttributeManager = GetComponent<CharacterAttributeManager>();
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

		EventHandler.RegisterEvent<Ability, bool>(gameObject, "OnCharacterAbilityActive", OnAbilityActive);
	}

	void Start()
	{
		if (PV == null) return;
	}

	bool jaSaiuDaAgua = true, tt=false;
	bool recarregandoEnergia = false;
	void Update()
	{
		if (PV == null || !PV.IsMine) return;

		if (!inventario.canvasInventario.activeSelf && canMove)
		{
			if (Input.GetButtonDown("Crouch"))
			{
				if (heightChangeAbility.IsActive) heightChangeAbility.StopAbility(true);
                else heightChangeAbility.StartAbility();
			}

			bool isRunning = Input.GetButton("Change Speeds") && pesoGrab == 0 && !recarregandoEnergia;
			if (isRunning)
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
					inventario.itemNaMao.DroparItem();
				}
			}
            if (Input.GetButtonDown("Use"))
            {
				armaduras.slotLanterna.TurnOffOnLanterna();
			}

			if (swimAbility.IsActive || climbWaterAbility.IsActive)
			{
				// Se o personagem está nadando, desequipe todos os itens
				if (jaSaiuDaAgua)
				{
					Debug.Log("Entrou na água: desequipando items");
					GetComponent<ItemSetManagerBase>().UnEquipAllItems(true, true);
					jaSaiuDaAgua = false;
				}
			}
			else
			{
				// Se o personagem não está nadando, equipe o item apenas uma vez quando ele sair da água
				if (!jaSaiuDaAgua)
				{
					IItemIdentifier itemIdBody = inventario.inventory.DefaultLoadout[0].ItemIdentifier;
					GetComponent<ItemSetManagerBase>().EquipItem(itemIdBody, -1, true, true);
					Debug.Log("Saiu da água: equipando body");
					jaSaiuDaAgua = true;
				}
			}
		}
		if (characterLocomotion.Moving)
		{
			PararAbilitys();
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

	public void AlertarJogadorComMensagem(string texto)
    {
		CancelInvoke("SumirAlerta");
		txMsgAlerta.text = texto;
		Invoke("SumirAlerta", 2);
	}

	void SumirAlerta()
    {
		txMsgAlerta.text = "";
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
		if(ability.AbilityIndexParameter == pescarAbility.AbilityIndexParameter)
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

	public void OnDestroy()
	{
		EventHandler.UnregisterEvent<Ability, bool>(gameObject, "OnCharacterAbilityActive", OnAbilityActive);
	}

}