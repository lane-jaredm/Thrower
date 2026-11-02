using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform player;
    [SerializeField] private Transform[] spawnPoints;

    [SerializeField] private float startingSpawnInterval = 3f;
    [SerializeField] private float minimumSpawnInterval = 0.5f;
    [SerializeField] private float spawnRateIncrease = 0.05f;

    [SerializeField] private int startingMaxEnemies = 10;
    [SerializeField] private int maximumEnemies = 50;
    [SerializeField] private int maxEnemiesIncrease = 1;

    private float spawnTimer;
    private float spawnInterval;
    private int maxEnemies;

    private void Start()
    {
        spawnInterval = startingSpawnInterval;
        maxEnemies = startingMaxEnemies;
    }

    private void Update()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f)
        {
            TrySpawnEnemy();
            spawnTimer = spawnInterval;

            IncreaseDifficulty();
        }
    }

    private void TrySpawnEnemy()
    {
        GameObject[] enemies =
            GameObject.FindGameObjectsWithTag("Enemy");

        if (enemies.Length >= maxEnemies)
            return;

        if (spawnPoints.Length == 0 || enemyPrefab == null)
            return;

        Transform spawnPoint =
            spawnPoints[Random.Range(0, spawnPoints.Length)];

        GameObject enemy =
            Instantiate(
                enemyPrefab,
                spawnPoint.position,
                Quaternion.identity
            );

        EnemyShooter shooter =
            enemy.GetComponent<EnemyShooter>();

        if (shooter != null)
            shooter.SetTarget(player);
    }

    private void IncreaseDifficulty()
    {
        spawnInterval = Mathf.Max(
            minimumSpawnInterval,
            spawnInterval - spawnRateIncrease
        );

        maxEnemies = Mathf.Min(
            maximumEnemies,
            maxEnemies + maxEnemiesIncrease
        );
    }
}