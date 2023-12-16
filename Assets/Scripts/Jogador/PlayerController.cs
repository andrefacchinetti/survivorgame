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
	[SerializeField] public PointRopeFollow ropeGrab;
	[SerializeField] public GameObject contentItemsTP, contentItemsFP;
	[SerializeField] public TMP_Text txMsgAlerta;

	[SerializeField] [HideInInspector] public StatsJogador statsJogador;
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
	[SerializeField] [HideInInspector] public HeightChange heightChange;
	[SerializeField] [HideInInspector] public Pescar pescarAbility;
	[SerializeField] [HideInInspector] public AcenderFogueira acenderFogueira;
	[SerializeField] [HideInInspector] public ApagarFogueira apagarFogueira;

	[HideInInspector] public VaraDePesca varaDePescaTP, varaDePescaFP;
	[HideInInspector] public AcendedorFogueira acendedorFogueiraTP, acendedorFogueiraFP;

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
		characterLocomotion = GetComponent<UltimateCharacterLocomotion>();
		swimAbility = characterLocomotion.GetAbility<Swim>();
		climbWaterAbility = characterLocomotion.GetAbility<ClimbFromWater>();
		heightChange = characterLocomotion.GetAbility<HeightChange>();
		pescarAbility = characterLocomotion.GetAbility<Pescar>();
		acenderFogueira = characterLocomotion.GetAbility<AcenderFogueira>();
		apagarFogueira = characterLocomotion.GetAbility<ApagarFogueira>();

		EventHandler.RegisterEvent<Ability, bool>(gameObject, "OnCharacterAbilityActive", OnAbilityActive);
	}

	void Start()
	{
		if (PV == null) return;
	}

	bool jaSaiuDaAgua = true, tt=false;
	void Update()
	{
		if (PV == null) return;
		if (!PV.IsMine)
			return;

		if (!inventario.canvasInventario.activeSelf && canMove)
		{
			if (Input.GetButtonDown("Crouch"))
			{
				if (heightChange.IsActive) heightChange.StopAbility(true);
                else heightChange.StartAbility();
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
		PararAbility(acenderFogueira);
		PararAbility(apagarFogueira);
	}

	public void PararAbility(Ability abilidade)
    {
		if (abilidade.IsActive)
        {
			abilidade.StopAbility();
        }
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
		}else if (ability.AbilityIndexParameter == acenderFogueira.AbilityIndexParameter)
		{
			if (activated)
			{
				acendedorFogueiraTP.IniciarAcendedorFogueira();
				acendedorFogueiraFP.IniciarAcendedorFogueira();
			}
			else
			{
				if (acendedorFogueiraTP != null && acendedorFogueiraFP != null)
				{
					acendedorFogueiraTP.FinalizaAcendedorFogueira();
					acendedorFogueiraFP.FinalizaAcendedorFogueira();
				}
			}
		}
	}

	public void OnDestroy()
	{
		EventHandler.UnregisterEvent<Ability, bool>(gameObject, "OnCharacterAbilityActive", OnAbilityActive);
	}

}