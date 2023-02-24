using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ControleConstruir : MonoBehaviour
{
    public bool isAtivo = false, podeJuntar, isConectado;
    public float distanciaMax, rotacao, velRotacao;
    public GameObject objeto;
    public GameObject constructionUI;
    public Construcao construcao;
    public Construcao.TipoConstrucao tipoConstrucao;
    public Mesh meshObjeto;
    [SerializeField]
    public List<Construcao.conStruct> conStructs;
 
    
    void Update(){
        if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleModoConstrucao();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (tipoConstrucao.Equals(Construcao.TipoConstrucao.chao)) tipoConstrucao = Construcao.TipoConstrucao.parede;
            else if (tipoConstrucao.Equals(Construcao.TipoConstrucao.parede)) tipoConstrucao = Construcao.TipoConstrucao.chao;
        }
        if (isAtivo)
        {
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
                
                Instantiate(construcao.gameObject,objeto.transform.position,objeto.transform.rotation);
            }
        }
    }

    private void ToggleModoConstrucao()
    {
        constructionUI.SetActive(!constructionUI.activeSelf);
        isAtivo = !isAtivo;
    }

    public void LocalConstrucao()
    {
        Ray r = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        isConectado = false;
        if (Physics.Raycast(r, out hit, distanciaMax, construcao.layerMask))
        {
            //Ray frontal tocou algo

            if (hit.collider.gameObject.tag == "construcao" && podeJuntar)
            {
                if(System.Array.IndexOf(construcao.nomeTerreno, hit.transform.name) != -1){
                    //se o objeto tocado tiver o nome do encaixe certo
                    objeto.transform.rotation = hit.transform.rotation;
                    objeto.transform.Rotate(new Vector3(0f,rotacao-(rotacao%90),0f));
                    objeto.transform.position = new Vector3(hit.transform.position.x, hit.transform.position.y + construcao.altura, hit.transform.position.z);
                    isConectado = true;
                }
                else
                {
                    objeto.transform.position = new Vector3(hit.point.x, hit.point.y + construcao.altura + (meshObjeto.bounds.size.y * objeto.transform.localScale.y / 2), hit.point.z);
                    objeto.transform.LookAt(new Vector3(transform.position.x, objeto.transform.position.y, transform.position.z));
                    objeto.transform.Rotate(new Vector3(0f,rotacao,0f));
                }
            }
            else{
                objeto.transform.position = new Vector3(hit.point.x, hit.point.y + construcao.altura + (meshObjeto.bounds.size.y * objeto.transform.localScale.y / 2), hit.point.z);
                objeto.transform.LookAt(new Vector3(transform.position.x, objeto.transform.position.y,transform.position.z));
                objeto.transform.Rotate(new Vector3(0f, rotacao, 0f));
            }
        }
        else{
            //Ray frontal não tocou algo

            Ray r2 = new Ray(new Vector3(r.GetPoint(distanciaMax).x, r.GetPoint(distanciaMax).y + 100f, r.GetPoint(distanciaMax).z),new Vector3(0f,-1f,0f));
            Debug.DrawRay(r2.origin,r2.direction,Color.blue);
            if(Physics.Raycast(r2, out hit, Mathf.Infinity,construcao.layerMask)){
                //Ray secundario, vindo do ceu, tocou algo
                
                objeto.transform.position = new Vector3(hit.point.x,hit.point.y + construcao.altura + (meshObjeto.bounds.size.y * objeto.transform.localScale.y / 2),hit.point.z);
                objeto.transform.LookAt(new Vector3(transform.position.x,   objeto.transform.position.y, transform.position.z));
                objeto.transform.Rotate(new Vector3(0f, rotacao, 0f));
            }
            else{
                Debug.Log("Sem chão");
            }
        }
    }

}