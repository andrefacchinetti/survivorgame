using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using Steamworks;
using TMPro;

public class LojaCashManager : MonoBehaviour
{
    private string catalogVersion = "Catalogo Domination";
    public TMP_Text txStatus, txPlayerCoins;
    private string atualOrderId;
    private ItemCash itemAtual;

    protected Callback<MicroTxnAuthorizationResponse_t> m_MicroTxnAuthorizationResponse;

    private void Start()
    {
        if (SteamManager.Initialized)
        {
            m_MicroTxnAuthorizationResponse = Callback<MicroTxnAuthorizationResponse_t>.Create(OnMicroTxnAuthorizationResponse);
            Debug.Log("SteamUserID: " + SteamUser.GetSteamID());
            Debug.Log("Language: " + SteamApps.GetCurrentGameLanguage());
        }
    }
    private void OnMicroTxnAuthorizationResponse(MicroTxnAuthorizationResponse_t pCallback)
    {
        if (pCallback.m_bAuthorized == 1)
        {
            txStatus.color = Color.blue;
            txStatus.text = "Authorized payment!";
            ConfirmPurchase(atualOrderId);
        }
        else
        {
            txStatus.color = Color.red;
            txStatus.text = "Failed to authorize payment!";
        }
    }

    public void addItemAoCarrinho(ItemCash item)
    {
        itemAtual = item;
        PlayFabClientAPI.StartPurchase(new StartPurchaseRequest()
        {
            CatalogVersion = catalogVersion,
            Items = new List<ItemPurchaseRequest>()
            {
                new ItemPurchaseRequest()
                {
                    ItemId = item.itemId,
                    Quantity = 1,
                    Annotation = item.Annotation
                }
            }
        }, result =>
        {
            Debug.Log("StartPurchase");
            txStatus.color = Color.blue;
            txStatus.text = "Waiting";
            confirmarCompraBasicChest(result.OrderId);
        }, error =>
        {
            txStatus.color = Color.red;
            txStatus.text = "Error to purchase item";
            Debug.Log("StartPurchase FAILED: " + error.ErrorMessage);
        });
    }
    private void confirmarCompraBasicChest(string orderId)
    {
        PlayFabClientAPI.PayForPurchase(new PayForPurchaseRequest()
        {
            OrderId = orderId,
            ProviderName = "Steam",
            Currency = "RM"
        }, result => {
            atualOrderId = orderId;
            txStatus.color = Color.blue;
            txStatus.text = "Purchase order issued successfully";
            Debug.Log("Pedido de compra confirmada");
        }, error => {
            txStatus.color = Color.red;
            txStatus.text = "Failed to issue purchase order";
            Debug.LogError("Erro PayForPurchase: " + error.ErrorMessage);
        });
    }

    private void ConfirmPurchase(string orderId)
    {
        PlayFabClientAPI.ConfirmPurchase(new ConfirmPurchaseRequest()
        {
            OrderId = orderId
        }, result =>
        {
            Debug.Log("CONFIRMED PURCHASE");
            txStatus.color = Color.green;
            txStatus.text = "Thank you, Reward obtained successfully."; //Só atualiza os Coins qdo reloga
            if (itemAtual.itemId == "BasicChest") PlayerPrefs.SetInt("GEMS", PlayerPrefs.GetInt("GEMS") + 100);
            else if (itemAtual.itemId == "MediumChest") PlayerPrefs.SetInt("GEMS", PlayerPrefs.GetInt("GEMS") + 200);
            else if (itemAtual.itemId == "HighChest") PlayerPrefs.SetInt("GEMS", PlayerPrefs.GetInt("GEMS") + 300);
        }, error =>
        {
            txStatus.color = Color.red;
            txStatus.text = "Error sending reward";
            Debug.LogError("Erro ConfirmPurchase: " + error.ErrorMessage);
        });
    }

}
