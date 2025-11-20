using System.Collections;
using UnityEngine;

public class DoorTeleport : MonoBehaviour
{
    [Header("Door Configuration")]
    public EdgeDirection2 direction;
    public Cell.RoomType originRoomType;
    public Cell.RoomType neighborRoomType;
    
    [Header("Teleport Settings")]
    public float teleportDistanceHorizontal = 2f; // How far to teleport player past the door (left/right)
    public float teleportDistanceVertical = 2f; // How far to teleport player past the door (up/down)
    public float activationRadius = 1f; // Distance to check for player
    public float cooldownTime = 1f; // Prevent rapid back-and-forth teleporting
    public LayerMask playerLayer; // Assign player layer in inspector
    
    private bool canTeleport = true;
    private Transform playerTransform;
    private static bool isAnyDoorTeleporting = false; // Global lock
    private static DoorTeleport lastUsedDoor = null; // Track last door used
    private void Update()
    {
        if (!canTeleport || isAnyDoorTeleporting) return;
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;
        
        float distance = Vector3.Distance(transform.position, player.transform.position);
        
        if (distance < activationRadius && lastUsedDoor != this)
        {
            TeleportPlayer(player.transform);
        }
    }
    
    private void TeleportPlayer(Transform player)
    {
        // Set locks immediately
        isAnyDoorTeleporting = true;
        canTeleport = false;
        lastUsedDoor = this;
        playerTransform = player;
        
        // Determine which side of the door the player is on
        Vector3 playerToDoor = transform.position - player.position;
        Vector3 teleportOffset = GetSmartTeleportOffset(playerToDoor);
        Vector3 newPosition = transform.position + teleportOffset;
        
        
        // Stop player dash if they're dashing
        var playerMovement = player.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            // Force end the dash
            playerMovement.SendMessage("EndDash", SendMessageOptions.DontRequireReceiver);
        }
        
        // Stop player movement if they have a Rigidbody2D
        Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
        if (playerRb != null)
        {
            playerRb.linearVelocity = Vector2.zero;
        }
        
        // Teleport the player
        player.position = newPosition;
        
        // Start cooldown to prevent immediate re-teleport
        StartCoroutine(TeleportCooldown());
    }
    
    private Vector3 GetSmartTeleportOffset(Vector3 playerToDoor)
    {
        // Determine which side of the door the player is approaching from
        // and teleport them to the opposite side
        
        switch (direction)
        {
            case EdgeDirection2.Up:
            case EdgeDirection2.Down:
                // Door is horizontal (connects rooms vertically)
                // Check if player is above or below the door
                if (playerToDoor.y > 0)
                {
                    // Player is below, teleport them up
                    PlayerStats.Instance.currentRoomID -= 10;

                    return new Vector3(0, teleportDistanceVertical, 0);

                }
                else
                {
                    // Player is above, teleport them down
                    PlayerStats.Instance.currentRoomID += 10;
                    return new Vector3(0, -teleportDistanceVertical, 0);
                }
                
            case EdgeDirection2.Left:
            case EdgeDirection2.Right:
                // Door is vertical (connects rooms horizontally)
                // Check if player is left or right of the door
                if (playerToDoor.x > 0)
                {
                    // Player is on the left, teleport them right
                    PlayerStats.Instance.currentRoomID += 1;
                    return new Vector3(teleportDistanceHorizontal, 0, 0);

                }
                else
                {
                    // Player is on the right, teleport them left
                    PlayerStats.Instance.currentRoomID -= 1;
                    return new Vector3(-teleportDistanceHorizontal, 0, 0);
                }
                
            default:
                return Vector3.zero;
        }
    }
    
    private Vector3 GetTeleportOffset()
    {
        // Teleport player to the opposite side of the door
        switch (direction)
        {
            case EdgeDirection2.Up:
                return new Vector3(0, teleportDistanceVertical, 0);

            case EdgeDirection2.Down:
                PlayerStats.Instance.currentRoomID += 10;
                return new Vector3(0, -teleportDistanceVertical, 0);
            case EdgeDirection2.Left:
                PlayerStats.Instance.currentRoomID -= 1;
                return new Vector3(-teleportDistanceHorizontal, 0, 0);
            case EdgeDirection2.Right:
                PlayerStats.Instance.currentRoomID += 1;
                return new Vector3(teleportDistanceHorizontal, 0, 0);
            default:
                return Vector3.zero;
        }
    }
    
    private IEnumerator TeleportCooldown()
    {
        yield return new WaitForSeconds(cooldownTime);
        
        // Additional check: only re-enable if player has moved away from door
        if (playerTransform != null)
        {
            float distanceFromDoor = Vector3.Distance(transform.position, playerTransform.position);
            if (distanceFromDoor > activationRadius * 1.5f)
            {
                canTeleport = true;
                isAnyDoorTeleporting = false;
            }
            else
            {
                // Wait longer if player is still too close
                yield return new WaitForSeconds(cooldownTime * 0.5f);
                canTeleport = true;
                isAnyDoorTeleporting = false;
            }
        }
        else
        {
            canTeleport = true;
            isAnyDoorTeleporting = false;
        }
        
        // Clear last used door after enough time has passed
        yield return new WaitForSeconds(0.5f);
        if (lastUsedDoor == this)
        {
            lastUsedDoor = null;
        }
    }
    
    public void SetDoorType(Cell.RoomType origin, Cell.RoomType neighbor, EdgeDirection2 dir)
    {
        originRoomType = origin;
        neighborRoomType = neighbor;
        direction = dir;
        
        // Optional: Rotate door sprite based on direction
        RotateDoorVisual();
    }
    
    private void RotateDoorVisual()
    {
        // Rotate the door's visual representation to match its direction
        switch (direction)
        {
            case EdgeDirection2.Up:
                transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case EdgeDirection2.Down:
                transform.rotation = Quaternion.Euler(0, 0, 180);

                break;
            case EdgeDirection2.Left:
                transform.rotation = Quaternion.Euler(0, 0, 90);

                break;
            case EdgeDirection2.Right:
                transform.rotation = Quaternion.Euler(0, 0, -90);
                break;
        }
    }
    
    private void OnDrawGizmos()
    {
        // Visualize activation radius
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, activationRadius);
        
        // Visualize teleport direction
        Gizmos.color = Color.cyan;
        Vector3 offset = GetTeleportOffset();
        Gizmos.DrawLine(transform.position, transform.position + offset);
        Gizmos.DrawWireSphere(transform.position + offset, 0.3f);
    }
}

// Make sure these enums match your existing code
public enum EdgeDirection2
{
    Up,
    Down,
    Left,
    Right
}

public enum RoomType
{
    Normal,
    Boss,
    // Add other room types as needed
}