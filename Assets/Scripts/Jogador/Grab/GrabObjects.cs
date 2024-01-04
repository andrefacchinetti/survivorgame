using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Opsive.UltimateCharacterController.Inventory;
using Opsive.Shared.Inventory;

public class GrabObjects : MonoBehaviourPunCallbacks
{

    private string tagInterruptor = "Interruptor", tagObjGrab = "ObjetoGrab", tagItemDrop = "ItemDrop", tagEnemy = "Inimigo", tagAgua = "Agua", tagPesca = "Pesca", tagConsumivelNaPanela = "ConsumivelNaPanela", tagIncendiavel = "Incendiavel", tagArvore = "Arvore";
    private string tagAreaColeta = "AreaColeta", tagReconstruivelQuebrado = "ReconstruivelQuebrado", tagAnimalCollider = "AnimalCollider", tagToggleAnimationObjeto = "ToggleAnimationObjeto";
    private string tagKeypagButton = "KeypadButton", tagNote = "Note";

    [Tooltip("Force to apply in object")]
    [SerializeField] public float forceGrab = 5;
    [SerializeField] public float maxDistPlayer, distanciaMaxParaPegarObj = 3;
    float anguloLimiteOlhandoPraBaixo = 140.0f;

    [HideInInspector] public GameObject grabedObj;
    [HideInInspector] public bool possibleGrab = false, possibleInteraction = false;
    private Vector2 rigSaveGrabed;

    [SerializeField] Camera cam;
    [SerializeField] PlayerController playerController;
    [SerializeField] Inventario inventario;

    PhotonView PV;

