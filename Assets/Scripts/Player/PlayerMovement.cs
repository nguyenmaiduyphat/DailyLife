using UnityEngine;

/// <summary>
/// This is player's movement
/// </summary>

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpHeight = 2f;
    [SerializeField] float gravity = -9.81f;

    private CharacterController controller;
    [SerializeField] PlayerCamera playerCamera;
    private Animator animator;
    private Vector3 velocity;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }
    float x;
    float z;

    [HideInInspector]public  bool hasJump;

    private float DecayToZero(float value, float rate)
    {
        if (value > 0)
        {
            value -= rate;
            if (value < 0) value = 0;
        }
        else if (value < 0)
        {
            value += rate;
            if (value > 0) value = 0;
        }
        return value;
    }
    void Update()
    {
        audioSource.volume = SettingData.Instance.gameSetting.step_Sound / 100;
        // --- Ground Check using CharacterController ---
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // keeps player grounded
            hasJump = false;
        }

        // --- Movement ---
        if (controller.isGrounded && !playerCamera.hasOpen)
        {
            x = Input.GetAxis("Horizontal"); // A/D
            z = Input.GetAxis("Vertical");   // W/S
        }
        else
        {
            float rate = Time.deltaTime;
            x = DecayToZero(x, rate);
            z = DecayToZero(z, rate);
        }

        Vector3 move = transform.right * x + transform.forward * z;

        // --- Prevent diagonal speed boost ---
        if (move.magnitude > 1f)
            move.Normalize();

        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? moveSpeed * 3f : moveSpeed;


        // --- Jump ---
        if (Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            animator.SetTrigger("Jump"); // Trigger jump animation
            hasJump = true;
        }

        // --- Apply Gravity ---
        velocity.y += gravity * Time.deltaTime; 
        controller.Move(move * currentSpeed * Time.deltaTime);
        controller.Move(velocity * Time.deltaTime);

        animator.SetFloat("Horizontal", x * currentSpeed); // normalized speed
        animator.SetFloat("Vertical", z * currentSpeed); // normalized speed

    }

    /// <summary>
    /// This part for sound when move. attach into animation clip (Event part)
    /// </summary>

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip stepClip;
    [SerializeField] AudioClip jumpClip;
    void Step()
    {
        audioSource.PlayOneShot(stepClip);
    }
    void Jump()
    {
        audioSource.PlayOneShot(jumpClip);
    }
}
