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
        GameObject objPai = null;
        if (other.gameObject.CompareTag("Player")) //Dar dano no player qdo collide
        {
            objPai = other.gameObject;
        }
        if(other.gameObject.GetComponent<CollisorSofreDano>() != null)
        {
            objPai = other.gameObject.GetComponent<CollisorSofreDano>().statsGeral.gameObject;
        }
        if (objPai != null && statsGeral.isAttacking)
        {
            objPai.GetComponent<StatsGeral>().TakeDamage(statsGeral.damage);
            statsGeral.isAttacking = false;
        }
    }

}
