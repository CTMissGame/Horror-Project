using UnityEngine;

public class FlashlightBattery : MonoBehaviour
{
    public static FlashlightBattery Instance;

    [Header("Battery Settings")]
    [SerializeField] private float maxBatteryTime = 600f; // 10 นาที
    private float currentBattery;

    [Header("References")]
    [SerializeField] private Light flashlightLight;

    private bool isOn = false;

    void Awake()
    {
        Instance = this;
        currentBattery = maxBatteryTime;
    }

    void Start()
    {
        if (flashlightLight != null)
            flashlightLight.enabled = false;
        isOn = false;
    }

    void Update()
    {
        if (isOn && currentBattery > 0f)
        {
            currentBattery -= Time.deltaTime;
            currentBattery = Mathf.Clamp(currentBattery, 0f, maxBatteryTime);

            if (flashlightLight != null)
            {
                float percent = currentBattery / maxBatteryTime;
                flashlightLight.intensity = Mathf.Lerp(0f, 3f, percent);
            }

            if (currentBattery <= 0f)
                TurnOff();
        }
    }

    public void TurnOn()
    {
        if (currentBattery <= 0f || isOn) return; 
        isOn = true;
        if (flashlightLight != null)
            flashlightLight.enabled = true;
    }

    public void TurnOff()
    {
        if (!isOn) return; 
        isOn = false;
        if (flashlightLight != null)
            flashlightLight.enabled = false;
    }

    public void AddBattery(float percent)
    {
        currentBattery += maxBatteryTime * percent;
        currentBattery = Mathf.Clamp(currentBattery, 0f, maxBatteryTime);
    }

    public float GetBatteryPercent() => currentBattery / maxBatteryTime;
    public bool IsOn() => isOn;
    public bool IsDead() => currentBattery <= 0f;

}