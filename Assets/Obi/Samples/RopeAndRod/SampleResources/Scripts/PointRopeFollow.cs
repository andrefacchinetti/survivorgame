using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointRopeFollow : MonoBehaviour
{

    [SerializeField] public Transform posicaoInicial, objFollowed;

    void LateUpdate()
    {
        if (objFollowed == null)
        {
            this.transform.position = posicaoInicial.transform.position;
        }
        else
        {
            this.transform.position = objFollowed.transform.position;
        }
    }
}
