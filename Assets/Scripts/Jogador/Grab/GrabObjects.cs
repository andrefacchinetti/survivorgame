using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class GrabObjects : MonoBehaviourPunCallbacks
{
    
    public string tagObjGrab = "ObjetoGrab", tagItemDrop = "ItemDrop";

    [Tooltip("Force to apply in object")]
    public float forceGrab = 5;
    public float maxDistPlayer, maxDistanceObjeto;
    [Tooltip("Put all layers, the player layer not!")]
    public LayerMask acceptLayers = 0;

    [HideInInspector] public GameObject grabedObj;
    [HideInInspector] public bool possibleGrab = false;
    private Vector2 rigSaveGrabed;

    [SerializeField] Camera cam;
    [SerializeField] PlayerController playerController;
    [SerializeField] PlayerMovement playerMovimentController;
    [SerializeField] Inventario inventario;
    [SerializeField] Animator animator;

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
        if (!playerMovimentController.canMove || inventario.itemNaMao != null || inventario.canvasInventario.activeSelf) return;
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        ray.origin = cam.transform.position;

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistanceObjeto))
        {
            possibleGrab = false;
            if (hit.transform.tag == tagObjGrab || hit.transform.tag == tagItemDrop) //Precisa estar sem nenhum item na mao pra pegar
            {
                if (Input.GetMouseButtonDown(1)) //Segura objeto
                {
                    if (hit.transform.gameObject.GetComponent<PhotonView>().Owner != PhotonNetwork.LocalPlayer)
                    {
                        hit.transform.gameObject.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);
                    }
                    grabedObj = hit.transform.gameObject;
                }
                else if (Input.GetMouseButtonDown(0)) //Pega item do chao
                {
                    if (hit.transform.gameObject.GetComponent<PhotonView>().Owner != PhotonNetwork.LocalPlayer)
                    {
                        hit.transform.gameObject.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);
                    }
                    animator.SetTrigger("pegandoItemChao");
                    ItemDrop itemDrop = hit.transform.gameObject.GetComponent<ItemDrop>();
                    if (inventario.AdicionarItemAoInventario(itemDrop)) //adicionou ao inventario do jogador
                    {
                        PhotonNetwork.Destroy(hit.transform.gameObject); //destruir recurso apos jogador pegar
                    }
                    else
                    {
                        //nao foi possivel adicionar ao inventario do jogador
                    }
                }
                possibleGrab = true;
            }
        }
        else
        {
            possibleGrab = false;
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
}
