using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.Serialization;
using Photon.Pun;
using UnityEngine.EventSystems;
using Opsive.Shared.Inventory;
using Opsive.UltimateCharacterController.Inventory;

public class Item : MonoBehaviourPunCallbacks
{
    [SerializeField] public TiposItems tipoItem;
    [SerializeField] public string nomePortugues, nomeIngles;
    [SerializeField] public ItemIdentifierAmount itemIdentifierAmount;
    [SerializeField] public int groupIndex;
    [SerializeField] public bool isConsumivel;
    [SerializeField] public int curaSede, curaFome, curaVida;
    [SerializeField] public int quantidade = 0, clicks;

    [SerializeField] public Inventario inventario;
    [SerializeField] public Hotbar hotbar;
    [SerializeField] public Armaduras armaduras;
    [SerializeField] public ArrastarItensInventario arrastarItensInventario;

    //PADRAO
    [SerializeField] public RawImage imagemItem;
    [SerializeField] public TMP_Text txQuantidade, txNomeItem;
    PhotonView PV;
    private float lastTimeClicked;


    public enum TiposItems
    {
        Nenhum,
        Ferramenta,
        Arma,
        Consumivel,
        Recurso,
        Armadura,
        Objeto,
        Municao,
        ConsumivelCozinha
    }

    [System.Serializable]
    public struct ItemDropStruct
    {
        public ItemDefinitionBase itemDefinition;
        public TiposItems tipoItem;
        public Material materialPersonalizado;
        public int qtdMinDrops;
        public int qtdMaxDrops;
    }

    [System.Serializable]
    public struct ItemStruct
    {
        public TiposItems tipoItem;
        public ItemIdentifierAmount itemIdentifierAmount;
        public int groupIndex;
        public string nomePortugues, nomeIngles;
        public bool isConsumivel;
        public int curaSede, curaFome, curaVida;
        public Texture textureImgItem;
        public GameObject objInventario;
    }

    public Item setupItemFromItemStruct(ItemStruct itemResponse)
    {
        tipoItem = itemResponse.tipoItem;
        nomePortugues = itemResponse.nomePortugues;
        nomeIngles = itemResponse.nomeIngles;
        itemIdentifierAmount = itemResponse.itemIdentifierAmount;
        groupIndex = itemResponse.groupIndex;
        isConsumivel = itemResponse.isConsumivel;
        curaSede = itemResponse.curaSede;
        curaFome = itemResponse.curaFome;
        curaVida = itemResponse.curaVida;
        quantidade = 1;
        imagemItem.texture = itemResponse.textureImgItem;
        inventario = itemResponse.objInventario.GetComponent<Inventario>();
        hotbar = itemResponse.objInventario.GetComponent<Hotbar>();
        armaduras = itemResponse.objInventario.GetComponent<Armaduras>();
        arrastarItensInventario = itemResponse.objInventario.GetComponent<ArrastarItensInventario>();
        txNomeItem.text = PlayerPrefs.GetInt("INDEXIDIOMA") == 1 ? itemResponse.nomePortugues : itemResponse.nomeIngles;
        txQuantidade.text = quantidade + "";
        return this;
    }

