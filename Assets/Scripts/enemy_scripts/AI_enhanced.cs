using UnityEngine;

public class AI_Enhanced : MonoBehaviour
{
    public Transform player;
    public float speed = 3f;
    public float avoidanceDistance = 2f;
    public LayerMask obstacleLayer;

    void Update()
    {
        if (player == null) return;

        Vector2 toPlayer = (player.position - transform.position).normalized;
        
        // Check for obstacles ahead
        RaycastHit2D hit = Physics2D.Raycast(transform.position, toPlayer, avoidanceDistance, obstacleLayer);
        
        if (hit.collider != null)
        {
            // Obstacle detected - try to go around
            Vector2 avoidDirection = Vector2.Perpendicular(toPlayer);
            
            // Check which side is clear
            bool leftClear = !Physics2D.Raycast(transform.position, avoidDirection, avoidanceDistance, obstacleLayer);
            bool rightClear = !Physics2D.Raycast(transform.position, -avoidDirection, avoidanceDistance, obstacleLayer);
            
            if (leftClear)
                toPlayer = (toPlayer + avoidDirection).normalized;
            else if (rightClear)
                toPlayer = (toPlayer - avoidDirection).normalized;
        }
        
        transform.position += (Vector3)toPlayer * speed * Time.deltaTime;
    }
}