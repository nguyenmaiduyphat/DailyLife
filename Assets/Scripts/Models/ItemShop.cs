using UnityEngine;


[CreateAssetMenu(fileName = "NewItemShop", menuName = "Shop/Item")]
public class ItemShop : ScriptableObject
{
    public string namePrefab;
    [Range(0, 999999999)] public int price;
    public bool isConsume;
    public PropType propType;
    public Sprite imagePrefab;
    public string category;
}