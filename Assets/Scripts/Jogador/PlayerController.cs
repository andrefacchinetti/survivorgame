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
	private Animator animator;
	private GameController gameController;
	private PlayerMovement playerMovement;
	[SerializeField][HideInInspector] StatsJogador statsJogador;
	[SerializeField] [HideInInspector] public List<Item.ItemDropStruct> itemsDropsPosDissecar;
	[SerializeField] [HideInInspector] public GameObject corpoDissecando;
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

		if (!gameController.isComecou)
		{
			gameController.isComecou = true;
			PV.RPC("RPC_RespawnarPlayer", RpcTarget.All, false);
		}

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
			if (!atk)
			{
				animator.SetTrigger(atkName);
			}
		}
		else if (itemResponse.nomeItem.GetTipoItemEnum().Equals(Item.TiposItems.Consumivel.ToString()))
        {
			animator.SetTrigger("comendoEmPe");
			itemConsumindo = itemResponse;
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

	void AnimEventBebeuAgua()
    {
		Debug.Log("bebeu agua");
		statsJogador.setarSedeAtual(statsJogador.sedeAtual + 100);
	}

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