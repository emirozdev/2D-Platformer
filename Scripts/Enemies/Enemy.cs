using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody2D enemyRigidbody;
    [SerializeField] float enemySpeed;
    CapsuleCollider2D capsuleCollider;
    AudioManager audioManager;
    

    void Start()
    {
        enemyRigidbody = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        audioManager = GameObject.FindObjectOfType<AudioManager>(); 
    }

    
    void Update()
    {
        enemyRigidbody.velocity = new Vector2(enemySpeed, 0f);
        enemyDying();
    }
    void OnTriggerExit2D(Collider2D other)
    {   
        // Obje hedef bölgeden çýktýðý zaman hýzýný tersine çevirir. 
        enemySpeed = -enemySpeed;
        flipEnemy();
    }
    void flipEnemy()
    {
        // Objenin yönünü hýzýyla ayný yöne çevirir.
        transform.localScale = new Vector2(-Mathf.Sign(enemyRigidbody.velocity.x), 1f);
    }
    void enemyDying()
    {   
        // Objeyi öldürür.
        if (capsuleCollider.IsTouchingLayers(LayerMask.GetMask("Arrows")))
            {
            audioManager.playSFX(audioManager.enemyDeath);
            Destroy(gameObject);
        }
    } }
