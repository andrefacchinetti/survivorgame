using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AldeiaController : MonoBehaviour
{

    [SerializeField] public GameObject centroDaAldeia, armazemPesca;
    [SerializeField] public List<LocalProfissao> locaisSeguranca, locaisPesca;


    [System.Serializable]
    public struct LocalProfissao
    {
        public Transform localPosicao;
        public Transform localOlhando;
    }

}
