using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.Serialization;
using Photon.Pun;
using UnityEngine.EventSystems;

public class Item : MonoBehaviourPunCallbacks
{

    [SerializeField] public string nomePortugues, nomeIngles;
    [SerializeField] public Item.NomeItem nomeItem;
    [SerializeField] public bool isConsumivel;
    [SerializeField] public int quantidade = 0, peso;
    [SerializeField] public int durabilidadeAtual = 100, durabilidadeMaxima = 100;

    [SerializeField] public Texture textureImgItem;
    [SerializeField] public ItemObjMao itemObjMao;
    [SerializeField] public Inventario inventario;
    [SerializeField] public Hotbar hotbar;
    [SerializeField] public Armaduras armaduras;
    [SerializeField] public ArrastarItensInventario arrastarItensInventario;

    //PADRAO
    [SerializeField] public RawImage imagemItem;
    [SerializeField] public TMP_Text txQuantidade, txNomeItem;
    PhotonView PV;


    public enum TiposItems
    {
        [EnumMember(Value = "Nenhum")]
        Nenhum,
        [EnumMember(Value = "Ferramenta")]
        Ferramenta,
        [EnumMember(Value = "Arma")]
        Arma,
        [EnumMember(Value = "Consumivel")]
        Consumivel,
        [EnumMember(Value = "Recurso")]
        Recurso,
        [EnumMember(Value = "Armadura")]
        Armadura,
        [EnumMember(Value = "Objeto")]
        Objeto,
        [EnumMember(Value = "Municao")]
        Municao
    }

