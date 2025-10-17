using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


/// <summary>
/// This class have implement interface to get all method need to set up
/// I rebuid this class to use Facade Pattern  to handle actions that player
/// do. (Updating)
/// </summary>
public class PlayerInteraction : MonoBehaviour, ISetting
{

    [SerializeField] TextMeshProUGUI deadLineMissionText;
    [SerializeField] TextMeshProUGUI notificationText;
    [SerializeField] RectTransform notification;
    [SerializeField] RectTransform quest;
    public RectTransform shop;
    [SerializeField] RectTransform inventory;

    private void Start()
    {
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

    bool openMenu, openInventory, openQuest;
    public bool openShop;
    private void Update()
    {
        audioSource.volume = SettingData.Instance.gameSetting.ui_Sound / 100;
        playerCamera.hasOpen = openMenu || openInventory || openQuest || openShop;

        if (Input.GetKeyDown(KeyCode.Escape) && !playerCamera.hasOpen)
        {
            openMenu = true;
            StartCoroutine(UIAnimation.ZoomOut(pause, duration));
        }

        if (Input.GetKeyDown(KeyCode.B) && (!playerCamera.hasOpen || (playerCamera.hasOpen &&  openInventory)))
        {
            if (openInventory)
            {
                openInventory = false;
                StartCoroutine(UIAnimation.ZoomIn(inventory, duration));
            }
            else
            {
                openInventory = true;
                StartCoroutine(UIAnimation.ZoomOut(inventory, duration));
            }
        }

        if (Input.GetKeyDown(KeyCode.E) && !playerCamera.hasOpen)
        {
            Interact();
        }
    }

    #region Interact
    public LayerMask npcMask;
    [SerializeField] PlayerCamera playerCamera;
    [SerializeField] float rangeInteract = 60f;

    GameObject npc;
    void Interact()
    {
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit hit, rangeInteract, npcMask))
        {
            npc = hit.transform.gameObject;
            switch (hit.transform.gameObject.tag)
            {
                case "Quest":
                    if (openQuest)
                        return;
                    npc.GetComponent<NPCController>().audioSource.PlayOneShot(npc.GetComponent<NPCController>().greetingClip);
                    LoadQuestion();
                    openQuest = true;
                    StartCoroutine(UIAnimation.ZoomOut(quest, duration));
                    break;
                case "Shop":
                    if (openShop)
                        return;

                    openShop = true;
                    StartCoroutine(UIAnimation.ZoomOut(shop, duration));
                    break;
            }
        }
    }
    #endregion


    #region Quest

    [SerializeField] TextMeshProUGUI question;
    [SerializeField] TextMeshProUGUI answer1_Text;
    [SerializeField] TextMeshProUGUI answer2_Text;
    [SerializeField] TextMeshProUGUI answer3_Text;

    Question currentQuestion;
    void LoadQuestion()
    {
        currentQuestion = npc.GetComponent<NPCController>().questions[Random.Range(0, npc.GetComponent<NPCController>().questions.Count - 1)];
   
        question.text = currentQuestion.questionText;
        answer1_Text.text = currentQuestion.answers[0];
        answer2_Text.text = currentQuestion.answers[1];
        answer3_Text.text = currentQuestion.answers[2];
    }

    public void Answer(int index)
    {
        if (currentQuestion.indexCorrectAnswer == index)
        {
            GetComponent<PlayerData>().playerInfo.money += 100;
            npc.GetComponent<NPCController>().audioSource.PlayOneShot(npc.GetComponent<NPCController>().congratulationClip);

            notificationText.text = "Congratulation!";
        }
        else
        {
            GetComponent<PlayerData>().playerInfo.money -= 200;
            if (GetComponent<PlayerData>().playerInfo.money < 0)
                GetComponent<PlayerData>().playerInfo.money = 0;

            npc.GetComponent<NPCController>().audioSource.PlayOneShot(npc.GetComponent<NPCController>().wishGoodLuckClip);
            notificationText.text = "You answered wrong, so sad!";
        }

        GetComponent<PlayerData>().UpdateMoney();
        StartCoroutine(ShowAndHide());

        openQuest = false;
        StartCoroutine(UIAnimation.ZoomIn(quest, duration));
    }
    [SerializeField] private float showTime = 3f;

    private IEnumerator ShowAndHide()
    {
        StartCoroutine(UIAnimation.ZoomOut(notification, duration));

        yield return new WaitForSeconds(showTime); // chờ vài giây

        StartCoroutine(UIAnimation.ZoomIn(notification, duration));
    }
    #endregion


    #region Setting
    /// <summary>
    /// Setting Part
    /// </summary>
    [SerializeField] RectTransform pause; 
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
    public void Resume()
    {
        if (setting.localScale == Vector3.one)
            return;
        audioSource.PlayOneShot(openUIMenuClip);
        openMenu = false;
        StartCoroutine(UIAnimation.ZoomIn(pause, duration));
    }

    bool hasOpenSetting;
    public void Setting()
    {
        if(hasOpenSetting)
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
        audioSource.PlayOneShot(closeUIMenuClip);
        openMenu = false;
        StartCoroutine(UIAnimation.ZoomIn(pause, duration));
        GetComponent<PlayerData>().SaveBeforeEnd();
        SceneManager.LoadScene(0);
    }

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
