using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.Serialization;
using Photon.Pun;

public class Item : MonoBehaviourPunCallbacks
{

    [SerializeField] public string nomePortugues, nomeIngles;
    [SerializeField] public Item.NomeItem nomeItem;
    [SerializeField] public bool isConsumivel;
    [SerializeField] public int quantidade = 0, peso;
    [SerializeField] public int durabilidadeAtual = 100, durabilidadeMaxima = 100;
    [SerializeField] public ItemObjMao itemObjMao;
    [SerializeField] public Inventario inventario;
    [SerializeField] public Hotbar hotbar;

    [SerializeField] public TMP_Text txQuantidade, txNomeItem;
    [SerializeField] public RawImage imagemItem;
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
        Objeto
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
        //Consumivel
        [EnumMember(Value = "Consumivel")]
        FrutaLimao,
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
        [EnumMember(Value = "Objeto")]
        Cantil,
        [EnumMember(Value = "Objeto")]
        FlechaDeMadeira,
        [EnumMember(Value = "Objeto")]
        FlechaDeOsso,
        [EnumMember(Value = "Objeto")]
        FlechaDeMetal,
        [EnumMember(Value = "Objeto")]
        Fogueira,

    }

    [System.Serializable]
    public struct ItemDropStruct
    {
        public Item.NomeItem nomeItemEnum;
        public int qtdMinDrops;
        public int qtdMaxDrops;
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
    }

    public void DeselecionarItem()
    {
        if(itemObjMao != null) itemObjMao.gameObject.SetActive(false);
        inventario.itemNaMao = null;
        inventario.playerMovement.anim.SetBool("isPlayerArmado", (inventario.itemNaMao != null));
    }

    public void SelecionarItem()
    {
        if (hotbar.estaSelecionandoSlotHotbar)
        {
            hotbar.SelecionouItemParaSlotHotbar(this);
            return;
        }

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
            inventario.itemNaMao = this.nomeItem.GetTipoItemEnum().Equals(TiposItems.Nenhum) ? null : this;
            if (itemObjMao != null) itemObjMao.gameObject.SetActive(true);
            inventario.FecharInventario();
        }

        inventario.playerMovement.anim.SetBool("isPlayerArmado", (inventario.itemNaMao != null &&  inventario.itemNaMao.itemObjMao != null));
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
            inventario.setarQtdItensAtual(inventario.qtdItensAtual - 1);
            desativarOuAtivarUsoItemDaHotbar(true);
            if (itemObjMao != null) itemObjMao.gameObject.SetActive(false);
            inventario.itemNaMao = null;
            gameObject.SetActive(false);
        }
        txQuantidade.text = quantidade + "";
        inventario.playerMovement.anim.SetBool("isPlayerArmado", (inventario.itemNaMao != null && inventario.itemNaMao.itemObjMao != null));
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

}
