using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] [Range(0f, 1f)] private float spawnChance = 0.1f; // 10% chance
    public int myRoom;

    void Start()
    {
        // Generate random number between 0 and 1
        float randomValue = Random.value;
        
        Room parentScript = GetComponentInParent<Room>();
        myRoom = parentScript.gridIndex;

        // Check if random value is within spawn chance
        if (randomValue <= spawnChance)
        {
            // Instantiate enemy at this GameObject's position and rotation
            GameObject newEnemy = Instantiate(enemyPrefab, transform.position, transform.rotation);
            Debug.Log("Enemy spawned at: " + transform.position);
            Enemy enemyScript = newEnemy.GetComponent<Enemy>();

            if (enemyScript != null)
            {
                enemyScript.myRoom = myRoom;
            }
        }
        else
        {
            Debug.Log("Enemy did not spawn (rolled " + randomValue + ")");
        }
    }
}