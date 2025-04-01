using UnityEngine;

public class GroundTile : MonoBehaviour {

    GroundSpawner groundSpawner;
    [SerializeField] GameObject obstaclePrefab;
    [SerializeField] GameObject obstaclePrefab2;
    [SerializeField] GameObject coinPrefab;
    [SerializeField] GameObject tallObstaclePrefab;
    [SerializeField] float tallObstacleChance = 0.1f;

    private void Start () 
    {
        groundSpawner = GameObject.FindObjectOfType<GroundSpawner>();   
	}

    private void OnTriggerExit (Collider other)
    {
        groundSpawner.SpawnTile(true);
        Destroy(gameObject ,1);
    }
    
    public void SpawnObstacle()
    {
        //choose which obstacle to spawn
        GameObject obstacleToSpawn = obstaclePrefab;
        float random = Random.Range(0f,1f);
        if (random < tallObstacleChance)
        {
            obstacleToSpawn = tallObstaclePrefab;
        }
        int obstacleSpawnIndex = Random.Range(2, 6);
        Transform spawnPoint = transform.GetChild(obstacleSpawnIndex).transform;
        Instantiate(obstaclePrefab , spawnPoint.position , Quaternion.identity, transform);
        int obstacleSpawnIndex1 = Random.Range(6, 8);
        Transform spawnPoint1 = transform.GetChild(obstacleSpawnIndex1).transform;
        Instantiate(obstaclePrefab2 , spawnPoint1.position , Quaternion.identity, transform);
        Instantiate(tallObstaclePrefab , spawnPoint1.position , Quaternion.identity, transform);
    }
    public void SpawnCoins()
    {
        int coinstospawn = 2;
        for(int i = 0; i < coinstospawn; i++)
        {
            GameObject temp = Instantiate(coinPrefab, transform);
            temp.transform.position = GetRandomPointInCollider(GetComponent<Collider>());

        }
    }
    Vector3 GetRandomPointInCollider (Collider collider)
    {
        Vector3 point = new Vector3
        (
            Random.Range(collider.bounds.min.x , collider.bounds.max.x),
            Random.Range(collider.bounds.min.y , collider.bounds.max.y),
            Random.Range(collider.bounds.min.z , collider.bounds.max.z)
        );
        if (point != collider.ClosestPoint(point))
        {
            point = GetRandomPointInCollider(collider);
        }
        point.y = 1;
        return point;
    }
}
