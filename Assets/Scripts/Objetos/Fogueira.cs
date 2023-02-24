using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fogueira : MonoBehaviour
{

    [SerializeField] public Fogo fogo;
    [SerializeField] public Panela panela;
    [SerializeField] public GameObject slotPanela, slotTigela;

    public bool ColocarPanelaTigela(Item item)
    {
        if (panela != null) return false;
        if (item.nomeItem.Equals(Item.NomeItem.Panela))
        {
            slotPanela.SetActive(true);
            panela = slotPanela.GetComponent<Panela>();
        }
        if (item.nomeItem.Equals(Item.NomeItem.Tigela))
        {
            slotPanela.SetActive(true);
            panela = slotTigela.GetComponent<Panela>();
        }
        return true;
    }

    public void RetirarPanelaTigela()
    {
        panela = null;
        slotPanela.SetActive(false);
        slotPanela.SetActive(false);
    }

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
