using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class EventsAnimJogador : MonoBehaviourPunCallbacks
{

	[SerializeField] PlayerController playerController;

	void AnimEventComeu()
	{
		if (playerController.inventario.itemNaMao == null)
		{
			return;
		}
		playerController.inventario.itemNaMao.UsarItem();
	}

	void AnimEventDissecado()
	{
		foreach (Item.ItemDropStruct drop in playerController.itemsDropsPosDissecar)
		{
			int quantidade = Random.Range(drop.qtdMinDrops, drop.qtdMaxDrops);
			string nomePrefab = drop.tipoItem + "/" + drop.itemDefinition.name;
			string prefabPath = Path.Combine("Prefabs/ItensInventario/", nomePrefab);
			ItemDrop.InstanciarPrefabPorPath(prefabPath, quantidade, playerController.corpoDissecando.GetComponent<StatsGeral>().dropPosition.transform.position,
				playerController.corpoDissecando.GetComponent<StatsGeral>().dropPosition.transform.rotation, drop.materialPersonalizado, playerController.PV.ViewID);
		}
		playerController.itemsDropsPosDissecar = new List<Item.ItemDropStruct>();
		if (PhotonNetwork.IsConnected) PhotonNetwork.Destroy(playerController.corpoDissecando);
		else GameObject.Destroy(playerController.corpoDissecando);
		playerController.corpoDissecando = null;
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
		playerController.objConsertando.GetComponent<ReconstruivelQuebrado>().ConsertarReconstruivel();
		playerController.objConsertando = null;
	}

	void AnimEventArremessoLanca()
	{
		ArremessarItemNaMao();
	}

	// For�a do arremesso
	public float throwForce = 300f;
	private void ArremessarItemNaMao() // Fun��o que arremessa o objeto na dire��o da c�mera
	{
		if (playerController.inventario.itemNaMao == null) return;
		// Cria um ray que parte da posi��o da c�mera na dire��o em que ela est� apontando
		Ray ray = new Ray(transform.position, transform.forward);
		// Declara uma vari�vel para armazenar o ponto em que o ray colide com a superf�cie
		RaycastHit hit;
		// Se o ray atingir alguma superf�cie, calcula a dire��o do arremesso
		if (Physics.Raycast(ray, out hit))
		{
			Vector3 direction = hit.point - transform.position;
			direction.Normalize();

			string nomePrefab = playerController.inventario.itemNaMao.tipoItem + "/" + playerController.inventario.itemNaMao.itemIdentifierAmount.ItemDefinition.name;
			string prefabPath = Path.Combine("Prefabs/ItensInventario/", nomePrefab);
			GameObject meuObjLancado = ItemDrop.InstanciarPrefabPorPath(prefabPath, 1, transform.position, Quaternion.LookRotation(direction), null, playerController.PV.ViewID);
			// Aplica a for�a na dire��o calculada
			meuObjLancado.GetComponent<Rigidbody>().AddForce(direction * throwForce, ForceMode.Impulse);
			//REMOVER ITEM DA MAO
			playerController.inventario.ConsumirItemDaMao();
		}
	}

	void AnimEventBebeuAgua()
	{
		playerController.statsJogador.setarSedeAtual(playerController.statsJogador.sedeAtual + 100);
		playerController.PararAbility(playerController.beberAguaRioAbility);
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
		playerController.varaDePescaTP.peixeDaVara.SetActive(true);
		playerController.varaDePescaFP.peixeDaVara.SetActive(true);
		playerController.varaDePescaTP.animator.SetTrigger("pegandoPeixe");
		playerController.varaDePescaFP.animator.SetTrigger("pegandoPeixe");
		playerController.pescaPescando.GetComponent<Pesca>().DesativarAreaDePesca();
	}

	public void TerminouDePescarComSucesso()
	{
		if (playerController.pescaPescando == null) return;
		if(!playerController.inventario.AdicionarItemAoInventario(null, playerController.inventario.itemPeixeCru, 1))
        {
			//TODO: DROPAR PEIXE NO CHAO
        }
		playerController.characterLocomotion.TryStopAbility(playerController.pescarAbility);
	}

	void AnimEventColetouItem()
	{
		if (playerController.itemDefinitionBaseColentando == null) return;
		if(!playerController.inventario.AdicionarItemAoInventario(null, playerController.itemDefinitionBaseColentando, 1))
        {
			Debug.Log("AnimEventColetouItem: inventario cheio");
		}
		playerController.itemDefinitionBaseColentando = null;
	}

}
