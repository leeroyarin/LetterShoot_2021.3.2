using System;
using System.Threading.Tasks;
using UnityEngine;

public static class PlayerData
{
    // cached player's current level
    private static int s_playerCurrentLevel = 1;
    private static bool s_levelInitialized;

    // returns player's current level (synchronous). The level is updated asynchronously on first access.
    public static int PlayerCurrentLevel
    {
        get
        {
            if (!s_levelInitialized)
            {
                s_levelInitialized = true;
                _ = InitializePlayerLevelAsync();
            }

            return s_playerCurrentLevel;
        }
        set
        {
            NetworkManager.Instance.SetPlayerLevel(
            Mathf.Clamp(value, 1, 20),
            (success) => Debug.Log("Successfully Saved Status: " + success),
            (error) => Debug.LogError(error));
            s_playerCurrentLevel = Mathf.Max(1, value);
        }
    }

    private static async Task InitializePlayerLevelAsync()
    {
        try
        {
            var lvl = await GetPlayerLevelFromServer();
            s_playerCurrentLevel = Mathf.Max(1, lvl);
            Debug.Log("Player Current Level updated: " + s_playerCurrentLevel);
        }
        catch (Exception ex)
        {
            Debug.LogError("Error retrieving player level: " + ex.Message);
        }
    }

    async static private Task<int> GetPlayerLevelFromServer()
    {
        var tcs = new TaskCompletionSource<int>();

        try
        {
            NetworkManager.Instance.GetPlayerLevel(
                (lvl) =>
                {
                    tcs.TrySetResult(lvl);
                },
                (error) =>
                {
                    tcs.TrySetException(new Exception(error ?? "Unknown error"));
                });
        }
        catch (Exception ex)
        {
            tcs.TrySetException(ex);
        }

        return await tcs.Task;
    }

  
    //returns if the player has played before or not
    public static bool FirstGame { get => !PlayerPrefs.HasKey("PlayerHasPlayerBefore"); set => PlayerPrefs.SetInt("PlayerHasPlayerBefore", System.Convert.ToInt16(value)); }
}
