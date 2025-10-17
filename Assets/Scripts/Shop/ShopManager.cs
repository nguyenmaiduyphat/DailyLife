using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



/// <summary>
/// Shop Manager: Buy, Sell
/// </summary>
public class ShopManager : MonoBehaviour, IShop
{
    [SerializeField] Transform root;
    [SerializeField] GameObject shopItemPrefab;

    [SerializeField] Transform productListLocate;
    [SerializeField] List<ItemShop> itemShops;
    List<ItemShop> itemShopsDisplay;

    [SerializeField] PlayerData playerData;
    [SerializeField] InventorySystem inventorySystem;

    public TextMeshProUGUI notificationText;
    public RectTransform notification;

    [SerializeField] string folderShop = "Shop";
    List<GameObject> itemDisplayList;
    private void Start()
    {
        propType = "";
        categoryIndex = 0;
        price = "";
        sortIndex = 0;
        searchName = "";

        itemDisplayList = new List<GameObject>();
        itemShops = Resources.LoadAll<ItemShop>(folderShop).ToList();

        CreateItemShop(itemShops);
        itemShopsDisplay = itemShops;
    }

    void CreateItemShop(List<ItemShop> list)
    {
        if (productListLocate.transform.childCount > 0)
        {
            foreach(GameObject itemDisplay in itemDisplayList)
            {
                Destroy(itemDisplay);
            }
            itemDisplayList.Clear();
            itemDisplayList = new List<GameObject>();
        }
        for (int i = 0; i < list.Count(); i++)
        {
            GameObject shopItem = Instantiate(shopItemPrefab, productListLocate.transform);
            shopItem.GetComponent<Image>().sprite = list[i].imagePrefab;
            shopItem.GetComponent<ItemShopPointer>().itemInfo = new ItemInfo(list[i].namePrefab, list[i].price, list[i].isConsume, list[i].propType, list[i].imagePrefab, list[i].category);
            shopItem.GetComponent<ItemShopPointer>().playerData = playerData;
            shopItem.GetComponent<ItemShopPointer>().inventorySystem = inventorySystem;
            shopItem.GetComponent<ItemShopPointer>().notificationText = notificationText;
            shopItem.GetComponent<ItemShopPointer>().notification = notification;
            itemDisplayList.Add(shopItem);
        }
    }

    void SetFilterChoice()
    {
        itemShopsDisplay = itemShops;
        if (!string.IsNullOrEmpty(propType))
        {
            itemShopsDisplay = itemShopsDisplay.Where(x => x.propType.ToString() == propType).ToList();
        }

        if (!string.IsNullOrEmpty(price))
        {
            if(price.Contains("-"))
            {
                string[] priceRange = price.Split(" - ");
                priceRange[0] = priceRange[0].Replace(",", "");
                priceRange[1] = priceRange[1].Replace(",", "");

                int minPrice = int.Parse(priceRange[0]);
                int maxPrice = int.Parse(priceRange[1]);
                itemShopsDisplay = itemShopsDisplay.Where(x => x.price >= minPrice && x.price <= maxPrice).ToList();
            }
            else
            {                                                                                         
                if (price.Contains("<="))
                {
                    int max = int.Parse(price.Split(" ")[1].Replace(",", ""));
                    itemShopsDisplay = itemShopsDisplay.Where(x => x.price <= max).ToList();
                }
                else if (price.Contains(">="))
                {
                    int min = int.Parse(price.Split(" ")[1].Replace(",", ""));
                    itemShopsDisplay = itemShopsDisplay.Where(x => x.price >= min).ToList();
                }
            }

        }

        if(categoryIndex != 0)
        {
            itemShopsDisplay = itemShopsDisplay.Where(x => x.category == categoryDropdown.options[categoryIndex].text).ToList();
        }

        if(sortIndex != 0)
        {
            switch(sortIndex)
            {
                case 1:
                    itemShopsDisplay = itemShopsDisplay
                    .OrderBy(x => x.namePrefab)
                    .ToList();
                    break;
                case 2:
                    itemShopsDisplay = itemShopsDisplay
                    .OrderByDescending(x => x.namePrefab)
                    .ToList();
                    break;
                case 3:
                    itemShopsDisplay = itemShopsDisplay
                    .OrderBy(x => x.price)
                    .ToList();
                    break;
                case 4:
                    itemShopsDisplay = itemShopsDisplay
                    .OrderByDescending(x => x.price)
                    .ToList();
                    break;
            }
        }

        if(!string.IsNullOrEmpty(searchName))
        {
            itemShopsDisplay = itemShopsDisplay.Where(x => x.namePrefab.ToLower().Trim().Contains(searchName.ToLower().Trim())).ToList();
        }

        CreateItemShop(itemShopsDisplay);
    }

