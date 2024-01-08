using System.Collections;
using Photon.Pun;
using UnityEngine;

public class Desaparecer : MonoBehaviour
{

    public float duracaoDesaparecimento = 2f; // A duração do efeito de desaparecimento
    private Material material; // Material do objeto para modificar a cor

    void Start()
    {
        // Certifique-se de que o objeto tem um material
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            material = renderer.material;
        }
        else
        {
            Debug.LogError("O objeto não tem um Renderer com um material!");
        }
    }

    public void DesaparecerObj()
    {
        // Inicia a rotina de desaparecimento
        StartCoroutine(DesaparecimentoGradual());
    }

    IEnumerator DesaparecimentoGradual()
    {
        float tempoDecorrido = 0f;
        Color corInicial = material.color;
        Color corFinal = new Color(corInicial.r, corInicial.g, corInicial.b, 0f); // Cor final com alpha zero

        while (tempoDecorrido < duracaoDesaparecimento)
        {
            // Interpola a cor gradualmente
            material.color = Color.Lerp(corInicial, corFinal, tempoDecorrido / duracaoDesaparecimento);

            // Atualiza o tempo decorrido
            tempoDecorrido += Time.deltaTime;

            yield return null;
        }

        // Garante que o objeto seja destruído após o efeito de desaparecimento
        if (PhotonNetwork.IsConnected) PhotonNetwork.Destroy(this.gameObject);
        else GameObject.Destroy(this.gameObject);
    }
}
