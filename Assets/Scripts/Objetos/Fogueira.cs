using Opsive.UltimateCharacterController.Inventory;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Fogueira : MonoBehaviour
{

    [SerializeField] float durabilidadeSegundos = 500;
    [SerializeField] bool isQuebrada = false;
    [SerializeField] public Fogo fogo;
    [SerializeField] [HideInInspector] public Panela panela;
    [SerializeField] public GameObject slotPanela, slotTigela;
    [SerializeField] public GameObject fogueiraInteira, fogueiraQuebrada;


    void Update()
    {
        if (isQuebrada) return;
        if (fogo != null && fogo.isFogoAceso)
        {
            durabilidadeSegundos -= Time.deltaTime;
            if (durabilidadeSegundos <= 0) Quebrar();
        }
    }

    void Quebrar()
    {
        ApagarFogueira();
        gameObject.layer = 2;
        fogueiraInteira.SetActive(false);
        fogueiraQuebrada.SetActive(true);
        isQuebrada = true;
    }

    public bool ColocarPanelaTigela(Item item)
    {
        if (panela != null) { 
            return false;
        }
        else
        {
            if (item.itemIdentifierAmount.ItemDefinition.name.Equals(item.inventario.itemPanela.name))
            {
                slotPanela.SetActive(true);
                panela = slotPanela.GetComponent<Panela>();
            }
            if (item.itemIdentifierAmount.ItemDefinition.name.Equals(item.inventario.itemTigela.name))
            {
                slotTigela.SetActive(true);
                panela = slotTigela.GetComponent<Panela>();
            }
            return true;
        }
    }

    public void RetirarPanelaTigela()
    {
        panela = null;
        slotPanela.SetActive(false);
        slotTigela.SetActive(false);
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
        fogo.isFogoAceso = false;
        fogo.gameObject.SetActive(false);
    }

}
