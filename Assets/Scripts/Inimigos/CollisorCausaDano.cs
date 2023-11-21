using UnityEngine;
using Photon.Pun;

public class CollisorCausaDano : MonoBehaviourPunCallbacks
{

    StatsGeral statsGeral;
    PhotonView PV;
    BFX_DemoTest bloodController;

    private void Awake()
    {
        statsGeral = GetComponentInParent<StatsGeral>();
        PV = GetComponentInParent<PhotonView>();
        bloodController = GameObject.FindGameObjectWithTag("BloodController").GetComponent<BFX_DemoTest>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!statsGeral.isAttacking) return;

        if(other.transform.tag == "PlayerCollider")
        {
            Debug.Log("bateu no player collider");
            bloodController.SangrarAlvo(other, this.GetComponent<Collider>());
            other.GetComponentInParent<StatsGeral>().TakeDamage(statsGeral.damage);
            statsGeral.isAttacking = false;
        }
        CollisorSofreDano collisorSofreDano = other.gameObject.GetComponent<CollisorSofreDano>();
        if (collisorSofreDano != null && collisorSofreDano.PV.ViewID != PV.ViewID) //VERIFICA SE NAO ESTA BATENDO EM SI PROPRIO
        {
            if (!(GetComponentInParent<LobisomemController>() != null && other.gameObject.GetComponentInParent<LobisomemController>() != null)) //Verificar que um lobo nao esta batendo em outro lobo
            {
                float damage = statsGeral.damage;
                StatsGeral objPai = collisorSofreDano.gameObject.GetComponentInParent<StatsGeral>();
                if (collisorSofreDano.isConstrucao)
                {
                    if(this.GetComponent<ItemObjMao>() != null && this.GetComponent<ItemObjMao>().itemDefinition.name.Equals(statsGeral.itemRepairHammer.name))
                    {
                        objPai.TakeCura(damage);
                    }
                    else if(this.GetComponent<ItemObjMao>() != null && this.GetComponent<ItemObjMao>().itemDefinition.name.Equals(statsGeral.itemDemolitionHammer.name))
                    {
                        objPai.GetComponent<ConstrucoesController>().DemolirConstrucao();
                    }
                    else
                    {
                        objPai.TakeDamage(damage);
                    }
                }
                else
                {
                    bloodController.SangrarAlvo(other, this.GetComponent<Collider>());
                    objPai.TakeDamage(damage);
                }
                statsGeral.isAttacking = false;
            }
        }
    }

}
