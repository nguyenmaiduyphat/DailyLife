using UnityEngine;

public class PropInfo : MonoBehaviour
{
    public PropType propType;
    public ItemInfo itemInfo;
}

public enum PropType
{
    Food,
    Drink,
    Home,
    Tool
}
