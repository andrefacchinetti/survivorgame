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
{// lembrete: nome de usuarios iguais buga a mudança de cena

	//stats
	[SerializeField] public bool isMorto = false, isAttacking = false;
	[SerializeField] public Inventario inventario;
	[SerializeField] public GrabObjects grabObjects;
	[SerializeField] public Animator animator, animatorVaraDePesca;
	private GameController gameController;
	private PlayerMovement playerMovement;
	[SerializeField][HideInInspector] public StatsJogador statsJogador;
	[SerializeField] [HideInInspector] public List<Item.ItemDropStruct> itemsDropsPosDissecar;
	[SerializeField] [HideInInspector] public GameObject corpoDissecando, fogueiraAcendendo, pescaPescando;
	[SerializeField] public GameObject acendedorFogueira, peixeDaVara;
	[SerializeField] [HideInInspector] public Item itemConsumindo;
	PhotonView PV;

	void Awake()
	{
		PV = GetComponent<PhotonView>();
		animator = GetComponent<Animator>();
		GameObject gc = GameObject.FindGameObjectWithTag("GameController");
		if (gc != null) gameController = gc.GetComponent<GameController>();
		playerMovement = GetComponent<PlayerMovement>();
		statsJogador = GetComponent<StatsJogador>();
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
			Die();
		}

		if (!inventario.canvasInventario.activeSelf && inventario.itemNaMao != null)
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
		verificarAnimacoes();
	}

    private void verificarAnimacoes()
    {
		bool acendendo = animator.GetCurrentAnimatorStateInfo(0).IsName("AcendendoFogueira");
		if (!acendendo) acendedorFogueira.SetActive(false);
		else acendedorFogueira.SetActive(true);
	}

	private void ativarAnimacaoPorTipoItem(Item itemResponse)
    {
		if (itemResponse.nomeItem.GetTipoItemEnum().Equals(Item.TiposItems.Ferramenta.ToString()))
		{
			string atkName;
			if (grabObjects.isPlayerEstaOlhandoPraBaixo())
			{
                if (itemResponse.nomeItem.Equals(Item.NomeItem.MarteloSimples))
                {
					atkName = "atkFerramentaMarteloBaixo";
                }
                else
                {
					atkName = "atkFerramentaBaixo";
				}
				
			}
			else
			{
				if (itemResponse.nomeItem.Equals(Item.NomeItem.MarteloSimples))
				{
					atkName = "atkFerramentaMarteloFrente";
				}
				else
				{
					atkName = "atkFerramentaFrente";
				}
			}
			bool atk = animator.GetCurrentAnimatorStateInfo(0).IsName(atkName);
			if (!atk) animator.SetTrigger(atkName);
		}
		else if (itemResponse.nomeItem.GetTipoItemEnum().Equals(Item.TiposItems.Consumivel.ToString()))
        {
			animator.SetTrigger("comendoEmPe");
			itemConsumindo = itemResponse;
		}
		else if (itemResponse.nomeItem.GetTipoItemEnum().Equals(Item.TiposItems.Arma.ToString()))
		{
			if(itemResponse.nomeItem.Equals(Item.NomeItem.LancaSimples) || itemResponse.nomeItem.Equals(Item.NomeItem.LancaAvancada))
            {
				bool atk = animator.GetCurrentAnimatorStateInfo(0).IsName("arremessandoLanca");
				if (!atk) animator.SetTrigger("arremessandoLanca");
			}
        }
	}

	void GoAtk()
	{
		isAttacking = true;
	}

	void NotAtk()
	{
		isAttacking = false;
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
    }
	public void EventPescou()
	{
		Debug.Log("event pescou");
		if (pescaPescando == null) return;
		peixeDaVara.SetActive(false);
		inventario.AdicionarItemAoInventario(Item.NomeItem.PeixeCru, 1);
		pescaPescando.GetComponent<Pesca>().DesativarAreaDePesca();
	}


	//Fim Acoes Animacoes

	public void TakeDamage(float damage)
	{
		if (PhotonNetwork.IsConnected) PV.RPC("RPC_TakeDamage", RpcTarget.All, damage);
		else acoesTakeDamage(damage);
	}

	[PunRPC]
	void RPC_TakeDamage(float damage)
	{
		if (!PV.IsMine)
			return;
		acoesTakeDamage(damage);
	}

	private void acoesTakeDamage(float damage)
    {
		statsJogador.setarVidaAtual(statsJogador.vidaAtual - damage);
		animator.SetTrigger("Hit");
		Debug.Log("player tomou " + damage + " de hit. Vida: " + statsJogador.vidaAtual);
		if (statsJogador.vidaAtual <= 0)
		{
			Die();
		}
	}

	void Die()
	{
		if (isMorto) return;
		if (PhotonNetwork.IsConnected) PV.RPC("RPC_ExecutarAcoesDie", RpcTarget.All);
		else acoesExecutarAcoesDie();
	}

	[PunRPC]
	void RPC_ExecutarAcoesDie()
	{
		acoesExecutarAcoesDie();
	}

	private void acoesExecutarAcoesDie()
    {
		animator.SetBool("isDead", true);
		isMorto = true;
		playerMovement.canMove = false;
	}

	[PunRPC]
	void RPC_ExecutarAcoesRessurgimento()
	{
		isMorto = false;
	}

	[PunRPC]
	void RPC_RespawnarPlayer(bool isLoading)
	{
		PhotonNetwork.Destroy(gameObject);
	}

}