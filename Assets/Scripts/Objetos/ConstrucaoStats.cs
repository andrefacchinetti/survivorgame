using UnityEngine;
using Opsive.Shared.Inventory;

public class ConstrucaoStats : MonoBehaviour
{

    [SerializeField] public ItemDefinitionBase itemMarteloReparador, itemMarteloDemolidor;
    [SerializeField] public ConstrucoesController construcoesController;

    public void AcoesTomouDano()
    {
        Debug.Log("construcao tomou dano");
        //TODO: Mostrar dano visual
    }

    public void AcoesMorreu()
    {
        Debug.Log("construcao morreu");
        construcoesController.MandarDestruirTodasAsConstrucoesConectadas();
    }

}
