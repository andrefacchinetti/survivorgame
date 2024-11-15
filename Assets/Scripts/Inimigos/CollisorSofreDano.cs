using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Opsive.Shared.Inventory;
using Opsive.Shared.Events;
using Opsive.UltimateCharacterController.Items.Actions.Impact;
using Opsive.UltimateCharacterController.Items;
using Opsive.UltimateCharacterController.Items.Actions;
using Opsive.UltimateCharacterController.Character;

public class CollisorSofreDano : MonoBehaviourPunCallbacks
{

    [SerializeField] ItemDefinitionBase[] ferramentasRecomendadas;
    [SerializeField] public bool isApenasFerramentaRecomendadaCausaDano = false;
  
    [HideInInspector] public StatsGeral statsGeral;
    [SerializeField] public bool isConstrucao;
    [HideInInspector] public PhotonView PV;

    private void Awake()
    {
        EventHandler.RegisterEvent<ImpactCallbackContext>(gameObject, "OnObjectImpact", OnImpact);
        statsGeral = GetComponentInParent<StatsGeral>();
        PV = GetComponentInParent<PhotonView>();
    }

    private void OnImpact(ImpactCallbackContext ctx)
    {
        Debug.Log("Event received " + name + " impacted by " + ctx.ImpactCollisionData.SourceGameObject + " on collider " + ctx.ImpactCollisionData.ImpactCollider + ".");
        if(isConstrucao && ctx.ImpactCollisionData.SourceGameObject != null)
        {
            CharacterItem ci = ctx.ImpactCollisionData.SourceGameObject.GetComponent<CharacterItem>();
            if(ci != null)
            {
                if(ci.ItemDefinition.name == statsGeral.construcaoStats.itemMarteloDemolidor.name)
                {
                    statsGeral.DestruirGameObject();
                }
            }
        }
    }

    public float CalcularDanoPorArmaCausandoDano(ItemDefinitionBase itemNaMao, float damage)
    {
        if (itemNaMao == null) return damage;
        if (isApenasFerramentaRecomendadaCausaDano)
        {
            if (!estaNaListaDeFerramentas(itemNaMao))
            {
                damage = 0;
            }
        }
        return damage;
    }

    private bool estaNaListaDeFerramentas(ItemDefinitionBase itemDefinition)
    {
        foreach(ItemDefinitionBase ferramentaRecomendada in this.ferramentasRecomendadas)
        {
            if (itemDefinition.name.Equals(ferramentaRecomendada.name))
            {
                return true;
            }
        }
        return false;
    }

}
