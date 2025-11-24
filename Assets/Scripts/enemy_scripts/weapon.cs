using UnityEngine;

public class weapon : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireForce = 15f;

    public void Fire()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        Vector2 direction = ((Vector2)mousePos - (Vector2)firePoint.position).normalized;

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(direction * fireForce, ForceMode2D.Impulse);
    
    }   

    public void enemyFire()
    {
        GameObject gun = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = gun.GetComponent<Rigidbody2D>();
        
        // Find the player and shoot directly at them
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            Vector2 direction = (playerObj.transform.position - firePoint.position).normalized;
            
            if (gun.CompareTag("sniper")){
                rb.linearVelocity = direction * 13;
            }
            else{
                rb.linearVelocity = direction * fireForce;
            }
        }
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("here");
        Destroy(gameObject);
    }
}