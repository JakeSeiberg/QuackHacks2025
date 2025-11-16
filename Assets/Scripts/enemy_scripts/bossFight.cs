using UnityEngine;

public class BossFight : MonoBehaviour
{
    [Header("Boss Settings")]
    public float phaseTime = 6f; 
    public float moveSpeed = 2f;
    public Transform player;

    [Header("Bullet Settings")]
    public GameObject bulletPrefab;
    public float bulletSpeed = 12f;
    public float fireCooldown = 0.5f;
    private float fireTimer;

    private int currentPhase = 0;
    private float phaseTimer;

    void Start()
    {
        phaseTimer = phaseTime;
    }

    void Update()
    {
        HandlePhases();
        HandleAttacks();
    }

    void HandlePhases()
    {
        phaseTimer -= Time.deltaTime;

        if (phaseTimer <= 0f)
        {
            currentPhase++;
            phaseTimer = phaseTime;

            // loop phases
            if (currentPhase > 2)
                currentPhase = 0;
        }
    }

    void HandleAttacks()
    {
        fireTimer -= Time.deltaTime;

        if (fireTimer > 0f)
            return;

        fireTimer = fireCooldown;

        switch (currentPhase)
        {
            case 0:
                ShootTowardPlayer();
                break;

            case 1:
                ShootRadial(8); 
                break;

            case 2:
                ShootSpray(5, 10f);
                break;
        }
    }


    void ShootTowardPlayer()
    {
        if (!player) return;

        Vector2 dir = (player.position - transform.position).normalized;
        FireBullet(dir);
    }

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

    void ShootSpray(int count, float angleSpread)
    {
        if (!player) return;

        Vector2 baseDir = (player.position - transform.position).normalized;
        float baseAngle = Mathf.Atan2(baseDir.y, baseDir.x) * Mathf.Rad2Deg;

        for (int i = 0; i < count; i++)
        {
            float offset = Random.Range(-angleSpread, angleSpread);
            float newAngle = baseAngle + offset;

            Vector2 dir = new Vector2(
                Mathf.Cos(newAngle * Mathf.Deg2Rad),
                Mathf.Sin(newAngle * Mathf.Deg2Rad)
            );

            FireBullet(dir);
        }
    }

    void FireBullet(Vector2 direction)
    {
        GameObject b = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Rigidbody2D rb = b.GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction * bulletSpeed;
    }
}
