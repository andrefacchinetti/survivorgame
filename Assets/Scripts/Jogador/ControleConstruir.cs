using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ControleConstruir : MonoBehaviour
{
    public bool isAtivo = false, podeJuntar, isConectado, podeConstruir;
    public float distanciaMax, rotacao, velRotacao;
    public GameObject objeto;
    public GameObject constructionUI;
    public Construcao construcao;
    public Construcao.TipoConstrucao tipoConstrucao;
    public Mesh meshObjeto;
    [SerializeField]
    public List<Construcao.conStruct> conStructs;
    public Inventario inventario;
    public LayerMask lMaskProibidos;
    private Vector3 objPosition;
    private Quaternion objRotation;
    private RaycastHit hit;
    
 
    
    void Update(){
        if(Input.GetButtonDown("Cancel")) ToggleModoConstrucao(false);
        if (Input.GetButtonDown("Construir"))
        {
            ToggleModoConstrucao(!isAtivo);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (tipoConstrucao.Equals(Construcao.TipoConstrucao.chao)) tipoConstrucao = Construcao.TipoConstrucao.parede;
            else if (tipoConstrucao.Equals(Construcao.TipoConstrucao.parede)) tipoConstrucao = Construcao.TipoConstrucao.chao;
        }
        if (isAtivo)
        {
            objeto.SetActive(true);
            Debug.DrawRay(transform.position, transform.forward * distanciaMax, Color.red);
            if(construcao == null || construcao.tipoConstrucaoEnum != tipoConstrucao){
                foreach(Construcao.conStruct c in conStructs){
                    if(c.c1.tipoConstrucaoEnum == tipoConstrucao){
                        construcao = c.c1;
                        podeJuntar = construcao.podeJuntar;
                        objeto.GetComponent<MeshFilter>().mesh = meshObjeto = c.mesh;
                        Debug.Log(objeto.GetComponent<MeshFilter>().mesh);
                    }
                }
            }
            LocalConstrucao();
            if(Input.GetButton("Rotacionar")){
                if(isConectado && Input.GetButtonDown("Rotacionar")){
                    rotacao+=90f;
                }
                else{
                    rotacao+=velRotacao;
                }
                rotacao = rotacao%360;
            }
            if(Input.GetButtonDown("Fire1")){
                if(inventario.VerificarQtdItem(construcao.material,construcao.custo) && podeConstruir){
                    Instantiate(construcao.gameObject, objeto.transform.position, objeto.transform.rotation);
                    inventario.RemoverItemDoInventarioPorNome(construcao.material, construcao.custo);
                }
                
            }
        }
        else{
            objeto.SetActive(false);
        }
    }

    private void ToggleModoConstrucao(bool toggle)
    {
        constructionUI.SetActive(toggle);
        isAtivo = toggle;
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
                objRotation = hit.transform.rotation;
                objRotation = Quaternion.Euler(new Vector3(objRotation.eulerAngles.x, objRotation.eulerAngles.y + rotacao - (rotacao % 90), objRotation.eulerAngles.z));
                objPosition = new Vector3(hit.transform.position.x, hit.transform.position.y + construcao.altura, hit.transform.position.z);
                isConectado = true;
                /* if(System.Array.IndexOf(construcao.nomeTerreno, hit.transform.name) != -1){
                    //se o objeto tocado tiver o nome do encaixe certo
                    objRotation = hit.transform.rotation;
                    objRotation = Quaternion.Euler(new Vector3(objRotation.eulerAngles.x,objRotation.eulerAngles.y + rotacao-(rotacao%90),objRotation.eulerAngles.z));
                    //objRotation.(new Vector3(0f,rotacao-(rotacao%90),0f));
                    objPosition = new Vector3(hit.transform.position.x, hit.transform.position.y + construcao.altura, hit.transform.position.z);
                    isConectado = true;
                }
                else
                {
                    objPosition = new Vector3(hit.point.x, hit.point.y + construcao.altura + (meshObjeto.bounds.size.y * objeto.transform.localScale.y / 2), hit.point.z);
                    Vector3 direction = transform.position - objPosition;
                    direction.y = 0f;
                    objRotation.SetLookRotation(direction);
                    //-objRotation(new Vector3(transform.position.x, objeto.transform.position.y, transform.position.z));
                    objRotation = Quaternion.Euler(new Vector3(objRotation.eulerAngles.x,rotacao, objRotation.eulerAngles.z));
                    //-objeto.transform.Rotate(new Vector3(0f,rotacao,0f));
                } */
            }
            else{
                objPosition = new Vector3(hit.point.x, hit.point.y + construcao.altura + (meshObjeto.bounds.size.y * objeto.transform.localScale.y / 2), hit.point.z);
                Vector3 direction = transform.position - objPosition;
                direction.y = 0f;
                objRotation = Quaternion.LookRotation(direction);
                objRotation = Quaternion.Euler(new Vector3(objRotation.eulerAngles.x,objRotation.eulerAngles.y + rotacao, objRotation.eulerAngles.z));

                
                //-objeto.transform.LookAt(new Vector3(transform.position.x, objeto.transform.position.y,transform.position.z));
                //-objeto.transform.Rotate(new Vector3(0f, rotacao, 0f));
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
                Debug.Log("Sem chão");
            }
        }
    }

    public void AlterarCor(bool podeConstruir){
        if(podeConstruir){
            objeto.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", Color.green);
            objeto.GetComponent<MeshRenderer>().material.SetColor("_EmissiveColor", (Vector4)Color.green);
        }
        else{
            objeto.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", Color.red);
            objeto.GetComponent<MeshRenderer>().material.SetColor("_EmissiveColor", (Vector4)Color.red);
        }
        
    }

    /* public bool VerificarSePodeConstruir(){
        Ray r1 = new Ray(new Vector3(objeto.transform.position.x - (meshObjeto.bounds.size.x/2),objeto.transform.position.y - (meshObjeto.bounds.size.y/2), objeto.transform.position.z - (meshObjeto.bounds.size.z/2)),new Vector3(objeto.transform.position.x - (meshObjeto.bounds.size.x / 2), objeto.transform.position.y - (meshObjeto.bounds.size.y / 2)- 0.1f, objeto.transform.position.z - (meshObjeto.bounds.size.z / 2)));
    } */

    void LateUpdate(){
        objeto.transform.position = objPosition;
        objeto.transform.rotation = objRotation;
        VerificarSePodeConstruir();
        AlterarCor(podeConstruir);
    }

    void OnDrawGizmos(){
        Gizmos.DrawWireCube(objeto.transform.position + objeto.transform.up * -1, new Vector3(objeto.transform.localScale.x * meshObjeto.bounds.size.x, objeto.transform.lossyScale.y * meshObjeto.bounds.size.y, objeto.transform.localScale.z * meshObjeto.bounds.size.z));
    }

    void VerificarSePodeConstruir(){
        Collider[] colliders = Physics.OverlapBox(objeto.transform.position, new Vector3(objeto.transform.localScale.x * meshObjeto.bounds.size.x / 2 * 0.95f, objeto.transform.localScale.y * meshObjeto.bounds.size.y / 2 * 0.95f, objeto.transform.localScale.z * meshObjeto.bounds.size.z / 2 * 0.95f), objeto.transform.rotation, lMaskProibidos);
        if (colliders.Length > 0)
        {
            foreach (Collider col in colliders)
            {
                if (col.gameObject != objeto)
                {
                    podeConstruir = false;
                }
            }
        }
        else
        {
            podeConstruir = true;
        }
    }

    bool PegarHitMaisProx(){
        Ray r = new Ray(transform.position, transform.forward);
        RaycastHit[] hits = Physics.RaycastAll(r, distanciaMax, construcao.layerMask);
        float menorDistancia = 999999;
        RaycastHit hitMaisProx;
        if(hits.Length > 0){
            hitMaisProx = hits[0];
            foreach(RaycastHit h in hits){
                Debug.Log(h.collider.tag);
                if(h.collider.tag == "construcao"){
                    Debug.Log("oi");
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