using System.Collections;
using System.Collections.Generic;
using Obi;
using UnityEngine;

public class CordaWeapon : MonoBehaviour
{

    [SerializeField] public bool isThirdPerson = false;
    [SerializeField] public GameObject objObiSolver, objObiRope, objCordaSemGrab, objCordaMaos;
    [SerializeField] public RopeEstoura ropeEstoura;
    [SerializeField] public PointRopeFollow ropeGrab;
    [SerializeField] public GameObject pivotRopeStart, pivotRopeEnd;
    [SerializeField] public GameObject prefabCorda;

    [SerializeField] Material materialInvisivel;
    
    [HideInInspector] public PlayerController playerController;

    private void Start()
    {
        foreach(MeshRenderer mesh in GetComponentsInChildren<MeshRenderer>())
        {
            mesh.material = materialInvisivel;
        }
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

    public void AcoesRenovarCordaEstourada(bool isCordaPartindo)
    {
        Debug.LogWarning("AcoesRenovarCordaEstourada 2");
        CancelInvoke("SumirObjRopeStart");
        Transform positionRope = objObiRope.gameObject.transform;
        GameObject novaCorda = Instantiate(prefabCorda, positionRope.position, positionRope.rotation, objObiSolver.transform);
        foreach (MeshRenderer mesh in novaCorda.GetComponentsInChildren<MeshRenderer>())
        {
            mesh.material = materialInvisivel;
        }
        ropeEstoura = novaCorda.GetComponent<RopeEstoura>();
        ropeEstoura.playerController = playerController;
        if(ropeEstoura != null)
        {
            Debug.LogWarning("rope estoura setado 3");
        }
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
        playerController.inventario.UngrabCoisasCapturadasComCorda(isCordaPartindo);
    }

}
