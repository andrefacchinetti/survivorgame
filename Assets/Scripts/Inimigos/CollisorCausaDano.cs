using UnityEngine;

public class CollisorCausaDano : MonoBehaviour
{

    StatsGeral statsGeral;

    private void Awake()
    {
        statsGeral = GetComponentInParent<StatsGeral>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!statsGeral.isAttacking) return;
        CollisorSofreDano collisorSofreDano = other.gameObject.GetComponent<CollisorSofreDano>();
        if (collisorSofreDano != null)
        {
            float damage = statsGeral.damage;
            ItemObjMao itemObjMao = GetComponent<ItemObjMao>();
            if (itemObjMao != null)
            {
                damage = collisorSofreDano.CalcularDanoPorArmaCausandoDano(itemObjMao, statsGeral.damage);
            }
            GameObject objPai = collisorSofreDano.gameObject.GetComponentInParent<StatsGeral>().gameObject;
            objPai.GetComponent<StatsGeral>().TakeDamage(damage);
            statsGeral.isAttacking = false;
            if (statsGeral.gameObject.tag == "Player")
            {
                statsGeral.gameObject.GetComponent<Animator>().SetTrigger("ferramentaFrenteExit");
            }
        }
    }

}
