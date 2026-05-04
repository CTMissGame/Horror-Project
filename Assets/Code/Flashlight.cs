using UnityEngine;

public class Flashlight : MonoBehaviour
{
    [Header("Sound Settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip turnOnSound;
    [SerializeField] private AudioClip turnOffSound;

    [Header("Bob Settings")]
    [SerializeField] private Transform flashlightModel;
    [SerializeField] private float bobSpeed = 10f;
    [SerializeField] private float bobAmount = 0.05f;
    [SerializeField] private float bobSmooth = 10f;

    private Vector3 originalPosition;
    private float timer = 0f;

    void Start()
    {
        if (flashlightModel != null)
            originalPosition = flashlightModel.localPosition;
    }

    void Update()
    {
        HandleToggle();
        HandleBob();
    }

    void HandleToggle()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (FlashlightBattery.Instance.IsOn())
            {
                FlashlightBattery.Instance.TurnOff();
                if (turnOffSound != null && audioSource != null)
                    audioSource.PlayOneShot(turnOffSound);
            }
            else
            {
                if (!FlashlightBattery.Instance.IsDead())
                {
                    FlashlightBattery.Instance.TurnOn();
                    if (turnOnSound != null && audioSource != null)
                        audioSource.PlayOneShot(turnOnSound);
                }
            }
        }
    }

    void HandleBob()
    {
        if (flashlightModel == null) return;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        bool isMoving = horizontal != 0 || vertical != 0;

        if (isMoving)
        {
            timer += Time.deltaTime * bobSpeed;
            float bobX = Mathf.Sin(timer) * bobAmount;
            float bobY = Mathf.Abs(Mathf.Sin(timer)) * bobAmount;
            Vector3 targetPos = originalPosition + new Vector3(bobX, bobY, 0);
            flashlightModel.localPosition = Vector3.Lerp(
                flashlightModel.localPosition, targetPos, Time.deltaTime * bobSmooth);
        }
        else
        {
            timer = 0f;
            flashlightModel.localPosition = Vector3.Lerp(
                flashlightModel.localPosition, originalPosition, Time.deltaTime * bobSmooth);
        }
    }
}