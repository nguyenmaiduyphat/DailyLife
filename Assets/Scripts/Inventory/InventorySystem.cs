using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// This is Inventory System where save items in inventoy by checking all items each slots
/// </summary>
public class InventorySystem : MonoBehaviour
{
    public List<InventorySlot> slots;
    [SerializeField] PlayerData playerData;

    public GameObject itemPrefab;
    [SerializeField] string folderitemData = "Shop";
    private void Start()
    {
        slots = GetComponentsInChildren<InventorySlot>().ToList();
        int indexSlot = 0;
        
        foreach(Item item in playerData.playerInfo.items)
        {
            GameObject inventoryItem = Instantiate(itemPrefab, slots[indexSlot].transform);
            ItemShop[] itemShops = Resources.LoadAll<ItemShop>(folderitemData);

            ItemShop itemShop = itemShops.FirstOrDefault(x => x.name == item.namePrefab);

            inventoryItem.GetComponent<DraggableItem>().image.sprite = itemShop.imagePrefab;

            inventoryItem.GetComponent<DraggableItem>().itemInfo = new ItemInfo(itemShop.namePrefab, itemShop.price, itemShop.isConsume, itemShop.propType, itemShop.imagePrefab, itemShop.category);

            indexSlot++;
        }
    }

    public void CheckItemsLeft()
    {
        playerData.playerInfo.items.Clear();
        foreach(InventorySlot slot in slots)
        {
            if(slot.transform.childCount > 0)
            {
                playerData.playerInfo.items.Add(new Item
                {
                    namePrefab = slot.transform.GetChild(0).GetComponent<DraggableItem>().itemInfo.namePrefab,
                    price = slot.transform.GetChild(0).GetComponent<DraggableItem>().itemInfo.price,
                    isConsume = slot.transform.GetChild(0).GetComponent<DraggableItem>().itemInfo.isConsume,
                    propType = slot.transform.GetChild(0).GetComponent<DraggableItem>().itemInfo.propType.ToString(),
                    imagePrefab = slot.transform.GetChild(0).GetComponent<DraggableItem>().itemInfo.imagePrefab.name,
                    category = slot.transform.GetChild(0).GetComponent<DraggableItem>().itemInfo.category
                });
            }
        }
    }
}
