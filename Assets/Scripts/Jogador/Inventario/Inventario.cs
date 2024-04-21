using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Obi;
using Opsive.UltimateCharacterController.Inventory;
using Opsive.Shared.Utility;
using Opsive.UltimateCharacterController.Items;
using Opsive.Shared.Events;
using Opsive.Shared.Inventory;

public class Inventario : MonoBehaviour
{

    [SerializeField] public Inventory inventory;
    [SerializeField] public int pesoCapacidadeMaxima, qtdItensMaximo;
    [SerializeField] public TMP_Text txPesoInventario, txQtdItensInventario;
    [SerializeField][HideInInspector] public int pesoAtual, qtdItensAtual;
    [SerializeField] public GameObject canvasInventario, cameraInventario;
    [SerializeField] GameObject contentItensMochila;
    [SerializeField] public GameObject prefabItem;
    [SerializeField] public Hotbar hotbar;

    [SerializeField] public List<Item.ItemStruct> itensStruct;

    [HideInInspector] public List<Item> itens;
    [SerializeField] public Item itemNaMao;
    [SerializeField] [HideInInspector] public PlayerController playerController;
    [SerializeField] [HideInInspector] public StatsJogador statsJogador;
    [SerializeField] [HideInInspector] public CraftMaos craftMaos;
    [SerializeField] [HideInInspector] public ArrastarItensInventario arrastarItensInventario;

    [SerializeField] public TMP_Text txMsgLogItem, txMunicoesHud;
    [SerializeField] RawImage imgLogItem;
    [SerializeField] Texture texturaTransparente;

    [SerializeField] public ItemDefinitionBase itemBody, itemPeixeCru, itemGarrafa, itemPanela, itemTigela, itemMarteloReparador, 
        itemCorda, itemVaraDePesca, itemLanterna, itemFaca, itemIsqueiro;

    private void Awake()
    {
        itens = new List<Item>();
        playerController = GetComponentInParent<PlayerController>();
        statsJogador = GetComponentInParent<StatsJogador>();
        craftMaos = GetComponent<CraftMaos>();
        arrastarItensInventario = GetComponent<ArrastarItensInventario>();
    }

    void Start()
    {
        setarQtdItensAtual(0);
        foreach (Item item in itens) //Ativando os itens que tem quantidade no inventario e desativando os que nao tem quantidade
        {
            if (item.quantidade > 0)
            {
                setarQtdItensAtual(qtdItensAtual + 1);
                item.gameObject.SetActive(true);
            }
            else
            {
                item.gameObject.SetActive(false);
            }
            item.DeselecionarItem();
        }
        itemNaMao = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Inventario"))
        {
            ToggleInventario();
        }
        if (Input.GetButtonDown("Hide") || !playerController.characterHealth.IsAlive())
        {
            FecharInventario();
        }
    }

    public void setarQtdItensAtual(int valor)
    {
        qtdItensAtual = valor;
        txQtdItensInventario.text = PlayerPrefs.GetInt("INDEXIDIOMA") == 1 ? "Itens: " : "Items: ";
        txQtdItensInventario.text += qtdItensAtual + "/" + qtdItensMaximo;
        craftMaos.LimparTodosSlotsCraft();
    }

    public void ToggleInventario()
    {
        if (canvasInventario.activeSelf)
        {
            FecharInventario();
        }
        else
        {
            AbrirInventario();
        }
    }

    public void GuardarItemDaMao()
    {
        if(itemNaMao != null)
        {
            itemNaMao.DeselecionarItem();
        }
    }

