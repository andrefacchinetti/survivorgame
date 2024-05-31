using System.Collections;
using System.Collections.Generic;
using Obi;
using UnityEngine;

public class CordaWeapon : MonoBehaviour
{

    [SerializeField] public bool isThirdPerson = false;
    [SerializeField] public GameObject objObiSolver, objObiRope, objCordaSemGrab, objCordaMaos, objCordaEnvoltaMaos, objCordaNode;
    [SerializeField] public RopeEstoura ropeEstoura;
    [SerializeField] public PointRopeFollow ropeGrab;
    [SerializeField] public GameObject pivotRopeStart, pivotRopeEnd;
    [SerializeField] public GameObject prefabCorda;

    [SerializeField] Material materialInvisivel, materialCorda;
    MeshRenderer meshCordaGrab;
    
    [HideInInspector] public PlayerController playerController;

    private void Start()
    {
        foreach(MeshRenderer mesh in GetComponentsInChildren<MeshRenderer>())
        {
            if(mesh.gameObject != objCordaEnvoltaMaos && mesh.gameObject != objCordaNode && mesh.gameObject != objCordaSemGrab)
            {
                mesh.material = materialInvisivel;
            }
        }
        meshCordaGrab = objObiRope.GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        VerificarCordaPartindo();
    }

    private void VerificarCordaPartindo()
    {
        if (ropeEstoura.isCordaPartida && !ropeEstoura.isCordaEstourou)
        {
            Debug.LogWarning("VerificarCordaPartindo");
            if (isThirdPerson)
            {
                playerController.inventario.ConsumirItemDaMao(true);
                //TODO: Sound de corda partindo
            }
            ropeEstoura.isCordaEstourou = true;
            Invoke("SumirObjRopeStart", 1f);
        }
    }

    public void AcoesRenovarCordaEstourada(bool isCordaPartindo, bool isTerceiraPessoa)
    {
        CancelInvoke("SumirObjRopeStart");
        Transform positionRope = objObiRope.gameObject.transform;
        GameObject novaCorda = Instantiate(prefabCorda, positionRope.position, positionRope.rotation, objObiSolver.transform);
        if(isTerceiraPessoa) novaCorda.layer = 22;
        else novaCorda.layer = 29;
        foreach (MeshRenderer mesh in novaCorda.GetComponentsInChildren<MeshRenderer>())
        {
            if (mesh.gameObject != objCordaEnvoltaMaos && mesh.gameObject != objCordaNode && mesh.gameObject != objCordaSemGrab)
            {
                mesh.material = materialInvisivel;
            }
        }
        ropeEstoura = novaCorda.GetComponent<RopeEstoura>();
        ropeEstoura.playerController = playerController;
        
        ObiParticleAttachment[] attachs = novaCorda.GetComponents<ObiParticleAttachment>();
        foreach (ObiParticleAttachment attach in attachs)
        {
            if (attach.particleGroup.name == "START1")
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
        meshCordaGrab = objObiRope.GetComponent<MeshRenderer>();
        playerController.inventario.UngrabCoisasCapturadasComCorda(isCordaPartindo);
    }

    public void AcoesGrabandoAlvo()
    {
        objCordaMaos.SetActive(false);
        objObiRope.SetActive(true);
        meshCordaGrab.material = materialCorda;
    }

}
