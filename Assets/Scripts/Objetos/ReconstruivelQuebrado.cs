using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReconstruivelQuebrado : MonoBehaviour
{

    [SerializeField] public ReconstruivelStats reconstruivelStats;

    public void ConsertarReconstruivel()
    {
        reconstruivelStats.ConsertarReconstruivel();
    }

}
