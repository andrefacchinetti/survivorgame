using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.Serialization;
using Photon.Pun;
using UnityEngine.EventSystems;
using Opsive.Shared.Inventory;
using Opsive.UltimateCharacterController.Inventory;
using System.IO;

public class Item : MonoBehaviourPunCallbacks
{
    //VARIAVEIS CRIADAS VIA ItemStruct
    [SerializeField] public TiposItems tipoItem;
    [SerializeField] public string nomePortugues, nomeIngles;
    [SerializeField] public ItemIdentifierAmount itemIdentifierAmount;
    [SerializeField] public ItemDefinitionBase tipoMunicao;
    [SerializeField] public int groupIndex;
    [SerializeField] public bool isConsumivel;
    [SerializeField] public EstadoConsumivel estadoConsumivel;
    [SerializeField] public int curaSede, curaFome, curaVida;
    [SerializeField] public bool isCuraIndigestao, isCuraInfeccao, isCuraFratura, isCuraSangramento;
    [SerializeField] public int quantidade = 0;
    [SerializeField] public int peso = 0;

    [SerializeField] public Inventario inventario;
    [SerializeField] public Hotbar hotbar;
    [SerializeField] public Armaduras armaduras;
    [SerializeField] public ArrastarItensInventario arrastarItensInventario;
    //FIM VARIAVEIS CRIADAS VIA ItemStruct

