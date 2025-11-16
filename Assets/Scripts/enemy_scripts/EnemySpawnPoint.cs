using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] [Range(0f, 1f)] private float spawnChance = 0.1f; // 10% chance

    void Start()
    {
        // Generate random number between 0 and 1
        float randomValue = Random.value;
        
        // Check if random value is within spawn chance
        if (randomValue <= spawnChance)
        {
            // Instantiate enemy at this GameObject's position and rotation
            Instantiate(enemyPrefab, transform.position, transform.rotation);
            Debug.Log("Enemy spawned at: " + transform.position);
        }
        else
        {
            Debug.Log("Enemy did not spawn (rolled " + randomValue + ")");
        }
    }
}