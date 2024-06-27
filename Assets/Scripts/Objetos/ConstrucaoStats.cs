using UnityEngine;
using Opsive.Shared.Inventory;

public class ConstrucaoStats : MonoBehaviour
{

    [SerializeField] public ItemDefinitionBase itemMarteloReparador, itemMarteloDemolidor;
    [SerializeField] public ConstrucoesController construcoesController;
    private Armazenamento armazenamento;

    private void Awake()
    {
        armazenamento = GetComponent<Armazenamento>();
    }

    public void AcoesTomouDano()
    {
        Debug.Log("construcao tomou dano");
    }

    public void AcoesMorreu()
    {
        Debug.Log("construcao morreu");
        construcoesController.MandarDestruirTodasAsConstrucoesConectadas();
        if (armazenamento != null) armazenamento.DroparTodosItensNoChao();
    }

}
