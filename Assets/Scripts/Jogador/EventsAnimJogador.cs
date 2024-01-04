using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EventsAnimJogador : MonoBehaviourPunCallbacks
{

	[SerializeField] PlayerController playerController;

	void AnimEventComeu()
	{
		if (playerController.inventario.itemNaMao == null)
		{
			Debug.LogWarning("nenhum item consumindo setado");
			return;
		}
		Debug.Log("consumiu: " + playerController.inventario.itemNaMao.itemIdentifierAmount.ItemDefinition.name.ToString());
		playerController.inventario.itemNaMao.UsarItem();
	}

	void AnimEventDissecado()
	{
		Debug.Log("dissecou");
		foreach (Item.ItemDropStruct drop in playerController.itemsDropsPosDissecar)
		{
			int quantidade = Random.Range(drop.qtdMinDrops, drop.qtdMaxDrops);
			string nomePrefab = drop.tipoItem + "/" + drop.itemDefinition.name;
			ItemDrop.InstanciarPrefabPorPath(nomePrefab, quantidade, playerController.corpoDissecando.GetComponent<StatsGeral>().dropPosition.transform.position,
				playerController.corpoDissecando.GetComponent<StatsGeral>().dropPosition.transform.rotation, drop.materialPersonalizado, playerController.PV.ViewID);
		}
		playerController.itemsDropsPosDissecar = new List<Item.ItemDropStruct>();
		if (PhotonNetwork.IsConnected) PhotonNetwork.Destroy(playerController.corpoDissecando);
		else GameObject.Destroy(playerController.corpoDissecando);
		playerController.corpoDissecando = null;
	}

	void AnimEventCapturou()
	{
		Debug.Log("capturou");
		if (playerController.animalCapturado == null) return;
		playerController.animalCapturado.isCapturado = true;
		playerController.animalCapturado.targetCapturador = this.gameObject;
		playerController.animalCapturado.objColeiraRope.SetActive(true);
		if(playerController.cordaWeaponTP != null)
        {
			playerController.cordaWeaponTP.ropeGrab.objFollowed = playerController.animalCapturado.objRopePivot.transform;
			playerController.cordaWeaponFP.ropeGrab.objFollowed = playerController.animalCapturado.objRopePivot.transform;
		}
	}

	void AnimEventReanimouJogador()
	{
		if (playerController.corpoReanimando != null)
		{
			playerController.corpoReanimando.GetComponent<MorteController>().ReanimarJogador();
		}
	}

	void AnimEventConsertado()
	{
		Debug.Log("consertou");
		playerController.objConsertando.GetComponent<ReconstruivelQuebrado>().ConsertarReconstruivel();
		playerController.objConsertando = null;
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
		if (playerController.inventario.itemNaMao == null) return;
		// Cria um ray que parte da posição da câmera na direção em que ela está apontando
		Ray ray = new Ray(transform.position, transform.forward);
		// Declara uma variável para armazenar o ponto em que o ray colide com a superfície
		RaycastHit hit;
		// Se o ray atingir alguma superfície, calcula a direção do arremesso
		if (Physics.Raycast(ray, out hit))
		{
			Vector3 direction = hit.point - transform.position;
			direction.Normalize();

			string nomePrefab = playerController.inventario.itemNaMao.tipoItem + "/" + playerController.inventario.itemNaMao.itemIdentifierAmount.ItemDefinition.name;
			GameObject meuObjLancado = ItemDrop.InstanciarPrefabPorPath(nomePrefab, 1, transform.position, Quaternion.LookRotation(direction), null, playerController.PV.ViewID);
			// Aplica a força na direção calculada
			meuObjLancado.GetComponent<Rigidbody>().AddForce(direction * throwForce, ForceMode.Impulse);
			//REMOVER ITEM DA MAO
			playerController.inventario.ConsumirItemDaMao();
		}
	}

	void AnimEventBebeuAgua()
	{
		playerController.statsJogador.setarSedeAtual(playerController.statsJogador.sedeAtual + 100);
	}

	void AnimEventBebeuGarrafa()
	{
		if (playerController.inventario.itemNaMao != null && playerController.inventario.itemNaMao.itemIdentifierAmount.ItemDefinition.name.Equals(playerController.inventario.itemGarrafa.name))
		{
			int qtdAguaBebendo = playerController.inventario.itemNaMao.GetComponent<Garrafa>().BeberAgua();
			if(qtdAguaBebendo == 0)
            {
				playerController.AlertarJogadorComMensagem(EnumMensagens.ObterAlertaGarrafaVazia());
            }
            else
            {
				Debug.Log("bebeu agua na garrafa");
				playerController.statsJogador.setarSedeAtual(playerController.statsJogador.sedeAtual + qtdAguaBebendo);
			}
		}
	}

	void AnimEventEncheuGarrafa()
	{
		if (playerController.inventario.itemNaMao != null && playerController.inventario.itemNaMao.itemIdentifierAmount.ItemDefinition.name.Equals(playerController.inventario.itemGarrafa.name))
		{
			playerController.inventario.itemNaMao.GetComponent<Garrafa>().EncherRepositorioComAgua();
		}
	}

	void AnimEventAcendendoFogueira()
	{
		if (playerController.fogueiraAcendendo == null) return;
		playerController.fogueiraAcendendo.GetComponent<Fogueira>().AcenderFogueira();
		playerController.fogueiraAcendendo = null;
		playerController.PararAbility(playerController.acenderFogueiraAbility);
		//playerController.inventario.ConsumirItemDaMao();
	}

	void AnimEventAcenderIsqueiro()
    {
		if (playerController.acendedorFogueiraFP != null)
		{
			playerController.acendedorFogueiraFP.AcenderFogo();
			playerController.acendedorFogueiraTP.AcenderFogo();
		}
	}

	void AnimEventApagarIsqueiro()
	{
		if (playerController.acendedorFogueiraFP != null)
		{
			playerController.acendedorFogueiraFP.ApagarFogo();
			playerController.acendedorFogueiraTP.ApagarFogo();
		}
	}

	void AnimEventApagandoFogueira()
	{
		if (playerController.fogueiraAcendendo == null) return;
		playerController.fogueiraAcendendo.GetComponent<Fogueira>().ApagarFogueira();
		playerController.fogueiraAcendendo = null;
	}

	void AnimEventAtivarPegandoPeixe()
	{
		Debug.Log("pegando peixe");
		playerController.varaDePescaTP.peixeDaVara.SetActive(true);
		playerController.varaDePescaFP.peixeDaVara.SetActive(true);
		playerController.varaDePescaTP.animator.SetTrigger("pegandoPeixe");
		playerController.varaDePescaFP.animator.SetTrigger("pegandoPeixe");
		playerController.pescaPescando.GetComponent<Pesca>().DesativarAreaDePesca();
	}

	public void TerminouDePescarComSucesso()
	{
		Debug.Log("terminou de pescar");
		if (playerController.pescaPescando == null) return;
		playerController.inventario.AdicionarItemAoInventario(null, playerController.inventario.itemPeixeCru, 1);
		playerController.characterLocomotion.TryStopAbility(playerController.pescarAbility);
	}

	void AnimEventColetouFruta()
	{
		if (playerController.arvoreColetando == null) return;
		List<Item.ItemDropStruct> itemDrops = new List<Item.ItemDropStruct>();
		foreach (Item.ItemDropStruct itemDropScruct in playerController.arvoreColetando.GetComponent<StatsGeral>().dropsItems)
		{
			if (itemDropScruct.tipoItem.Equals(Item.TiposItems.Consumivel.ToString()))
			{
				if (itemDropScruct.qtdMaxDrops > 0)
				{
					Debug.Log("coletou fruta: " + itemDropScruct.itemDefinition.name);
					playerController.inventario.AdicionarItemAoInventario(null, itemDropScruct.itemDefinition, 1);
					playerController.arvoreColetando.GetComponent<ArvoreFrutifera>().DesaparecerUmaFrutaDaArvore();
					Item.ItemDropStruct novo = new Item.ItemDropStruct();
					novo.itemDefinition = itemDropScruct.itemDefinition;
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
		playerController.arvoreColetando.GetComponent<StatsGeral>().dropsItems = itemDrops;
	}

	void AnimEventColetouItem()
	{
		if (playerController.itemDefinitionBaseColentando == null) return;
		Debug.Log("coletou item: " + playerController.itemDefinitionBaseColentando.name);
		playerController.inventario.AdicionarItemAoInventario(null, playerController.itemDefinitionBaseColentando, 1);
		playerController.itemDefinitionBaseColentando = null;
	}

}
