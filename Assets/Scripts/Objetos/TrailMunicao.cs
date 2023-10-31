using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailMunicao : MonoBehaviour
{

    public GameObject trailObj;

    public void AtivarTrailMunicao()
    {
        trailObj.SetActive(true);
        StartCoroutine(DesativarParticulasAposTempo());
    }

    IEnumerator DesativarParticulasAposTempo()
    {
        yield return new WaitForSeconds(0.5f); // Tempo de vida do rastro
        trailObj.SetActive(false); // Desativar o sistema de partículas
        Destroy(this.gameObject);
    }

}