    public enum NomeItem
    {
        [EnumMember(Value = "Nenhum")]
        Nenhum,
        //Recurso
        [EnumMember(Value = "Recurso")]
        Madeira,
        [EnumMember(Value = "Recurso")]
        Pedra,
        [EnumMember(Value = "Recurso")]
        Metal,
        [EnumMember(Value = "Recurso")]
        Couro,
        [EnumMember(Value = "Recurso")]
        Osso,
        [EnumMember(Value = "Recurso")]
        Cipo,
        [EnumMember(Value = "Recurso")]
        Seiva,
        [EnumMember(Value = "Recurso")]
        Folha,
        [EnumMember(Value = "Recurso")]
        Carvao,
        [EnumMember(Value = "Recurso")]
        Argila,
        [EnumMember(Value = "Recurso")]
        Graveto,
        [EnumMember(Value = "Recurso")]
        Veneno,
        [EnumMember(Value = "Recurso")]
        SementeMaca,
        [EnumMember(Value = "Recurso")]
        SementeBanana,
        [EnumMember(Value = "Recurso")]
        SementeLaranja,
        [EnumMember(Value = "Recurso")]
        SementeManga,
        [EnumMember(Value = "Recurso")]
        SementeCogumelo,
        [EnumMember(Value = "Recurso")]
        SementeCogumeloVenenoso,
        //Consumivel
        [EnumMember(Value = "Consumivel")]
        Laranja,
        [EnumMember(Value = "Consumivel")]
        CogumeloCru,
        [EnumMember(Value = "Consumivel")]
        CogumeloCozido,
        [EnumMember(Value = "Consumivel")]
        PeixeCru,
        [EnumMember(Value = "Consumivel")]
        PeixeCozido,
        [EnumMember(Value = "Consumivel")]
        CarneCrua,
        [EnumMember(Value = "Consumivel")]
        CarneCozida,
        [EnumMember(Value = "Consumivel")]
        Manga,
        [EnumMember(Value = "Consumivel")]
        Maca,
        [EnumMember(Value = "Consumivel")]
        Banana,
        [EnumMember(Value = "Consumivel")]
        CogumeloVenenosoCru,
        [EnumMember(Value = "Consumivel")]
        CogumeloVenenosoCozido,
        [EnumMember(Value = "Consumivel")]
        CarneEstragada,
        [EnumMember(Value = "Consumivel")]
        PeixeEstragado,
        [EnumMember(Value = "Consumivel")]
        BananaEstragada,
        [EnumMember(Value = "Consumivel")]
        LaranjaEstragada,
        [EnumMember(Value = "Consumivel")]
        MangaEstragada,
        [EnumMember(Value = "Consumivel")]
        MacaEstragada,
        [EnumMember(Value = "Consumivel")]
        CogumeloEstragado,
        [EnumMember(Value = "Consumivel")]
        CogumeloVenenosoEstragado,
        //Ferramenta
        [EnumMember(Value = "Ferramenta")]
        MachadoSimples,
        [EnumMember(Value = "Ferramenta")]
        MarteloSimples,
        [EnumMember(Value = "Ferramenta")]
        PicaretaSimples,
        [EnumMember(Value = "Ferramenta")]
        MachadoAvancado,
        [EnumMember(Value = "Ferramenta")]
        MarteloAvancado,
        [EnumMember(Value = "Ferramenta")]
        PicaretaAvancada,
        [EnumMember(Value = "Ferramenta")]
        FacaSimples,
        [EnumMember(Value = "Ferramenta")]
        FacaAvancada,
        [EnumMember(Value = "Ferramenta")]
        Bussola,
        [EnumMember(Value = "Ferramenta")]
        Tocha,
        [EnumMember(Value = "Ferramenta")]
        Cantil,
        [EnumMember(Value = "Ferramenta")]
        VaraDePesca,
        //Arma
        [EnumMember(Value = "Arma")]
        LancaSimples,
        [EnumMember(Value = "Arma")]
        LancaAvancada,
        [EnumMember(Value = "Arma")]
        ArcoSimples,
        [EnumMember(Value = "Arma")]
        ArcoAvancado,
        [EnumMember(Value = "Arma")]
        EspadaSimples,
        [EnumMember(Value = "Arma")]
        EspadaAvancada,
        [EnumMember(Value = "Arma")]
        Besta,
        //Municao
        [EnumMember(Value = "Municao")]
        FlechaDeMadeira,
        [EnumMember(Value = "Municao")]
        FlechaDeOsso,
        [EnumMember(Value = "Municao")]
        FlechaDeMetal,
        //Armadura
        [EnumMember(Value = "Armadura")]
        CapaceteDeCouro,
        [EnumMember(Value = "Armadura")]
        CasacoDeCouro,
        [EnumMember(Value = "Armadura")]
        CalcaDeCouro,
        [EnumMember(Value = "Armadura")]
        BotasDeCouro,
        [EnumMember(Value = "Armadura")]
        EscudoSimples,
        [EnumMember(Value = "Armadura")]
        EscudoAvancado,
        //Objeto
        [EnumMember(Value = "Objeto")]
        Tigela,
        [EnumMember(Value = "Objeto")]
        Panela,
        [EnumMember(Value = "Consumivel")]
        Coco,
    }

    [System.Serializable]
    public struct ItemDropStruct
    {
        public Item.NomeItem nomeItemEnum;
        public int qtdMinDrops;
        public int qtdMaxDrops;
    }

    [System.Serializable]
    public struct ItemStruct
    {
        public Item.NomeItem nomeItemEnum;
        public string nomePortugues, nomeIngles;
        public bool isConsumivel;
        public int peso, durabilidadeAtual, durabilidadeMaxima;
        public ItemObjMao itemObjMao;
        public Texture textureImgItem;
        public GameObject objInventario;
    }

