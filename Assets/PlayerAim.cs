using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAim : MonoBehaviour
{
    public Vector2 AimDirection { get; private set; }

    [SerializeField] private Transform aimCursor;

    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        Vector2 mouseScreenPosition =
            Mouse.current.position.ReadValue();

        float distanceFromCamera =
            Mathf.Abs(mainCamera.transform.position.z - transform.position.z);

        Vector3 mouseWorldPosition =
            mainCamera.ScreenToWorldPoint(
                new Vector3(
                    mouseScreenPosition.x,
                    mouseScreenPosition.y,
                    distanceFromCamera
                )
            );

        AimDirection =
            ((Vector2)mouseWorldPosition - (Vector2)transform.position)
            .normalized;

        if (AimDirection == Vector2.zero)
            return;

        // Assumes the player's default facing direction is right.
        transform.right = AimDirection;

        if (aimCursor != null)
            aimCursor.position = mouseWorldPosition;
    }
}
