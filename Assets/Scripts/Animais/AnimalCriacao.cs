using UnityEngine;
using Photon.Pun;
using Opsive.Shared.Inventory;
using System.Collections.Generic;

public class AnimalCriacao : MonoBehaviour
{
    [SerializeField] TipoAnimal tipo;
    [SerializeField] Genero genero;
    private Idade idade;
    [SerializeField] string[] pathsPrefabMachoFemea;
    [SerializeField] Item.ItemDropStruct[] itemsExtraAdultos;

    float tempoDeVida = 0, tempoEngravidou = 0;
    [SerializeField] float tempoParaVirarAdulto = 300, tempoParaEngravidarDenovo = 800; // Tempo em segundos para se tornar adulto (300 = 5 minutos)
    [SerializeField] float distanciaProcriacao = 5.0f; // Distância para considerar outro animal próximo

    PhotonView PV;
    private AnimalCriacao parceiroAtual;
    private int verificacoes;
    LayerMask enemyLayerMask;

    StatsGeral statsGeral;

    public enum Idade
    {
        Adulto,
        Filhote
    }

    public enum Genero
    {
        Macho,
        Femea
    }

    public enum TipoAnimal
    {
        Porco,
        Vaca,
        Veado,
        Coelho,
        Galinha,
        Ganso
    }

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        statsGeral = GetComponent<StatsGeral>();

        idade = (Idade)Random.Range(0, System.Enum.GetValues(typeof(Idade)).Length);
        enemyLayerMask = LayerMask.GetMask("Enemy");
    }

    void Start()
    {
        tempoDeVida = 0;
        if (idade == Idade.Filhote)
        {
            transform.localScale /= 1.3f;
        }

        if (genero == Genero.Femea)
        {
            InvokeRepeating("VerificarProcriacaoPeriodicamente", 0, tempoParaEngravidarDenovo); // Verifica a cada 5 minutos
        }
    }

    void Update()
    {
        // Incrementar o tempo de vida
        tempoDeVida += Time.deltaTime;

        // Incrementar o tempo desde que o animal gerou um filhote
        if (tempoEngravidou > 0)
        {
            tempoEngravidou += Time.deltaTime;
        }

        // Verificar se o animal é um filhote e se o tempo para virar adulto foi atingido
        if (idade == Idade.Filhote && tempoDeVida >= tempoParaVirarAdulto)
        {
            TransformarFilhoteEmAdulto();
        }
    }

    private void TransformarFilhoteEmAdulto()
    {
        idade = Idade.Adulto;
        transform.localScale *= 1.3f;
        AcrescentarMaisItensNoDrop();
    }

    public bool PodemProcriar(AnimalCriacao outroAnimal)
    {
        return tipo == outroAnimal.tipo && genero != outroAnimal.genero && idade == Idade.Adulto && outroAnimal.idade == Idade.Adulto;
    }

    private void VerificarProcriacaoPeriodicamente()
    {
        if (tempoEngravidou >= tempoParaEngravidarDenovo || tempoEngravidou == 0)
        {
            AnimalCriacao parceiro = EncontrarParceiroProcriacaoProximo();
            if (parceiro != null)
            {
                parceiroAtual = parceiro;
                verificacoes = 0;
                InvokeRepeating("VerificarParceiroProximo", 0, tempoParaEngravidarDenovo/2); // Verifica a cada 2 minutos
            }
        }
    }

    private AnimalCriacao EncontrarParceiroProcriacaoProximo()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, distanciaProcriacao, enemyLayerMask);
        foreach (var hitCollider in hitColliders)
        {
            // Verifica se o collider tem a tag "AnimalCollider"
            if (hitCollider.CompareTag("AnimalCollider"))
            {
                AnimalCriacao animal = hitCollider.GetComponentInParent<AnimalCriacao>();
                if (animal != null && animal != this && PodemProcriar(animal))
                {
                    return animal;
                }
            }
        }
        return null;
    }

    private void VerificarParceiroProximo()
    {
        if (parceiroAtual == null || Vector3.Distance(transform.position, parceiroAtual.transform.position) > distanciaProcriacao)
        {
            CancelInvoke("VerificarParceiroProximo");
            return; // Se o parceiro se distanciar, sai do método
        }

        verificacoes++;
        if (verificacoes >= 3)
        {
            // Se passar pelas 3 verificações, instancia o novo prefab e reinicia o tempo de engorda
            InstanciarNovoAnimal();
            tempoEngravidou = 1; // Reinicia o contador de tempo para engravidar
            CancelInvoke("VerificarParceiroProximo");
        }
    }

    private void InstanciarNovoAnimal()
    {
        int indexMachoOuFemea = Random.Range(0, pathsPrefabMachoFemea.Length);

        GameObject filhote = null;
        if (PhotonNetwork.IsConnected)
        {
            filhote = PhotonNetwork.Instantiate(pathsPrefabMachoFemea[indexMachoOuFemea], transform.position, Quaternion.identity, 0, new object[] { PV.ViewID });
        }
        else
        {
            GameObject prefab = Resources.Load<GameObject>(pathsPrefabMachoFemea[indexMachoOuFemea]);
            if (prefab != null)
            {
                filhote = Instantiate(prefab, transform.position, Quaternion.identity);
            }
        }
        if (filhote != null)
        {
            filhote.GetComponent<AnimalCriacao>().idade = Idade.Filhote;
        }
    }

    public void AcrescentarMaisItensNoDrop()
    {
        List<Item.ItemDropStruct> itemDrops = new List<Item.ItemDropStruct>();
        foreach (Item.ItemDropStruct itemDropScruct in statsGeral.dropsItems)
        {
            foreach(Item.ItemDropStruct itemExtra in itemsExtraAdultos)
            {
                if (itemDropScruct.itemDefinition.Equals(itemExtra.itemDefinition) && itemDropScruct.materialPersonalizado == itemExtra.materialPersonalizado)
                {
                    Item.ItemDropStruct novo = new Item.ItemDropStruct();
                    novo.itemDefinition = itemDropScruct.itemDefinition;
                    novo.tipoItem = itemDropScruct.tipoItem;
                    novo.materialPersonalizado = itemDropScruct.materialPersonalizado;
                    novo.qtdMinDrops = itemDropScruct.qtdMinDrops + itemExtra.qtdMinDrops;
                    novo.qtdMaxDrops = itemDropScruct.qtdMaxDrops + itemExtra.qtdMaxDrops;
                    itemDrops.Add(novo);
                }
                else
                {
                    itemDrops.Add(itemDropScruct);
                }
            }
        }
        statsGeral.dropsItems = itemDrops;
    }

}
