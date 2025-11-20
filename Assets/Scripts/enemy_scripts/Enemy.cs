using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health = 20f;

    public int myRoom;

    public void Start()
    {
        Room parentScript = GetComponentInParent<Room>();
        myRoom = parentScript.gridIndex;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}