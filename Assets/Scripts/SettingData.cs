using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Setting In Game
/// </summary>
public class SettingData : MonoBehaviour
{
    public GameSetting gameSetting;

    #region Singleton
    public static SettingData Instance { get; private set; }
    #endregion

    private void Start()
    {
        if (Instance)
            Destroy(gameObject);
        else
        {
            gameSetting = DataManager.LoadSettings();
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnApplicationPause(bool pause)
    {
        if (!pause)
            return;

        DataManager.SaveSettings(gameSetting);
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus)
            return;

        DataManager.SaveSettings(gameSetting);
    }

    private void OnApplicationQuit()
    {
        DataManager.SaveSettings(gameSetting);
    }
}

[System.Serializable]
public class GameSetting
{
    // Setting
    [Range(0, 100)] public float ui_Sound;
    [Range(0, 100)] public float voice_Sound;
    [Range(0, 100)] public float step_Sound;
    [Range(0, 100)] public float song_Sound;
    [Range(0, 100)] public float sensitivity;
    [Range(40, 100)]public int viewRange;
    [Range(0, 2)]public int indexGraphic;
}