    public void FecharInventario()
    {
        craftMaos.LimparTodosSlotsCraft();
        canvasInventario.SetActive(false);
        cameraInventario.SetActive(false);
        playerController.canMove = true;
        EventHandler.ExecuteEvent(playerController.gameObject, "OnEnableGameplayInput", true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void AbrirInventario()
    {
        if (!playerController.canMove) return;
        arrastarItensInventario.HoverNothing();
        canvasInventario.SetActive(true);
        cameraInventario.SetActive(true);
        playerController.canMove = false;
        EventHandler.ExecuteEvent(playerController.gameObject, "OnEnableGameplayInput", false);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public bool AdicionarItemAoInventarioPorNome(ItemDefinitionBase itemDefinition, int quantidadeResponse)
    {
        return AdicionarItemAoInventario(null, itemDefinition, quantidadeResponse);
    }

    public bool AdicionarItemAoInventario(ItemDrop itemDrop, ItemDefinitionBase itemDefinition, int quantidadeResponse)
    {
        foreach(Item.ItemStruct itemStruct in itensStruct)
        {
            if(itemStruct.itemIdentifierAmount.ItemDefinition.name.Equals(itemDefinition.name))
            {
                return AdicionarItemAoInventario(itemDrop, itemStruct, quantidadeResponse);
            }
        }
        return false;
    }

    public bool AdicionarItemAoInventario(ItemDrop itemDrop, Item.ItemStruct itemStructResponse, int quantidadeResponse)
    {
        if (itemDrop == null || !itemDrop.item.Equals(itemGarrafa)) //itens que nao stackam
        {
            foreach (Item item in itens)
            {
                if (item.itemIdentifierAmount.ItemDefinition.name.Equals(itemStructResponse.itemIdentifierAmount.ItemDefinition.name))
                {
                    return item.aumentarQuantidade(quantidadeResponse); //stackando item
                }
            }
        }

        //Adiciona um novo item na mochila
        GameObject novoObjeto = Instantiate(prefabItem, new Vector3(), new Quaternion(), contentItensMochila.transform);
        novoObjeto.transform.SetParent(contentItensMochila.transform);
        if (itemDrop != null && itemDrop.item.Equals(itemGarrafa))
        {
            novoObjeto.GetComponent<Garrafa>().Setup(itemDrop.GetComponent<Garrafa>());
        }
        Item novoItem = novoObjeto.GetComponent<Item>().setupItemFromItemStruct(itemStructResponse);
        itens.Add(novoItem);
        AlertarJogadorComLogItem(novoItem.obterNomeItemTraduzido(), novoItem.imagemItem.texture, true, quantidadeResponse);
        inventory.AddItemIdentifierAmount(novoItem.itemIdentifierAmount.ItemIdentifier, quantidadeResponse);
        return true;
    }

    public void RemoverItemsDoInventarioPorNome(ItemIdentifierAmount[] ingredientes)
    {
        foreach (ItemIdentifierAmount ingrediente in ingredientes)
        {
            inventory.RemoveItemIdentifierAmount(ingrediente.ItemIdentifier, ingrediente.Amount);
        }
    }

    public void RemoverItemDoInventarioPorNome(ItemDefinitionBase itemDefinition, int quantidadeResponse)
    {
        inventory.RemoveItemIdentifierAmount(itemDefinition.CreateItemIdentifier(), quantidadeResponse);
    }

    public void RemoverItemDoInventarioPorItemIdentifier(IItemIdentifier itemIdentifier, int quantidadeResponse)
    {
        inventory.RemoveItemIdentifierAmount(itemIdentifier, quantidadeResponse);
    }

    public void RemoverItemDoInventario(Item itemResponse, int quantidadeResponse)
    {
        inventory.RemoveItemIdentifierAmount(itemResponse.itemIdentifierAmount.ItemIdentifier, quantidadeResponse);
    }

    public void ConsumirItemDaMao()
    {
        ConsumirItemDaMao(false);
    }
    public void removendoItemDoInventarioPorNome(ItemDefinitionBase itemDefinition, int quantidadeResponse)
    {
        foreach (Item item in itens)
        {
            if (item.itemIdentifierAmount.ItemDefinition.name.Equals(itemDefinition.name))
            {
                item.diminuirQuantidade(quantidadeResponse, false);
                return;
            }
        }
    }

    public void ConsumirItemDaMao(bool isCordaPartindo)
    {
        if (itemNaMao == null) return;
       
        foreach (Item item in itens)
        {
            if (item.itemIdentifierAmount.ItemDefinition.name.Equals(itemNaMao.itemIdentifierAmount.ItemDefinition.name))
            {
                item.diminuirQuantidade(1, isCordaPartindo);
                return;
            }
        }
    }

    public bool VerificarQtdItems(ItemIdentifierAmount[] ingredientes, bool alertar)
    {
        foreach(ItemIdentifierAmount ingrediente in ingredientes)
        {
            if(!VerificarQtdItem(ingrediente.ItemDefinition, ingrediente.Amount, alertar))
            {
                return false;
            }
        }
        return true;
    }

    public bool VerificarQtdItem(ItemDefinitionBase itemDefinition, int quantidadeNecessaria, bool alertar){
        if(quantidadeNecessaria <= 0) return true;
        int qtdItemAtual = 0;
        string nomeItem = ObterNomeItemTraduzidoPorItemDefinition(itemDefinition);
        foreach(Item item in itens){
            if(item.itemIdentifierAmount.ItemDefinition.name.Equals(itemDefinition.name))
            {
                qtdItemAtual = item.quantidade;
                if(item.quantidade >= quantidadeNecessaria)
                {
                    return true;
                }
            }
        }
        if(alertar) playerController.AlertarJogadorComMensagem(EnumMensagens.ObterAlertaNaoPossuiMaterialSuficiente(nomeItem, qtdItemAtual, quantidadeNecessaria));
        return false;
    }

    public Item.ItemStruct ObterItemStructPeloNome(ItemDefinitionBase itemDefinition)
    {
        foreach(Item.ItemStruct itemSt in itensStruct){
            if(itemSt.itemIdentifierAmount.ItemDefinition.name.Equals(itemDefinition.name))
            {
                return itemSt;
            }
        }
        return itensStruct[0];
    }

    
    public void ToggleGrabUngrabCorda(bool isCordaPartindo)
    {
        if (!playerController.cordaWeaponFP.objObiRope.activeSelf) //Grabando Animal
        {
            playerController.cordaWeaponFP.objCordaMaos.SetActive(false);
            playerController.cordaWeaponTP.objCordaMaos.SetActive(false);
            playerController.cordaWeaponFP.objObiRope.SetActive(true);
            playerController.cordaWeaponTP.objObiRope.SetActive(true);
        }
        else //Ungrab Animal
        {
            UngrabCoisasCapturadasComCorda(isCordaPartindo);
        }
    }

    public void UngrabCoisasCapturadasComCorda(bool isCordaPartindo)
    {
        UngrabAnimalCapturado(isCordaPartindo);
        UngrabObjetoCapturado();
    }

    public void UngrabAnimalCapturado(bool isCordaPartindo)
    {
        Debug.LogWarning("UngrabAnimalCapturado 4");
        if (!isCordaPartindo)
        {
            if (playerController.cordaWeaponTP != null)
            {
                if(playerController.cordaWeaponTP.objObiRope != null)
                {
                    playerController.cordaWeaponTP.objObiRope.SetActive(false);
                    playerController.cordaWeaponFP.objObiRope.SetActive(false);
                }
                playerController.cordaWeaponFP.objCordaMaos.SetActive(true);
                playerController.cordaWeaponTP.objCordaMaos.SetActive(true);
            }
        }
        if (playerController.animalCapturado != null)
        {
            playerController.animalCapturado.objColeiraRope.SetActive(false);
            playerController.animalCapturado.isCapturado = false;
            playerController.animalCapturado.targetCapturador = null;
            playerController.animalCapturado.agent.ResetPath();
            playerController.animalCapturado = null;
            if(playerController.cordaWeaponTP != null)
            {
                playerController.cordaWeaponTP.ropeGrab.objFollowed = null;
                playerController.cordaWeaponFP.ropeGrab.objFollowed = null;
            }
        }
    }

    public void UngrabObjetoCapturado()
    {
        Debug.LogWarning("UngrabObjetoCapturado");
        if (playerController.objCapturado != null)
        {
            playerController.objCapturado.GetComponent<ObjetoGrab>().DesativarCordaGrab();
            playerController.objCapturado = null;
            if (playerController.cordaWeaponTP != null)
            {
                playerController.cordaWeaponFP.objCordaSemGrab.SetActive(true);
                playerController.cordaWeaponTP.objCordaSemGrab.SetActive(true);
            }
        }
    }

    public void SumirObjRopeStart()
    {
        Debug.LogWarning("SumirObjRopeStart");
        if (playerController.cordaWeaponFP != null && playerController.cordaWeaponTP != null)
        {
            playerController.cordaWeaponTP.AcoesRenovarCordaEstourada(false);
            playerController.cordaWeaponFP.AcoesRenovarCordaEstourada(false);
        }
    }

    public string ObterNomeItemTraduzidoPorItemDefinition(ItemDefinitionBase itemDefinition)
    {
        foreach(Item.ItemStruct itemStruct in itensStruct)
        {
            if (itemDefinition.name == itemStruct.itemIdentifierAmount.ItemDefinition.name)
            {
                return PlayerPrefs.GetInt("INDEXIDIOMA") == 1 ? itemStruct.nomePortugues : itemStruct.nomeIngles;
            }
        }
        return "";
    }

    public void AtualizarHudMunicoesComArmaAtual()
    {
        this.txMunicoesHud.text = "";
        if (itemNaMao != null && itemNaMao.tipoMunicao != null)
        {
            int qtdMunicaoNoInventario = ObterQtdItemNoInventario(itemNaMao.itemIdentifierAmount.ItemDefinition);
            this.txMunicoesHud.text = "0" + "/" + qtdMunicaoNoInventario;
        }
    }

    public int ObterQtdItemNoInventario(ItemDefinitionBase itemDefinition)
    {
        int qtdItemAtual = 0;
        foreach (Item item in itens)
        {
            if (item.itemIdentifierAmount.ItemDefinition.name.Equals(itemDefinition.name))
            {
                qtdItemAtual += item.quantidade;
                break; //Tirar break se as municoes poderem ficar em slots diferentes
            }
        }
        return qtdItemAtual;
    }

    public void AlertarJogadorComLogItem(string nomeItemTraduzido, Texture imgItem, bool isAumentandoQtd, int quantidade)
    {
        CancelInvoke("SumirLogItem");
        string texto = "";
        if (isAumentandoQtd)
        {
            texto += "+";
            txMsgLogItem.color = Color.green;
        }
        else
        {
            texto += "-";
            txMsgLogItem.color = Color.red;
        }
        texto += quantidade + " ";
        texto += nomeItemTraduzido;
        txMsgLogItem.text = texto;
        imgLogItem.texture = imgItem;
        Invoke("SumirLogItem", 1);
    }

    void SumirLogItem()
    {
        txMsgLogItem.text = "";
        imgLogItem.texture = texturaTransparente;
    }

}
