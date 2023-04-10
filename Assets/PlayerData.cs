using UnityEngine;

public static class PlayerData
{

    public static int PlayerCurrentLevel { get => PlayerPrefs.GetInt("PlayerCurrentLevel",1); set => PlayerPrefs.SetInt("PlayerCurrentLevel", Mathf.Clamp(value,1,20)); }

    public static bool FirstGame { get=> !PlayerPrefs.HasKey("PlayerHasPlayerBefore"); set=> PlayerPrefs.SetInt("PlayerHasPlayerBefore",System.Convert.ToInt16(value)); }
}