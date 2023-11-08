using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Inventario : MonoBehaviour
{

    [SerializeField] public int pesoCapacidadeMaxima, qtdItensMaximo;
    [SerializeField] public TMP_Text txPesoInventario, txQtdItensInventario;
    [SerializeField][HideInInspector] public int pesoAtual, qtdItensAtual;
    [SerializeField] public GameObject canvasInventario, cameraInventario;
    [SerializeField] GameObject contentItensMochila;
    [SerializeField] public GameObject prefabItem;
    [SerializeField] public Hotbar hotbar;
    [SerializeField] public List<Item.ItemStruct> itensStruct;

    [HideInInspector] public List<Item> itens;
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
        if (Input.GetKeyDown(KeyCode.Escape) || playerMovement.playerController.statsGeral.isDead)
        {
            FecharInventario();
        }
        VerificarCordaPartindo();
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
        cameraInventario.SetActive(false);
        playerMovement.canMove = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void AbrirInventario()
    {
        if (!playerMovement.canMove) return;
        canvasInventario.SetActive(true);
        cameraInventario.SetActive(true);
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

    public void RemoverItemDaMao(bool isPartindo)
    {
        if (itemNaMao == null) return;
       
        foreach (Item item in itens)
        {
            if (item.nomeItem.Equals(itemNaMao.nomeItem))
            {
                item.diminuirQuantidade(1, isPartindo);
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

    [SerializeField] public GameObject objRopeStart, objCordaMao;
    public void ToggleGrabUngrabCorda(bool isPartindo)
    {
        if (itemNaMao == null || !itemNaMao.nomeItem.Equals(Item.NomeItem.Cipo)) return;
        if (!objRopeStart.activeSelf) //Grabando Animal
        {
            objCordaMao.SetActive(false);
            objRopeStart.SetActive(true);
        }
        else //Ungrab Animal
        {
            UngrabAnimalCapturado(isPartindo);
        }
    }

    public void UngrabAnimalCapturado(bool isPartindo)
    {
        Debug.Log("UngrabAnimalCapturado");
        if (!isPartindo)
        {
            objRopeStart.SetActive(false);
            objCordaMao.SetActive(true);
        }
        if (playerMovement.playerController.animalCapturado != null)
        {
            playerMovement.playerController.animalCapturado.objColeiraRope.SetActive(false);
            playerMovement.playerController.animalCapturado.isCapturado = false;
            playerMovement.playerController.animalCapturado.targetCapturador = null;
            playerMovement.playerController.animalCapturado.agent.ResetPath();
            playerMovement.playerController.animalCapturado = null;
        }
    }

    void SumirObjRopeStart()
    {
        Debug.Log("sumindo corda");
        objRopeStart.SetActive(false);
        objCordaMao.SetActive(true);
        ropeEstoura.RenovarCorda();
    }

    [SerializeField] RopeEstoura ropeEstoura;
    private void VerificarCordaPartindo()
    {
        if (ropeEstoura.isPartido && !ropeEstoura.jaEstourou)
        {
            Debug.Log("VerificarCordaPartindo");
            RemoverItemDaMao(true);
            ropeEstoura.jaEstourou = true;
            Invoke("SumirObjRopeStart", 1f);
            //TODO: Sound de corda partindo
        }
    }

}