    public string obterNomeItemTraduzido()
    {
        return PlayerPrefs.GetInt("INDEXIDIOMA") == 1 ? nomePortugues : nomeIngles;
    }

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    private void Start()
    {
        txNomeItem.text = obterNomeItemTraduzido();
        txQuantidade.text = quantidade + "";
        EventTrigger trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        EventTrigger.Entry entry2 = new EventTrigger.Entry();
        EventTrigger.Entry entry3 = new EventTrigger.Entry();
        EventTrigger.Entry entry4 = new EventTrigger.Entry();
        EventTrigger.Entry entry5 = new EventTrigger.Entry();
        EventTrigger.Entry entry6 = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.BeginDrag;
        entry2.eventID = EventTriggerType.EndDrag;
        entry3.eventID = EventTriggerType.Drop;
        entry4.eventID = EventTriggerType.PointerEnter;
        entry5.eventID = EventTriggerType.PointerExit;
        entry6.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((data) => { OnBeginDragDelegate((PointerEventData)data); });
        entry2.callback.AddListener((data) => { OnEndDragDelegate((PointerEventData)data); });
        entry3.callback.AddListener((data) => { OnDropDelegate((PointerEventData)data); });
        entry4.callback.AddListener((data) => { OnPointerEnterDelegate((PointerEventData)data);});
        entry5.callback.AddListener((data) => { OnPointerExitDelegate((PointerEventData)data);});
        entry6.callback.AddListener((data) => { OnPointerDownDelegate((PointerEventData)data);});
        trigger.triggers.Add(entry);
        trigger.triggers.Add(entry2);
        trigger.triggers.Add(entry3);
        trigger.triggers.Add(entry4);
        trigger.triggers.Add(entry5);
        trigger.triggers.Add(entry6);
    }

    public void DeselecionarItem()
    {
        inventario.itemNaMao = null;
        if (itemIdentifierAmount.ItemDefinition.Equals(inventario.itemCorda))
        {
            inventario.UngrabAnimalCapturado(false);
            inventario.UngrabObjetoCapturado();
        }
        UnequipItemInventory();
    }

    public void SelecionarItem()
    {
        if (this.itemIdentifierAmount.ItemDefinition.GetItemCategory().name.Equals("NaoEquipavel")) return; //Esse tipo de item não pode ser equipado
        if (inventario.itemNaMao != null)
        {
            if (inventario.itemNaMao.itemIdentifierAmount.ItemDefinition.Equals(this.itemIdentifierAmount.ItemDefinition))
            {
                inventario.itemNaMao.DeselecionarItem();
                return;
            }
            else
            {
                inventario.itemNaMao.DeselecionarItem();
            }
        }

        if(quantidade > 0)
        {
            inventario.itemNaMao = this;
            EquipItemInventory();
            if (itemIdentifierAmount.ItemDefinition.Equals(inventario.itemVaraDePesca)) {
                inventario.playerController.varaDePescaTP = inventario.playerController.contentItemsTP.GetComponentInChildren<VaraDePesca>();
                inventario.playerController.varaDePescaFP = inventario.playerController.contentItemsFP.GetComponentInChildren<VaraDePesca>();
                if (inventario.playerController.varaDePescaTP != null && inventario.playerController.varaDePescaFP != null)
                {
                    inventario.playerController.varaDePescaTP.eventsAnimJogador = inventario.playerController.eventsAnimJogador;
                    inventario.playerController.varaDePescaFP.eventsAnimJogador = inventario.playerController.eventsAnimJogador;
                    inventario.playerController.varaDePescaTP.peixeDaVara.SetActive(false); 
                    inventario.playerController.varaDePescaFP.peixeDaVara.SetActive(false);
                }
            }
            else if (itemIdentifierAmount.ItemDefinition.Equals(inventario.itemAcendedorFogueira))
            {
                inventario.playerController.acendedorFogueiraTP = inventario.playerController.contentItemsTP.GetComponentInChildren<AcendedorFogueira>();
                inventario.playerController.acendedorFogueiraFP = inventario.playerController.contentItemsFP.GetComponentInChildren<AcendedorFogueira>();
                if (inventario.playerController.acendedorFogueiraTP != null && inventario.playerController.acendedorFogueiraFP != null)
                {
                    inventario.playerController.acendedorFogueiraTP.eventsAnimJogador = inventario.playerController.eventsAnimJogador;
                    inventario.playerController.acendedorFogueiraFP.eventsAnimJogador = inventario.playerController.eventsAnimJogador;
                }
            }
            else if (itemIdentifierAmount.ItemDefinition.Equals(inventario.itemLanterna) && !inventario.VerificarQtdItem(itemIdentifierAmount.ItemDefinition, 2, false) 
                && armaduras.slotLanterna.item != null)
            {
                armaduras.slotLanterna.objEquipLanterna.SetActive(false);
                armaduras.slotLanterna.item = null;
            }
        }

        if (itemIdentifierAmount.ItemDefinition.Equals(inventario.itemCorda))
        {
            inventario.playerController.cordaWeaponFP = inventario.playerController.contentItemsTP.GetComponentInChildren<CordaWeapon>();
            inventario.playerController.cordaWeaponTP = inventario.playerController.contentItemsFP.GetComponentInChildren<CordaWeapon>();

            if (inventario.playerController.cordaWeaponFP != null && inventario.playerController.cordaWeaponTP != null)
            {
                Debug.LogWarning("equipando corda 1");
                inventario.playerController.cordaWeaponTP.playerController = inventario.playerController;
                inventario.playerController.cordaWeaponFP.playerController = inventario.playerController;
                inventario.playerController.cordaWeaponTP.AcoesRenovarCordaEstourada(false);
                inventario.playerController.cordaWeaponFP.AcoesRenovarCordaEstourada(false);
            }
        }

    }

