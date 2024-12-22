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
        // Obje hedef b�lgeden ��kt��� zaman h�z�n� tersine �evirir. 
        enemySpeed = -enemySpeed;
        flipEnemy();
    }
    void flipEnemy()
    {
        // Objenin y�n�n� h�z�yla ayn� y�ne �evirir.
        transform.localScale = new Vector2(-Mathf.Sign(enemyRigidbody.velocity.x), 1f);
    }
    void enemyDying()
    {   
        // Objeyi �ld�r�r.
        if (capsuleCollider.IsTouchingLayers(LayerMask.GetMask("Arrows")))
            {
            audioManager.playSFX(audioManager.enemyDeath);
            Destroy(gameObject);
        }
    } }
