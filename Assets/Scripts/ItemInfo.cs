using UnityEngine;

[System.Serializable]
public class ItemInfo
{
    public string namePrefab;
    [Range(0, 999999999)] public int price;
    public bool isConsume;
    public PropType propType;
    public Sprite imagePrefab;
    public string category;

    public ItemInfo(string namePrefab, int price, bool isConsume, PropType propType, Sprite imagePrefab, string category)
    {
        this.namePrefab = namePrefab;
        this.price = price;
        this.isConsume = isConsume;
        this.propType = propType;
        this.imagePrefab = imagePrefab;
        this.category = category;
    }
}
