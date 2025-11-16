using UnityEngine;
public class enemyShooting : MonoBehaviour
{
    public weapon weapon;   // drag the weapon component here
    public float fireRate = 1f;
    private float nextFireTime = 0f;

    private void Update()
    {
        if (Time.time >= nextFireTime)
        {
            weapon.enemyFire();
            nextFireTime = Time.time + 1f / fireRate;
        }
    }
}