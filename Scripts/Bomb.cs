using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    BoxCollider2D boxCollider;
    Animator animator;
    [SerializeField] float bombingTime;
    [SerializeField] float powTime;
    CircleCollider2D explosianArea;
    public Player2 playerScript;
    AudioManager audioManager;
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        explosianArea = GetComponent<CircleCollider2D>();
        explosianArea.enabled = false;
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

    }
    private void Update()
    {
        bombing();
        killPlayer();
        
    }
    void bombing()
    {
        if (boxCollider.IsTouchingLayers(LayerMask.GetMask("Player")))
        {
            StartCoroutine(bomb());
        }
    }
    IEnumerator bomb()
    {
            animator.SetBool("Bombing",true);
            yield return new WaitForSeconds(bombingTime);
            animator.SetBool("Bombing",false);
            animator.SetTrigger("Pow");
        audioManager.playSFX(audioManager.bomb);
        explosianArea.enabled=true;
            yield return new WaitForSeconds(powTime);
            Destroy(gameObject);

    }
   void killPlayer()
    {
        if(explosianArea.IsTouchingLayers(LayerMask.GetMask("Player")))
        {
            playerScript.dying2();


        }
    }
}
