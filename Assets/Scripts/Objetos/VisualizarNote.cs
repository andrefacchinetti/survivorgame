using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualizarNote : MonoBehaviour
{

    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] Texture texturaBR, texturaEN;
    [SerializeField] GameObject canvasNote;
    private GameObject playerVisualizando;
    
    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.GetInt("INDEXIDIOMA") == 1) //PORTUGUES
        {
            meshRenderer.material.mainTexture = texturaBR;
        }
        else
        {
            meshRenderer.material.mainTexture = texturaEN;
        }
    }

    private void Update()
    {
        if(playerVisualizando != null)
        {
            float dist = Vector3.Distance(playerVisualizando.transform.position, this.transform.position);
            if (dist > 4)
            {
                FecharNote();
            }
        }
    }

    public void ToggleVisualizarNote(GameObject playerObj)
    {
        if (canvasNote.activeSelf)
        {
            FecharNote();
        }
        else
        {
            canvasNote.SetActive(true);
            playerVisualizando = playerObj;
        }
        
    }

    public void FecharNote()
    {
        canvasNote.SetActive(false);
        playerVisualizando = null;
    }

}
