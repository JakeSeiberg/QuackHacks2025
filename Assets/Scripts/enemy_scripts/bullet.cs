using UnityEngine;

public class bullet : MonoBehaviour{
    public float damage = 10f;
private void OnTriggerEnter2D(Collider2D other)
{
    PlayerMovement enemy = other.GetComponent<PlayerMovement>();
    if (enemy != null)
    {
        enemy.TakeDamage(damage);
        Destroy(gameObject); 
    }

}
}
