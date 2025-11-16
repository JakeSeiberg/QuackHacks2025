using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_basicMovement : MonoBehaviour
{
    public float rushSpeed = 3f;
    public float strafeSpeed = 2f;
    public float maxStrafeDistance = 4f;    // Too far - rush back in
    public float enterStrafeDistance = 3f;  // Start strafing at this distance

    private Transform player;
    private bool isStrafing = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        
        if (player == null)
        {
            Debug.LogError("Player not found! Make sure player has 'Player' tag.");
        }
    }

    void Update()
    {
        if (player == null) return;

        Vector2 toPlayer = player.position - transform.position;
        float distance = toPlayer.magnitude;
        Vector2 direction = toPlayer.normalized;

        float rushRange = 10f;  

        if (!isStrafing)
        {

            if (distance <= rushRange){
            // ---- RUSH MODE ----
            transform.position += (Vector3)(direction * rushSpeed * Time.deltaTime);
          }

            // Enter strafe mode when in range
            if (distance <= enterStrafeDistance)
            {
                isStrafing = true;
            }
        }
        else
        {
            // ---- STRAFE MODE ----
            
            // Exit strafe if too far or too close
            if (distance >= maxStrafeDistance)
            {
                isStrafing = false;
                return;
            }

            // Strafe perpendicular to player
            Vector2 perpendicular = new Vector2(-direction.y, direction.x);
            transform.position += (Vector3)(perpendicular * strafeSpeed * Time.deltaTime);
        }
        //Debug.Log($"Distance: {distance:F2}, Strafing: {isStrafing}");
    }
}