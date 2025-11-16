using UnityEngine;

public class weapon : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireForce = 20f;

    public void Fire()
{
    GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    Vector2 direction = (mousePos - firePoint.position).normalized;

    Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
    rb.linearVelocity = direction * fireForce;
}
}