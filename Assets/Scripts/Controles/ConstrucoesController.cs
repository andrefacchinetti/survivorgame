using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class ConstrucoesController : MonoBehaviourPunCallbacks
{
    
    [SerializeField] public bool isPlataforma;

    [SerializeField] public ConstrucoesController plataformaPai;

    [SerializeField] public List<ConstrucoesController> listaConstrucoesConectadas;

    private void Start()
    {
        listaConstrucoesConectadas = new List<ConstrucoesController>();
    }

    private bool isTenhoPai()
    {
        return plataformaPai != null;
    }

    public void inserirConstrucaoNaPlataforma(ConstrucoesController construcaoNova)
    {
        if (construcaoNova.isPlataforma) return;
        if (isTenhoPai())
        {
            Debug.Log("conectou no pai da construcao");
            plataformaPai.listaConstrucoesConectadas.Add(construcaoNova);
            construcaoNova.plataformaPai = plataformaPai;
        }
        else 
        {
            Debug.Log("conectou na plataforma");
            listaConstrucoesConectadas.Add(construcaoNova);
            construcaoNova.plataformaPai = this;
        }
    }
    public void MandarDestruirTodasAsConstrucoesConectadas()
    {
        if (isTenhoPai())
        {
            plataformaPai.DestruirTodasAsConstrucoesConectadas();
        }
        else
        {
            DestruirTodasAsConstrucoesConectadas();
        }
    }

    public void DestruirTodasAsConstrucoesConectadas()
    {
        foreach(ConstrucoesController construcao in listaConstrucoesConectadas)
        {
            PhotonNetwork.Destroy(construcao.gameObject);
        }
        PhotonNetwork.Destroy(this.gameObject);
    }

}
