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
    [SerializeField] public List<Item> itens = new List<Item>();
    [SerializeField][HideInInspector] public Item itemNaMao;
    [SerializeField] public Hotbar hotbar;
    [SerializeField] [HideInInspector] public PlayerMovement playerMovement;
    [SerializeField] [HideInInspector] public StatsJogador statsJogador;


    private void Awake()
    {
        //TODO: obterItensDoPlayfab() //cada player preserva seus itens independente do servidor que estiver jogando
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
        canvasInventario.SetActive(true);
        playerMovement.canMove = false;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public bool AdicionarItemAoInventario(Item.NomeItem nomeItemResponse, int quantidadeResponse)
    {
        foreach (Item item in itens)
        {
            if (item.nomeItem.Equals(nomeItemResponse))
            {
                if(pesoAtual + item.peso * quantidadeResponse > pesoCapacidadeMaxima)
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
        return false;
    }

    public bool RemoverItemDoInventario(Item itemResponse, int quantidadeResponse)
    {
        foreach (Item item in itens)
        {
            if (item.nomeItem.Equals(itemResponse.nomeItem))
            {
                if (itemNaMao != null && itemResponse.nomeItem.Equals(itemNaMao.nomeItem))
                {
                    itemNaMao = null;
                }
                item.diminuirQuantidade(quantidadeResponse);
                return true;
            }
        }
        return false;
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



}
