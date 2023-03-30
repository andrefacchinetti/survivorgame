using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArvoreFrutifera : MonoBehaviour
{

    StatsGeral statsGeral;
    [SerializeField] GameObject[] objFrutas;

    private void Awake()
    {
        statsGeral = GetComponent<StatsGeral>();
    }

    public void DesaparecerUmaFrutaDaArvore()
    {
        foreach(GameObject obj in objFrutas)
        {
            if (obj.activeSelf)
            {
                obj.SetActive(false);
                return;
            }
        }
    }

}
