using UnityEngine;
using System.Collections;
using UnityEditor;
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5f;
    
    [Header("Dash Settings")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    public LayerMask collisionLayers; // Set this to include your tilemap layer
    public float dashCheckDistance = 1f; // How far ahead to check for walls
    
    [Header("Dash Ghost Effect")]
    public float ghostSpawnInterval = 0.05f;
    public float ghostFadeDuration = 0.5f;
    public Color ghostColor = new Color(1f, 1f, 1f, 0.5f);
    
    private bool isDashing = false;
    private float dashTimeRemaining;
    private float dashCooldownRemaining;
    private Vector2 dashDirection;
    private float ghostSpawnTimer;
    private SpriteRenderer spriteRenderer;
    private Collider2D playerCollider;
    private PlayerAnimation animator;
    private PlayerAnimation.AnimationDirection lastDirection = PlayerAnimation.AnimationDirection.Down;
    
    public weapon playerWeapon;
    public PlayerAudioController audioController;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogWarning("PlayerMovement: No SpriteRenderer found! Ghost effect requires a SpriteRenderer.");
        }
        
        playerCollider = GetComponent<Collider2D>();
        if (playerCollider == null)
        {
            Debug.LogWarning("PlayerMovement: No Collider2D found! Collision toggling requires a Collider2D.");
        }
        
        animator = GetComponent<PlayerAnimation>();
        if (animator == null)
        {
            Debug.LogWarning("PlayerMovement: No PlayerAnimation found! Add PlayerAnimation component for animations.");
        }
    }
    
    
    private float reloadCooldown = 1f;
    private float reloadTimeRemaining = 0f;

    void Update()
    {
        // Countdown reload timer
        if (reloadTimeRemaining > 0)
        {
            reloadTimeRemaining -= Time.deltaTime;
        }
        
        if (Input.GetButtonDown("Fire1"))
        {
            // Only allow firing if not currently reloading
            if (reloadTimeRemaining <= 0)
            {
                if (PlayerStats.Instance.currentAmmo > 0)
                {
                    playerWeapon.Fire();
                    PlayerStats.Instance.UseAmmo();
                    audioController.PlayGunFire();
                }
                else
                {
                    PlayerStats.Instance.Reload();
                    audioController.PlayReload();
                    reloadTimeRemaining = reloadCooldown; // Start reload cooldown
                }
            }
        }
        
        if (dashCooldownRemaining > 0)
        {
            dashCooldownRemaining -= Time.deltaTime;
        }
        
        if (isDashing)
        {
            HandleDash();
        }
        else
        {
            HandleMovement();
            if (Input.GetKeyDown(KeyCode.Space) && dashCooldownRemaining <= 0)
            {
                StartDash();
            }
        }
    }
    
    void HandleMovement()
    {
        Vector3 pos = transform.position;
        bool isMoving = false;
        PlayerAnimation.AnimationDirection moveDirection = lastDirection;
        
        // Handle horizontal movement
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            pos.x += speed * Time.deltaTime;
            moveDirection = PlayerAnimation.AnimationDirection.Right;
            isMoving = true;
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            pos.x -= speed * Time.deltaTime;
            moveDirection = PlayerAnimation.AnimationDirection.Left;
            isMoving = true;
        }
        
        // Handle vertical movement (allows diagonal)
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            pos.y += speed * Time.deltaTime;
            // Only change animation to Up if not moving horizontally
            if (!isMoving)
            {
                moveDirection = PlayerAnimation.AnimationDirection.Up;
            }
            isMoving = true;
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            pos.y -= speed * Time.deltaTime;
            // Only change animation to Down if not moving horizontally
            if (!isMoving)
            {
                moveDirection = PlayerAnimation.AnimationDirection.Down;
            }
            isMoving = true;
        }
        
        // Update animator
        if (animator != null)
        {
            if (isMoving)
            {
                lastDirection = moveDirection;
                animator.SetDirectionAndState(moveDirection, PlayerAnimation.AnimationState.Walking);
            }
            else
            {
                animator.SetDirectionAndState(lastDirection, PlayerAnimation.AnimationState.Idle);
            }
        }
        
        transform.position = pos;
    }
    
    void StartDash()
    {
        Vector2 inputDir = Vector2.zero;
        
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            inputDir.x = 1;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            inputDir.x = -1;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            inputDir.y = 1;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            inputDir.y = -1;
        
        if (inputDir == Vector2.zero)
        {
            // Dash in the direction player is facing
            switch (lastDirection)
            {
                case PlayerAnimation.AnimationDirection.Right:
                    inputDir = Vector2.right;
                    break;
                case PlayerAnimation.AnimationDirection.Left:
                    inputDir = Vector2.left;
                    break;
                case PlayerAnimation.AnimationDirection.Up:
                    inputDir = Vector2.up;
                    break;
                case PlayerAnimation.AnimationDirection.Down:
                    inputDir = Vector2.down;
                    break;
            }
        }
        
        dashDirection = inputDir.normalized;
        isDashing = true;
        dashTimeRemaining = dashDuration;
        dashCooldownRemaining = dashCooldown;
        ghostSpawnTimer = 0;
        
        if (playerCollider != null)
        {
            playerCollider.enabled = false;
        }
    }
    
    void HandleDash()
    {
        dashTimeRemaining -= Time.deltaTime;
        ghostSpawnTimer -= Time.deltaTime;
        
        // Spawn ghost effect
        if (ghostSpawnTimer <= 0)
        {
            SpawnDashGhost();
            ghostSpawnTimer = ghostSpawnInterval;
        }
        
        if (dashTimeRemaining <= 0)
        {
            EndDash();
            return;
        }
        
        Vector3 dashMovement = dashDirection * dashSpeed * Time.deltaTime;
        Vector3 targetPosition = transform.position + dashMovement;
        
        // Check for wall collision before moving
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dashDirection, dashMovement.magnitude, collisionLayers);
        
        if (hit.collider != null)
        {
            // Wall detected, stop the dash
            EndDash();
            return;
        }
        
        transform.position = targetPosition;
    }
    
    void EndDash()
    {
        isDashing = false;
        if (playerCollider != null)
        {
            playerCollider.enabled = true;
        }
    }
    
    void SpawnDashGhost()
    {
        if (spriteRenderer == null) return;
        
        GameObject ghost = new GameObject("DashGhost");
        ghost.transform.position = transform.position;
        ghost.transform.rotation = transform.rotation;
        ghost.transform.localScale = transform.localScale;
        
        SpriteRenderer ghostSprite = ghost.AddComponent<SpriteRenderer>();
        ghostSprite.sprite = spriteRenderer.sprite;
        ghostSprite.color = ghostColor;
        ghostSprite.sortingLayerName = spriteRenderer.sortingLayerName;
        ghostSprite.sortingOrder = spriteRenderer.sortingOrder - 1;
        
        StartCoroutine(FadeGhost(ghostSprite, ghost));
    }
    
    IEnumerator FadeGhost(SpriteRenderer ghostSprite, GameObject ghost)
    {
        float elapsed = 0f;
        Color startColor = ghostSprite.color;
        
        while (elapsed < ghostFadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(startColor.a, 0f, elapsed / ghostFadeDuration);
            ghostSprite.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }
        
        Destroy(ghost);
    }
}