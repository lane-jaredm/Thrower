using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float movSpeed = 5f;
    [SerializeField] private float movAccel = 20f;
    [SerializeField] private InputActionReference moveActionReference;

    private Rigidbody2D playerRB;
    private Vector2 moveInput;
    private Vector2 currentVelocity;

    private void Awake()
    {
        playerRB = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Vector2 desiredVelocity = Vector2.ClampMagnitude(moveInput, 1f) * movSpeed;

        currentVelocity = Vector2.MoveTowards(currentVelocity, desiredVelocity, movAccel * Time.fixedDeltaTime);
        playerRB.MovePosition(playerRB.position + currentVelocity * Time.fixedDeltaTime);
    }

    void OnEnable()
    {
        if(moveActionReference == null) return;
        moveActionReference.action.Enable();
        moveActionReference.action.performed += OnMovePerformed;
        moveActionReference.action.canceled += OnMoveCanceled;
    }

    void OnDisable()
    {
        if(moveActionReference == null) return;
        moveActionReference.action.performed -= OnMovePerformed;
        moveActionReference.action.canceled -= OnMoveCanceled;
        moveActionReference.action.Disable();
    }
    private void OnMovePerformed(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext ctx)
    {
        moveInput = Vector2.zero;
    }
}