    private void EquipItemInventory()
    {
        Debug.Log("equipou com sucesso");
        inventario.inventory.GetComponent<ItemSetManagerBase>().EquipItem(itemIdentifierAmount.ItemIdentifier, groupIndex, true, true);
        inventario.playerController.PararAbilitys();
    }
    
    private void UnequipItemInventory()
    {
        Debug.Log("unequipou com sucesso");
        inventario.inventory.GetComponent<ItemSetManagerBase>().UnEquipItem(itemIdentifierAmount.ItemIdentifier, groupIndex, true, true);
        IItemIdentifier itemIdBody = inventario.inventory.DefaultLoadout[0].ItemIdentifier;
        inventario.inventory.GetComponent<ItemSetManagerBase>().EquipItem(itemIdBody, groupIndex, true, true);
        inventario.playerController.PararAbilitys();
    }

    public void DroparItem()
    {
        int quantidade = 1;
        if (this.tipoItem.Equals(TiposItems.Nenhum)) return;
        string nomePrefab = this.tipoItem + "/"+ this.itemIdentifierAmount.ItemDefinition.name.ToString();
        GameObject objDropado = ItemDrop.InstanciarPrefabPorPath(nomePrefab, 1, new Vector3(transform.root.position.x, 
            transform.root.position.y+1, transform.root.position.z) + transform.root.forward , transform.root.rotation, null, PV.ViewID);
        if (this.itemIdentifierAmount.ItemDefinition.Equals(inventario.itemGarrafa))
        {
            objDropado.GetComponent<Garrafa>().Setup(this.GetComponent<Garrafa>());
        }
        inventario.RemoverItemDoInventario(this, quantidade); //TODO: implementar opcao de dropar itens em quantidade
        inventario.inventory.RemoveItemIdentifierAmount(itemIdentifierAmount.ItemIdentifier, quantidade);
    }

    public void UsarItem() //Usa item qdo aperta o botoa do mouse com o item na mao
    {
        if (quantidade <= 0) return;
        if (isConsumivel)
        {
            aplicarEfeitoConsumivel();
            diminuirQuantidade(1);
        }
    }

    private void aplicarEfeitoConsumivel()
    {
        inventario.statsJogador.setarSedeAtual(inventario.statsJogador.sedeAtual + curaSede);
        inventario.statsJogador.setarFomeAtual(inventario.statsJogador.fomeAtual + curaFome);
        inventario.statsJogador.TakeHealHealth(curaVida);
    }

