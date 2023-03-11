using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumivel : MonoBehaviour
{

    [SerializeField] public TipoConsumivel tipoConsumivel;

    public enum TipoConsumivel
    {
        Fruta,
        Carne,
        Vegetal
    }

}