    void Awake()
    {
        PV = playerController.PV;
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
        playerController.pesoGrab = 0;
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

    void Update()
    {
        if (!playerController.podeSeMexer() || inventario.canvasInventario.activeSelf) return;
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        ray.origin = cam.transform.position;
        RaycastHit hit;
        bool rayBool = false;
        if(playerController.controleConstruir.isAtivo) {
            rayBool = Physics.Raycast(ray, out hit, distanciaMaxParaPegarObj);
        } else {
            int layerMask = (1 << LayerMask.NameToLayer("InteracaoComGrab"));
            rayBool = Physics.Raycast(ray, out hit, distanciaMaxParaPegarObj, layerMask);
        }

        if (rayBool)
        {
            executarAcoesDoHit(hit);
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
            playerController.pesoGrab = objRig.mass;

            if (Input.GetMouseButtonUp(1) || objRig.velocity.magnitude >= 20 || dist >= 10 )
            {
                UngrabObject();
            }
        }
        else
        {
            playerController.pesoGrab = 0;
        }
    }


    private void executarAcoesDoHit(RaycastHit hit)
    {
        possibleGrab = false;
        possibleInteraction = false;

        if (hit.transform.tag == tagObjGrab || hit.transform.tag == tagItemDrop || hit.transform.tag == tagConsumivelNaPanela)
        {
            if (hit.transform.tag == tagObjGrab && inventario.itemNaMao != null && inventario.itemNaMao.itemIdentifierAmount.ItemDefinition.Equals(inventario.itemCorda))
            {
                interacaoCapturarObjeto(hit);
            }
            else if (hit.transform.tag == tagItemDrop && 
                (hit.transform.GetComponent<ItemDrop>().item.name.Equals(inventario.itemPanela.name) || hit.transform.GetComponent<ItemDrop>().item.name.Equals(inventario.itemTigela.name))
                && inventario.itemNaMao != null && inventario.itemNaMao.tipoItem.Equals(Item.TiposItems.Consumivel) && hit.transform.GetComponent<Panela>().fogueira != null)
            {
                interacaoPanelas(hit);
            }
            else
            {
                if (Input.GetMouseButtonDown(1)) //Segura objeto
                {
                    transferOwnerPV(hit.transform.gameObject);
                    grabedObj = hit.transform.gameObject;
                }
                if (hit.transform.tag == tagItemDrop && Input.GetButtonDown("Use")) //Pega item do chao
                {
                    transferOwnerPV(hit.transform.gameObject);
                    ItemDrop itemDrop = hit.transform.gameObject.GetComponent<ItemDrop>();
                    bool destruirObjetoDaCena = true;
                    if (hit.transform.tag == tagConsumivelNaPanela)
                    {
                        pegarConsumivelNaPanela(hit);
                        destruirObjetoDaCena = false;
                        return;
                    }
                    if (hit.transform.tag == tagItemDrop && (itemDrop.item.Equals(inventario.itemPanela) || itemDrop.item.Equals(inventario.itemTigela)) && itemDrop.gameObject.GetComponent<Panela>().fogueira != null) //PANELA NA FOGUEIRA
                    {
                        Panela panela = itemDrop.gameObject.GetComponent<Panela>();
                        ItemDefinitionBase itemNaPanela = panela.ObterConsumivelDaPanela();
                        if (itemNaPanela != null)
                        {
                            if (inventario.AdicionarItemAoInventario(null, itemNaPanela, 1))
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
                    
                    if (inventario.AdicionarItemAoInventario(itemDrop, itemDrop.item, 1)) //adicionou ao inventario do jogador
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
        else if (hit.transform.tag == tagAnimalCollider && inventario.itemNaMao != null
            && (inventario.itemNaMao.itemIdentifierAmount.ItemDefinition.Equals(inventario.itemFaca)))
        {
            StatsGeral objPai = hit.transform.GetComponentInParent<StatsGeral>();
            if (objPai != null && !objPai.health.IsAlive())
            {
                interacaoDissecar(objPai);
            }
        }
        else if (hit.transform.tag == tagAnimalCollider && inventario.itemNaMao != null 
            && inventario.itemNaMao.itemIdentifierAmount.ItemDefinition.Equals(inventario.itemCorda))
        {
            StatsGeral objPai = hit.transform.GetComponentInParent<StatsGeral>();
            if(objPai != null && objPai.health.IsAlive())
            {
                AnimalController animalController = objPai.gameObject.GetComponent<AnimalController>();
                if (animalController != null)
                {
                    if (playerController.animalCapturado == null && animalController.objRopePivot != null)
                    {
                        interacaoCapturar(animalController);

                    }
                    else if (playerController.animalCapturado != null && animalController.PV.ViewID == playerController.animalCapturado.PV.ViewID)
                    {
                        interacaoDescapturarAnimal(animalController);
                    }
                }
            }
        }
        else if (hit.transform.tag == tagAgua && inventario.itemNaMao == null)
        {
            interacaoBeberAgua(hit);
        }
        else if (hit.transform.tag == tagAgua && inventario.itemNaMao != null && inventario.itemNaMao.itemIdentifierAmount.ItemDefinition.Equals(inventario.itemGarrafa))
        {
            interacaoEncherGarrafa(hit);
        }
        else if (hit.transform.tag == tagPesca && hit.transform.gameObject.GetComponent<Pesca>().isAreaDePescaAtiva && inventario.itemNaMao != null && inventario.itemNaMao.itemIdentifierAmount.ItemDefinition.Equals(inventario.itemVaraDePesca))
        {
            interacaoPesca(hit);
        }
        else if (hit.transform.tag == tagAreaColeta && hit.transform.gameObject.GetComponent<AreaDeColeta>().isAreaAtiva && inventario.itemNaMao == null)
        {
            interacaoAreaDeColeta(hit);
        }
        else if (hit.transform.tag == tagIncendiavel && hit.transform.GetComponent<Fogueira>() != null)
        {
            interacaoIncendiaveis(hit);
        }
        else if (hit.transform.tag == tagReconstruivelQuebrado && inventario.itemNaMao != null && inventario.itemNaMao.itemIdentifierAmount.ItemDefinition.Equals(inventario.itemMarteloReparador))
        {
            interacaoReconstruivelQuebrado(hit);
        }
        else if (hit.transform.tag == "Player")
        {
            StatsGeral objPai = hit.transform.GetComponentInParent<StatsGeral>();
            if (objPai != null && !objPai.health.IsAlive())
            {
                interacaoReanimar(objPai);
            }
        }
        else if (hit.transform.tag == tagToggleAnimationObjeto)
        {
            interacaoComToggleAnimationObjeto(hit);
        }
        else if(hit.transform.tag == tagInterruptor)
        {
            interacaoComToggleInterruptor(hit);
        }
        else if(hit.transform.tag == tagKeypagButton)
        {
            interacaoComKeypagButton(hit);
        }
        else if(hit.transform.tag == tagNote)
        {
            interacaoComNote(hit);
        }
    }

    //INTERAÇÕES
    private void interacaoIncendiaveis(RaycastHit hit)
    {
        if (inventario.itemNaMao != null)
        {
            if (inventario.itemNaMao.itemIdentifierAmount.ItemDefinition.name.Equals(inventario.itemPanela.name) || inventario.itemNaMao.itemIdentifierAmount.ItemDefinition.name.Equals(inventario.itemTigela.name))
            {
                if (Input.GetButtonDown("Use"))
                {
                    Fogueira fogueira = hit.transform.GetComponent<Fogueira>();
                    if (fogueira.ColocarPanelaTigela(inventario.itemNaMao))
                    {
                        inventario.ConsumirItemDaMao();
                    }
                }
                possibleInteraction = true;
            }
            else if (inventario.itemNaMao.itemIdentifierAmount.ItemDefinition.Equals(inventario.itemAcendedorFogueira))
            {
                playerController.fogueiraAcendendo = hit.transform.gameObject;
                if (!hit.transform.gameObject.GetComponent<Fogueira>().fogo.isFogoAceso)
                {
                    if (Input.GetButtonDown("Use"))
                    {
                        playerController.StartarAbility(playerController.acenderFogueiraAbility);
                    }
                    possibleInteraction = true;
                }
            }
        }
        else
        {
            if (hit.transform.gameObject.GetComponent<Fogueira>().fogo.isFogoAceso)
            {
                if (Input.GetButtonDown("Use"))
                {
                    hit.transform.gameObject.GetComponent<Fogueira>().ApagarFogueira();
                    //playerController.StartarAbility(playerController.apagarFogueiraAbility);
                }
                possibleInteraction = true;
            }
        }
    }

    private void interacaoPanelas(RaycastHit hit)
    {
        if (Input.GetButtonDown("Use"))
        {
            Panela panela = hit.transform.GetComponent<ItemDrop>().GetComponent<Panela>();
            if (panela.ColocarConsumivelNaPanela(inventario.itemNaMao))
            {
                inventario.ConsumirItemDaMao();
            }
        }
        possibleInteraction = true;
    }

    private void interacaoPesca(RaycastHit hit)
    {
        if (Input.GetButtonDown("Use")) //Interagir Pescar
        {
            if(playerController.varaDePescaTP != null && playerController.varaDePescaFP != null)
            {
                transferOwnerPV(hit.transform.gameObject);
                playerController.StartarAbility(playerController.pescarAbility);
                playerController.pescaPescando = hit.transform.gameObject;
            }
            else
            {
                Debug.Log("nao achou a vara");
                playerController.characterLocomotion.TryStopAbility(playerController.pescarAbility);
            }
        }
        possibleInteraction = true;
    }

    private void interacaoReconstruivelQuebrado(RaycastHit hit)
    {
        if (Input.GetButtonDown("Use")) //Interagir Reconstruivel Quebrado
        {
            transferOwnerPV(hit.transform.gameObject);
            playerController.objConsertando = hit.transform.gameObject;
            //playerController.animator.SetTrigger("consertando");
        }
        possibleInteraction = true;
    }

    private void interacaoAreaDeColeta(RaycastHit hit)
    {
        if (Input.GetButtonDown("Use")) //Interagir Pescar
        {
            transferOwnerPV(hit.transform.gameObject);
            playerController.itemColetando = hit.transform.gameObject.GetComponent<AreaDeColeta>().itemColetavel;
            //playerController.animator.SetTrigger("coletandoBaixo");
        }
        possibleInteraction = true;
    }

    private void interacaoDissecar(StatsGeral objPai)
    {
        if (Input.GetButtonDown("Use")) //Interagir Dissecar
        {
            transferOwnerPV(objPai.gameObject);
            playerController.StartarAbility(playerController.dissecarAbility);
            playerController.itemsDropsPosDissecar = objPai.dropsItems;
            playerController.corpoDissecando = objPai.gameObject;
        }
        possibleInteraction = true;
    }

    private void interacaoCapturar(AnimalController animalController)
    {
        if (Input.GetButtonDown("Use")) 
        {
            transferOwnerPV(animalController.gameObject);
            playerController.StartarAbility(playerController.capturarAbility);
            playerController.animalCapturado = animalController;
            playerController.inventario.ToggleGrabUngrabCorda(false);
        }
        possibleInteraction = true;
    }

    private void interacaoCapturarObjeto(RaycastHit hit)
    {
        if (Input.GetButtonDown("Use")) 
        {
            if(playerController.objCapturado != null && playerController.objCapturado.GetComponent<PhotonView>().ViewID == hit.transform.gameObject.GetComponent<PhotonView>().ViewID) //descapturando objeto
            {
                playerController.objCapturado.GetComponent<ObjetoGrab>().DesativarCordaGrab();
                playerController.objCapturado = null;
                playerController.cordaWeaponTP.objCordaSemGrab.SetActive(true);
                playerController.cordaWeaponFP.objCordaSemGrab.SetActive(true);
            }
            else
            {
                if(hit.transform.gameObject.GetComponent<ObjetoGrab>().objFollowed == null) //Capturando objeto
                {
                    transferOwnerPV(hit.transform.gameObject);
                    playerController.objCapturado = hit.transform.gameObject;
                    playerController.cordaWeaponTP.objCordaSemGrab.SetActive(false);
                    playerController.cordaWeaponFP.objCordaSemGrab.SetActive(false);
                    hit.transform.gameObject.GetComponent<ObjetoGrab>().AtivarCordaGrab(playerController.cordaWeaponFP.pivotRopeStart);
                    //TODO: FAZER PRA FP E TD... hit.transform.gameObject.GetComponent<ObjetoGrab>().AtivarCordaGrab(playerController.cordaWeaponTP.pivotRopeStart);
                }
                else //O objeto ja esta sendo capturado por outro jogador
                {
                    playerController.AlertarJogadorComMensagem(EnumMensagens.ObterAlertaInteracaoNaoDisponivelAgora());
                }
            }
        }
        possibleGrab = true;
    }

    private void interacaoDescapturarAnimal(AnimalController animalController)
    {
        if (Input.GetButtonDown("Use")) 
        {
            transferOwnerPV(animalController.gameObject);
            playerController.inventario.ToggleGrabUngrabCorda(false);
        }
        possibleInteraction = true;
    }

    private void interacaoReanimar(StatsGeral objPai)
    {
        if (Input.GetButtonDown("Use")) 
        {
            //playerController.animator.SetTrigger("reanimando");
            playerController.corpoReanimando = objPai.gameObject;
        }
        possibleInteraction = true;
    }

    private void interacaoBeberAgua(RaycastHit hit)
    {
        if (Input.GetButtonDown("Use")) //Interagir BeberAgua
        {
            transferOwnerPV(hit.transform.gameObject);
            //playerController.animator.SetTrigger("bebendoAguaBaixo");
        }
        possibleInteraction = true;
    }

    private void interacaoEncherGarrafa(RaycastHit hit)
    {
        if (Input.GetButtonDown("Use")) //Interagir EncherGarrafa
        {
            transferOwnerPV(hit.transform.gameObject);
            //playerController.animator.SetTrigger("enchendoGarrafa");
        }
        possibleInteraction = true;
    }

    private void pegarConsumivelNaPanela(RaycastHit hit)
    {
        ItemDefinitionBase itemNaPanela = hit.transform.gameObject.GetComponent<ConsumivelCozinha>().slotConsumivelPanela.itemDefinitionNoSlot;
        if (inventario.AdicionarItemAoInventario(null, itemNaPanela, 1))
        {
            hit.transform.gameObject.GetComponent<ConsumivelCozinha>().panela.RetirarConsumivelDoSlot(hit.transform.gameObject.GetComponent<ConsumivelCozinha>().slotConsumivelPanela);
        }
        else
        {
            Debug.Log("nao foi possivel adicionar ao inventario do jogador");
        }
    }

    private void interacaoComToggleAnimationObjeto(RaycastHit hit)
    {
        if (Input.GetButtonDown("Use"))
        {
            ToggleAnimationObjeto toggle = hit.transform.GetComponent<ToggleAnimationObjeto>();
            if (toggle != null && (!toggle.isAtivado || !toggle.isAtivaApenasUmaVez))
            {
                toggle.AtivarToggleAnimacaoObjeto();
            }
        }
        possibleInteraction = true;
    }
    private void interacaoComToggleInterruptor(RaycastHit hit)
    {
        if (Input.GetButtonDown("Use"))
        {
            hit.transform.GetComponent<Interruptor>().ToggleInterruptor();
        }
        possibleInteraction = true;
    }

    private void interacaoComKeypagButton(RaycastHit hit)
    {
        if (Input.GetButtonDown("Use"))
        {
            hit.transform.GetComponent<NavKeypad.KeypadButton>().PressButton();
        }
        possibleInteraction = true;
    }

    private VisualizarNote noteSendoVisualizada;
    private void interacaoComNote(RaycastHit hit)
    {
        if (Input.GetButtonDown("Use"))
        {
            if (noteSendoVisualizada != null) noteSendoVisualizada.FecharNote();
            VisualizarNote note = hit.transform.GetComponent<VisualizarNote>();
            note.ToggleVisualizarNote(playerController.gameObject);
            noteSendoVisualizada = note;
        }
        possibleInteraction = true;
    }

}
