using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.Serialization;
using Photon.Pun;

public class Item : MonoBehaviourPunCallbacks
{

    [SerializeField] public string nomePortugues, nomeIngles;
    [SerializeField][HideInInspector] public string nomeId;
    [SerializeField] public TipoItem tipoItem;
    [SerializeField] public Item.NomeRecursoItemId nomeRecurso;
    [SerializeField] public Item.NomeFerramentaItemId nomeFerramenta;
    [SerializeField] public Item.NomeConsumivelItemId nomeConsumivel;
    [SerializeField] public Item.NomeArmaItemId nomeArma;
    [SerializeField] public bool isConsumivel;
    [SerializeField] public int quantidade = 0, peso;
    [SerializeField] public int durabilidadeAtual = 100, durabilidadeMaxima = 100;
    [SerializeField] public GameObject objItemMaoJogador;
    [SerializeField] public Inventario inventario;
    [SerializeField] public Hotbar hotbar;

    [SerializeField] public TMP_Text txQuantidade, txNomeItem;
    [SerializeField] public RawImage imagemItem;
    PhotonView PV;


    public enum TipoItem
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
        Recurso
    }

    public enum NomeFerramentaItemId
    {
        [EnumMember(Value = "Nenhum")]
        Nenhum,
        [EnumMember(Value = "Machado")] 
        Machado,
        [EnumMember(Value = "Martelo")] 
        Martelo,
        [EnumMember(Value = "Picareta")] 
        Picareta
    }

    public enum NomeRecursoItemId
    {
        [EnumMember(Value = "Nenhum")]
        Nenhum,
        [EnumMember(Value = "Madeira")] 
        Madeira,
        [EnumMember(Value = "Pedra")] 
        Pedra
    }

    public enum NomeArmaItemId
    {
        [EnumMember(Value = "Pedra")]
        Nenhum,
        [EnumMember(Value = "Lanca")] 
        Lanca
    }

    public enum NomeConsumivelItemId
    {
        [EnumMember(Value = "Lanca")]
        Nenhum,
        [EnumMember(Value = "Fruta")] 
        Fruta
    }

    public static string ObterNomeIdPorTipoItem(TipoItem tipoItemResponse, NomeRecursoItemId nomeRecursoResponse, NomeFerramentaItemId nomeFerramentaResponse, NomeConsumivelItemId nomeConsumivelResponse, NomeArmaItemId nomeArmaResponse)
    {
        if (Item.TipoItem.Recurso.Equals(tipoItemResponse))
        {
            return nomeRecursoResponse.GetEnumMemberValue();
        }
        else if (Item.TipoItem.Ferramenta.Equals(tipoItemResponse))
        {
            return nomeFerramentaResponse.GetEnumMemberValue();
        }
        else if (Item.TipoItem.Consumivel.Equals(tipoItemResponse))
        {
            return nomeConsumivelResponse.GetEnumMemberValue();
        }
        else if (Item.TipoItem.Arma.Equals(tipoItemResponse))
        {
            return nomeArmaResponse.GetEnumMemberValue();
        }
        return "Nenhum";
    }

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        AtualizarNomeId();
    }

    private void Start()
    {
        txNomeItem.text = PlayerPrefs.GetInt("INDEXIDIOMA") == 1 ? nomePortugues : nomeIngles;
        txQuantidade.text = quantidade + "";
    }

    public void AtualizarNomeId()
    {
        nomeId = ObterNomeIdPorTipoItem(tipoItem, nomeRecurso, nomeFerramenta, nomeConsumivel, nomeArma);
    }

    public void DeselecionarItem()
    {
        if(objItemMaoJogador != null) objItemMaoJogador.SetActive(false);
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
            inventario.itemNaMao.DeselecionarItem();
            if (inventario.itemNaMao.nomeId.Equals(this.nomeId))
            {
                inventario.itemNaMao = null;
                return;
            }
        }

        if (objItemMaoJogador != null) objItemMaoJogador.SetActive(true);
        inventario.itemNaMao = this.tipoItem.Equals(TipoItem.Nenhum) ? null : this;

        inventario.ToggleInventario();
    }

    public void DroparItem()
    {
        if (tipoItem.Equals(TipoItem.Nenhum)) return;
        string nomePrefab = tipoItem + nomeId;
        ItemDrop.InstanciarPrefabPorPath(nomePrefab, new Vector3(transform.root.position.x, transform.root.position.y+1, transform.root.position.z) + transform.root.forward , transform.root.rotation, PV.ViewID);
        inventario.RemoverItemDoInventario(this, 1); //TODO: implementar opcao de dropar itens em quantidade
    }

    public void UsarItem() //Usa item qdo aperta o botoa do mouse com o item na mao
    {
        if (isConsumivel)
        {
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

    public void diminuirQuantidade(int valorQtd)
    {
        inventario.setarPesoAtual(inventario.pesoAtual - peso * valorQtd);
        quantidade -= valorQtd;
        if (quantidade <= 0)
        {
            quantidade = 0;
            inventario.setarQtdItensAtual(inventario.qtdItensAtual - 1);
            desativarOuAtivarUsoItemDaHotbar(true);
            if (objItemMaoJogador != null) objItemMaoJogador.SetActive(false);
            gameObject.SetActive(false);
        }
        txQuantidade.text = quantidade + "";
    }

    public bool aumentarQuantidade()
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
            inventario.setarQtdItensAtual(inventario.qtdItensAtual + 1);
            quantidade += 1;
            txQuantidade.text = quantidade + "";
            gameObject.SetActive(true);
            return true;
        }
    }

    private void desativarOuAtivarUsoItemDaHotbar(bool isDesativando)
    {
        foreach (SlotHotbar slot in hotbar.slots)
        {
            if (slot.item != null && slot.item.nomeId.Equals(this.nomeId))
            {
                slot.objEmbacarImg.SetActive(isDesativando);
            }
        }
    }

}
