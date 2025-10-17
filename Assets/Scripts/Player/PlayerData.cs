using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TMPro;
using UnityEngine;

/// <summary>
/// Player Data In Game
/// </summary>
public class PlayerData : MonoBehaviour
{
    public PlayerInfo playerInfo;
    [SerializeField] InventorySystem inventorySystem;

    [SerializeField] TextMeshProUGUI nameUserText;
    [SerializeField] TextMeshProUGUI moneyText;


    private void Start()
    {
        playerInfo.namePlayer = DataManager.namePlayer;
        nameUserText.text = playerInfo.namePlayer;
        UpdateMoney();
    }

    private void Update()
    {
        if (transform.position.y < -100 && !GetComponent<PlayerMovement>().hasJump && !GetComponent<CharacterController>().isGrounded)
        {
            transform.position = FindAnyObjectByType<SpawnPlayer>().gameObject.transform.position;       
        }

        if(Input.GetKeyDown(KeyCode.P))
        {
            transform.position = FindAnyObjectByType<SpawnPlayer>().gameObject.transform.position;
        }



    }

    public void UpdateMoney()
    {
        moneyText.text = ConvertToMoneyFormat(playerInfo.money);

        if (playerInfo.money >= 1000 && playerInfo.money <= 1000000)
        {
            moneyText.text = ConvertToMoneyFormat(playerInfo.money / 1000) + " K";
        }
        if (playerInfo.money > 1000000 && playerInfo.money <= 1000000000)
        {
            moneyText.text = ConvertToMoneyFormat(playerInfo.money / 1000000) + " M";
        }
        if (playerInfo.money > 1000000000)
        {
            moneyText.text = ConvertToMoneyFormat(playerInfo.money / 1000000000) + " B";
        }

        moneyText.text += " AZO";
    }

    string ConvertToMoneyFormat(int value)
    {
        return value.ToString("N0", CultureInfo.InvariantCulture);
    }

    public void SaveBeforeEnd()
    {
        playerInfo.positionX = transform.position.x;
        playerInfo.positionY = transform.position.y;
        playerInfo.positionZ = transform.position.z;

        playerInfo.rotationX = transform.rotation.x;
        playerInfo.rotationY = transform.rotation.y;
        playerInfo.rotationZ = transform.rotation.z;

        inventorySystem.CheckItemsLeft();
        PrivacyObject[] privacies = FindObjectsByType<PrivacyObject>(FindObjectsSortMode.None);

        var matches = from p in privacies
                      where p.nameOwner == playerInfo.namePlayer
                      select p;

        foreach (PrivacyObject privacy in matches)
        {
            playerInfo.privacyProps.Add(new PrivacyProp {
                nameObj = privacy.nameObj,
                positionX = privacy.transform.position.x,
                positionY = privacy.transform.position.y,
                positionZ = privacy.transform.position.z,
                rotationX = privacy.transform.rotation.x,
                rotationY = privacy.transform.rotation.y,
                rotationZ = privacy.transform.rotation.z,
            });
        }

        DataManager.SavePlayer(playerInfo);
    }


    private void OnApplicationQuit()
    {
        SaveBeforeEnd();    
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus)
            return;

        SaveBeforeEnd();

    }

    private void OnApplicationPause(bool pause)
    {
        if (!pause)
            return;
        SaveBeforeEnd();

    }
}

[System.Serializable]
public class PlayerInfo
{
    // Player
    public string namePlayer;
    public int money;
    public float positionX;
    public float positionY;
    public float positionZ;
    public float rotationX;
    public float rotationY;
    public float rotationZ;
    public List<Item> items;
    public List<PrivacyProp> privacyProps;
}