    public void diminuirQuantidade(int valorQtd)
    {
        diminuirQuantidade(valorQtd, false);
    }
    public void diminuirQuantidade(int quantidadeResponse, bool isCordaPartindo)
    {
        quantidade -= quantidadeResponse;

        if (inventario.itemNaMao != null && inventario.itemNaMao.itemIdentifierAmount.ItemDefinition.Equals(inventario.itemCorda))
        {
            inventario.ToggleGrabUngrabCorda(isCordaPartindo);
            inventario.UngrabObjetoCapturado();
            SetarItemNaMaoNull();
        }
        if (quantidade <= 0)
        {
            quantidade = 0;
            inventario.setarQtdItensAtual(inventario.qtdItensAtual - 1);
            desativarOuAtivarUsoItemDaHotbar(true);
            if(inventario.itemNaMao != null && inventario.itemNaMao.itemIdentifierAmount.ItemDefinition.name.Equals(this.itemIdentifierAmount.ItemDefinition.name))
            {
                SetarItemNaMaoNull();
            }
        }
        
        if (armaduras.slotAljava.item != null && this.itemIdentifierAmount.ItemDefinition.name.Equals(armaduras.slotAljava.item.itemIdentifierAmount.ItemDefinition.name))
        {
            armaduras.slotAljava.SetupItemNoSlot(this);
        }
        txQuantidade.text = quantidade + "";

        if (quantidade <= 0)
        {
            RemoverItemDaMochila();
        }
        inventario.AlertarJogadorComLogItem(this.obterNomeItemTraduzido(), imagemItem.texture, false, quantidadeResponse);
    }

    private void SetarItemNaMaoNull()
    {
        inventario.itemNaMao = null;
        if (inventario.playerController.cordaWeaponTP != null && inventario.playerController.cordaWeaponTP.objObiRope != null)
        {
            inventario.playerController.cordaWeaponTP.objObiRope.SetActive(false);
            inventario.playerController.cordaWeaponFP.objObiRope.SetActive(false);
        }
        UnequipItemInventory();
    }

    private void RemoverItemDaMochila()
    {
        inventario.itens.Remove(this);
        GameObject.Destroy(this.gameObject);
    }

    public bool aumentarQuantidade(int quantidadeResponse) ///INVENTORY PICKUP ITEM
    {
        if (!gameObject.activeSelf && inventario.qtdItensAtual >= inventario.qtdItensMaximo)
        {
            Debug.Log("Inventario cheio");
            return false;
        }
        else
        {
            //AUMENTAR QUANTIDADE INVENTORY
            inventario.inventory.AddItemIdentifierAmount(itemIdentifierAmount.ItemIdentifier, quantidadeResponse);
            if (quantidade <= 0)
            {
                desativarOuAtivarUsoItemDaHotbar(false);
                gameObject.transform.SetAsLastSibling();
            }
            inventario.setarQtdItensAtual(inventario.qtdItensAtual + quantidadeResponse);
            quantidade += quantidadeResponse;
            txQuantidade.text = quantidade + "";
            gameObject.SetActive(true);
            inventario.AlertarJogadorComLogItem(this.obterNomeItemTraduzido(), imagemItem.texture, true, quantidadeResponse);
            return true;
        }
    }

    private void desativarOuAtivarUsoItemDaHotbar(bool isDesativando)
    {
        foreach (SlotHotbar slot in hotbar.slots)
        {
            if (slot.item != null && slot.item.itemIdentifierAmount.ItemDefinition.name.Equals(this.itemIdentifierAmount.ItemDefinition.name))
            {
                slot.objEmbacarImg.SetActive(isDesativando);
            }
        }
    }

    public void OnBeginDragDelegate(PointerEventData data){
        arrastarItensInventario.DragStartItemInventario(this, gameObject);
    }

    public void OnEndDragDelegate(PointerEventData data){
        arrastarItensInventario.StopDrag();
    }

    public void OnDropDelegate(PointerEventData data)
    {
        arrastarItensInventario.TrocarLugarInventario(gameObject);
    }
    
    public void OnPointerEnterDelegate(PointerEventData data){
        arrastarItensInventario.HoverItem(this);
    }

    public void OnPointerExitDelegate(PointerEventData data){
        arrastarItensInventario.HoverNothing();
    }

    public void OnPointerDownDelegate(PointerEventData data){
        if(data.button == PointerEventData.InputButton.Left){
            if(Time.time <= lastTimeClicked +0.5f){
                clicks++;
                
                if(clicks == 2){
                    SelecionarItem();
                    clicks = 0;
                }
            }
            else{
                clicks = 1;
            }
            lastTimeClicked = Time.time;
        }
    }

}
