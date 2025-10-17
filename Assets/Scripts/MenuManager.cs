using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour, ISetting
{
    private IEnumerator Start()
    {
        yield return new WaitUntil(() => SettingData.Instance != null);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        // Set Setting Layout
        lowToggle.isOn = SettingData.Instance.gameSetting.indexGraphic == 0;
        mediumToggle.isOn = SettingData.Instance.gameSetting.indexGraphic == 1;
        highToggle.isOn = SettingData.Instance.gameSetting.indexGraphic == 2;
        lowToggle.enabled = true;
        mediumToggle.enabled = true;
        highToggle.enabled = true;

        sensitivitySlider.value = SettingData.Instance.gameSetting.sensitivity;
        songSoundSlider.value = SettingData.Instance.gameSetting.song_Sound;
        stepSoundSlider.value = SettingData.Instance.gameSetting.step_Sound;
        uiSoundSlider.value = SettingData.Instance.gameSetting.ui_Sound;
        viewRangeSlider.value = SettingData.Instance.gameSetting.viewRange;
        voiceSoundSlider.value = SettingData.Instance.gameSetting.voice_Sound;

        sensitivity_Percent.text = sensitivitySlider.value.ToString();
        songSound_Percent.text = songSoundSlider.value.ToString() + "%";
        stepSound_Percent.text = stepSoundSlider.value.ToString() + "%";
        uiSound_Percent.text = uiSoundSlider.value.ToString() + "%";
        viewRange_Percent.text = viewRangeSlider.value.ToString();
        voiceSound_Percent.text = voiceSoundSlider.value.ToString() + "%";

        volumeIcon_VoiceSound.sprite = voiceSoundSlider.value == 0 ? muteSoundSprite : unmuteSoundSprite;
        volumeIcon_UISound.sprite = uiSoundSlider.value == 0 ? muteSoundSprite : unmuteSoundSprite;
        volumeIcon_StepSound.sprite = stepSoundSlider.value == 0 ? muteSoundSprite : unmuteSoundSprite;
        volumeIcon_SongSound.sprite = songSoundSlider.value == 0 ? muteSoundSprite : unmuteSoundSprite;
    }

    private void Update()
    {
        audioSource.volume = SettingData.Instance.gameSetting.ui_Sound / 100;

        playBtn.interactable = !string.IsNullOrEmpty(inputField.text);
        if (string.IsNullOrEmpty(inputField.text))
        {
            playBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Fill your name Player";
            playBtn.GetComponentInChildren<TextMeshProUGUI>().color = Color.yellow;

        }
        else
        {
            playBtn.GetComponentInChildren<TextMeshProUGUI>().text = inputField.text.Length < 10 ? "Fill larger 9 keywords pls?" : "Start Game";
            playBtn.GetComponentInChildren<TextMeshProUGUI>().color = inputField.text.Length < 10 ? Color.red : Color.white;
        }


    }
    [SerializeField] Button playBtn;
    [SerializeField] TMP_InputField inputField;

    public void Play()
    {
        if (setting.localScale == Vector3.one || string.IsNullOrEmpty(inputField.text)  || inputField.text.Length < 10)
            return;

        DataManager.namePlayer = inputField.text;
        SceneManager.LoadScene(1);
    }

    bool hasOpenSetting;
    public void Setting()
    {
        if (hasOpenSetting)
        {
            hasOpenSetting = false;
            audioSource.PlayOneShot(closeUIMenuClip);
            StartCoroutine(UIAnimation.ZoomIn(setting, duration));
        }
        else
        {
            hasOpenSetting = true;
            audioSource.PlayOneShot(openUIMenuClip);
            StartCoroutine(UIAnimation.ZoomOut(setting, duration));
        }

    }
    public void Quit()
    {
        if (setting.localScale == Vector3.one)
            return;
        DataManager.SaveSettings(SettingData.Instance.gameSetting);
        Application.Quit();
    }


    #region Setting
    /// <summary>
    /// Setting Part
    /// </summary>
    [SerializeField] RectTransform setting;
    [SerializeField] float duration = 0.5f;

    [SerializeField] Slider uiSoundSlider;
    [SerializeField] Slider voiceSoundSlider;
    [SerializeField] Slider stepSoundSlider;
    [SerializeField] Slider songSoundSlider;
    [SerializeField] Slider sensitivitySlider;
    [SerializeField] Slider viewRangeSlider;

    [SerializeField] TextMeshProUGUI uiSound_Percent;
    [SerializeField] TextMeshProUGUI voiceSound_Percent;
    [SerializeField] TextMeshProUGUI stepSound_Percent;
    [SerializeField] TextMeshProUGUI songSound_Percent;
    [SerializeField] TextMeshProUGUI sensitivity_Percent;
    [SerializeField] TextMeshProUGUI viewRange_Percent;

    [SerializeField] ToggleGroup graphicToggleGroup;
    [SerializeField] Toggle lowToggle;
    [SerializeField] Toggle mediumToggle;
    [SerializeField] Toggle highToggle;


    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip openUIMenuClip;
    [SerializeField] AudioClip closeUIMenuClip;
    [SerializeField] AudioClip clickClip;

    [SerializeField] Sprite muteSoundSprite;
    [SerializeField] Sprite unmuteSoundSprite;

    [SerializeField] Image volumeIcon_UISound;
    [SerializeField] Image volumeIcon_VoiceSound;
    [SerializeField] Image volumeIcon_StepSound;
    [SerializeField] Image volumeIcon_SongSound;

    public void SetGraphic_Low()
    {
        audioSource.PlayOneShot(clickClip);
        SettingData.Instance.gameSetting.indexGraphic = 0;
        QualitySettings.SetQualityLevel(0);
    }

    public void SetGraphic_Meidum()
    {
        audioSource.PlayOneShot(clickClip);
        SettingData.Instance.gameSetting.indexGraphic = 1;
        QualitySettings.SetQualityLevel(1);
    }

    public void SetGraphic_High()
    {
        audioSource.PlayOneShot(clickClip);
        SettingData.Instance.gameSetting.indexGraphic = 2;
        QualitySettings.SetQualityLevel(2);
    }

    public void SetSensitivity()
    {
        audioSource.PlayOneShot(clickClip);
        SettingData.Instance.gameSetting.sensitivity = sensitivitySlider.value;
        sensitivity_Percent.text = sensitivitySlider.value.ToString();
    }

    public void SetSongSound()
    {
        audioSource.PlayOneShot(clickClip);
        SettingData.Instance.gameSetting.song_Sound = songSoundSlider.value;
        songSound_Percent.text = songSoundSlider.value.ToString() + "%";
        volumeIcon_SongSound.sprite = songSoundSlider.value == 0 ? muteSoundSprite : unmuteSoundSprite;
    }

    public void SetStepSound()
    {
        audioSource.PlayOneShot(clickClip);
        SettingData.Instance.gameSetting.step_Sound = stepSoundSlider.value;
        stepSound_Percent.text = stepSoundSlider.value.ToString() + "%";
        volumeIcon_StepSound.sprite = stepSoundSlider.value == 0 ? muteSoundSprite : unmuteSoundSprite;
    }


    public void SetUISound()
    {
        audioSource.PlayOneShot(clickClip);
        SettingData.Instance.gameSetting.ui_Sound = uiSoundSlider.value;
        uiSound_Percent.text = uiSoundSlider.value.ToString() + "%";
        volumeIcon_UISound.sprite = uiSoundSlider.value == 0 ? muteSoundSprite : unmuteSoundSprite;
    }

    public void SetViewRange()
    {
        audioSource.PlayOneShot(clickClip);
        SettingData.Instance.gameSetting.viewRange = (int)viewRangeSlider.value;
        viewRange_Percent.text = viewRangeSlider.value.ToString();
    }

    public void SetVoiceSound()
    {
        audioSource.PlayOneShot(clickClip);
        SettingData.Instance.gameSetting.voice_Sound = voiceSoundSlider.value;
        voiceSound_Percent.text = voiceSoundSlider.value.ToString() + "%";
        volumeIcon_VoiceSound.sprite = voiceSoundSlider.value == 0 ? muteSoundSprite : unmuteSoundSprite;
    }
    #endregion
}
