using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class GrabObjects : MonoBehaviourPunCallbacks
{
    
    public string tagObjGrab = "ObjetoGrab", tagItemDrop = "ItemDrop", tagEnemy = "Inimigo", tagAgua = "Agua", tagPesca = "Pesca";

    [Tooltip("Force to apply in object")]
    [SerializeField] public float forceGrab = 5;
    [SerializeField] public float maxDistPlayer, maxDistanceObjeto;
    float anguloLimiteOlhandoPraBaixo = 140.0f;

    [HideInInspector] public GameObject grabedObj;
    [HideInInspector] public bool possibleGrab = false, possibleInteraction = false;
    private Vector2 rigSaveGrabed;

    [SerializeField] Camera cam;
    [SerializeField] [HideInInspector] PlayerController playerController;
    [SerializeField] [HideInInspector] PlayerMovement playerMovimentController;
    [SerializeField] [HideInInspector] Inventario inventario;
    [SerializeField] [HideInInspector] Animator animator;

    PhotonView PV;

    void Awake()
    {
        PV = GetComponentInParent<PhotonView>();
        playerController = GetComponentInParent<PlayerController>();
        playerMovimentController = GetComponentInParent<PlayerMovement>();
        animator = GetComponentInParent<Animator>();
    }

    void Update()
    {
        if (!playerMovimentController.canMove || inventario.canvasInventario.activeSelf) return;
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        ray.origin = cam.transform.position;

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistanceObjeto))
        {
            possibleGrab = false;
            possibleInteraction = false;
            
            if ((hit.transform.tag == tagObjGrab || hit.transform.tag == tagItemDrop)) 
            {

                if (hit.transform.tag == tagItemDrop && hit.transform.GetComponent<ItemDrop>().nomeItem.Equals(Item.NomeItem.Fogueira) )
                { 
                    if(inventario.itemNaMao != null)
                    {
                        if (inventario.itemNaMao != null && (inventario.itemNaMao.nomeItem.Equals(Item.NomeItem.Panela) || inventario.itemNaMao.nomeItem.Equals(Item.NomeItem.Tigela)))
                        {
                            if (Input.GetKeyDown(KeyCode.E))
                            {
                                Fogueira fogueira = hit.transform.GetComponent<ItemDrop>().GetComponent<Fogueira>();
                                if (fogueira.ColocarPanelaTigela(inventario.itemNaMao))
                                {
                                    inventario.RemoverItemDaMao();
                                }
                            }
                            possibleInteraction = true;
                        }
                    }
                    else
                    {
                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            playerController.fogueiraAcendendo = hit.transform.gameObject;
                            if (!hit.transform.gameObject.GetComponent<Fogueira>().fogo.isFogoAceso)
                            {
                                animator.SetTrigger("acendendoFogueira");
                            }
                            else
                            {
                                animator.SetTrigger("apagandoFogueira");
                            }
                        }
                        possibleInteraction = true;
                    }
                }
                else if (hit.transform.tag == tagItemDrop && (hit.transform.GetComponent<ItemDrop>().nomeItem.Equals(Item.NomeItem.Panela) || hit.transform.GetComponent<ItemDrop>().nomeItem.Equals(Item.NomeItem.Tigela))
                    && inventario.itemNaMao != null && inventario.itemNaMao.itemObjMao != null && inventario.itemNaMao.itemObjMao.GetComponent<ConsumivelCozinha>())
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        Panela panela = hit.transform.GetComponent<ItemDrop>().GetComponent<Panela>();
                        if (panela.ColocarConsumivelNaPanela(inventario.itemNaMao))
                        {
                            inventario.RemoverItemDaMao();
                        }
                    }
                    possibleInteraction = true;
                }
                else
                {
                    if(inventario.itemNaMao == null)
                    {
                        if (Input.GetMouseButtonDown(1)) //Segura objeto
                        {
                            transferOwnerPV(hit.transform.gameObject);
                            grabedObj = hit.transform.gameObject;
                        }
                        else if (Input.GetMouseButtonDown(0)) //Pega item do chao
                        {
                            transferOwnerPV(hit.transform.gameObject);
                            animator.SetTrigger("pegandoItemChao");
                            ItemDrop itemDrop = hit.transform.gameObject.GetComponent<ItemDrop>();
                            bool destruirObjetoDaCena = true;
                            if((itemDrop.nomeItem.Equals(Item.NomeItem.Panela) || itemDrop.nomeItem.Equals(Item.NomeItem.Tigela)) && itemDrop.gameObject.GetComponent<Panela>().fogueira != null)
                            {
                                Panela panela = itemDrop.gameObject.GetComponent<Panela>();
                                Item.NomeItem itemNaPanela = panela.ObterConsumivelDaPanela();
                                if (!itemNaPanela.Equals(Item.NomeItem.Nenhum))
                                {
                                    if(inventario.AdicionarItemAoInventario(itemNaPanela, 1))
                                    {
                                        panela.RetirarConsumivelDaPanela();
                                    }
                                    else
                                    {
                                        Debug.Log("nao foi possivel adicionar ao inventario do jogador");
                                    }
                                    return;
                                }
                                else
                                {
                                    panela.fogueira.RetirarPanelaTigela();
                                }
                                destruirObjetoDaCena = false;
                            }
                            if (inventario.AdicionarItemAoInventario(itemDrop.nomeItem, 1)) //adicionou ao inventario do jogador
                            {
                                if (destruirObjetoDaCena)
                                {
                                    if (PhotonNetwork.IsConnected) PhotonNetwork.Destroy(hit.transform.gameObject); //destruir recurso apos jogador pegar
                                    else GameObject.Destroy(hit.transform.gameObject);
                                }
                            }
                            else
                            {
                                Debug.Log("nao foi possivel adicionar ao inventario do jogador");
                            }
                        }
                        possibleGrab = true;
                    }
                }
            }
            else if (hit.transform.tag == tagEnemy && hit.transform.GetComponent<EnemyStats>().isDead && inventario.itemNaMao != null 
                && (inventario.itemNaMao.nomeItem.Equals(Item.NomeItem.FacaSimples) || inventario.itemNaMao.nomeItem.Equals(Item.NomeItem.FacaAvancada)))
            {
                if (Input.GetKeyDown(KeyCode.E)) //Interagir Dissecar
                {
                    transferOwnerPV(hit.transform.gameObject);
                    animator.SetTrigger("dissecando");
                    playerController.itemsDropsPosDissecar = hit.transform.gameObject.GetComponent<EnemyStats>().dropsItems;
                    playerController.corpoDissecando = hit.transform.gameObject;
                }
                possibleInteraction = true;
            }
            else if(hit.transform.tag == tagAgua && inventario.itemNaMao == null)
            {
                if (Input.GetKeyDown(KeyCode.E)) //Interagir BeberAgua
                {
                    transferOwnerPV(hit.transform.gameObject);
                    animator.SetTrigger("bebendoAguaBaixo");
                }
                possibleInteraction = true;
            }
            else if (hit.transform.tag == tagPesca && hit.transform.gameObject.GetComponent<Pesca>().isAreaDePescaAtiva && inventario.itemNaMao != null && inventario.itemNaMao.nomeItem.Equals(Item.NomeItem.VaraDePesca))
            {
                if (Input.GetKeyDown(KeyCode.E)) //Interagir Pescar
                {
                    transferOwnerPV(hit.transform.gameObject);
                    playerController.peixeDaVara.SetActive(false);
                    playerController.pescaPescando = hit.transform.gameObject;
                    animator.SetTrigger("pescando");
                }
                possibleInteraction = true;
            }
        }
        else
        {
            possibleGrab = false;
            possibleInteraction = false;
        }

        if (grabedObj != null)
        {
            if (!grabedObj.GetComponent<Rigidbody>())
            {
                Debug.LogError("Coloque um Rigidbody no objeto!");
                return;
            }

            Rigidbody objRig = grabedObj.GetComponent<Rigidbody>();
            Vector3 posGrab = cam.transform.position + cam.transform.forward * maxDistPlayer;
            float dist = Vector3.Distance(grabedObj.transform.position, posGrab);
            float calc = forceGrab * dist * 6 * Time.deltaTime;

            if (rigSaveGrabed == Vector2.zero)
            {
                rigSaveGrabed = new Vector2(objRig.drag, objRig.angularDrag);
            }
            objRig.drag = 2.5f;
            objRig.angularDrag = 2.5f;
            objRig.AddForce(-(grabedObj.transform.position - posGrab).normalized * calc, ForceMode.Impulse);
            playerMovimentController.pesoGrab = objRig.mass;

            if (Input.GetMouseButtonUp(1) || objRig.velocity.magnitude >= 20 || dist >= 10 || playerMovimentController.isPulando)
            {
                UngrabObject();
            }
        }
        else
        {
            playerMovimentController.pesoGrab = 0;
        }
    }

    private void transferOwnerPV(GameObject obj)
    {
        if (obj.GetComponent<PhotonView>() != null && obj.GetComponent<PhotonView>().Owner != PhotonNetwork.LocalPlayer)
        {
            obj.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);
        }
    }


    public void UngrabObject()
    {
        if (grabedObj == null) return;
        Rigidbody objRig = grabedObj.GetComponent<Rigidbody>();
        objRig.drag = rigSaveGrabed.x;
        objRig.angularDrag = rigSaveGrabed.y;
        rigSaveGrabed = Vector2.zero;
        grabedObj = null;
        playerMovimentController.pesoGrab = 0;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Transform cam = GetComponent<Camera>().transform;
        if (!Physics.Raycast(cam.position, cam.forward, maxDistPlayer))
        {
            Gizmos.DrawLine(cam.position, cam.position + cam.forward * maxDistPlayer);
        }
    }

    public bool isPlayerEstaOlhandoPraBaixo()
    {
        // Obter a direção da câmera
        Vector3 cameraForward = Camera.main.transform.forward;
        // Obter o ângulo em relação ao eixo Y (vertical)
        float angle = Mathf.Abs(Vector3.Angle(cameraForward, Vector3.up));
        // Verificar se o ângulo é menor que o limite
        if (angle > anguloLimiteOlhandoPraBaixo)
        {
            // O player está olhando para baixo
            return true;
        }
        else
        {
            // O player não está olhando para baixo
            return false;
        }
    }

}
