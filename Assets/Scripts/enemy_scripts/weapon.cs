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
    mousePos.z = 0;

    Vector2 direction = ((Vector2)mousePos - (Vector2)firePoint.position).normalized;

    Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
    rb.AddForce(direction * fireForce, ForceMode2D.Impulse);
}
}