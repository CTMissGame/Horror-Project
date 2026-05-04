using UnityEngine;

namespace PlayerController
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float walkSpeed = 3f;
        [SerializeField] private float runSpeed = 6f;
        [SerializeField] private float jumpHeight = 2f;
        [SerializeField] private float gravity = -9.81f;

        [Header("Ground Check")]
        [SerializeField] private Transform groundCheck;
        [SerializeField] private float groundDistance = 0.4f;
        [SerializeField] private LayerMask groundMask = 1;

        [Header("Movement Smoothing")]
        [SerializeField] private float accelerationTime = 0.1f;
        [SerializeField] private float decelerationTime = 0.1f;

        [Header("Input Settings")]
        [SerializeField] private KeyCode runKey = KeyCode.LeftShift;
        [SerializeField] private KeyCode jumpKey = KeyCode.Space;

        [Header("Stamina Settings")]
        [SerializeField] private float maxStamina = 100f;
        [SerializeField] private float staminaDrainRate = 20f;   // ลดต่อวินาทีตอนวิ่ง
        [SerializeField] private float staminaRegenRate = 10f;   // เพิ่มต่อวินาทีตอนหยุดวิ่ง
        [SerializeField] private float staminaRegenDelay = 1.5f; // รอกี่วินาทีก่อน regen

        // Components
        private CharacterController controller;

        // Movement variables
        private Vector3 velocity;
        private bool isGrounded;
        private Vector2 currentInputVector;
        private Vector2 smoothInputVelocity;

        // Movement state
        private bool isRunning;
        private float currentSpeed;

        // Stamina variables
        private float currentStamina;
        private float regenDelayTimer;
        private bool isExhausted; // หมดแรง ต้องรอ regen ก่อนวิ่งได้

        void Start()
        {
            controller = GetComponent<CharacterController>();
            currentStamina = maxStamina; // เริ่มต้นเต็ม

            if (groundCheck == null)
            {
                GameObject groundCheckObj = new GameObject("GroundCheck");
                groundCheckObj.transform.SetParent(transform);
                groundCheckObj.transform.localPosition = new Vector3(0, -controller.height / 2, 0);
                groundCheck = groundCheckObj.transform;
            }
        }

        void Update()
        {
            HandleGroundCheck();
            HandleInput();
            HandleStamina();
            HandleMovement();
            HandleGravityAndJump();

            controller.Move(velocity * Time.deltaTime);
        }

        private void HandleGroundCheck()
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }
        }

        private void HandleInput()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            // วิ่งได้เฉพาะตอนมี stamina และไม่ exhausted
            isRunning = Input.GetKey(runKey) && currentStamina > 0 && !isExhausted;

            Vector2 targetInputVector = new Vector2(horizontal, vertical).normalized;
            float smoothTime = targetInputVector.magnitude > 0 ? accelerationTime : decelerationTime;
            currentInputVector = Vector2.SmoothDamp(currentInputVector, targetInputVector, ref smoothInputVelocity, smoothTime);
        }

        private void HandleStamina()
        {
            bool actuallyRunning = isRunning && currentInputVector.magnitude > 0.1f;

            if (actuallyRunning)
            {
                // ลด stamina ตอนวิ่ง
                currentStamina -= staminaDrainRate * Time.deltaTime;
                currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
                regenDelayTimer = staminaRegenDelay; // รีเซ็ต delay

                // หมดแรง
                if (currentStamina <= 0)
                    isExhausted = true;
            }
            else
            {
                // รอ delay ก่อน regen
                if (regenDelayTimer > 0)
                {
                    regenDelayTimer -= Time.deltaTime;
                }
                else
                {
                    // regen stamina
                    currentStamina += staminaRegenRate * Time.deltaTime;
                    currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);

                    // หายเหนื่อยตอน stamina ถึง 30%
                    if (isExhausted && currentStamina >= maxStamina * 0.3f)
                        isExhausted = false;
                }
            }
        }

        private void HandleMovement()
        {
            currentSpeed = isRunning ? runSpeed : walkSpeed;
            Vector3 moveDirection = transform.right * currentInputVector.x + transform.forward * currentInputVector.y;
            velocity.x = moveDirection.x * currentSpeed;
            velocity.z = moveDirection.z * currentSpeed;
        }

        private void HandleGravityAndJump()
        {
            if (Input.GetKeyDown(jumpKey) && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
            velocity.y += gravity * Time.deltaTime;
        }

        // Public methods
        public bool IsGrounded() => isGrounded;
        public bool IsRunning() => isRunning && currentInputVector.magnitude > 0.1f;
        public bool IsMoving() => currentInputVector.magnitude > 0.1f;
        public float GetCurrentSpeed() => currentSpeed;
        public Vector3 GetVelocity() => velocity;

        // Stamina public methods
        public float GetStamina() => currentStamina;
        public float GetMaxStamina() => maxStamina;
        public float GetStaminaPercent() => currentStamina / maxStamina;
        public bool IsExhausted() => isExhausted;

        public void SetMovementSpeeds(float newWalkSpeed, float newRunSpeed)
        {
            walkSpeed = newWalkSpeed;
            runSpeed = newRunSpeed;
        }

        public void SetJumpHeight(float newJumpHeight)
        {
            jumpHeight = newJumpHeight;
        }

        private void OnDrawGizmosSelected()
        {
            if (groundCheck != null)
            {
                Gizmos.color = isGrounded ? Color.green : Color.red;
                Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
            }
        }
    }
}