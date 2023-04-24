using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Inventario : MonoBehaviour
{

    [SerializeField] public int pesoCapacidadeMaxima, qtdItensMaximo;
    [SerializeField] public TMP_Text txPesoInventario, txQtdItensInventario;
    [SerializeField][HideInInspector] public int pesoAtual, qtdItensAtual;
    [SerializeField] public GameObject canvasInventario;
    [SerializeField] GameObject contentItensMochila;
    [SerializeField] public GameObject prefabItem;
    [SerializeField] public Hotbar hotbar;
    [SerializeField] public List<Item.ItemStruct> itensStruct;

     public List<Item> itens;
    [SerializeField][HideInInspector] public Item itemNaMao;
    [SerializeField] [HideInInspector] public PlayerMovement playerMovement;
    [SerializeField] [HideInInspector] public StatsJogador statsJogador;


    private void Awake()
    {
        itens = new List<Item>();
        //itens.AddRange(contentItensMochila.GetComponentsInChildren<Item>());
    }

    public void setarPesoAtual(int valor)
    {
        pesoAtual = valor;
        txPesoInventario.text = PlayerPrefs.GetInt("INDEXIDIOMA") == 1 ? "Peso: " : "Weight: ";
        txPesoInventario.text += pesoAtual + "/" + pesoCapacidadeMaxima;
    }

    public void setarQtdItensAtual(int valor)
    {
        qtdItensAtual = valor;
        txQtdItensInventario.text = PlayerPrefs.GetInt("INDEXIDIOMA") == 1 ? "Itens: " : "Items: ";
        txQtdItensInventario.text += qtdItensAtual + "/" + qtdItensMaximo;
    }

    void Start()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();
        statsJogador = GetComponentInParent<StatsJogador>();
        setarQtdItensAtual(0);
        setarPesoAtual(0);
        foreach (Item item in itens) //Ativando os itens que tem quantidade no inventario e desativando os que nao tem quantidade
        {
            if (item.quantidade > 0)
            {
                setarPesoAtual(pesoAtual + item.peso);
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            FecharInventario();
        }
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

    public void FecharInventario()
    {
        canvasInventario.SetActive(false);
        playerMovement.canMove = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void AbrirInventario()
    {
        if (!playerMovement.canMove) return;
        canvasInventario.SetActive(true);
        playerMovement.canMove = false;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        //playerMovement.anim.SetBool("")
    }

    public bool AdicionarItemAoInventario(Item.NomeItem nomeItemEnumResponse, int quantidadeResponse)
    {
        foreach(Item.ItemStruct itemStruct in itensStruct)
        {
            if(itemStruct.nomeItemEnum == nomeItemEnumResponse)
            {
                return AdicionarItemAoInventario(itemStruct, quantidadeResponse);
            }
        }
        return false;
    }

    public bool AdicionarItemAoInventario(Item.ItemStruct itemStructResponse, int quantidadeResponse)
    {
        if(itemStructResponse.nomeItemEnum.GetTipoItemEnum().Equals(Item.TiposItems.Consumivel.ToString()) //Verifica se ja existe algum item igual pra adicionar no mesmo slot
            || itemStructResponse.nomeItemEnum.GetTipoItemEnum().Equals(Item.TiposItems.Recurso.ToString())
            || itemStructResponse.nomeItemEnum.GetTipoItemEnum().Equals(Item.TiposItems.Municao.ToString()))
        {
            foreach (Item item in itens) 
            {
                if (item.nomeItem.Equals(itemStructResponse.nomeItemEnum))
                {
                    if (pesoAtual + item.peso * quantidadeResponse > pesoCapacidadeMaxima)
                    {
                        Debug.Log("Peso maximo do inventario atingido");
                        return false;
                    }
                    else
                    {
                        return item.aumentarQuantidade(quantidadeResponse);
                    }
                }
            }
        }

        //Adiciona um novo item na mochila
        if (pesoAtual + itemStructResponse.peso * quantidadeResponse > pesoCapacidadeMaxima)
        {
            Debug.Log("Peso maximo do inventario atingido");
            return false;
        }
        else
        {
            GameObject novoObjeto = Instantiate(prefabItem, new Vector3(), new Quaternion(), contentItensMochila.transform);
            novoObjeto.transform.SetParent(contentItensMochila.transform);
            Item novoItem = novoObjeto.GetComponent<Item>().setupItemFromItemStruct(itemStructResponse);
            itens.Add(novoItem);
            return true;
        }
    }

    public void RemoverItemDoInventarioPorNome(Item.NomeItem nomeItemResponse, int quantidadeResponse)
    {
        foreach (Item item in itens)
        {
            if (item.nomeItem.Equals(nomeItemResponse))
            {
                item.diminuirQuantidade(quantidadeResponse);
                return;
            }
        }
    }

    public void RemoverItemDoInventario(Item itemResponse, int quantidadeResponse)
    {
        itemResponse.diminuirQuantidade(quantidadeResponse);
    }

    public void RemoverItemDaMao()
    {
        if (itemNaMao == null) return;
        foreach (Item item in itens)
        {
            if (item.nomeItem.Equals(itemNaMao.nomeItem))
            {
                item.diminuirQuantidade(1);
                return;
            }
        }
    }

    public bool VerificarQtdItem(Item.NomeItem nomeItemResponse, int quantidade){
        if(quantidade<=0) return true;
        foreach(Item item in itens){
            if(item.nomeItem == nomeItemResponse && item.quantidade >= quantidade){
                return true;
            }
        }
        return false;
    }

    public Item.ItemStruct PegarStructPeloNome(Item.NomeItem nome){
        foreach(Item.ItemStruct itemSt in itensStruct){
            if(itemSt.nomeItemEnum == nome){
                return itemSt;
            }
        }
        return itensStruct[0];
    }



}
