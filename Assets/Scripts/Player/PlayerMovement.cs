using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Transform cameraTransform;
    public CharacterController controller;

    [SerializeField] private float speed = 12f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpForce = 10f;

    [SerializeField] private float acceleration;
    Vector2 smoothedInput;
    Vector3 velocity;

    InputAction moveAction;
    InputAction jumpAction;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
    }

    void Update()
    {
        // rotate player with camera
        Vector3 cameraEuler = cameraTransform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(0f, cameraEuler.y, 0f);

        // reset vertical speed when grounded
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // movement
        Vector2 moveVector = moveAction.ReadValue<Vector2>();
        HandleMovement(moveVector);

        // jumping
        if (jumpAction.WasPressedThisFrame() && controller.isGrounded)
        {
            velocity.y = jumpForce;
        }


    }

    private void HandleMovement(Vector2 moveVector)
    {
        // walking
        smoothedInput = Vector2.MoveTowards(smoothedInput, moveVector, acceleration * Time.deltaTime);
        Vector3 move = transform.right * smoothedInput.x + transform.forward * smoothedInput.y; controller.Move(move * speed * Time.deltaTime);

        // gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}