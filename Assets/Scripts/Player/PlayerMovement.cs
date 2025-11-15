using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5f;
    
    [Header("Dash Settings")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    
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

    public weapon playerWeapon;
    
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
    }
    
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
        playerWeapon.Fire();
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
        
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            pos.x += speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            pos.x -= speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            pos.y += speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            pos.y -= speed * Time.deltaTime;
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
            inputDir = Vector2.right; // Default dash direction, can change in future
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
            isDashing = false;
            if (playerCollider != null)
            {
                playerCollider.enabled = true;
            }
            return;
        }
        
        Vector3 dashMovement = dashDirection * dashSpeed * Time.deltaTime;
        transform.position += dashMovement;
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
        ghostSprite.sortingOrder = spriteRenderer.sortingOrder;
        
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