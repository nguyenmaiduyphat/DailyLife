using UnityEngine;

[System.Serializable]
public class Item
{
    public string namePrefab;
    [Range(0, 999999999)] public int price;
    public bool isConsume;
    public string propType;
    public string imagePrefab;
    public string category;
}
