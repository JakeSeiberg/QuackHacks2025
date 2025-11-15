using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_BasicMov : MonoBehaviour
{
    public GameObject player;
    public float speed;
    public float distanceBetween;

    private float distance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
      distance = Vector2.Distance(transform.position, player.transform.position);
      Vector2 direction = player.transform.position - transform.position;
      if (distance < distanceBetween)
      {
        transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
    }

    }
}