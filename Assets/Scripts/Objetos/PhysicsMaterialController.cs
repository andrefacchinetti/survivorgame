using UnityEngine;

public class PhysicsMaterialController : MonoBehaviour
{
    public PhysicsMaterial iceMaterial; // Arraste seu Physics Material de gelo aqui no inspector
    void OnCollisionEnter(Collision collision)
    {
        // Obtém o collider do objeto com o qual colidimos
        Collider otherCollider = collision.collider;

        // Verifica se o collider possui um material de física
        if (otherCollider.material != null)
        {
            // Compara o material de física do collider com o material de gelo
            if (otherCollider.material == iceMaterial)
            {
                Debug.Log("Entrou em contato com o material de gelo!");
                // Execute a lógica necessária aqui
            }
        }
    }
}
