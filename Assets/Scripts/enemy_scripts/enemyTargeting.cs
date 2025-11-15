using UnityEngine;
using Unity;

public class enemyTargeting : MonoBehaviour
{
    private GameObject player;
    private Rigidbody2D rb;
    public float bulletspeed;
    public float lifetime; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");

        Vector3 direction = player.transform.position - transform.position;
        rb.linearVelocity = new Vector2(direction.x, direction.y).normalized * bulletspeed;
        float rot = Mathf.Atan2(-direction.y,-direction.x) * Mathf.Rad2Deg;
        transform.rotation= Quaternion.Euler(0,0,rot+90);      
    }
        
}
