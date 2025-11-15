using UnityEngine;

public class weapon : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireForce = 20f;

    public void Fire()
    {
      GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
      Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
      rb.linearVelocity = firePoint.right * fireForce;
    }
}