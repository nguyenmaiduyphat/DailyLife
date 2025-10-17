using UnityEngine;

/// <summary>
/// This is camera class to player can see around where this character live
/// </summary>
public class PlayerCamera : MonoBehaviour
{
    [Header("Mouse Settings")]
    public float mouseSensitivity = 100f;

    private Transform player;
    private float xRotation = 0f;
    public bool hasOpen;

    void Start()
    {
        // Lock cursor in the center
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        player = transform.parent; // assume camera is child of player
    }

    void Update()
    {
        Cursor.lockState= hasOpen ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible= hasOpen;
        GetComponent<Camera>().fieldOfView = SettingData.Instance.gameSetting.viewRange;
        mouseSensitivity = SettingData.Instance.gameSetting.sensitivity;
        // --- Mouse input ---
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        if(hasOpen)
        {
            mouseX = 0;
            mouseY = 0;
        }

        // Vertical rotation (camera only)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f); // prevent flipping
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Horizontal rotation (rotate player body)
        player.Rotate(Vector3.up * mouseX);
    }
}
