using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    [SerializeField] private HotbarUI hotbarUI;
    [SerializeField] private ItemData flashlightItem; // ลาก ItemData ของไฟฉายใส่

    private ItemData[] slots = new ItemData[4];

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // ใส่ไฟฉายใน slot 0 ตั้งแต่แรก
        slots[0] = flashlightItem;
        hotbarUI.SetItem(0, flashlightItem);
        hotbarUI.SelectSlot(0);
    }

    public bool AddItem(ItemData item)
    {
        // เริ่มจาก slot 1 (slot 0 สำรองไว้ให้ไฟฉาย)
        for (int i = 1; i < slots.Length; i++)
        {
            if (slots[i] == null)
            {
                slots[i] = item;
                hotbarUI.SetItem(i, item);
                return true;
            }
        }
        Debug.Log("Inventory เต็มแล้ว!");
        return false;
    }

    public ItemData GetItem(int slot) => slots[slot];
    public ItemData GetSelectedItem() => slots[hotbarUI.GetSelectedSlot()];

    public void RemoveSelectedItem()
    {
        int slot = hotbarUI.GetSelectedSlot();
        if (slot == 0) return; // ลบไฟฉายไม่ได้
        slots[slot] = null;
        hotbarUI.SetItem(slot, null);
    }
}