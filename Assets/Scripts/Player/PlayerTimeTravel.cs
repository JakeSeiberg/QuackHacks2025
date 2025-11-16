using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerTimeTravel : MonoBehaviour
{
    private float teleportCooldown = 0.5f;
    private float lastTeleportTime = -0.5f;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (Time.time >= lastTeleportTime + teleportCooldown)
            {
                TeleportPlayer();
                lastTeleportTime = Time.time;
            }
        }
    }
    
    void TeleportPlayer()
    {
        if (PlayerStats.Instance.inFuture)
        {
            Vector3 tmpPos = gameObject.transform.position;
            tmpPos.x += 100;
            gameObject.transform.position = tmpPos;
            PlayerStats.Instance.inFuture = false;
        }
        else
        {
            Vector3 tmpPos = gameObject.transform.position;
            tmpPos.x -= 100;
            gameObject.transform.position = tmpPos;
            PlayerStats.Instance.inFuture = true;
        }
    }
}