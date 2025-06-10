using UnityEngine;
using System.Collections.Generic;

public class GroundTile : MonoBehaviour
{
    GroundSpawner groundSpawner;

    [Header("Obstacle Prefabs")]
    [SerializeField] GameObject obstaclePrefab1;
    [SerializeField] GameObject obstaclePrefab2;
    [SerializeField] GameObject obstaclePrefab3;        // tall variant
    [SerializeField] float tallObstacleChance = 0.1f;

    [Header("Coin Prefab")]
    [SerializeField] GameObject coinPrefab;

    [Header("Spawn Point References")]
    [Tooltip("Parent containing lane points for obstacles")]
    [SerializeField] Transform obstacleSpawnPointsParent;
    [Tooltip("Parent containing fixed coin spawn points (9 children)")]
    [SerializeField] Transform coinSpawnPointsParent;

    private void Start()
    {
        groundSpawner = FindObjectOfType<GroundSpawner>();

        if (obstacleSpawnPointsParent == null)
            obstacleSpawnPointsParent = transform.Find("ObstacleSpawnPoints");
        if (coinSpawnPointsParent == null)
            coinSpawnPointsParent   = transform.Find("CoinSpawnPoints");
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            groundSpawner.SpawnTile(true);
            Destroy(gameObject, 1f);
        }
    }

    public void SpawnObstacle()
    {
        if (obstacleSpawnPointsParent == null)
        {
            Debug.LogError("ObstacleSpawnPointsParent is not assigned or found! Cannot spawn obstacles.", this);
            return;
        }

        if (obstacleSpawnPointsParent.childCount == 0)
        {
            Debug.LogWarning("ObstacleSpawnPointsParent has no children. Cannot spawn obstacles.", this);
            return;
        }

        int totalSpawnPoints = obstacleSpawnPointsParent.childCount;
        bool[] occupiedIndices = new bool[totalSpawnPoints];

        // --- First Obstacle Spawning Logic ---
        GameObject primaryObstacleToSpawn = obstaclePrefab1;

        if (Random.value < tallObstacleChance)
        {
            primaryObstacleToSpawn = obstaclePrefab3;
        }

        // Pick random spawn point for the primary obstacle
        int spawnIndex1 = Random.Range(0, totalSpawnPoints);
        Transform spawnPoint1 = obstacleSpawnPointsParent.GetChild(spawnIndex1);
        Instantiate(primaryObstacleToSpawn, spawnPoint1.position, Quaternion.identity, transform);

        // Mark it as occupied
        occupiedIndices[spawnIndex1] = true;

        // If the first obstacle is tall, block its adjacent points
        if (primaryObstacleToSpawn == obstaclePrefab3)
        {
            if (spawnIndex1 - 1 >= 0) occupiedIndices[spawnIndex1 - 1] = true;
            if (spawnIndex1 + 1 < totalSpawnPoints) occupiedIndices[spawnIndex1 + 1] = true;
        }

        // --- Second Obstacle Spawning Logic ---
        // Try to find a free spot for the second obstacle
        int spawnIndex2 = GetRandomUnoccupiedIndex(occupiedIndices);

        if (spawnIndex2 != -1) // -1 means no available spot
        {
            Transform spawnPoint2 = obstacleSpawnPointsParent.GetChild(spawnIndex2);
            Instantiate(obstaclePrefab2, spawnPoint2.position, Quaternion.identity, transform);
            occupiedIndices[spawnIndex2] = true;

            // Optional: If obstaclePrefab2 is also a tall obstacle, block adjacent spots (if needed)
        }

        // --- Third Obstacle Spawning Logic ---
        // Try to find another free spot for the third obstacle
        int spawnIndex3 = GetRandomUnoccupiedIndex(occupiedIndices);

        if (spawnIndex3 != -1)
        {
            Transform spawnPoint3 = obstacleSpawnPointsParent.GetChild(spawnIndex3);
            Instantiate(obstaclePrefab3, spawnPoint3.position, Quaternion.identity, transform);
            occupiedIndices[spawnIndex3] = true;

            // Block adjacent positions for this third obstacle if it's tall
            if (spawnIndex3 - 1 >= 0) occupiedIndices[spawnIndex3 - 1] = true;
            if (spawnIndex3 + 1 < totalSpawnPoints) occupiedIndices[spawnIndex3 + 1] = true;
        }
    }
    public void SpawnCoins()
    {
        // 1) Validate
        if (coinSpawnPointsParent == null || coinSpawnPointsParent.childCount == 0 || coinPrefab == null)
            return;

        int totalPoints = coinSpawnPointsParent.childCount;
        bool[] occupied = new bool[totalPoints];

        // 2) Decide how many coins to spawn this tile: between 1 and 6
        int coinsToSpawn = Random.Range(1, 3);

        // 3) Clamp in case you have fewer than 6 spawn points
        coinsToSpawn = Mathf.Min(coinsToSpawn, totalPoints);

        // 4) Pick that many *unique* spawn points
        for (int i = 0; i < coinsToSpawn; i++)
        {
            int idx = GetRandomUnoccupiedIndex(occupied);
            if (idx == -1) 
                break;  // no more free points

            Transform cp = coinSpawnPointsParent.GetChild(idx);
            Instantiate(coinPrefab, cp.position, Quaternion.identity, transform);
            occupied[idx] = true;
        }
    }

    /// <summary>
    /// Returns a random index where occupied[index] == false, or -1 if none remain.
    /// </summary>
    private int GetRandomUnoccupiedIndex(bool[] occupied)
    {
        var avail = new List<int>();
        for (int i = 0; i < occupied.Length; i++)
            if (!occupied[i]) avail.Add(i);

        if (avail.Count == 0) return -1;
        return avail[Random.Range(0, avail.Count)];
    }
}
