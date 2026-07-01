using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float rotationSpeed = 10f;

    private CharacterController characterController;
    private PlayerControls controls;
    private Vector2 moveInput;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        controls = new PlayerControls();
    }

    private void OnEnable()
    {
        controls.Enable();

        controls.Player.Move.performed += OnMove;
        controls.Player.Move.canceled += OnMove;
    }

    private void OnDisable()
    {
        controls.Player.Move.performed -= OnMove;
        controls.Player.Move.canceled -= OnMove;

        controls.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        // Convert keyboard/controller input into world movement
        Vector3 moveDirection = new Vector3(moveInput.x, 0f, moveInput.y);

        // Normalize so diagonal movement isn't faster
        if (moveDirection.magnitude > 1f)
            moveDirection.Normalize();

        // Rotate player toward movement direction
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }

        // Move the player
        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
    }
}