using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Audio;
using Cinemachine;
using System.IO;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Opsive.Shared.Inventory;
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
	[SerializeField] [HideInInspector] public StatsGeral statsGeral;
	[SerializeField] [HideInInspector] public List<Item.ItemDropStruct> itemsDropsPosDissecar;
	[SerializeField] [HideInInspector] public GameObject corpoDissecando, fogueiraAcendendo, pescaPescando, arvoreColetando, objConsertando, corpoReanimando, objCapturado;
	[SerializeField] [HideInInspector] public AnimalController animalCapturado;
	[SerializeField] [HideInInspector] public Item itemConsumindo, itemColetando;
	[SerializeField] [HideInInspector] public ItemDefinitionBase itemDefinitionBaseColentando;
	[SerializeField] [HideInInspector] public GameController gameController;

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
		txMsgAlerta.text = "";
	}

	void Start()
	{
		if (PV == null) return;
	}

	void Update()
	{
		if (PV == null) return;
		if (!PV.IsMine)
			return;

		if (transform.position.y < -40f) // Die if you fall out of the world
		{
			statsGeral.TakeDamage(9999);
		}

		if (!inventario.canvasInventario.activeSelf && canMove)
		{
			if(inventario.itemNaMao != null)
            {
				/*if (Input.GetMouseButtonDown(0))
				{
					ativarAnimacaoPorTipoItem(inventario.itemNaMao);
				}*/
				if (Input.GetButtonDown("Dropar"))
				{
					inventario.itemNaMao.DroparItem();
				}
			}
            if (Input.GetButtonDown("Action"))
            {
				armaduras.slotLanterna.TurnOffOnLanterna();
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

	void GoAtk()
	{
		statsGeral.isAttacking = true;
	}

	void NotAtk()
	{
		statsGeral.isAttacking = false;
	}

	void AnimEventComeu()
    {
		if (itemConsumindo == null) return;
		itemConsumindo.UsarItem();
		Debug.Log("consumiu: " + itemConsumindo.itemIdentifierAmount.ItemDefinition.name.ToString());
		itemConsumindo = null;
	}

	void AnimEventDissecado()
    {
		Debug.Log("dissecou");
		foreach (Item.ItemDropStruct drop in itemsDropsPosDissecar)
		{
			int quantidade = Random.Range(drop.qtdMinDrops, drop.qtdMaxDrops);
			string nomePrefab = drop.tipoItem + "/" + drop.itemIdentifierAmount.ItemDefinition.name;
			ItemDrop.InstanciarPrefabPorPath(nomePrefab, quantidade, corpoDissecando.GetComponent<StatsGeral>().dropPosition.transform.position, corpoDissecando.GetComponent<StatsGeral>().dropPosition.transform.rotation, PV.ViewID);
        }
		itemsDropsPosDissecar = new List<Item.ItemDropStruct>();
		if (PhotonNetwork.IsConnected) PhotonNetwork.Destroy(corpoDissecando);
		else GameObject.Destroy(corpoDissecando);
		corpoDissecando = null;
	}

	void AnimEventCapturou()
	{
		Debug.Log("capturou");
		if (animalCapturado == null) return;
		animalCapturado.isCapturado = true;
		animalCapturado.targetCapturador = this.gameObject;
		animalCapturado.objColeiraRope.SetActive(true);
		ropeGrab.objFollowed = animalCapturado.objRopePivot.transform;
	}

	void AnimEventReanimouJogador()
    {
		if (corpoReanimando != null)
        {
			corpoReanimando.GetComponent<MorteController>().ReanimarJogador();
		}
	}

	void AnimEventConsertado()
	{
		Debug.Log("consertou");
		objConsertando.GetComponent<ReconstruivelQuebrado>().ConsertarReconstruivel();
		objConsertando = null;
	}

	void AnimEventArremessoLanca()
    {
		Debug.Log("arremessou lança");
		ArremessarItemNaMao();
	}

	// Força do arremesso
	public float throwForce = 300f;
	private void ArremessarItemNaMao() // Função que arremessa o objeto na direção da câmera
	{
		if (inventario.itemNaMao == null) return;
		// Cria um ray que parte da posição da câmera na direção em que ela está apontando
		Ray ray = new Ray(transform.position, transform.forward);
		// Declara uma variável para armazenar o ponto em que o ray colide com a superfície
		RaycastHit hit;
		// Se o ray atingir alguma superfície, calcula a direção do arremesso
		if (Physics.Raycast(ray, out hit))
		{
			Vector3 direction = hit.point - transform.position;
			direction.Normalize();
			
			string nomePrefab = inventario.itemNaMao.tipoItem + "/" + inventario.itemNaMao.itemIdentifierAmount.ItemDefinition.name;
			GameObject meuObjLancado = ItemDrop.InstanciarPrefabPorPath(nomePrefab, 1, transform.position, Quaternion.LookRotation(direction), PV.ViewID);
			// Aplica a força na direção calculada
			meuObjLancado.GetComponent<Rigidbody>().AddForce(direction * throwForce, ForceMode.Impulse);
			//REMOVER ITEM DA MAO
			inventario.RemoverItemDaMao();
		}
	}

	void AnimEventBebeuAgua()
    {
		statsJogador.setarSedeAtual(statsJogador.sedeAtual + 100);
	}
	void AnimEventBebeuGarrafa()
	{
		if(inventario.itemNaMao != null && inventario.itemNaMao.itemIdentifierAmount.ItemDefinition.name.Equals("Bottle"))
        {
			statsJogador.setarSedeAtual(statsJogador.sedeAtual + inventario.itemNaMao.GetComponent<Garrafa>().BeberAgua());
		}
	}

	void AnimEventEncheuGarrafa()
    {
		if (inventario.itemNaMao != null && inventario.itemNaMao.itemIdentifierAmount.ItemDefinition.name.Equals("Garrafa"))
        {
			inventario.itemNaMao.GetComponent<Garrafa>().EncherRepositorioComAgua();
		}
	}

	void AnimEventAcendendoFogueira()
	{
		if (fogueiraAcendendo == null) return;
		fogueiraAcendendo.GetComponent<Fogueira>().AcenderFogueira();
		fogueiraAcendendo = null;
	}

	void AnimEventApagandoFogueira()
	{
		if (fogueiraAcendendo == null) return;
		fogueiraAcendendo.GetComponent<Fogueira>().ApagarFogueira();
		fogueiraAcendendo = null;
	}

	void AnimEventApareceAcendedorFogueira()
    {
		acendedorFogueira.SetActive(true);
		acendedorFogueira.GetComponent<Animator>().Play("default");
	}

	void AnimEventDesapareceAcendedorFogueira()
	{
		acendedorFogueira.SetActive(false);
	}

	void AnimEventAtivarPegandoPeixe()
    {
		Debug.Log("pegando peixe");
		peixeDaVara.SetActive(true);
		animatorVaraDePesca.SetTrigger("pegandoPeixe");
		pescaPescando.GetComponent<Pesca>().DesativarAreaDePesca();
	}

	public void EventPescou()
	{
		Debug.Log("event pescou");
		if (pescaPescando == null) return;
		peixeDaVara.SetActive(false);
		inventario.AdicionarItemAoInventario(null, inventario.itemPeixeCru, 1);
	}

	void AnimEventColetouFruta()
    {
		if (arvoreColetando == null) return;
		List<Item.ItemDropStruct> itemDrops = new List<Item.ItemDropStruct>();
		foreach (Item.ItemDropStruct itemDropScruct in arvoreColetando.GetComponent<StatsGeral>().dropsItems)
		{
			if (itemDropScruct.tipoItem.Equals(Item.TiposItems.Consumivel.ToString()))
			{
				if(itemDropScruct.qtdMaxDrops > 0)
                {
					Debug.Log("coletou fruta: " + itemDropScruct.itemIdentifierAmount.ItemDefinition.name);
					inventario.AdicionarItemAoInventario(null, itemDropScruct.itemIdentifierAmount.ItemDefinition, 1);
					arvoreColetando.GetComponent<ArvoreFrutifera>().DesaparecerUmaFrutaDaArvore();
					Item.ItemDropStruct novo = new Item.ItemDropStruct();
					novo.itemIdentifierAmount = itemDropScruct.itemIdentifierAmount;
					novo.qtdMinDrops = itemDropScruct.qtdMinDrops - 1;
					novo.qtdMaxDrops = itemDropScruct.qtdMaxDrops - 1;
					itemDrops.Add(novo);
				}
			}
            else
            {
				itemDrops.Add(itemDropScruct);
			}
		}
		arvoreColetando.GetComponent<StatsGeral>().dropsItems = itemDrops;
	}

	void AnimEventColetouItem()
	{
		if (itemDefinitionBaseColentando == null) return;
		Debug.Log("coletou item: " + itemDefinitionBaseColentando.name);
		inventario.AdicionarItemAoInventario(null, itemDefinitionBaseColentando, 1);
		itemDefinitionBaseColentando = null;
	}

	public bool podeSeMexer()
	{
		return canMove && !statsGeral.isDead;
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