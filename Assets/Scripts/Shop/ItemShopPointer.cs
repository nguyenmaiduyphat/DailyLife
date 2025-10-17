using System.Collections;
using System.Globalization;
using System.Linq;
using TMPro;
using UnityEngine;

public class ItemShopPointer : MonoBehaviour
{
    public ItemInfo itemInfo;

    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI priceText;

    [HideInInspector] public PlayerData playerData;
    [HideInInspector] public InventorySystem inventorySystem;

    private void Start()
    {
        nameText.text = itemInfo.namePrefab;
        priceText.text = ConvertToMoneyFormat(itemInfo.price);

        if (itemInfo.price >= 1000 && itemInfo.price <= 1000000)
        {
            priceText.text = ConvertToMoneyFormat(itemInfo.price / 1000) + " K";
        }
        if(itemInfo.price > 1000000 && itemInfo.price <= 1000000000)
        {
            priceText.text = ConvertToMoneyFormat(itemInfo.price / 1000000) + " M";
        }
        if(itemInfo.price > 1000000000)
        {
            priceText.text = ConvertToMoneyFormat(itemInfo.price / 1000000000) + " B";
        }

        priceText.text += " AZO";

    }


    public GameObject itemPrefab;
    [SerializeField] string folderSprite = "ShopItem";
    public void Buy()
    {
        bool buyStatus = false;


        for (int i = 0; i < inventorySystem.slots.Count(); i++)
        {
            if (inventorySystem.slots[i].transform.childCount == 0)
            {
                GameObject inventoryItem = Instantiate(itemPrefab, inventorySystem.slots[i].transform);

                inventoryItem.GetComponent<DraggableItem>().image.sprite = itemInfo.imagePrefab;
                inventoryItem.GetComponent<DraggableItem>().itemInfo = itemInfo;
                buyStatus = true;
                break;
            }
        }
        if (buyStatus)
        {
            playerData.playerInfo.money -= itemInfo.price;
            playerData.UpdateMoney();
            notificationText.text = "You bought " + itemInfo.namePrefab + " for " + priceText.text;
            StartCoroutine(ShowAndHide());
        }
        else
        {
            notificationText.text = "Inventory Full!";
            StartCoroutine(ShowAndHide());
        }
    }

    string ConvertToMoneyFormat(int value)
    {
        return value.ToString("N0", CultureInfo.InvariantCulture);
    }

    public TextMeshProUGUI notificationText;
    public RectTransform notification;
    [SerializeField] private float showTime = 3f;
    [SerializeField] float duration = 0.5f;

    private IEnumerator ShowAndHide()
    {
        StartCoroutine(UIAnimation.ZoomOut(notification, duration));

        yield return new WaitForSeconds(showTime);

        StartCoroutine(UIAnimation.ZoomIn(notification, duration));
    }
}
