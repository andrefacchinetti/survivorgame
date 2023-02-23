using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fogueira : MonoBehaviour
{

    [SerializeField] public Fogo fogo;
    //[SerializeField] public Panela panela;
    //[SerializeField] public GameObject slotPanela;

    public void AcenderFogueira()
    {
        Debug.Log("fogueira acesa");
        fogo.gameObject.SetActive(true);
        fogo.isFogoAceso = true;
    }

    public void ApagarFogueira()
    {
        Debug.Log("fogueira apagada");
        fogo.gameObject.SetActive(false);
        fogo.isFogoAceso = false;
    }

}
