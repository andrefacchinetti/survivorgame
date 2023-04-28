using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using Cinemachine;
using System.IO;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerController : MonoBehaviourPunCallbacks
{

	//stats
	[SerializeField] public Inventario inventario;
	[SerializeField] public Armaduras armaduras;
	[SerializeField] public GrabObjects grabObjects;
	[SerializeField] public Animator animator, animatorVaraDePesca;
	
	[SerializeField] [HideInInspector] public StatsJogador statsJogador;
	[SerializeField] [HideInInspector] public StatsGeral statsGeral;
	[SerializeField] [HideInInspector] public PlayerMovement playerMovement;
	[SerializeField] [HideInInspector] public List<Item.ItemDropStruct> itemsDropsPosDissecar;
	[SerializeField] [HideInInspector] public GameObject corpoDissecando, fogueiraAcendendo, pescaPescando, arvoreColetando;
	[SerializeField] [HideInInspector] public Item itemConsumindo, itemColetando;
	[SerializeField] [HideInInspector] public Item.NomeItem nomeItemColetando;
	[SerializeField] public GameObject acendedorFogueira, peixeDaVara;
	[SerializeField][HideInInspector] public GameController gameController;
	

	PhotonView PV;

	void Awake()
	{
		PV = GetComponent<PhotonView>();
		animator = GetComponent<Animator>();
		GameObject gc = GameObject.FindGameObjectWithTag("GameController");
		if (gc != null) gameController = gc.GetComponent<GameController>();
		playerMovement = GetComponent<PlayerMovement>();
		statsJogador = GetComponent<StatsJogador>();
		statsGeral = GetComponent<StatsGeral>();
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

		if (!inventario.canvasInventario.activeSelf && inventario.itemNaMao != null && playerMovement.canMove)
		{
            if (Input.GetMouseButtonDown(0))
            {
				ativarAnimacaoPorTipoItem(inventario.itemNaMao);
			}
			else if (Input.GetKeyDown(KeyCode.G))
			{
				inventario.itemNaMao.DroparItem();
			}
		}
		verificarAnimacoesSegurandoItem();
	}

    private void verificarAnimacoesSegurandoItem()
    {
		bool acendendo = animator.GetCurrentAnimatorStateInfo(0).IsName("AcendendoFogueira");
		if (!acendendo) acendedorFogueira.SetActive(false);
		else acendedorFogueira.SetActive(true);

		if(inventario.itemNaMao != null && (inventario.itemNaMao.nomeItem.Equals(Item.NomeItem.ArcoSimples) || inventario.itemNaMao.nomeItem.Equals(Item.NomeItem.ArcoAvancado)))
        {
			animator.SetBool("segurandoArcoFlecha", true);
        }
        else
        {
			animator.SetBool("segurandoArcoFlecha", false);
		}
		if (inventario.itemNaMao != null && inventario.itemNaMao.nomeItem.Equals(Item.NomeItem.Besta))
		{
			animator.SetBool("segurandoCrossbow", true);
		}
		else
		{
			animator.SetBool("segurandoCrossbow", false);
		}
		if (inventario.itemNaMao != null && inventario.itemNaMao.nomeItem.Equals(Item.NomeItem.Tocha))
		{
			animator.SetBool("segurandoTocha", true);
		}
		else
		{
			animator.SetBool("segurandoTocha", false);
		}
	}

	private void ativarAnimacaoPorTipoItem(Item itemResponse)
    {
		if (itemResponse.nomeItem.GetTipoItemEnum().Equals(Item.TiposItems.Ferramenta.ToString()))
		{
			string atkName = "atkFerramentaFrente";
			if (itemResponse.nomeItem == Item.NomeItem.MarteloSimples)
			{
				atkName = "atkFerramentaMarteloFrente";
			}
			if (!animator.GetCurrentAnimatorStateInfo(0).IsName(atkName))
			{
				animator.SetTrigger(atkName);
			}
		}
		else if (itemResponse.nomeItem.GetTipoItemEnum().Equals(Item.TiposItems.Consumivel.ToString()))
        {
			animator.SetTrigger("comendoEmPe");
			itemConsumindo = itemResponse;
		}
		else if (itemResponse.nomeItem.GetTipoItemEnum().Equals(Item.TiposItems.Arma.ToString()))
		{
			if (itemResponse.nomeItem == Item.NomeItem.Besta)
			{
				if (!animator.GetCurrentAnimatorStateInfo(0).IsName("usandoBesta"))
				{
					itemResponse.itemObjMao.GetComponent<TipoFlechaNoArco>().AtivarTipoFlechaNoArco();
					animator.SetTrigger("usandoBesta");
				}
			}
			else if (itemResponse.nomeItem == Item.NomeItem.ArcoSimples || itemResponse.nomeItem == Item.NomeItem.ArcoAvancado)
			{
				if (!animator.GetCurrentAnimatorStateInfo(0).IsName("usandoArcoFlecha"))
				{
					itemResponse.itemObjMao.GetComponent<TipoFlechaNoArco>().AtivarTipoFlechaNoArco();
					animator.SetTrigger("usandoArcoFlecha");
				}
			}
			else if (itemResponse.nomeItem == Item.NomeItem.LancaSimples || itemResponse.nomeItem == Item.NomeItem.LancaAvancada)
			{
				if (!animator.GetCurrentAnimatorStateInfo(0).IsName("arremessandoLanca"))
				{
					animator.SetTrigger("arremessandoLanca");
				}
			}
			else
			{
				string atkName = "atkArmaFrente";
				if (!animator.GetCurrentAnimatorStateInfo(0).IsName(atkName))
				{
					animator.SetTrigger(atkName);
				}
			}
		}
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
		Debug.Log("comeu: " + itemConsumindo.nomeItem.ToString());
		itemConsumindo = null;
	}

	void AnimEventDissecado()
    {
		Debug.Log("dissecou");
		foreach (Item.ItemDropStruct drop in itemsDropsPosDissecar)
		{
			int quantidade = Random.Range(drop.qtdMinDrops, drop.qtdMaxDrops);
			string nomePrefab = drop.nomeItemEnum.GetTipoItemEnum() + "/" + drop.nomeItemEnum.ToString();
			ItemDrop.InstanciarPrefabPorPath(nomePrefab, quantidade, corpoDissecando.transform.position, corpoDissecando.transform.rotation, PV.ViewID);
        }
		itemsDropsPosDissecar = new List<Item.ItemDropStruct>();
		if (PhotonNetwork.IsConnected) PhotonNetwork.Destroy(corpoDissecando);
		else GameObject.Destroy(corpoDissecando);
		corpoDissecando = null;
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
		Ray ray = new Ray(playerMovement.pivotTiroBase.transform.position, playerMovement.pivotTiroBase.transform.forward);
		// Declara uma variável para armazenar o ponto em que o ray colide com a superfície
		RaycastHit hit;
		// Se o ray atingir alguma superfície, calcula a direção do arremesso
		if (Physics.Raycast(ray, out hit))
		{
			Vector3 direction = hit.point - playerMovement.pivotTiroBase.transform.position;
			direction.Normalize();
			
			string nomePrefab = inventario.itemNaMao.nomeItem.GetTipoItemEnum() + "/" + inventario.itemNaMao.nomeItem.ToString();
			GameObject meuObjLancado = ItemDrop.InstanciarPrefabPorPath(nomePrefab, 1, playerMovement.pivotTiroBase.transform.position, Quaternion.LookRotation(direction), PV.ViewID);
			// Aplica a força na direção calculada
			meuObjLancado.GetComponent<Rigidbody>().AddForce(direction * throwForce, ForceMode.Impulse);
			//REMOVER ITEM DA MAO
			inventario.RemoverItemDaMao();
		}
	}

	void AnimEventBebeuAgua()
    {
		Debug.Log("bebeu agua");
		statsJogador.setarSedeAtual(statsJogador.sedeAtual + 100);
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
		inventario.AdicionarItemAoInventario(Item.NomeItem.PeixeCru, 1);
	}

	void AnimEventTiroArcoFlecha()
    {
		Debug.Log("tiro flecha");
		if (inventario.itemNaMao == null) return;
		// Cria um ray que parte da posição da câmera na direção em que ela está apontando
		Ray ray = new Ray(playerMovement.pivotTiroBase.transform.position, playerMovement.pivotTiroBase.transform.forward);
		// Declara uma variável para armazenar o ponto em que o ray colide com a superfície
		RaycastHit hit;
		// Se o ray atingir alguma superfície, calcula a direção do arremesso
		if (Physics.Raycast(ray, out hit))
		{
			Vector3 direction = hit.point - playerMovement.pivotTiroBase.transform.position;
			direction.Normalize();

			Item flechaNaAljava = armaduras.ObterItemFlechaNaAljava();
			if (flechaNaAljava == null || flechaNaAljava.quantidade <= 0) return; // NAO TEM FLECHA
			string nomePrefab = flechaNaAljava.nomeItem.GetTipoItemEnum() + "/" + flechaNaAljava.nomeItem.ToString();
			GameObject meuObjLancado = ItemDrop.InstanciarPrefabPorPath(nomePrefab, 1, playerMovement.pivotTiroBase.transform.position, Quaternion.LookRotation(direction), PV.ViewID);
			// Aplica a força na direção calculada
			meuObjLancado.GetComponent<Rigidbody>().AddForce(direction * throwForce, ForceMode.Impulse);
			//REMOVER ITEM DO INVENTARIO
			inventario.RemoverItemDoInventario(flechaNaAljava, 1);
		}
	}

	void AnimEventColetouFruta()
    {
		if (arvoreColetando == null) return;
		List<Item.ItemDropStruct> itemDrops = new List<Item.ItemDropStruct>();
		foreach (Item.ItemDropStruct itemDrop in arvoreColetando.GetComponent<StatsGeral>().dropsItems)
		{
			if (itemDrop.nomeItemEnum.GetTipoItemEnum().Equals(Item.TiposItems.Consumivel.ToString()))
			{
				if(itemDrop.qtdMaxDrops > 0)
                {
					Debug.Log("coletou fruta: " + itemDrop.nomeItemEnum.ToString());
					inventario.AdicionarItemAoInventario(itemDrop.nomeItemEnum, 1);
					arvoreColetando.GetComponent<ArvoreFrutifera>().DesaparecerUmaFrutaDaArvore();
					Item.ItemDropStruct novo = new Item.ItemDropStruct();
					novo.nomeItemEnum = itemDrop.nomeItemEnum;
					novo.qtdMinDrops = itemDrop.qtdMinDrops - 1;
					novo.qtdMaxDrops = itemDrop.qtdMaxDrops - 1;
					itemDrops.Add(novo);
				}
			}
            else
            {
				itemDrops.Add(itemDrop);
			}
		}
		arvoreColetando.GetComponent<StatsGeral>().dropsItems = itemDrops;
	}

	void AnimEventColetouItem()
	{
		if (nomeItemColetando.Equals(Item.NomeItem.Nenhum)) return;
		Debug.Log("coletou item: " + nomeItemColetando.ToString());
		inventario.AdicionarItemAoInventario(nomeItemColetando, 1);
		nomeItemColetando = Item.NomeItem.Nenhum;
	}

	[PunRPC]
	void RPC_ExecutarAcoesRessurgimento()
	{
		statsGeral.isDead = false;
	}

	[PunRPC]
	void RPC_RespawnarPlayer(bool isLoading)
	{
		PhotonNetwork.Destroy(gameObject);
	}

}