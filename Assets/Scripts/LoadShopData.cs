using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class PriceRange
{
    [Range(5000, 500000000)] public int min;
    [Range(5000, 500000000)] public int max;
}

public class LoadShopData : MonoBehaviour
{
    [SerializeField] string folderPath = "Assets/Resources/Shop";
    [SerializeField] string folderItem = "Item";
    [SerializeField] string folderSprite = "ShopItem";

    [SerializeField] PriceRange priceRange_Home;
    [SerializeField] PriceRange priceRange_Food;
    [SerializeField] PriceRange priceRange_Drink;
    [SerializeField] PriceRange priceRange_Tool;
    
    
    void Start()
    {
        List<GameObject> prefabs = Resources.LoadAll<GameObject>(folderItem).ToList();
        List<Sprite> sprites = Resources.LoadAll<Sprite>(folderSprite).ToList();

        foreach (GameObject prefab in prefabs)
        {
            string assetPath = AssetDatabase.GenerateUniqueAssetPath(folderPath + $"/{prefab.name}.asset");

            if (AssetDatabase.LoadAssetAtPath<ItemShop>(assetPath) == null)
            {
                ItemShop asset = ScriptableObject.CreateInstance<ItemShop>();

                asset.name = prefab.name;
                asset.namePrefab = prefab.name;
                asset.propType = prefab.GetComponent<PropInfo>().propType;

                // Category
                string prefabPath = AssetDatabase.GetAssetPath(prefab);
                string splitNameType = prefabPath.Substring(0, prefabPath.LastIndexOf('/'));
                string prefabType = splitNameType.Split('/').Last();
                asset.category = prefabType;
                asset.imagePrefab = sprites.FirstOrDefault(x => x.name == asset.namePrefab);

                switch (asset.propType)
                {
                    case PropType.Food:
                        asset.price = Random.Range(priceRange_Food.min, priceRange_Food.max) + (10000 * prefab.name.Length);
                        asset.isConsume = true;
                        break;
                    case PropType.Drink:
                        asset.price = Random.Range(priceRange_Drink.min, priceRange_Drink.max) + (10000 * prefab.name.Length);
                        asset.isConsume = true;
                        break;
                    case PropType.Home:
                        asset.price = Random.Range(priceRange_Home.min, priceRange_Home.max) + (10000 * prefab.name.Length);
                        break;
                    case PropType.Tool:
                        asset.price = Random.Range(priceRange_Tool.min, priceRange_Tool.max) + (10000 * prefab.name.Length);
                        break;
                }
                AssetDatabase.CreateAsset(asset, assetPath);
                AssetDatabase.SaveAssets();
                Debug.Log("Created New ItemShop at: " + assetPath);
            }
        }
        AssetDatabase.Refresh();
    }
}
