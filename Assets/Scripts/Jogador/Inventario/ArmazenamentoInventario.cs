using UnityEngine;
using System.Collections.Generic;

public class ArmazenamentoInventario : MonoBehaviour
{

    [SerializeField] public GameObject canvasArmazenamento;
    [SerializeField] public Inventario inventario;
    [SerializeField] public List<SlotHotbar> slots = new List<SlotHotbar>();


    [HideInInspector] public Armazenamento armazenamentoEmUso;

    public void AcessarArmazenamento(Armazenamento armazenamento)
    {
        armazenamentoEmUso = armazenamento;
        //Add itens na hud
        inventario.AbrirInventario();
        canvasArmazenamento.SetActive(true);
    }

    public void SairArmazenamento()
    {
        //Limpar itens da hud
        armazenamentoEmUso = null;
        inventario.FecharInventario();
    }

}