    public Item setupItemFromItemStruct(ItemStruct itemResponse)
    {
        nomeItem = itemResponse.nomeItemEnum;
        nomePortugues = itemResponse.nomePortugues;
        nomeIngles = itemResponse.nomeIngles;
        isConsumivel = itemResponse.isConsumivel;
        quantidade = 1;
        peso = itemResponse.peso;
        durabilidadeAtual = itemResponse.durabilidadeAtual;
        durabilidadeMaxima = itemResponse.durabilidadeMaxima;
        itemObjMao = itemResponse.itemObjMao;
        imagemItem.texture = itemResponse.textureImgItem;
        inventario = itemResponse.objInventario.GetComponent<Inventario>();
        hotbar = itemResponse.objInventario.GetComponent<Hotbar>();
        armaduras = itemResponse.objInventario.GetComponent<Armaduras>();
        arrastarItensInventario = itemResponse.objInventario.GetComponent<ArrastarItensInventario>();
        txNomeItem.text = PlayerPrefs.GetInt("INDEXIDIOMA") == 1 ? itemResponse.nomePortugues : itemResponse.nomeIngles;
        txQuantidade.text = quantidade + "";
        return this;
    }

    public static string ObterNomeIdPorTipoItem(NomeItem nomeItemResponse)
    {
        return nomeItemResponse.ToString();
    }

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    private void Start()
    {
        txNomeItem.text = PlayerPrefs.GetInt("INDEXIDIOMA") == 1 ? nomePortugues : nomeIngles;
        txQuantidade.text = quantidade + "";
        EventTrigger trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        EventTrigger.Entry entry2 = new EventTrigger.Entry();
        EventTrigger.Entry entry3 = new EventTrigger.Entry();
        EventTrigger.Entry entry4 = new EventTrigger.Entry();
        EventTrigger.Entry entry5 = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.BeginDrag;
        entry2.eventID = EventTriggerType.EndDrag;
        entry3.eventID = EventTriggerType.Drop;
        entry4.eventID = EventTriggerType.PointerEnter;
        entry5.eventID = EventTriggerType.PointerExit;
        entry.callback.AddListener((data) => { OnBeginDragDelegate((PointerEventData)data); });
        entry2.callback.AddListener((data) => { OnEndDragDelegate((PointerEventData)data); });
        entry3.callback.AddListener((data) => { OnDropDelegate((PointerEventData)data); });
        entry4.callback.AddListener((data) => { OnPointerEnterDelegate((PointerEventData)data);});
        entry5.callback.AddListener((data) => { OnPointerExitDelegate((PointerEventData)data);});
        trigger.triggers.Add(entry);
        trigger.triggers.Add(entry2);
        trigger.triggers.Add(entry3);
        trigger.triggers.Add(entry4);
        trigger.triggers.Add(entry5);
    }

    public void DeselecionarItem()
    {
        if(itemObjMao != null) itemObjMao.gameObject.SetActive(false);
        inventario.itemNaMao = null;
        inventario.playerMovement.anim.SetBool("isPlayerArmado", false);
    }

    public void SelecionarItem()
    {
        if (inventario.itemNaMao != null)
        {
            if (inventario.itemNaMao.nomeItem.Equals(this.nomeItem))
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
            if (itemObjMao != null)
            {
                inventario.itemNaMao = this;
                itemObjMao.gameObject.SetActive(true);
                if (nomeItem.Equals(NomeItem.ArcoSimples) || nomeItem.Equals(NomeItem.ArcoAvancado) || nomeItem.Equals(NomeItem.Besta)) itemObjMao.GetComponent<TipoFlechaNoArco>().AtivarTipoFlechaNoArco();
                if (nomeItem.Equals(NomeItem.VaraDePesca)) inventario.playerMovement.playerController.peixeDaVara.SetActive(false);
                //inventario.FecharInventario();
            }
            else
            {
                if (nomeItem.Equals(NomeItem.Nenhum)){}
                 //inventario.FecharInventario();
            }
        }

        inventario.playerMovement.anim.SetBool("isPlayerArmado", (inventario.itemNaMao != null && inventario.itemNaMao.itemObjMao != null));
    }

