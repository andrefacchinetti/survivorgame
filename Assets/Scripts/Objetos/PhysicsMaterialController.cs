using UnityEngine;

public class PhysicsMaterialController : MonoBehaviour
{
    public PhysicsMaterial iceMaterial; // Arraste seu Physics Material de gelo aqui no inspector
    void OnCollisionEnter(Collision collision)
    {
        // Obt�m o collider do objeto com o qual colidimos
        Collider otherCollider = collision.collider;

        // Verifica se o collider possui um material de f�sica
        if (otherCollider.material != null)
        {
            // Compara o material de f�sica do collider com o material de gelo
            if (otherCollider.material == iceMaterial)
            {
                Debug.Log("Entrou em contato com o material de gelo!");
                // Execute a l�gica necess�ria aqui
            }
        }
    }
}
