using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerificaObstaculo : MonoBehaviour
{
    [SerializeField] private LobisomemMovimentacao lobisomemMovimentacao;
    [SerializeField] private float detectionDistance = 2f;
    [SerializeField] private float raycastInterval = 0.9f;

    private float raycastTimer;

    void Update()
    {
        if (!lobisomemMovimentacao.statsGeral.health.IsAlive()) return;

        raycastTimer += Time.deltaTime;
        if (raycastTimer >= raycastInterval)
        {
            raycastTimer = 0;
            DetectObstacles();
        }
    }

    private void DetectObstacles()
    {
        RaycastHit hit;
        Vector3 forward = transform.TransformDirection(Vector3.forward);

        if (Physics.Raycast(transform.position, forward, out hit, detectionDistance))
        {
            if (hit.collider.CompareTag("ConstrucaoStats") && hit.collider.gameObject.name != "Fundação")
            {
                lobisomemMovimentacao.agent.ResetPath();
                lobisomemMovimentacao.targetObstaculo = hit.transform;
            }
        }
        else
        {
            lobisomemMovimentacao.targetObstaculo = null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * detectionDistance);
    }
}
