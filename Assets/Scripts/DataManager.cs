using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This is game not big, so  store PlayerPrefs
/// </summary>
public class DataManager : MonoBehaviour
{
    private const string PlayerKey = "PLAYER_DATA"; 
    private const string SettingKey = "SETTING_DATA";

    public static string playerKey => PlayerKey;
    public static string namePlayer;
    public static void SavePlayer(PlayerInfo data)
    {
        string json = JsonUtility.ToJson(data, true);
        PlayerPrefs.SetString(PlayerKey, json);
        PlayerPrefs.Save();
        Debug.Log("PlayerData Saved: " + json);
    }

    // Load
    public static PlayerInfo LoadPlayer()
    {
        if (PlayerPrefs.HasKey(PlayerKey))
        {
            string json = PlayerPrefs.GetString(PlayerKey);
            PlayerInfo data = JsonUtility.FromJson<PlayerInfo>(json);
            Debug.Log("PlayerData Loaded: " + json);
            return data;
        }
        Debug.Log("No PlayerData found. Returning default.");
        return new PlayerInfo
        {
            namePlayer = "Player",
            money = 100000000,
            items = new List<Item>(),
            privacyProps = new List<PrivacyProp>(),
        };
    }



    // Save SettingData
    public static void SaveSettings(GameSetting data)
    {
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(SettingKey, json);
        PlayerPrefs.Save();
        Debug.Log("GameSetting Saved: " + json);

    }

    // Load SettingData
    public static GameSetting LoadSettings()
    {
        if (PlayerPrefs.HasKey(SettingKey))
        {
            string json = PlayerPrefs.GetString(SettingKey);
            Debug.Log("GameSetting Loaded: " + json);
            return JsonUtility.FromJson<GameSetting>(json);
        }
        Debug.Log("No GameSetting found. Returning default.");
        return new GameSetting
        {
            ui_Sound = 100,
            voice_Sound = 100,
            step_Sound = 100,
            song_Sound = 100,
            sensitivity = 100,
            viewRange = 60,
            indexGraphic = 2,
        };
    }
}
