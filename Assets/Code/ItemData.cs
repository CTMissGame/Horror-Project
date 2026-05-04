using UnityEngine;

public enum ItemType
{
    Default,
    Battery,
    Key,
    KeyCard
}

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public GameObject worldPrefab;
    public GameObject handPrefab;
    public ItemType itemType;
}