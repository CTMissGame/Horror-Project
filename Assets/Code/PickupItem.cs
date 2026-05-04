using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [SerializeField] private ItemData itemData;
    [SerializeField] private float pickupRange = 2f;
    [SerializeField] private KeyCode pickupKey = KeyCode.E;

    private Transform player;
    private Camera playerCamera;
    private bool isLookingAt = false;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        playerCamera = Camera.main;
    }

    void Update()
    {
        isLookingAt = CheckCrosshairAim();
        float distance = Vector3.Distance(transform.position, player.position);

        if (isLookingAt && distance <= pickupRange)
        {
            if (Input.GetKeyDown(pickupKey))
                TryPickup();
        }
    }

    void TryPickup()
    {
        if (itemData.itemType == ItemType.Battery)
        {
            FlashlightBattery.Instance.AddBattery(0.5f);
            Destroy(gameObject);
            return;
        }

        if (Inventory.Instance.AddItem(itemData))
            Destroy(gameObject);
    }

    void OnGUI()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        if (!isLookingAt || distance > pickupRange) return;

        Vector3 screenPos = playerCamera.WorldToScreenPoint(
            transform.position + Vector3.up * 0.5f);
        if (screenPos.z < 0) return;

        screenPos.y = Screen.height - screenPos.y;

        GUIStyle style = new GUIStyle();
        style.fontSize = 24;
        style.fontStyle = FontStyle.Bold;
        style.normal.textColor = Color.white;
        style.alignment = TextAnchor.MiddleCenter;

        GUI.color = Color.black;
        GUI.Label(new Rect(screenPos.x - 99, screenPos.y - 19, 200, 40), "[E] เก็บ", style);
        GUI.color = Color.white;
        GUI.Label(new Rect(screenPos.x - 100, screenPos.y - 20, 200, 40), "[E] เก็บ", style);
    }

    bool CheckCrosshairAim()
    {
        Ray ray = playerCamera.ScreenPointToRay(
            new Vector3(Screen.width / 2, Screen.height / 2, 0));
        if (Physics.Raycast(ray, out RaycastHit hit, pickupRange))
            return hit.transform == transform || hit.transform.IsChildOf(transform);
        return false;
    }
}