    void ResetFilterChoice()
    {
        propType = "";
        categoryIndex = 0;
        price = "";
        sortIndex = 0;
        itemShopsDisplay = itemShops;
        CreateItemShop(itemShopsDisplay);
    }
    void ReloadShop()
    {
        propType = "";
        categoryIndex = 0;
        price = "";
        sortIndex = 0;
        searchName = "";
        itemShopsDisplay = itemShops;
        CreateItemShop(itemShopsDisplay);
    }

    [SerializeField] PlayerInteraction playerInteraction;
    [SerializeField] private float showTime = 3f;
    [SerializeField] float duration = 0.5f;
    public void Exit()
    {
        playerInteraction.openShop = false;
        StartCoroutine(UIAnimation.ZoomIn(playerInteraction.shop, duration));
    }

    [SerializeField] TMP_Dropdown categoryDropdown;
    [SerializeField] TMP_Dropdown sortDropdown;
    [SerializeField] TMP_InputField searchNameProduct_Field;

    [SerializeField] Toggle[] priceToggles;

    public void Reload()
    {
        searchNameProduct_Field.text = "";

        if (categoryDropdown.options.Count > 1)
        {
            categoryDropdown.options.RemoveRange(1, categoryDropdown.options.Count - 1);
        }


        if (homeToggle.GetChild(0).gameObject.activeInHierarchy)
        {
            homeToggle.GetChild(2).GetComponent<Animator>().Play("ToggleOff"); // animtion off
            homeToggle.GetChild(0).gameObject.SetActive(false);  // On
            homeToggle.GetChild(1).gameObject.SetActive(true);   // Off
        }

        if (foodToggle.GetChild(0).gameObject.activeInHierarchy)
        {
            foodToggle.GetChild(2).GetComponent<Animator>().Play("ToggleOff"); // animtion off
            foodToggle.GetChild(0).gameObject.SetActive(false);  // On
            foodToggle.GetChild(1).gameObject.SetActive(true);   // Off
        }

        if (drinkToggle.GetChild(0).gameObject.activeInHierarchy)
        {
            drinkToggle.GetChild(2).GetComponent<Animator>().Play("ToggleOff"); // animtion off
            drinkToggle.GetChild(0).gameObject.SetActive(false);  // On
            drinkToggle.GetChild(1).gameObject.SetActive(true);   // Off
        }

        if (toolToggle.GetChild(0).gameObject.activeInHierarchy)
        {
            toolToggle.GetChild(2).GetComponent<Animator>().Play("ToggleOff"); // animtion off
            toolToggle.GetChild(0).gameObject.SetActive(false);  // On
            toolToggle.GetChild(1).gameObject.SetActive(true);   // Off
        }

        foreach (var toggle in priceToggles)
        {
            toggle.isOn = false;
        }

        sortDropdown.value = 0;

        ReloadShop();
    }

