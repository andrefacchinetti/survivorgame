using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using Opsive.UltimateCharacterController.Inventory;
using Opsive.Shared.Utility;
using Opsive.UltimateCharacterController.Items;
using Opsive.Shared.Inventory;

public class ControleConstruir : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] public ItemDefinitionBase itemMadeira, itemPedra, itemMarteloReparador;
    public bool isAtivo = false, podeJuntar, isConectado, podeConstruir, isMadeira;
    public float distanciaMax, rotacao, velRotacao;
    [HideInInspector] public ConstrucoesController construcaoControllerHit;
    public GameObject objeto;
    public GameObject constructionUI, menuPrebfab, abaPrefab, butConsPrefab;
    public RectTransform indicadorHud;
    //public Construcao construcao;
    //public Construcao.TipoConstrucao tipoConstrucao;
    public Mesh meshObjeto;
   /*  [SerializeField]
    public List<Construcao.conStruct> conStructs; */
    public Inventario inventario;
    public LayerMask lMaskProibidos;
    private Vector3 objPosition;
    private Quaternion objRotation;
    private RaycastHit hit;
    public Vector2 abaPosInicial;
    public int indexAbas, indexConstrucoes;
    private Construcoes construcao;

    public List<Aba> abas;
    [System.Serializable]
    public struct Aba{
        public string name;
        public Texture icone;
        public List<Construcoes> construcoes;
    }
    [System.Serializable]
    public struct Construcoes{
        [Tooltip("O nome so deixa mais facil de identificar")]
        public string nome;
        public int custo;
        public float altura;
        public bool podeJuntar;
        public LayerMask layerMask;
        public Texture icone;
        [Header("Madeira")]
        [Tooltip("Mesh da estrutura de madeira")]
        public Mesh meshMad;
        [Tooltip("Prefab da estrutura de madeira" )]
        public GameObject madPrefab;
        [Header("Pedra")]
        [Tooltip("Mesh da estrutura de pedra")]
        public Mesh meshPed;
        [Tooltip("Prefab da estrutura de pedra")]
        public GameObject pedPrefab;
        
    }

    private void Start() {
        int i = 0;
        construcao = abas[indexAbas].construcoes[indexConstrucoes];
        foreach(Aba aba in abas){
            GameObject abaGO = Instantiate(abaPrefab, constructionUI.transform);
            abaGO.GetComponent<RectTransform>().anchoredPosition = new Vector2(abaPosInicial.x +(45*i),abaPosInicial.y);
            abaGO.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = aba.name;
            abaGO.transform.Find("Icone").GetComponent<RawImage>().texture = aba.icone;
            abaGO.transform.SetAsFirstSibling();
            GameObject menu = Instantiate(menuPrebfab, constructionUI.transform);
            abaGO.name = "aba" + i;
            menu.name = "menu" + i;
            GameObject layout = menu.transform.Find("Layout").gameObject;
            foreach(Construcoes construcoes in aba.construcoes){
                Instantiate(butConsPrefab, layout.transform).GetComponent<RawImage>().texture = construcoes.icone;
            }
            menu.transform.SetAsFirstSibling();
            menu.SetActive(i==0);
            i++;
        }
        constructionUI.transform.Find("menu"+indexAbas).gameObject.SetActive(true);
        constructionUI.transform.Find("aba" + indexAbas).gameObject.SetActive(true);
    }
    
    void Update(){
        if (!playerController.podeSeMexer()) return;
        podeConstruir = true;
        if (Input.GetButtonDown("MenuConstruir_Abrir"))
        {
            ToggleModoConstrucao(!isAtivo);
        }
        if (isAtivo)
        {
            if (Input.GetButtonDown("MenuConstruir_Direita") || Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                if (indexConstrucoes + 1 < abas[indexAbas].construcoes.Count)
                {
                    indexConstrucoes++;
                    construcao = abas[indexAbas].construcoes[indexConstrucoes];
                }
            }
            if (Input.GetButtonDown("MenuConstruir_Esquerda") || Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                if (indexConstrucoes - 1 >= 0)
                {
                    indexConstrucoes--;
                    construcao = abas[indexAbas].construcoes[indexConstrucoes];
                }
            }
            if(Input.GetButtonDown("MenuConstruir_Material")){
                isMadeira=!isMadeira;
                //ATUALIZAR HUD
            }
            if(Input.GetButtonDown("MenuConstruir_AvançarAba")){
                if(indexAbas + 1 < abas.Count){
                    constructionUI.transform.Find("menu" + indexAbas).gameObject.SetActive(false);
                    constructionUI.transform.Find("aba" + indexAbas).gameObject.SetActive(false);
                    indexAbas++;
                    indexConstrucoes = 0;
                    constructionUI.transform.Find("menu" + indexAbas).gameObject.SetActive(true);
                    constructionUI.transform.Find("aba" + indexAbas).gameObject.SetActive(true);

                }
            }
            if (Input.GetButtonDown("MenuConstruir_VoltarAba"))
            {
                if(indexAbas - 1 >= 0){
                    constructionUI.transform.Find("menu" + indexAbas).gameObject.SetActive(false);
                    //constructionUI.transform.Find("aba" + indexAbas).gameObject.SetActive(false);
                    indexAbas--;
                    indexConstrucoes = 0;
                    constructionUI.transform.Find("menu" + indexAbas).gameObject.SetActive(true);
                    constructionUI.transform.Find("aba" + indexAbas).gameObject.SetActive(true);
                }
            }
            objeto.SetActive(true);
            podeJuntar = construcao.podeJuntar;
            objeto.GetComponent<MeshFilter>().mesh = meshObjeto = isMadeira ? construcao.meshMad : construcao.meshPed;
            indicadorHud.anchoredPosition = new Vector2(Mathf.Clamp(-180+(140*indexConstrucoes), -180f, 240f),indicadorHud.anchoredPosition.y);
            if(-180f+(140*indexConstrucoes)>240f){
                constructionUI.transform.Find("menu" + indexAbas).Find("Layout").GetComponent<RectTransform>().anchoredPosition = new Vector2(-(indexConstrucoes*140)+420,0);
                Debug.Log(indexConstrucoes);
            }
            else{
                constructionUI.transform.Find("menu" + indexAbas).Find("Layout").GetComponent<RectTransform>().anchoredPosition = new Vector2(0,0);
            }

            /* if(construcao == null || construcao.tipoConstrucaoEnum != tipoConstrucao){
                foreach(Construcao.conStruct c in conStructs){
                    if(c.c1.tipoConstrucaoEnum == tipoConstrucao){
                        construcao = c.c1;
                        podeJuntar = construcao.podeJuntar;
                        objeto.GetComponent<MeshFilter>().mesh = meshObjeto = c.mesh;
                        Debug.Log(objeto.GetComponent<MeshFilter>().mesh);
                    }
                }
            } */
            LocalConstrucao();
            if(Input.GetButton("MenuConstruir_Rotacionar")){
                if(isConectado && Input.GetButtonDown("MenuConstruir_Rotacionar")){
                    rotacao+=90f;
                }
                else{
                    rotacao+=velRotacao;
                }
                rotacao = rotacao%360;
            }
            if(Input.GetButtonDown("Use")){
                if(inventario.VerificarQtdItem(isMadeira ? itemMadeira : itemPedra, construcao.custo, true) && (podeConstruir && VerificarSePodeConstruir())){
                    GameObject instanciado = Instantiate(isMadeira ? construcao.madPrefab : construcao.pedPrefab, objeto.transform.position, objeto.transform.rotation);
                    if(construcaoControllerHit != null)
                    {
                        construcaoControllerHit.inserirConstrucaoNaPlataforma(instanciado.GetComponentInChildren<ConstrucoesController>());
                    }
                    inventario.RemoverItemDoInventarioPorNome(isMadeira ? itemMadeira : itemPedra, construcao.custo);
                    try{
                        instanciado.GetComponent<Construcao>().disativarPlaceHolder(hit.collider.gameObject);
                    }
                    catch{}
                    construcaoControllerHit = null;
                }
            }
        }
        else{
            objeto.SetActive(false);
        }

        objeto.transform.position = objPosition;
        objeto.transform.rotation = objRotation;
        AlterarCor(VerificarSePodeConstruir() && podeConstruir);
    }

    private void ToggleModoConstrucao(bool toggle)
    {
        if(!isAtivo && !playerController.inventario.VerificarQtdItem(itemMarteloReparador, 1, false))
        {
            playerController.AlertarJogadorComMensagem(EnumMensagens.ObterAlertaNaoPossuiMartelo());
        }
        else
        {
            constructionUI.SetActive(toggle);
            isAtivo = toggle;
            playerController.TogglePlayerModoConstrucao(isAtivo);
        }
    }

    public void LocalConstrucao()
    {
        Ray r = new Ray(transform.position, transform.forward);
        isConectado = false;
        if (PegarHitMaisProx())
        {
            //Ray frontal tocou algo
            if (hit.collider.gameObject.tag == "construcao" && podeJuntar)
            {
                float altura = construcao.altura;
                if(construcao.nome == "Fundação")
                {
                    altura = 0;
                }
                objRotation = hit.collider.transform.rotation;
                objRotation = Quaternion.Euler(new Vector3(objRotation.eulerAngles.x, objRotation.eulerAngles.y + rotacao - (rotacao % 90), objRotation.eulerAngles.z));
                objPosition = new Vector3(hit.collider.transform.position.x, hit.collider.transform.position.y + altura, hit.collider.transform.position.z);
                construcaoControllerHit = hit.collider.gameObject.GetComponentInParent<ConstrucoesController>();
                isConectado = true;
            }
            else{
                objPosition = new Vector3(hit.point.x, hit.point.y + construcao.altura + (meshObjeto.bounds.size.y * objeto.transform.localScale.y / 2), hit.point.z);
                Vector3 direction = transform.position - objPosition;
                direction.y = 0f;
                objRotation = Quaternion.LookRotation(direction);
                objRotation = Quaternion.Euler(new Vector3(objRotation.eulerAngles.x,objRotation.eulerAngles.y + rotacao, objRotation.eulerAngles.z));
                construcaoControllerHit = null;
            }
        }
        else{
            //Ray frontal não tocou algo
            Ray r2 = new Ray(new Vector3(r.GetPoint(distanciaMax).x, r.GetPoint(distanciaMax).y + 100f, r.GetPoint(distanciaMax).z),new Vector3(0f,-1f,0f));
            Debug.DrawRay(r2.origin,r2.direction,Color.blue);
            RaycastHit[] hits = Physics.RaycastAll(r2,Mathf.Infinity,construcao.layerMask);
            if(hits.Length>0){
                //Ray secundario, vindo do ceu, tocou algo
                objPosition = new Vector3(hits[0].point.x,hit.point.y + construcao.altura + (meshObjeto.bounds.size.y * objeto.transform.localScale.y / 2),hits[0].point.z);
                Vector3 direction = transform.position - objPosition;
                direction.y = 0f;
                objRotation.SetLookRotation(direction);
                objRotation = Quaternion.Euler(new Vector3(objRotation.eulerAngles.x, objRotation.y + rotacao, objRotation.eulerAngles.z));
                //-objeto.transform.LookAt(new Vector3(transform.position.x,   objeto.transform.position.y, transform.position.z));
                //-objeto.transform.Rotate(new Vector3(0f, rotacao, 0f));
            }
            else{
                podeConstruir = false;
                Debug.Log("Sem chão");
            }
            construcaoControllerHit = null;
        }
    }

    public void AlterarCor(bool isPodeConstruir){
        if(isPodeConstruir){
            objeto.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", Color.green);
            objeto.GetComponent<MeshRenderer>().material.SetColor("_EmissiveColor", (Vector4)Color.green);
        }
        else{
            objeto.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", Color.red);
            objeto.GetComponent<MeshRenderer>().material.SetColor("_EmissiveColor", (Vector4)Color.red);
        }
    }

    public float percColisaoMax = 0.9f;
    bool VerificarSePodeConstruir(){
        Collider[] colliders = Physics.OverlapBox(objeto.transform.position, 
            new Vector3(objeto.transform.localScale.x * meshObjeto.bounds.size.x / 2 * percColisaoMax, objeto.transform.localScale.y * meshObjeto.bounds.size.y / 2 * percColisaoMax,
            objeto.transform.localScale.z * meshObjeto.bounds.size.z / 2 * percColisaoMax), objeto.transform.rotation, lMaskProibidos);
        if (colliders.Length > 0)
        {
            foreach (Collider col in colliders)
            {
                if (col.gameObject != objeto)
                {
                    return false;
                }
            }
        }
        return true;
    }

    bool PegarHitMaisProx(){
        Ray r = new Ray(transform.position, transform.forward);
        RaycastHit[] hits = Physics.RaycastAll(r, distanciaMax, construcao.layerMask);
        float menorDistancia = 999999;
        RaycastHit hitMaisProx;
        if(hits.Length > 0){
            hitMaisProx = hits[0];
            foreach(RaycastHit h in hits){
                if(h.collider.tag == "construcao"){
                    if(Vector3.Distance(h.collider.transform.position, h.point) < menorDistancia){
                        menorDistancia = Vector3.Distance(h.collider.transform.position, h.point);
                        hitMaisProx = h;
                    }
                }
                
            }
            hit = hitMaisProx;
            return true;
        }
        else return false;
    }

}