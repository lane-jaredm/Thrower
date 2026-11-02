using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private InputActionReference grabActionReference;
    [SerializeField] private InputActionReference throwActionReference;

    [SerializeField] private PlayerAim playerAim;

    [SerializeField] private Transform holdPoint;
    [SerializeField] private float grabRadius = 1f;
    [SerializeField] private LayerMask carryableLayer;
    [SerializeField] private float throwForce = 12f;

    private CarryableObj heldObject;

    private void OnEnable()
    {
        grabActionReference.action.Enable();
        throwActionReference.action.Enable();

        grabActionReference.action.performed += OnGrabPerformed;
        throwActionReference.action.performed += OnThrowPerformed;
    }

    private void OnDisable()
    {
        grabActionReference.action.performed -= OnGrabPerformed;
        throwActionReference.action.performed -= OnThrowPerformed;

        grabActionReference.action.Disable();
        throwActionReference.action.Disable();
    }

    private void FixedUpdate()
    {
        if (heldObject != null)
        {
            heldObject.Rigidbody.MovePosition(holdPoint.position);
        }
    }

    private void OnGrabPerformed(InputAction.CallbackContext context)
    {
        if (heldObject != null)
        {
            DropObject();
            return;
        }

        Collider2D hit = Physics2D.OverlapCircle(
            transform.position,
            grabRadius,
            carryableLayer
        );

        if (hit == null)
            return;

        CarryableObj carryable =
            hit.GetComponentInParent<CarryableObj>();

        if (carryable == null)
            return;

        heldObject = carryable;
        heldObject.SetThrown(false);

        heldObject.Rigidbody.linearVelocity = Vector2.zero;
        heldObject.Rigidbody.angularVelocity = 0f;
        heldObject.Rigidbody.bodyType = RigidbodyType2D.Kinematic;
    }

private void OnThrowPerformed(InputAction.CallbackContext context)
{
    if (heldObject == null || playerAim == null)
        return;

    CarryableObj objectToThrow = heldObject;
    heldObject = null;

    Vector2 throwDirection =
        playerAim.AimDirection.normalized;

    objectToThrow.SetThrown(true);
    objectToThrow.Rigidbody.bodyType = RigidbodyType2D.Dynamic;
    objectToThrow.Rigidbody.linearVelocity =
        throwDirection * throwForce;
}

private void DropObject()
{
    heldObject.SetThrown(false);
    heldObject.Rigidbody.bodyType = RigidbodyType2D.Dynamic;
    heldObject = null;
}

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, grabRadius);
    }
}
