using UnityEngine;
using UnityEngine.Rendering;

public class BFX_DemoTest : MonoBehaviour
{
    // Variáveis públicas
    public bool InfiniteDecal;
    public Light DirLight;
    public bool isVR = true;
    public GameObject BloodAttach;
    public GameObject[] BloodFX;

    // Função para encontrar o osso mais próximo a partir de um hit
    Transform GetNearestObject(Transform hit, Vector3 hitPos)
    {
        // Inicialização de variáveis
        var closestPos = 100f;
        Transform closestBone = null;
        var childs = hit.GetComponentsInChildren<Transform>();

        // Iteração sobre os filhos em busca do osso mais próximo
        foreach (var child in childs)
        {
            var dist = Vector3.Distance(child.position, hitPos);
            if (dist < closestPos)
            {
                closestPos = dist;
                closestBone = child;
            }
        }

        // Verifica a distância até o osso raiz
        var distRoot = Vector3.Distance(hit.position, hitPos);
        if (distRoot < closestPos)
        {
            closestPos = distRoot;
            closestBone = hit;
        }
        return closestBone;
    }

    // Variáveis da função Update
    public Vector3 direction;
    int effectIdx;
    int activeBloods;

    public void SangrarAlvo(Collider collider)
    {
        // Cálculo do ângulo para a orientação do efeito de sangue
        float angle = 0; // Modifique conforme necessário

        // Posição para instanciar o efeito de sangue
        Vector3 spawnPosition = collider.ClosestPointOnBounds(transform.position);

        // Instancia um efeito de sangue na posição do hit
        if (effectIdx == BloodFX.Length) effectIdx = 0;
        var instance = Instantiate(BloodFX[effectIdx], spawnPosition, Quaternion.Euler(0, angle + 90, 0));
        effectIdx++;
        activeBloods++;

        // Configurações do componente BFX_BloodSettings no efeito de sangue
        var settings = instance.GetComponent<BFX_BloodSettings>();
        settings.LightIntensityMultiplier = DirLight.intensity;

        // Encontra o osso mais próximo e instancia um objeto de sangue anexado a ele
        var nearestBone = GetNearestObject(collider.transform.root, spawnPosition);
        if (nearestBone != null)
        {
            var attachBloodInstance = Instantiate(BloodAttach);
            var bloodT = attachBloodInstance.transform;
            bloodT.position = spawnPosition;
            bloodT.localRotation = Quaternion.identity;
            bloodT.localScale = Vector3.one * Random.Range(0.75f, 1.2f);
            bloodT.LookAt(spawnPosition + collider.transform.up, direction);
            bloodT.Rotate(90, 0, 0);
            bloodT.transform.parent = nearestBone;
        }
    }

    // Função para calcular o ângulo entre dois vetores
    public float CalculateAngle(Vector3 from, Vector3 to)
    {
        return Quaternion.FromToRotation(Vector3.up, to - from).eulerAngles.z;
    }

    Collider GetColliderUnderMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            return hit.collider;
        }

        return null;
    }

}
