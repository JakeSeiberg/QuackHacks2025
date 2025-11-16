using UnityEngine;

public class bullet : MonoBehaviour{
    public float damage = 10f;
private void OnTriggerEnter2D(Collider2D other)
{
<<<<<<< HEAD
    PlayerMovement enemy = other.GetComponent<PlayerMovement>();
=======
    Enemy enemy = other.GetComponent<Enemy>();

>>>>>>> 200859caa6c9d2a515eee7f8e1c79425ab72b4e6
    if (enemy != null)
    {
        enemy.TakeDamage(damage);
        Destroy(gameObject); 
    }

    if (other.CompareTag("wall"))
    {
        Destroy(gameObject);
    }


}
}
