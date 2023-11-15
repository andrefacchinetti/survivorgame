using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisorTriggerCausaDano : MonoBehaviour
{

    [SerializeField] float damage = 10;

    void OnTriggerEnter(Collider other)
    {
        CollisorSofreDano collisorSofreDano = other.gameObject.GetComponent<CollisorSofreDano>();
        if (collisorSofreDano != null)
        {
            GameObject objPai = collisorSofreDano.gameObject.GetComponentInParent<StatsGeral>().gameObject;
            objPai.GetComponent<StatsGeral>().TakeDamage(damage);
        }
    }

}
