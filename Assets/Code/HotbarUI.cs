using UnityEngine;
using UnityEngine.UI;
using System;

public class HotbarUI : MonoBehaviour
{
    [Header("Slots")]
    [SerializeField] private Image[] slotBackgrounds;
    [SerializeField] private Image[] itemIcons;

    [Header("Selection")]
    [SerializeField] private Color normalColor = new Color(0.2f, 0.2f, 0.2f, 0.8f);
    [SerializeField] private Color selectedColor = new Color(0.8f, 0.7f, 0.1f, 0.9f);

    private int selectedSlot = 0;
    public Action<int, int> onSlotChanged; // (prevSlot, newSlot)

    void Update()
    {
        HandleSlotSelection();
    }

    void HandleSlotSelection()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SelectSlot(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SelectSlot(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SelectSlot(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) SelectSlot(3);

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f) SelectSlot((selectedSlot - 1 + 4) % 4);
        if (scroll < 0f) SelectSlot((selectedSlot + 1) % 4);
    }

    public void SelectSlot(int index)
    {
        int prev = selectedSlot;

        for (int i = 0; i < slotBackgrounds.Length; i++)
            slotBackgrounds[i].color = normalColor;

        selectedSlot = index;
        slotBackgrounds[selectedSlot].color = selectedColor;

        onSlotChanged?.Invoke(prev, selectedSlot);
    }

    public void SetItem(int slot, ItemData item)
    {
        if (item != null)
        {
            itemIcons[slot].sprite = item.icon;
            itemIcons[slot].color = Color.white;
        }
        else
        {
            itemIcons[slot].sprite = null;
            itemIcons[slot].color = new Color(1, 1, 1, 0);
        }
    }

    public int GetSelectedSlot() => selectedSlot;
}