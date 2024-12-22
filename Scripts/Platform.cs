using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public Transform PosA;
    public Transform PosB;
    public float platformspeed;
    private Vector2 initialPosA;
    private Vector2 initialPosB;
    public Player2 player;
    Rigidbody2D platformRigidbody;
    Vector3 moveDirection;
    
    
     
    private Vector3 targetPosition;
    void Start()
    {
        platformRigidbody = GetComponent<Rigidbody2D>();
        initialPosA = PosA.position;
        initialPosB = PosB.position;
        targetPosition = PosA.position;
        direction();
    }

    // Update is called once per frame
    void Update()
    {
        platformRigidbody.velocity = moveDirection * platformspeed;
        if (Vector2.Distance(transform.position, initialPosA) < 0.1f)
        {
            targetPosition = initialPosB;
            direction();
        }
        if (Vector2.Distance(transform.position, initialPosB) < 0.1f)
        {
            targetPosition = initialPosA;
            direction();
        }
        
    }
    void direction()
    {
        moveDirection = (targetPosition- transform.position).normalized;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
           player.isOnPlatform = true; 
           player.platformRigidbody = platformRigidbody;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.isOnPlatform=false;
            

        }

    }
}
