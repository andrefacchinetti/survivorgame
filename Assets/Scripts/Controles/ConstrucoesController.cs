using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class ConstrucoesController : MonoBehaviourPunCallbacks
{
    
    [SerializeField] public bool isPlataforma;
    [HideInInspector] public ConstrucoesController plataformaPai;
    [HideInInspector] public List<ConstrucoesController> listaConstrucoesConectadas;
    [HideInInspector] public StatsGeral statsGeral;

    private void Awake()
    {
        listaConstrucoesConectadas = new List<ConstrucoesController>();
        statsGeral = GetComponent<StatsGeral>();
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
            plataformaPai.listaConstrucoesConectadas.Add(construcaoNova);
            construcaoNova.plataformaPai = plataformaPai;
        }
        else 
        {
            listaConstrucoesConectadas.Add(construcaoNova);
            construcaoNova.plataformaPai = this;
        }
    }

    public void removerConstrucaoDaPlataforma(ConstrucoesController construcaoParaRemover)
    {
        foreach(ConstrucoesController construcao in listaConstrucoesConectadas)
        {
            if(construcao == construcaoParaRemover)
            {
                listaConstrucoesConectadas.Remove(construcao);
                return;
            }
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
            destruirConstrucao(construcao.gameObject);
        }
        destruirConstrucao(this.gameObject);
    }

    public void DemolirConstrucao()
    {
        if (isTenhoPai())
        {
            plataformaPai.removerConstrucaoDaPlataforma(this);
        }
        destruirConstrucao(this.gameObject);
    }

    private void destruirConstrucao(GameObject objConstrucao)
    {
        //TODO: EFEITO DA CONSTRUCAO SENDO DESTRUIDA EM PEDA�OS
        GameObject objPaiParaDestruir = objConstrucao.GetComponent<StatsGeral>().objPaiParaDestruir != null ? objConstrucao.GetComponent<StatsGeral>().objPaiParaDestruir : objConstrucao;
        objPaiParaDestruir.GetComponent<StatsGeral>().DroparObjetosAoSerDestruido();
        PhotonNetwork.Destroy(objPaiParaDestruir.gameObject);
    }

}
