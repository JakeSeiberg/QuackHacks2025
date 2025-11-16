using UnityEngine;
public class enemyShooting : MonoBehaviour
{
    public weapon weapon;   // drag the weapon component here
    public float fireRate = 1f;
    private float nextFireTime = 0f;
    public Transform player;
    public float range = 12f;

    private void Update(){

    if (!player) return;

    float distance = Vector2.Distance(transform.position, player.position);

    // Player too far → don't fire
    if (distance > range)
        return;

    // Player close enough → checking fire cooldown
    if (Time.time >= nextFireTime)
    {
        weapon.enemyFire();
        nextFireTime = Time.time + 1f / fireRate;
    }
}
}