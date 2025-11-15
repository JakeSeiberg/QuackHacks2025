using UnityEngine;

public class enemyTargeting : MonoBehaviour
{
    private GameObject player;
    private RigidBody2D rb;
    public float bulletspeed;
    public float lifetime; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<RigidBody2D>();
        player = GameObject.FindGameObjectWithTag("Player");

        Vector3 direction = player.transform.position - tranform.position;
        rb.linearvelo = new Vector2(direction.x, direction.y).normalized * bulletspeed;
        float rot = Mathf.Atan2(-directory.y,-direction.x) * Mathf.Rad2Deg;
        transform.rotation= Quaternion.Euler(0,0,rot+90);
        Destroy(GameObject, lifetime);        
    }
    void OnTriggerEnter2D(Collider2D other){
        if (other.CompareTag("Player"))
        {
            Debug.Log("Hit Player!");
        }

        Destory(gameObject);
    }
        
}
