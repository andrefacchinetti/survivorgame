using UnityEngine;
using Photon.Pun;

public class CollisorCausaDano : MonoBehaviourPunCallbacks
{

    StatsGeral statsGeral;
    PhotonView PV;

    private void Awake()
    {
        statsGeral = GetComponentInParent<StatsGeral>();
        PV = GetComponentInParent<PhotonView>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!statsGeral.isAttacking) return;

        if(other.transform.tag == "PlayerCollider")
        {
            Debug.Log("bateu no player collider");
            other.GetComponentInParent<StatsGeral>().TakeDamage(statsGeral.damage, true);
            statsGeral.isAttacking = false;
        }
        if(other.transform.tag == "AnimalCollider" || other.transform.tag == "ConstrucaoCollider")
        {
            Debug.Log("bateu no animal collider");
            CollisorSofreDano collisorSofreDano = other.gameObject.GetComponent<CollisorSofreDano>();
            if (collisorSofreDano != null && collisorSofreDano.PV.ViewID != PV.ViewID) //VERIFICA SE NAO ESTA BATENDO EM SI PROPRIO
            {
                if (!(GetComponentInParent<LobisomemController>() != null && other.gameObject.GetComponentInParent<LobisomemController>() != null)) //Verificar que um lobo nao esta batendo em outro lobo
                {
                    Debug.Log("lobo nao ta batendo em outro lobo amigo");
                    float damage = statsGeral.damage;
                    StatsGeral objPai = collisorSofreDano.gameObject.GetComponentInParent<StatsGeral>();
                    if (collisorSofreDano.isConstrucao)
                    {
                        if (this.GetComponent<ItemObjMao>() != null && this.GetComponent<ItemObjMao>().itemDefinition.name.Equals(objPai.construcaoStats.itemMarteloReparador.name))
                        {
                            Debug.Log("curando construcao");
                            objPai.TakeCura(damage);
                        }
                        else if (this.GetComponent<ItemObjMao>() != null && this.GetComponent<ItemObjMao>().itemDefinition.name.Equals(objPai.construcaoStats.itemMarteloDemolidor.name))
                        {
                            Debug.Log("demolindo contrucao");
                            objPai.GetComponent<ConstrucoesController>().DemolirConstrucao();
                        }
                        else
                        {
                            Debug.Log("dando dano em construcao");
                            objPai.TakeDamage(damage, false);
                        }
                    }
                    else
                    {
                        Debug.Log("dando dano take damake");
                        objPai.TakeDamage(damage, false);
                    }
                    statsGeral.isAttacking = false;
                }
            }
        }
        
    }

}
