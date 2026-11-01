using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    //walking mechanic fields
    [SerializeField] private float movSpeed = 5f; //Top speed for walking
    [SerializeField] private float movAccel = 20f; //How fast the player accelerates to walking speed/deaccelerates to motionless
    [SerializeField] private InputActionReference moveActionReference; //WASD or Arrow Key Inputs

    //dashing mechanic fields
    [SerializeField] private InputActionReference dashActionReference;
    [SerializeField] private float dashSpeed = 15f;
    [SerializeField] private float dashDuration = 0.15f;
    [SerializeField] private float dashCooldown = 0.5f;


    //
    private Rigidbody2D playerRB;
    private Vector2 moveInput;
    private Vector2 currentVelocity;

    private Vector2 dashDirection;
    private float dashTimeRemaining;
    private float dashCooldownRemaining;



    private void Awake()
    {
        playerRB = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        dashCooldownRemaining -= Time.fixedDeltaTime;

        if(dashTimeRemaining > 0f)
        {
            playerRB.MovePosition(playerRB.position + dashDirection * dashSpeed * Time.fixedDeltaTime);

            dashTimeRemaining -= Time.fixedDeltaTime;
            return;
        }

        Vector2 desiredVelocity = Vector2.ClampMagnitude(moveInput, 1f) * movSpeed; //The max speed normalized 

        //accelerate or deaccelerate from current velocity to desired velocity
        currentVelocity = Vector2.MoveTowards(currentVelocity, desiredVelocity, movAccel * Time.fixedDeltaTime);
        playerRB.MovePosition(playerRB.position + currentVelocity * Time.fixedDeltaTime);
    }

    void OnEnable()
    {
        if(moveActionReference == null) return;
        moveActionReference.action.Enable();
        moveActionReference.action.performed += OnMovePerformed;
        moveActionReference.action.canceled += OnMoveCanceled;

        dashActionReference.action.Enable();
        dashActionReference.action.performed += OnDashPerformed;
    }

    void OnDisable()
    {
        if(moveActionReference == null) return;
        moveActionReference.action.performed -= OnMovePerformed;
        moveActionReference.action.canceled -= OnMoveCanceled;
        moveActionReference.action.Disable();

        dashActionReference.action.performed -= OnDashPerformed;
        dashActionReference.action.Disable();
    }
    private void OnMovePerformed(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>(); //get velocity vector
    }

    private void OnMoveCanceled(InputAction.CallbackContext ctx)
    {
        moveInput = Vector2.zero; //when no movement is pressed zero the velocity
    }

    private void OnDashPerformed(InputAction.CallbackContext ctx)
    {
        if (dashCooldownRemaining > 0f || dashTimeRemaining > 0f) return;

        dashDirection = moveInput.normalized;

        if(dashDirection == Vector2.zero)
        {
            dashDirection = transform.right;
        }

        dashTimeRemaining = dashDuration;
        dashCooldownRemaining = dashCooldown;
    }
}
