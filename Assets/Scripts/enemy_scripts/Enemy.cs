using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health = 20f;

    public int myRoom;
    private PlayerStats playerScript;
    private GameObject playerObject;
    private BossFight bossScript; 

    public void Start()
    {
        var parentScript = GetComponentInParent<Room>();
        int myRoom = parentScript.gridIndex;

        playerObject = GameObject.FindGameObjectWithTag("Player");
        playerScript = playerObject.GetComponent<PlayerStats>();
        
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0 && (GetComponent<BossFight>() != null))
        {   
            bossScript = GetComponent<BossFight>();
            MapGenerator.instance.SetupDungeon();
            playerObject.transform.position = new Vector2(2, 5);
            playerScript.currentRoomID = 45;

            Destroy(gameObject);
        }
        else if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}