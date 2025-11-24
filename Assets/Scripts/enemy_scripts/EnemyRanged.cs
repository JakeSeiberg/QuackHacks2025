using UnityEngine;

public class EnemyRanged : MonoBehaviour
{
    public Transform player;

    public float desiredDistance = 6f;    
    public float moveSpeed = 3f;      
    public float strafeSpeed = 2f;      
    public float rotateSpeed = 360f;   
    public float range = 12f;    

    public GameObject projectilePrefab;
    public float shootCooldown = 2f; 
    void Start()
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }
       

    void Update()
    {
        if (!player) return;


     
        float distance = Vector2.Distance(transform.position, player.position);
        Vector2 direction = (player.position - transform.position).normalized;
        if (distance > range)
        {
            Debug.Log("hit");
            return;
        }

       
        else if (distance > desiredDistance + 0.5f)
        {
       
            transform.position += (Vector3)(direction * moveSpeed * Time.deltaTime);
        }
        else if (distance < desiredDistance - 0.5f)
        {
    
            transform.position -= (Vector3)(direction * moveSpeed * Time.deltaTime);
        }
        else
        {
           
            Vector3 perpendicular = Vector3.Cross(direction, Vector3.forward); 
            transform.position += perpendicular * strafeSpeed * Time.deltaTime;
        }

        
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        float newAngle = Mathf.MoveTowardsAngle(transform.eulerAngles.z, targetAngle, rotateSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0, 0, newAngle);

    }
    }