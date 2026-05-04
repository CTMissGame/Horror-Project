using UnityEngine;

public class ItemHolder : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private HotbarUI hotbarUI;
    [SerializeField] private Inventory inventory;
    [SerializeField] private Transform handTransform; // ตำแหน่งที่วางของในมือ
    [SerializeField] private GameObject flashlightObject; // GameObject ไฟฉายจริงในมือ

    private GameObject currentHandItem;
    private int currentSlot = 0;

    void Start()
    {
        hotbarUI.onSlotChanged += OnSlotChanged;

        ShowFlashlight(true);
        UpdateHandItem(hotbarUI.GetSelectedSlot());
    }

    void OnDestroy()
    {
        hotbarUI.onSlotChanged -= OnSlotChanged;
    }

    void OnSlotChanged(int prevSlot, int newSlot)
    {
        currentSlot = newSlot;
        UpdateHandItem(newSlot);
    }

    void UpdateHandItem(int slot)
    {
        if (currentHandItem != null)
        {
            Destroy(currentHandItem);
            currentHandItem = null;
        }

        if (slot == 0)
        {
            ShowFlashlight(true);
        }
        else
        {
            ShowFlashlight(false);

            ItemData item = inventory.GetItem(slot);
            Debug.Log($"Slot: {slot}, Item: {(item != null ? item.itemName : "NULL")}, HandPrefab: {(item?.handPrefab != null ? item.handPrefab.name : "NULL")}");

            if (item != null && item.handPrefab != null)
            {
                currentHandItem = Instantiate(item.handPrefab, handTransform);
                currentHandItem.transform.localPosition = Vector3.zero;
                currentHandItem.transform.localRotation = Quaternion.identity;
                Debug.Log("Spawned: " + currentHandItem.name);
            }
        }
    }

    void ShowFlashlight(bool show)
    {
        if (flashlightObject != null)
            flashlightObject.SetActive(show);
    }
}