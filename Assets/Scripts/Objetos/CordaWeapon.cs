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
    
    [HideInInspector] public PlayerController playerController;

    private void Update()
    {
        VerificarCordaPartindo();
    }

    private void VerificarCordaPartindo()
    {
        if (ropeEstoura.isCordaPartida && !ropeEstoura.isCordaEstourou)
        {
            Debug.Log("VerificarCordaPartindo");
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
        Debug.Log("sumindo corda");
        CancelInvoke("SumirObjRopeStart");
        Transform positionRope = objObiRope.gameObject.transform;
        GameObject novaCorda = Instantiate(prefabCorda, positionRope.position, positionRope.rotation, objObiSolver.transform);
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
        playerController.inventario.UngrabCoisasCapturadasComCorda(isCordaPartindo);
    }

}
