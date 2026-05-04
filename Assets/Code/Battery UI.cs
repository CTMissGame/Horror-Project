using UnityEngine;
using TMPro;

public class BatteryUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI batteryText;

    void Update()
    {
        if (FlashlightBattery.Instance == null) return;

        float percent = FlashlightBattery.Instance.GetBatteryPercent();

        if (batteryText != null)
            batteryText.text = $"{percent * 100f:F0}%";
    }
}