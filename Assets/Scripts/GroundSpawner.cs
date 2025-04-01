
using UnityEngine;

public class GroundSpawner : MonoBehaviour
{
    [SerializeField] GameObject groundtile;
    Vector3 nextSpawnPoint;
    // Start is called before the first frame update
    public void SpawnTile(bool spawnItems)
    {
        GameObject temp = Instantiate(groundtile, nextSpawnPoint, Quaternion.identity);
        nextSpawnPoint = temp.transform.GetChild(1).transform.position;
        if(spawnItems)
        {
            temp.GetComponent<GroundTile>().SpawnObstacle();
            temp.GetComponent<GroundTile>().SpawnCoins();
        }
    }
    private void Start()
    {
        for(int i = 0;i < 15; i++)
        {
            if (i<2)
            {
                SpawnTile(false);
            }
            else 
            {
                SpawnTile(true);
            }    
        }
    }
}
