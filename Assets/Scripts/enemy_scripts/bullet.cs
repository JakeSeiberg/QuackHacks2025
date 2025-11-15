using UnityEngine;
public class bullet : MonoBehaviour
{
    public float damage = 10f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();

        if (enemy != null)
            enemy.TakeDamage(damage);

        Destroy(gameObject);
    }
}

