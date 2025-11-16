using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [Header("References")]
    public PlayerStats playerStats;
    
    [Header("Damage Settings")]
    public int bulletDamage = 1;
    
    [Header("Invincibility Settings")]
    public float invincibilityDuration = 1f;
    public bool useInvincibility = true;
    
    private float invincibilityTimer = 0f;
    private SpriteRenderer spriteRenderer;
    
    void Start()
    {
        // Get PlayerStats if not assigned
        if (playerStats == null)
        {
            playerStats = GetComponent<PlayerStats>();
            if (playerStats == null)
            {
                Debug.LogError("PlayerDamageHandler: No PlayerStats found! Please assign or add PlayerStats component.");
            }
        }
        
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    void Update()
    {
        // Update invincibility timer
        if (invincibilityTimer > 0)
        {
            invincibilityTimer -= Time.deltaTime;
            
            // Optional: Flash sprite during invincibility
            if (spriteRenderer != null && useInvincibility)
            {
                float alpha = Mathf.PingPong(Time.time * 10f, 1f);
                spriteRenderer.color = new Color(1f, 1f, 1f, alpha);
            }
        }
        else
        {
            // Reset sprite color when invincibility ends
            if (spriteRenderer != null)
            {
                spriteRenderer.color = Color.white;
            }
        }
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object is on the "enemy_bullet" layer
        if (collision.gameObject.layer == LayerMask.NameToLayer("enemy_bullet"))
        {
            // Only take damage if not currently invincible
            if (!useInvincibility || invincibilityTimer <= 0)
            {
                TakeDamageFromBullet(collision.gameObject);
            }
        }
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the object is on the "enemy_bullet" layer
        if (collision.gameObject.layer == LayerMask.NameToLayer("enemy_bullet"))
        {
            // Only take damage if not currently invincible
            if (!useInvincibility || invincibilityTimer <= 0)
            {
                TakeDamageFromBullet(collision.gameObject);
            }
        }
    }
    
    void TakeDamageFromBullet(GameObject bullet)
    {
        if (playerStats != null)
        {
            // Call TakeDamage on PlayerStats
            playerStats.TakeDamage(bulletDamage);
            
            // Start invincibility period
            if (useInvincibility)
            {
                invincibilityTimer = invincibilityDuration;
            }
            
            // Destroy the bullet
            Destroy(bullet);
            
            // Optional: Add hit feedback here
            // Example: PlayHitSound(), SpawnHitEffect(), etc.
        }
    }
    
    public bool IsInvincible()
    {
        return useInvincibility && invincibilityTimer > 0;
    }
}