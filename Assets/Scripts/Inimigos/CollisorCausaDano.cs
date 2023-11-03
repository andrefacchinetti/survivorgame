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
        CollisorSofreDano collisorSofreDano = other.gameObject.GetComponent<CollisorSofreDano>();
        if (collisorSofreDano != null && collisorSofreDano.PV.ViewID != PV.ViewID) //VERIFICA SE NAO ESTA BATENDO EM SI PROPRIO
        {
            if (!(GetComponentInParent<LobisomemController>() != null && other.gameObject.GetComponentInParent<LobisomemController>() != null)) //Verificar que um lobo nao esta batendo em outro lobo
            {
                float damage = statsGeral.damage;
                ItemObjMao itemObjMao = GetComponent<ItemObjMao>();
                if (itemObjMao != null)
                {
                    damage = collisorSofreDano.CalcularDanoPorArmaCausandoDano(itemObjMao, statsGeral.damage);
                }
                GameObject objPai = collisorSofreDano.gameObject.GetComponentInParent<StatsGeral>().gameObject;
                if (collisorSofreDano.isConstrucao && this.GetComponent<ItemObjMao>() != null && (this.GetComponent<ItemObjMao>().nomeItem.Equals(Item.NomeItem.MarteloSimples) || this.GetComponent<ItemObjMao>().nomeItem.Equals(Item.NomeItem.MarteloAvancado)))
                {
                    objPai.GetComponent<StatsGeral>().TakeCura(damage);
                }
                else
                {
                    objPai.GetComponent<StatsGeral>().TakeDamage(damage);
                }
                statsGeral.isAttacking = false;
                if (statsGeral.gameObject.tag == "Player")
                {
                    statsGeral.gameObject.GetComponent<Animator>().SetTrigger("ferramentaFrenteExit");
                }
            }
        }
    }

}
