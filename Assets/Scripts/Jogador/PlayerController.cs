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
	[SerializeField][HideInInspector] public bool isMorto = false, isAttacking = false;
	[SerializeField] public Inventario inventario;
	private Animator animator;
	private GameController gameController;
	private PlayerMovement playerMovement;
	[SerializeField][HideInInspector] StatsJogador statsJogador;
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
				inventario.itemNaMao.UsarItem();
				if (inventario.itemNaMao.tipoItem == Item.TipoItem.Ferramenta)
				{
					bool atkFerramenta = animator.GetCurrentAnimatorStateInfo(0).IsName("atkFerramenta");
					if (!atkFerramenta)
					{
						animator.SetTrigger("atkFerramenta");
					}
				}
			}
			if (Input.GetKeyDown(KeyCode.G))
			{
				inventario.itemNaMao.DroparItem();
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

	public void TakeDamage(float damage)
	{
		PV.RPC("RPC_TakeDamage", RpcTarget.All, damage);
	}

	[PunRPC]
	void RPC_TakeDamage(float damage)
	{
		if (!PV.IsMine)
			return;

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
		PV.RPC("RPC_ExecutarAcoesDie", RpcTarget.All);
	}

	[PunRPC]
	void RPC_ExecutarAcoesDie()
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