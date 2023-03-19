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
            GameObject objPai = collisorSofreDano.gameObject.GetComponentInParent<StatsGeral>().gameObject;
            objPai.GetComponent<StatsGeral>().TakeDamage(statsGeral.damage);
            statsGeral.isAttacking = false;
        }
    }

}