    public void ResetFilter()
    {
        if (string.IsNullOrEmpty(propType) && categoryIndex == 0 && string.IsNullOrEmpty(price) && sortIndex == 0)
            return;

        if (categoryDropdown.options.Count > 1)
        {
            categoryDropdown.options.RemoveRange(1, categoryDropdown.options.Count - 1);
        }


        if (homeToggle.GetChild(0).gameObject.activeInHierarchy)
        {
            homeToggle.GetChild(2).GetComponent<Animator>().Play("ToggleOff"); // animtion off
            homeToggle.GetChild(0).gameObject.SetActive(false);  // On
            homeToggle.GetChild(1).gameObject.SetActive(true);   // Off
        }

        if (foodToggle.GetChild(0).gameObject.activeInHierarchy)
        {
            foodToggle.GetChild(2).GetComponent<Animator>().Play("ToggleOff"); // animtion off
            foodToggle.GetChild(0).gameObject.SetActive(false);  // On
            foodToggle.GetChild(1).gameObject.SetActive(true);   // Off
        }

        if (drinkToggle.GetChild(0).gameObject.activeInHierarchy)
        {
            drinkToggle.GetChild(2).GetComponent<Animator>().Play("ToggleOff"); // animtion off
            drinkToggle.GetChild(0).gameObject.SetActive(false);  // On
            drinkToggle.GetChild(1).gameObject.SetActive(true);   // Off
        }

        if (toolToggle.GetChild(0).gameObject.activeInHierarchy)
        {
            toolToggle.GetChild(2).GetComponent<Animator>().Play("ToggleOff"); // animtion off
            toolToggle.GetChild(0).gameObject.SetActive(false);  // On
            toolToggle.GetChild(1).gameObject.SetActive(true);   // Off
        }

        foreach (var toggle in priceToggles)
        {
            toggle.isOn = false;
        }

        sortDropdown.value = 0;

        ResetFilterChoice();
    }

    string propType;
    int categoryIndex;
    string price;
    int sortIndex;
    string searchName;

    [SerializeField] Transform homeToggle;
    [SerializeField] Transform foodToggle;
    [SerializeField] Transform drinkToggle;
    [SerializeField] Transform toolToggle;

    public void ResetType()
    {
        if (categoryDropdown.options.Count > 1)
        {
            categoryDropdown.options.RemoveRange(1, categoryDropdown.options.Count - 1);
        }


        propType = "";
        SetFilterChoice();
    }


