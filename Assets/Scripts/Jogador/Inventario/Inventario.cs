using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Obi;

public class Inventario : MonoBehaviour
{

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
        playerController = GetComponentInParent<PlayerController>();
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
        if (Input.GetKeyDown(KeyCode.Escape) || playerController.statsGeral.isDead)
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

    public void GuardarItemDaMao()
    {
        if(itemNaMao != null)
        {
            itemNaMao.DeselecionarItem();
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

    public bool AdicionarItemAoInventario(ItemDrop itemDrop, Item.NomeItem nomeItem, int quantidadeResponse)
    {
        foreach(Item.ItemStruct itemStruct in itensStruct)
        {
            if(itemStruct.nomeItemEnum == nomeItem)
            {
                return AdicionarItemAoInventario(itemDrop, itemStruct, quantidadeResponse);
            }
        }
        return false;
    }

    public bool AdicionarItemAoInventario(ItemDrop itemDrop, Item.ItemStruct itemStructResponse, int quantidadeResponse)
    {
        if (itemDrop == null || !itemDrop.nomeItem.Equals(Item.NomeItem.Garrafa)) //itens que nao stackam
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
                        return item.aumentarQuantidade(quantidadeResponse); //stackando item
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
            if (itemDrop.nomeItem.Equals(Item.NomeItem.Garrafa))
            {
                novoObjeto.GetComponent<Garrafa>().Setup(itemDrop.GetComponent<Garrafa>());
            }
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

    public void RemoverItemDaMao(bool isCordaPartindo)
    {
        if (itemNaMao == null) return;
       
        foreach (Item item in itens)
        {
            if (item.nomeItem.Equals(itemNaMao.nomeItem))
            {
                item.diminuirQuantidade(1, isCordaPartindo);
                return;
            }
        }
    }

    public bool VerificarQtdItem(Item.NomeItem nomeItemResponse, int quantidade, bool alertar){
        if(quantidade<=0) return true;
        int qtdItemAtual = 0;
        string nomeItem = "";
        foreach(Item item in itens){
            if(item.nomeItem == nomeItemResponse){
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

    public Item.ItemStruct PegarStructPeloNome(Item.NomeItem nome){
        foreach(Item.ItemStruct itemSt in itensStruct){
            if(itemSt.nomeItemEnum == nome){
                return itemSt;
            }
        }
        return itensStruct[0];
    }

    [SerializeField] public GameObject objObiSolver,objObiRope, objCordaMao;
    public void ToggleGrabUngrabCorda(bool isCordaPartindo)
    {
        if (!objObiRope.activeSelf) //Grabando Animal
        {
            objCordaMao.SetActive(false);
            objObiRope.SetActive(true);
        }
        else //Ungrab Animal
        {
            UngrabAnimalCapturado(isCordaPartindo);
        }
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
        UngrabAnimalCapturado(isCordaPartindo);
    }

    [SerializeField] public RopeEstoura ropeEstoura;
    [SerializeField] GameObject pivotRopeStart, pivotRopeEnd;
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

}
