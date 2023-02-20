using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine.UI;

public class PlayFabLeaderboard : MonoBehaviour
{
    [SerializeField] Transform leaderboardListContentTrofeusGlobal, leaderboardListContentTrofeusFriends, leaderboardListContentTimeMapsGlobal, leaderboardListContentTimeMapsFriends, leaderboardListContentBunnyhopGlobal, leaderboardListContentBunnyhopFriends;
    [SerializeField] GameObject PlayerLeaderboardItemPrefab;
    [HideInInspector] int maxScore = 0;

    private void Start()
    {
        atualizarLeaderboard();
    }

    public void atualizarLeaderboard()
    {
        GetLeaderboardTrofeuPlayer();

        GetLeaderboardTrofeuScore();
        GetFriendsLeaderboardTrofeu();
    }

    public void SendLeaderBoardStarScore(int score)
    {
        Debug.Log("enviando leaderboard de " + score + " StarScore");
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "StarScore",
                    Value = score
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderBoardUpdate, OnError);
    }

    public void SendLeaderBoardTrofeuScore(int score)
    {
        Debug.Log("enviando leaderboard de "+score+" trofeus");
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "TrofeuScore",
                    Value = score
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderBoardUpdate, OnError);
    }

    void OnLeaderBoardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Sucess update leaderboard sent");
        atualizarLeaderboard();
    }
    void OnError(PlayFabError error)
    {
        Debug.LogError(error.ErrorMessage);
    }

    public void GetLeaderboardTrofeuScore()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "TrofeuScore",
            StartPosition = 0,
            MaxResultsCount = 100
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderBoardGetTrofeu, OnError);
    }

    void OnLeaderBoardGetTrofeu(GetLeaderboardResult result)
    {
        if (leaderboardListContentTrofeusGlobal == null) return;
        foreach (Transform child in leaderboardListContentTrofeusGlobal)
        {
            Destroy(child.gameObject);
        }
        int position = 1;
        foreach (var item in result.Leaderboard)
        {
            Instantiate(PlayerLeaderboardItemPrefab, leaderboardListContentTrofeusGlobal).GetComponent<PlayerLeaderboardItem>().SetUp(item.Profile.DisplayName, item.StatValue + "", position, "trofeu");
            position++;
        }
    }

    public void GetLeaderboardTrofeuPlayer()
    {
        var request = new GetPlayerStatisticsRequest
        {
            StatisticNames = new List<string>() { "TrofeuScore" }
        };
        PlayFabClientAPI.GetPlayerStatistics(request, OnLeaderboardPlayerTrofeu, OnError);
    }

    void OnLeaderboardPlayerTrofeu(GetPlayerStatisticsResult result) //Pega o placar dos players
    {
        if (result.Statistics != null && result.Statistics.Count > 0)
        {
            maxScore = result.Statistics[0].Value;
        }
        else
        {
            maxScore = 0;
        }
        if(maxScore < 0)
        {
            SendLeaderBoardTrofeuScore(maxScore * -1);
            maxScore = maxScore * -1;
        }
        PlayerPrefs.SetInt("TROFEUS", maxScore);
        Debug.Log("Sucessleaderboard trofeu player: " + maxScore);
    }

    public void GetFriendsLeaderboardTrofeu()
    {
        var request = new GetFriendLeaderboardAroundPlayerRequest
        {
            StatisticName = "TrofeuScore",
            IncludeSteamFriends = true
        };
        PlayFabClientAPI.GetFriendLeaderboardAroundPlayer(request, OnFriendsLeaderboardTrofeu, OnError);
    }

    void OnFriendsLeaderboardTrofeu(GetFriendLeaderboardAroundPlayerResult result)
    {
        if (leaderboardListContentTrofeusFriends == null) return;
        foreach (Transform child in leaderboardListContentTrofeusFriends)
        {
            Destroy(child.gameObject);
        }
        int position = 1;
        foreach (var item in result.Leaderboard)
        {
            Instantiate(PlayerLeaderboardItemPrefab, leaderboardListContentTrofeusFriends).GetComponent<PlayerLeaderboardItem>().SetUp(item.Profile.DisplayName, item.StatValue + "", position, "trofeu");
            position++;
        }
    }

    public void GiveGems(int value)
    {
        AddUserVirtualCurrencyRequest request = new AddUserVirtualCurrencyRequest
        {
            Amount = value,
            VirtualCurrency = "GE"
        };
        PlayerPrefs.SetInt("GEMS", PlayerPrefs.GetInt("GEMS") + value);
        PlayFabClientAPI.AddUserVirtualCurrency(request, OnGiveGemsComplete, OnError);
    }

    private void OnGiveGemsComplete(ModifyUserVirtualCurrencyResult result) 
    {
        Debug.Log("Recebeu 1 gema com sucesso");
    }

}