    public void FilterType(string value)
    {
        if(categoryDropdown.options.Count > 1)
        {
            categoryDropdown.options.RemoveRange(1, categoryDropdown.options.Count - 1);
        }

        switch (value)
        {
            case "Home":
                if (foodToggle.GetChild(0).gameObject.activeInHierarchy)
                {
                    foodToggle.GetChild(2).GetComponent<Animator>().Play("ToggleOff"); // animtion off
                    foodToggle.GetChild(0).gameObject.SetActive(false);  // On
                    foodToggle.GetChild(1).gameObject.SetActive(true);   // Off
                }
                if (drinkToggle.GetChild(0).gameObject.activeInHierarchy)
                {
                    drinkToggle.GetChild(2).GetComponent<Animator>().Play("ToggleOff"); // animtion off
                    drinkToggle.GetChild(0).gameObject.SetActive(false);  // On
                    drinkToggle.GetChild(1).gameObject.SetActive(true);   // Off
                }
                if (toolToggle.GetChild(0).gameObject.activeInHierarchy)
                {
                    toolToggle.GetChild(2).GetComponent<Animator>().Play("ToggleOff"); // animtion off
                    toolToggle.GetChild(0).gameObject.SetActive(false);  // On
                    toolToggle.GetChild(1).gameObject.SetActive(true);   // Off
                }
                break;
            case "Food":
                if (homeToggle.GetChild(0).gameObject.activeInHierarchy)
                {
                    homeToggle.GetChild(2).GetComponent<Animator>().Play("ToggleOff"); // animtion off
                    homeToggle.GetChild(0).gameObject.SetActive(false);  // On
                    homeToggle.GetChild(1).gameObject.SetActive(true);   // Off
                }
                if (drinkToggle.GetChild(0).gameObject.activeInHierarchy)
                {
                    drinkToggle.GetChild(2).GetComponent<Animator>().Play("ToggleOff"); // animtion off
                    drinkToggle.GetChild(0).gameObject.SetActive(false);  // On
                    drinkToggle.GetChild(1).gameObject.SetActive(true);   // Off
                }
                if (toolToggle.GetChild(0).gameObject.activeInHierarchy)
                {
                    toolToggle.GetChild(2).GetComponent<Animator>().Play("ToggleOff"); // animtion off
                    toolToggle.GetChild(0).gameObject.SetActive(false);  // On
                    toolToggle.GetChild(1).gameObject.SetActive(true);   // Off
                }
                break;
            case "Drink":
                if (homeToggle.GetChild(0).gameObject.activeInHierarchy)
                {
                    homeToggle.GetChild(2).GetComponent<Animator>().Play("ToggleOff"); // animtion off
                    homeToggle.GetChild(0).gameObject.SetActive(false);  // On
                    homeToggle.GetChild(1).gameObject.SetActive(true);   // Off
                }
                if (foodToggle.GetChild(0).gameObject.activeInHierarchy)
                {
                    foodToggle.GetChild(2).GetComponent<Animator>().Play("ToggleOff"); // animtion off
                    foodToggle.GetChild(0).gameObject.SetActive(false);  // On
                    foodToggle.GetChild(1).gameObject.SetActive(true);   // Off
                }
                if (toolToggle.GetChild(0).gameObject.activeInHierarchy)
                {
                    toolToggle.GetChild(2).GetComponent<Animator>().Play("ToggleOff"); // animtion off
                    toolToggle.GetChild(0).gameObject.SetActive(false);  // On
                    toolToggle.GetChild(1).gameObject.SetActive(true);   // Off
                }
                break;
            case "Tool":
                if (homeToggle.GetChild(0).gameObject.activeInHierarchy)
                {
                    homeToggle.GetChild(2).GetComponent<Animator>().Play("ToggleOff"); // animtion off
                    homeToggle.GetChild(0).gameObject.SetActive(false);  // On
                    homeToggle.GetChild(1).gameObject.SetActive(true);   // Off
                }
                if (foodToggle.GetChild(0).gameObject.activeInHierarchy)
                {
                    foodToggle.GetChild(2).GetComponent<Animator>().Play("ToggleOff"); // animtion off
                    foodToggle.GetChild(0).gameObject.SetActive(false);  // On
                    foodToggle.GetChild(1).gameObject.SetActive(true);   // Off
                }
                if (drinkToggle.GetChild(0).gameObject.activeInHierarchy)
                {
                    drinkToggle.GetChild(2).GetComponent<Animator>().Play("ToggleOff"); // animtion off
                    drinkToggle.GetChild(0).gameObject.SetActive(false);  // On
                    drinkToggle.GetChild(1).gameObject.SetActive(true);   // Off
                }
                break;
        }

        propType = value;
        SetFilterChoice();
        if (value == "None")
            return;
        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();

        foreach (var item in itemShopsDisplay)
        {
            bool exists = options.Any(option => option.text == item.category);
            if(!exists)
            {
                options.Add(new TMP_Dropdown.OptionData(item.category));
            }
        }

        categoryDropdown.AddOptions(options);
        categoryDropdown.value = 0;
        categoryDropdown.RefreshShownValue();
    }


    public void FilterCategory(int index)
    {
        categoryIndex = index;
        SetFilterChoice();
    }

    public void FilterPrice(string value)
    {
        price = value;
        SetFilterChoice();
    }

    public void SortName(int index)
    {
        sortIndex = index;
        SetFilterChoice();
    }

    public void SearchName(string value)
    {
        searchName = value;
        SetFilterChoice();
    }
}
