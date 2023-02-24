using UnityEngine;
using System;
[System.Serializable]
public class Construcao : MonoBehaviour
{
    public TipoConstrucao tipoConstrucaoEnum;
    public int custo;
    public string[] nomeTerreno;
    public float altura;
    public bool podeJuntar;
    public LayerMask layerMask;
    [Serializable]
    public struct conStruct{
        public Construcao c1;
        public Mesh mesh;
    }
    public enum TipoConstrucao{chao,parede};
}
