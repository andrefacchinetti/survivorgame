using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Obi;
using Opsive.UltimateCharacterController.Inventory;
using Opsive.Shared.Utility;
using Opsive.UltimateCharacterController.Items;
using Opsive.Shared.Inventory;

public class Inventario : MonoBehaviour
{

    [SerializeField] public Inventory inventory;
    [SerializeField] public int pesoCapacidadeMaxima, qtdItensMaximo;
    [SerializeField] public TMP_Text txPesoInventario, txQtdItensInventario;
    [SerializeField][HideInInspector] public int pesoAtual, qtdItensAtual;
    [SerializeField] public GameObject canvasInventario, cameraInventario;
    [SerializeField] GameObject contentItensMochila;
    [SerializeField] public GameObject prefabItem, prefabCorda;
    [SerializeField] public Hotbar hotbar;

    [SerializeField] public List<Item.ItemStruct> itensStruct;

    [HideInInspector] public List<Item> itens;
    [SerializeField][HideInInspector] public Item itemNaMao;
    [SerializeField] [HideInInspector] public PlayerController playerController;
    [SerializeField] [HideInInspector] public StatsJogador statsJogador;
    [SerializeField] [HideInInspector] public CraftMaos craftMaos;

    [SerializeField] TMP_Text txMsgLogItem;
    [SerializeField] RawImage imgLogItem;
    [SerializeField] Texture texturaTransparente;

    [SerializeField] public ItemDefinitionBase itemBody, itemPeixeCru, itemBottle, itemPanela, itemTigela, itemRepairHammer, itemRope, itemFishingRod, itemFlashlight, itemKnife;

    private void Awake()
    {
        itens = new List<Item>();
        playerController = GetComponentInParent<PlayerController>();
        statsJogador = GetComponentInParent<StatsJogador>();
        craftMaos = GetComponent<CraftMaos>();
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
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventario();
        }
        if (Input.GetKeyDown(KeyCode.Escape) || playerController.statsGeral.isDead)
        {
            FecharInventario();
        }
        VerificarCordaPartindo();
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
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void AbrirInventario()
    {
        if (!playerController.canMove) return;
        canvasInventario.SetActive(true);
        cameraInventario.SetActive(true);
        playerController.canMove = false;
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
        if (itemDrop == null || !itemDrop.item.name.Equals(itemBottle.name)) //itens que nao stackam
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
        if (itemDrop != null && itemDrop.item.name.Equals(itemBottle.name))
        {
            novoObjeto.GetComponent<Garrafa>().Setup(itemDrop.GetComponent<Garrafa>());
        }
        Item novoItem = novoObjeto.GetComponent<Item>().setupItemFromItemStruct(itemStructResponse);
        itens.Add(novoItem);
        AlertarJogadorComLogItem(novoItem.obterNomeItemTraduzido(), novoItem.imagemItem.texture, true, quantidadeResponse);
        inventory.AddItemIdentifierAmount(novoItem.itemIdentifierAmount.ItemIdentifier, quantidadeResponse);
        return true;
    }

    public void RemoverItemDoInventarioPorNome(ItemDefinitionBase itemDefinition, int quantidadeResponse)
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

    public void RemoverItemDoInventario(Item itemResponse, int quantidadeResponse)
    {
        itemResponse.diminuirQuantidade(quantidadeResponse, false);
    }

    public void RemoverItemDaMao()
    {
        RemoverItemDaMao(false);
    }

    public void RemoverItemDaMao(bool isCordaPartindo)
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

    public bool VerificarQtdItem(ItemDefinitionBase itemDefinition, int quantidade, bool alertar){
        if(quantidade<=0) return true;
        int qtdItemAtual = 0;
        string nomeItem = "";
        foreach(Item item in itens){
            if(item.itemIdentifierAmount.ItemDefinition.name.Equals(itemDefinition.name))
            {
                qtdItemAtual = item.quantidade;
                nomeItem = item.obterNomeItemTraduzido();
                if(item.quantidade >= quantidade)
                {
                    return true;
                }
            }
        }
        if(alertar) playerController.AlertarJogadorComMensagem(EnumMensagens.ObterAlertaNaoPossuiMaterialSuficiente(nomeItem, qtdItemAtual, quantidade));
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

    [SerializeField] public GameObject objObiSolver, objObiRope, objCordaMao, objCordaSemGrab;
    public void ToggleGrabUngrabCorda(bool isCordaPartindo)
    {
        if (!objObiRope.activeSelf) //Grabando Animal
        {
            objCordaMao.SetActive(false);
            objObiRope.SetActive(true);
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
        Debug.Log("UngrabAnimalCapturado");
        if (!isCordaPartindo)
        {
            objObiRope.SetActive(false);
            objCordaMao.SetActive(true);
        }
        if (playerController.animalCapturado != null)
        {
            playerController.animalCapturado.objColeiraRope.SetActive(false);
            playerController.animalCapturado.isCapturado = false;
            playerController.animalCapturado.targetCapturador = null;
            playerController.animalCapturado.agent.ResetPath();
            playerController.animalCapturado = null;
            playerController.ropeGrab.objFollowed = null;
        }
    }

    public void UngrabObjetoCapturado()
    {
        Debug.Log("UngrabObjetoCapturado");
        if (playerController.objCapturado != null)
        {
            playerController.objCapturado.GetComponent<ObjetoGrab>().DesativarCordaGrab();
            playerController.objCapturado = null;
            objCordaSemGrab.SetActive(true);
        }
    }

    public void SumirObjRopeStart()
    {
        Debug.Log("SumirObjRopeStart");
        AcoesRenovarCordaEstourada(true);
    }

    public void AcoesRenovarCordaEstourada(bool isCordaPartindo)
    {
        Debug.Log("sumindo corda");
        CancelInvoke("SumirObjRopeStart");
        Transform positionRope = objObiRope.gameObject.transform;
        GameObject novaCorda = Instantiate(prefabCorda, positionRope.position, positionRope.rotation, objObiSolver.transform);
        ropeEstoura = novaCorda.GetComponent<RopeEstoura>();
        ropeEstoura.playerController = playerController;
    
        ObiParticleAttachment[] attachs = novaCorda.GetComponents<ObiParticleAttachment>();
        foreach(ObiParticleAttachment attach in attachs)
        {
            if(attach.particleGroup.name == "START1")
            {
                attach.target = pivotRopeStart.transform;
            }
            else
            {
                attach.target = pivotRopeEnd.transform;
            }
        }
        Destroy(objObiRope.gameObject);
        objObiRope = novaCorda;
        objObiRope.SetActive(false);
        UngrabCoisasCapturadasComCorda(isCordaPartindo);
    }

    [SerializeField] public RopeEstoura ropeEstoura;
    [SerializeField] public GameObject pivotRopeStart, pivotRopeEnd;
    private void VerificarCordaPartindo()
    {
        if (ropeEstoura.isCordaPartida && !ropeEstoura.isCordaEstourou)
        {
            Debug.Log("VerificarCordaPartindo");
            RemoverItemDaMao(true);
            ropeEstoura.isCordaEstourou = true;
            Invoke("SumirObjRopeStart", 1f);
            //TODO: Sound de corda partindo
        }
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

    public ItemObjMao ObterGameObjectItemNaMao()
    {
        //TODO: IMPLEMENTAR COM API INVENTORY
        return null;
    }

}
