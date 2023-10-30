using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gizmo : MonoBehaviour
{
    public Color corGizmo = Color.red;
    public float tamanhoEsfera = 0.1f;
    public float tamanhoArame = 0.2f;

    void OnDrawGizmos()
    {
        Gizmos.color = corGizmo;
        Gizmos.DrawSphere(transform.position, tamanhoEsfera);
        Gizmos.DrawWireSphere(transform.position, tamanhoArame);
    }
}
