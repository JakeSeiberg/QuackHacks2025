using UnityEngine;
using System.Collections;
public class bomberScript : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 1f;           // Slow patrol/approach speed
    public float chaseSpeed = 5f;          // Fast chase speed when enraged
    
    [Header("Detection Settings")]
    public float detectionRange = 5f;      // Range to detect player
    public float aggroRange = 8f;          // Range to maintain aggro once enraged
    
    [Header("Detonation Settings")]
    public float detonationRange = 1.5f;   // Distance to detonate
    public GameObject bulletPrefab;        // Bullet prefab for explosion
    public int explosionBulletCount = 16;  // Number of bullets in radial burst
    public float bulletSpeed = 8f;         // Speed of explosion bullets
    public float detonationDamage = 30f; 
    
    [Header("Detonation Animation")]
    public Sprite[] detonationSprites;     // Array of 5 sprites for animation
    public float spriteChangeDelay = 0.1f;
    public bool scaleUpDuringDetonation = true; // Enable size increase
    public float finalScale = 2f;          // Final size multiplier (2 = double size)
    public AnimationCurve scaleCurve = AnimationCurve.EaseInOut(0, 1, 1, 1);
    
    private Transform player;
    private PlayerStats playerScript;
    private GameObject playerObject;
    private bool isEnraged = false;
    private bool hasDetonated = false;
    
    private Enemy healthComponent;
    private SpriteRenderer spriteRenderer;
    private Vector3 initialScale;
    private float lastHealth;

    void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        player = playerObject.transform;
        playerScript = playerObject.GetComponent<PlayerStats>();
        
        healthComponent = GetComponent<Enemy>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        initialScale = transform.localScale;
        
        if (player == null)
        {
            Debug.LogError("Player not found! Make sure player has 'Player' tag.");
        }
        
        if (bulletPrefab == null)
        {
            Debug.LogError("Bullet prefab not assigned to suicide bomber!");
        }
        else
        {
            lastHealth = healthComponent.health;
        }
    }

    void Update()
    {
        if (hasDetonated) return;
        
        if (healthComponent != null && healthComponent.health < lastHealth)
        {
            EnrageFromDamage();
            lastHealth = healthComponent.health;
        }
        
        // Room check
        Room parentScript = GetComponentInParent<Room>();
        int RoomID = parentScript.gridIndex;
        if ((playerScript.currentRoomID) != RoomID)
        {
            return;
        }

        if (player == null) return;

        Vector2 toPlayer = player.position - transform.position;
        float distance = toPlayer.magnitude;
        Vector2 direction = toPlayer.normalized;
        if (distance <= detonationRange)
        {
            Detonate();
            return;
        }

        if (isEnraged)
        {
            transform.position += (Vector3)(direction * chaseSpeed * Time.deltaTime);
            
            // Lose aggro if player gets too far away
            if (distance > aggroRange)
            {
                isEnraged = false;
            }
        }
        else
        {
            if (distance <= detectionRange)
            {
                transform.position += (Vector3)(direction * walkSpeed * Time.deltaTime);
            }
        }
    }
    public void EnrageFromDamage()
    {
        if (!isEnraged)
        {
            isEnraged = true;
            Debug.Log($"{gameObject.name} has been enraged!");
        }
    }

    void Detonate()
    {
        if (hasDetonated) return;
        hasDetonated = true;

        Debug.Log($"{gameObject.name} detonating!");
        StartCoroutine(DetonationSequence());
    }
    IEnumerator DetonationSequence()
    {
        // Stop all movement
        enabled = false;
        
        float totalAnimTime = 0f;
        int spriteCount = (detonationSprites != null && detonationSprites.Length > 0) ? detonationSprites.Length : 1;
        float animationDuration = spriteChangeDelay * spriteCount;
        
        // Play through all detonation sprites with scaling
        if (detonationSprites != null && detonationSprites.Length > 0 && spriteRenderer != null)
        {
            for (int i = 0; i < detonationSprites.Length; i++)
            {
                spriteRenderer.sprite = detonationSprites[i];
                
                // Calculate scale based on progress through animation
                if (scaleUpDuringDetonation)
                {
                    float progress = (float)i / (detonationSprites.Length - 1);
                    float scaleMultiplier = Mathf.Lerp(1f, finalScale, scaleCurve.Evaluate(progress));
                    transform.localScale = initialScale * scaleMultiplier;
                }
                
                yield return new WaitForSeconds(spriteChangeDelay);
                totalAnimTime += spriteChangeDelay;
            }
        }
        

        // Fire radial burst of bullets
        //ShootRadial(explosionBulletCount);

        // Deal direct damage to player if very close (inside detonation range)
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer <= detonationRange && playerScript != null)
        {
            playerScript.TakeDamage((int)detonationDamage);
        }

        // Destroy this enemy
        Destroy(gameObject);
    }
    /*
    void ShootRadial(int bulletCount)
    {
        for (int i = 0; i < bulletCount; i++)
        {
            float angle = i * (360f / bulletCount);
            Vector2 dir = new Vector2(
                Mathf.Cos(angle * Mathf.Deg2Rad),
                Mathf.Sin(angle * Mathf.Deg2Rad)
            );

            FireBullet(dir);
        }
    }

    void FireBullet(Vector2 direction)
    {
        if (bulletPrefab == null) return;
        
        GameObject b = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Rigidbody2D rb = b.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direction * bulletSpeed;
        }
    }
    */

}