    public void DroparItem()
    {
        if (this.nomeItem.GetTipoItemEnum().Equals(TiposItems.Nenhum)) return;
        string nomePrefab = this.nomeItem.GetTipoItemEnum() + "/"+ this.nomeItem.ToString();
        ItemDrop.InstanciarPrefabPorPath(nomePrefab, 1, new Vector3(transform.root.position.x, transform.root.position.y+1, transform.root.position.z) + transform.root.forward , transform.root.rotation, PV.ViewID);
        inventario.RemoverItemDoInventario(this, 1); //TODO: implementar opcao de dropar itens em quantidade
    }

    public void UsarItem() //Usa item qdo aperta o botoa do mouse com o item na mao
    {
        if (quantidade <= 0) return;
        if (isConsumivel)
        {
            aplicarEfeitoConsumivel();
            diminuirQuantidade(1);
        }
        else
        {
            durabilidadeAtual--;
            if (durabilidadeAtual <= 0)
            {
                durabilidadeAtual = 0;
                diminuirQuantidade(1);
            }
        }
    }

    private void aplicarEfeitoConsumivel()
    {
        inventario.statsJogador.setarSedeAtual(inventario.statsJogador.sedeAtual + itemObjMao.curaSede);
        inventario.statsJogador.setarFomeAtual(inventario.statsJogador.fomeAtual + itemObjMao.curaFome);
    }

    public void diminuirQuantidade(int valorQtd)
    {
        inventario.setarPesoAtual(inventario.pesoAtual - peso * valorQtd);
        quantidade -= valorQtd;
        if (quantidade <= 0)
        {
            quantidade = 0;
            if (inventario.itemNaMao != null && inventario.itemNaMao.itemObjMao != null && inventario.itemNaMao.itemObjMao.GetComponent<TipoFlechaNoArco>() != null)
            {
                inventario.itemNaMao.itemObjMao.GetComponent<TipoFlechaNoArco>().DesativarTipoFlechaNoArco();
            }
            inventario.setarQtdItensAtual(inventario.qtdItensAtual - 1);
            desativarOuAtivarUsoItemDaHotbar(true);
            if(inventario.itemNaMao != null && inventario.itemNaMao.nomeItem.Equals(this.nomeItem))
            {
                if (itemObjMao != null) itemObjMao.gameObject.SetActive(false);
                inventario.itemNaMao = null;
            }
        }
        if (armaduras.slotAljava.item != null && nomeItem.Equals(armaduras.slotAljava.item.nomeItem))
        {
            armaduras.slotAljava.SetupItemNoSlot(this);
        }
        txQuantidade.text = quantidade + "";
        inventario.playerMovement.anim.SetBool("isPlayerArmado", (inventario.itemNaMao != null && inventario.itemNaMao.itemObjMao != null));

        if(quantidade <= 0)
        {
            RemoverItemDaMochila();
        }
    }

    private void RemoverItemDaMochila()
    {
        inventario.itens.Remove(this);
        GameObject.Destroy(this.gameObject);
    }

    public bool aumentarQuantidade(int quantidadeResponse)
    {
        if (!gameObject.activeSelf && inventario.qtdItensAtual >= inventario.qtdItensMaximo)
        {
            Debug.Log("Inventario cheio");
            return false;
        }
        else
        {
            if(quantidade <= 0)
            {
                desativarOuAtivarUsoItemDaHotbar(false);
                gameObject.transform.SetAsLastSibling();
            }
            inventario.setarPesoAtual(inventario.pesoAtual + peso);
            inventario.setarQtdItensAtual(inventario.qtdItensAtual + quantidadeResponse);
            quantidade += quantidadeResponse;
            txQuantidade.text = quantidade + "";
            gameObject.SetActive(true);
            return true;
        }
    }

    private void desativarOuAtivarUsoItemDaHotbar(bool isDesativando)
    {
        foreach (SlotHotbar slot in hotbar.slots)
        {
            if (slot.item != null && slot.item.nomeItem.Equals(this.nomeItem))
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

}
