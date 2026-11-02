using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float fireRate = 1.5f;
    [SerializeField] private float projectileSpeed = 8f;
    [SerializeField] private float shootingRange = 10f;
    [SerializeField] private int damage = 1;

    private Transform target;
    private float fireTimer;

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private void Update()
    {
        if (target == null)
            return;

        Vector2 direction =
            target.position - transform.position;

        if (direction.magnitude > shootingRange)
            return;

        transform.right = direction.normalized;

        fireTimer -= Time.deltaTime;

        if (fireTimer <= 0f)
        {
            Shoot(direction.normalized);
            fireTimer = fireRate;
        }
    }

    private void Shoot(Vector2 direction)
    {
        if (projectilePrefab == null || firePoint == null)
            return;

        GameObject projectile =
            Instantiate(
                projectilePrefab,
                firePoint.position,
                Quaternion.identity
            );

        projectile.transform.right = direction;

        Rigidbody2D projectileRb =
            projectile.GetComponent<Rigidbody2D>();

        if (projectileRb != null)
        {
            projectileRb.linearVelocity =
                direction * projectileSpeed;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
{
    CarryableObj carryable =
        collision.gameObject.GetComponentInParent<CarryableObj>();

    if (carryable == null || !carryable.IsThrown)
        return;

    Vector2 directionToEnemy =
        ((Vector2)transform.position -
         carryable.Rigidbody.position).normalized;

    Vector2 boxVelocity =
        carryable.Rigidbody.linearVelocity.normalized;

    // Ensures the box is moving toward the enemy.
    if (Vector2.Dot(boxVelocity, directionToEnemy) <= 0f)
        return;

    Destroy(gameObject);
}
}
