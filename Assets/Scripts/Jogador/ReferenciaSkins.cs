using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenciaSkins : MonoBehaviour
{

    [SerializeField] GameObject[] objetosDaSkin;

    public void ativarObjetosDaSkin()
    {
        foreach(GameObject obj in objetosDaSkin)
        {
            obj.SetActive(true);
        }
    }

}
