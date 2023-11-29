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
using Opsive.UltimateCharacterController.Character.Abilities.Items;
using Opsive.UltimateCharacterController.Inventory;

public class PlayerController : MonoBehaviourPunCallbacks
{

	public float fireRate = 1.0f; // Taxa de tiro por segundo
	private float nextFireTime = 0.0f; // Tempo do próximo tiro

	[SerializeField] public Inventario inventario;
	[SerializeField] public Armaduras armaduras;
	[SerializeField] public GrabObjects grabObjects;
	[SerializeField] public ControleConstruir controleConstruir;
	[SerializeField] public Animator animator, animatorVaraDePesca;
	[SerializeField] public PointRopeFollow ropeGrab;
	[SerializeField] public GameObject acendedorFogueira, peixeDaVara, kitModoConstrucao;
	[SerializeField] public TMP_Text txMsgAlerta;

	[SerializeField] [HideInInspector] public StatsJogador statsJogador;
	[SerializeField] [HideInInspector] public UltimateCharacterLocomotion characterLocomotion;
	[SerializeField] [HideInInspector] public CharacterHealth characterHealth;
	[SerializeField] [HideInInspector] public CharacterAttributeManager characterAttributeManager;
	[SerializeField] [HideInInspector] public StatsGeral statsGeral;
	[SerializeField] [HideInInspector] public List<Item.ItemDropStruct> itemsDropsPosDissecar;
	[SerializeField] [HideInInspector] public GameObject corpoDissecando, fogueiraAcendendo, pescaPescando, arvoreColetando, objConsertando, corpoReanimando, objCapturado;
	[SerializeField] [HideInInspector] public AnimalController animalCapturado;
	[SerializeField] [HideInInspector] public Item itemConsumindo, itemColetando;
	[SerializeField] [HideInInspector] public ItemDefinitionBase itemDefinitionBaseColentando;
	[SerializeField] [HideInInspector] public GameController gameController;
	[SerializeField] [HideInInspector] public Swim swimAbility;
	[SerializeField] [HideInInspector] public ClimbFromWater climbWaterAbility;


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
			if(inventario.itemNaMao != null)
            {
				if (Input.GetMouseButtonDown(0))
				{
					ativarAnimacaoPorTipoItem(inventario.itemNaMao);
				}
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
		//verificarAnimacoesSegurandoItem();
	}

	private void ativarAnimacaoPorTipoItem(Item itemResponse)
    {
		if (itemResponse.tipoItem.Equals(Item.TiposItems.Ferramenta.ToString()))
		{
			if (itemResponse.itemIdentifierAmount.ItemDefinition.name.Equals("Bottle"))
			{
				animator.SetTrigger("bebendoGarrafa");
            }
		}
		else if (itemResponse.tipoItem.Equals(Item.TiposItems.Consumivel.ToString()))
        {
            if (itemResponse.itemIdentifierAmount.ItemDefinition.name.Equals("KitMedico"))
            {
				animator.SetTrigger("usandoKitMedico");
            }
            else
            {
				animator.SetTrigger("comendoEmPe"); 
			}
			itemConsumindo = itemResponse;
		}
	}

	public void TogglePlayerModoConstrucao(bool construcaoAtiva)
    {
		animator.SetBool("construindoIdle", construcaoAtiva);
		if (construcaoAtiva)
		{
			inventario.GuardarItemDaMao();
		}
		kitModoConstrucao.SetActive(construcaoAtiva);
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

}