    //PADRAO
    [SerializeField] public RawImage imagemItem;
    [SerializeField] public TMP_Text txQuantidade, txNomeItem;
    PhotonView PV;
    private float lastTimeClicked;
    [HideInInspector] public int clicks;


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
        public GameObject[] prefabDropMarks;
        public Material materialPersonalizado;
        public int qtdMinDrops;
        public int qtdMaxDrops;
    }

    [System.Serializable]
    public struct ObjDropStruct
    {
        public GameObject[] prefabDropMarks;
        public string prefabPath;
    }

    [System.Serializable]
    public struct ItemStruct
    {
        public TiposItems tipoItem;
        public ItemIdentifierAmount itemIdentifierAmount;
        public int peso;
        public ItemDefinitionBase tipoMunicao;
        public int groupIndex;
        public string nomePortugues, nomeIngles;
        public bool isConsumivel;
        public EstadoConsumivel estadoConsumivel;
        public int curaSede, curaFome, curaVida;
        public bool isCuraIndigestao, isCuraInfeccao, isCuraFratura, isCuraSangramento;
        public Texture textureImgItem;
        public GameObject objInventario;
    }

    [System.Serializable]
    public enum EstadoConsumivel
    {
        Cozido,
        Cru,
        Queimado,
        Estragado
    }

    public Item setupItemFromItemStruct(ItemStruct itemResponse, int quantidadeResponse)
    {
        tipoItem = itemResponse.tipoItem;
        nomePortugues = itemResponse.nomePortugues;
        nomeIngles = itemResponse.nomeIngles;
        itemIdentifierAmount = itemResponse.itemIdentifierAmount;
        tipoMunicao = itemResponse.tipoMunicao;
        groupIndex = itemResponse.groupIndex;
        isConsumivel = itemResponse.isConsumivel;
        estadoConsumivel = itemResponse.estadoConsumivel;
        curaSede = itemResponse.curaSede;
        curaFome = itemResponse.curaFome;
        curaVida = itemResponse.curaVida;
        isCuraIndigestao = itemResponse.isCuraIndigestao;
        isCuraInfeccao = itemResponse.isCuraInfeccao;
        isCuraFratura = itemResponse.isCuraFratura;
        isCuraSangramento = itemResponse.isCuraSangramento;
        quantidade = quantidadeResponse;
        peso = itemResponse.peso;
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
        inventario.txMunicoesClipHud.text = "";
        inventario.txMunicoesInventarioHud.text = "";
        inventario.itemNaMao = null;
        if (itemIdentifierAmount.ItemDefinition.Equals(inventario.itemCorda))
        {
            DesequiparCordaNasMaos();
        }
        UnequipItemInventory();
    }

    public void SelecionarItem()
    {
        if (this.itemIdentifierAmount.ItemDefinition.GetItemCategory().name.Equals("NaoEquipavel")) return; //Esse tipo de item n�o pode ser equipado
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
            else if (itemIdentifierAmount.ItemDefinition.Equals(inventario.itemIsqueiro))
            {
                inventario.playerController.acendedorFogueiraTP = inventario.playerController.contentItemsTP.GetComponentInChildren<AcendedorFogueira>();
                inventario.playerController.acendedorFogueiraFP = inventario.playerController.contentItemsFP.GetComponentInChildren<AcendedorFogueira>();
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
            EquiparCordaNasMaos();
        }

        inventario.AtualizarHudMunicoesComArmaAtual();

    }

    public void EquiparCordaNasMaos()
    {
        inventario.playerController.cordaWeaponTP = inventario.playerController.contentItemsTP.GetComponentInChildren<CordaWeapon>();
        inventario.playerController.cordaWeaponFP = inventario.playerController.contentItemsFP.GetComponentInChildren<CordaWeapon>();

        if (inventario.playerController.cordaWeaponFP != null)
        {
            inventario.playerController.cordaWeaponTP.playerController = inventario.playerController;
            inventario.playerController.cordaWeaponFP.playerController = inventario.playerController;
            inventario.playerController.cordaWeaponTP.AcoesRenovarCordaEstourada(false, true);
            inventario.playerController.cordaWeaponFP.AcoesRenovarCordaEstourada(false, false);
        }
    }

    public void DesequiparCordaNasMaos()
    {
        inventario.UngrabCoisasCapturadasComCorda(false);
    }

    private void EquipItemInventory()
    {
        inventario.inventory.GetComponent<ItemSetManagerBase>().EquipItem(itemIdentifierAmount.ItemIdentifier, groupIndex, true, true);
        inventario.playerController.PararAbilitys();
    }
    
    private void UnequipItemInventory()
    {
        inventario.playerController.PararAbilitys();
        inventario.inventory.GetComponent<ItemSetManagerBase>().UnEquipItem(itemIdentifierAmount.ItemIdentifier, groupIndex, true, true);
        IItemIdentifier itemIdBody = inventario.inventory.DefaultLoadout[0].ItemIdentifier;
        inventario.inventory.GetComponent<ItemSetManagerBase>().EquipItem(itemIdBody, groupIndex, true, true);
    }

    public void DroparItem(int qtdResponse)
    {
        if (this.tipoItem.Equals(TiposItems.Nenhum)) return;
        string nomePrefab = this.tipoItem + "/"+ this.itemIdentifierAmount.ItemDefinition.name.ToString();
        string prefabPath = Path.Combine("Prefabs/ItensInventario/", nomePrefab);
        for(int i = 0; i < qtdResponse; i++)
        {
            GameObject objDropado = ItemDrop.InstanciarPrefabPorPath(prefabPath, 1, new Vector3(transform.root.position.x,
            transform.root.position.y + 1 + i/qtdResponse, transform.root.position.z) + transform.root.forward, transform.root.rotation, null, PV.ViewID);
            if (this.itemIdentifierAmount.ItemDefinition.Equals(inventario.itemGarrafa))
            {
                objDropado.GetComponent<Garrafa>().Setup(this.GetComponent<Garrafa>());
            }
        }
        inventario.RemoverItemDoInventarioPorItemIdentifier(itemIdentifierAmount.ItemIdentifier, qtdResponse);
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
        aplicarEfeitosConsumivelPorEstado();
        if (curaVida < 0) inventario.statsJogador.TakeDamageHealth(Mathf.Abs(curaVida), false);
        else inventario.statsJogador.TakeHealHealth(curaVida);
    }

    private void aplicarEfeitosConsumivelPorEstado()
    {
        if (isConsumivel)
        {
            float percentIndigestao = 0;

            // Determina a chance de indigestão com base no estado do consumível
            if (estadoConsumivel == EstadoConsumivel.Cru) percentIndigestao = 20;
            else if (estadoConsumivel == EstadoConsumivel.Estragado) percentIndigestao = 50;
            else if (estadoConsumivel == EstadoConsumivel.Queimado) percentIndigestao = 10;

            // Calcula se deve aplicar indigestão
            int calcIndigestao = Random.Range(0, 100);
            if (calcIndigestao < percentIndigestao)
            {
                inventario.statsJogador.AplicarIndigestao();
            }

            // Aplica cura para indigestão e infecção se aplicável
            if (isCuraIndigestao) inventario.statsJogador.CurarIndigestao();
            if (isCuraInfeccao) inventario.statsJogador.CurarInfeccao();
            if (isCuraFratura) inventario.statsJogador.CurarFratura();
            if (isCuraSangramento) inventario.statsJogador.CurarSangramento();
        }
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
            inventario.UngrabCoisasCapturadasComCorda(isCordaPartindo);
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
        if (!gameObject.activeSelf)
        {
            Debug.Log("nao aumentou item, pq ja tava inativo");
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
            inventario.AtualizarHudMunicoesComArmaAtual();
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
