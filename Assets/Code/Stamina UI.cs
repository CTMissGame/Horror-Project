using UnityEngine;
using UnityEngine.UI;

public class StaminaUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerController.PlayerController playerController;
    [SerializeField] private Image staminaBarFill;
    [SerializeField] private CanvasGroup staminaCanvasGroup; // สำหรับ fade

    [Header("UI Settings")]
    [SerializeField] private Color normalColor = Color.green;
    [SerializeField] private Color lowColor = Color.red;
    [SerializeField] private float lowStaminaThreshold = 0.3f; // ต่ำกว่า 30% เปลี่ยนสี

    [Header("Fade Settings")]
    [SerializeField] private float fadeOutDelay = 2f;    // รอกี่วินาทีก่อนหาย
    [SerializeField] private float fadeSpeed = 2f;        // ความเร็ว fade

    private float fadeTimer;
    private float lastStamina;

    void Start()
    {
        // หา PlayerController อัตโนมัติถ้าไม่ได้ assign
        if (playerController == null)
            playerController = FindFirstObjectByType<PlayerController.PlayerController>();

        lastStamina = playerController.GetStamina();
    }

    void Update()
    {
        if (playerController == null) return;

        UpdateBar();
        UpdateColor();
        HandleFade();
    }

    void UpdateBar()
    {
        // อัปเดตขนาด fill bar
        float staminaPercent = playerController.GetStaminaPercent();
        staminaBarFill.fillAmount = staminaPercent;
    }

    void UpdateColor()
    {
        float staminaPercent = playerController.GetStaminaPercent();

        // เปลี่ยนสีตาม stamina
        if (staminaPercent <= lowStaminaThreshold)
            staminaBarFill.color = lowColor;
        else
            staminaBarFill.color = normalColor;
    }

    void HandleFade()
    {
        float currentStamina = playerController.GetStamina();

        // ถ้า stamina เปลี่ยนแปลง ให้แสดง bar
        if (currentStamina != lastStamina)
        {
            staminaCanvasGroup.alpha = 1f;
            fadeTimer = fadeOutDelay;
        }
        else
        {
            // นับถอยหลัง fade out
            if (fadeTimer > 0)
                fadeTimer -= Time.deltaTime;
            else
                staminaCanvasGroup.alpha = Mathf.Lerp(
                    staminaCanvasGroup.alpha, 0f, Time.deltaTime * fadeSpeed
                );
        }

        lastStamina = currentStamina;
    }
}