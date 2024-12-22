using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.SceneManagement;


public class Player3 : MonoBehaviour
{
    Vector2 moveInput;
    Rigidbody2D rigidBody;
    [SerializeField] float hozirontalSpeed = 1f;
    [SerializeField] float verticalSpeed = 1f;
    [SerializeField] float climbSpeed = 1f;
    public GameObject arrow;
    public Transform bow;
    Animator animator;
    CapsuleCollider2D capsuleCollider;
    BoxCollider2D boxCollider;
    float gravityScaleAtStart;
    bool isAlive = true;
    [SerializeField] Cooldown cooldown;
    public BoxCollider2D savePointBoxCollider;
    Vector2 playerPos;
    AudioManager audioManager;
    

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = rigidBody.gravityScale;
        playerPos = transform.position;
        audioManager = GameObject.FindObjectOfType<AudioManager>();
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive) { return;}
        Run();
        flipSprite();
        climb();
        
        
    }
   

    void OnJump(InputValue value)
    {
        // E�er oyuncu hayatta de�ilse, i�lem yapma
        if (!isAlive) return;

        // E�er z�plama tu�una bas�lm��sa ve (oyuncu yerdeyse veya �ift z�plama hakk� varsa)
        if (value.isPressed && isGrounded())
        {
            // Yukar� do�ru h�z uygula
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, verticalSpeed);
            
           
        }
    }
   

    
    void OnMove(InputValue value)
    {

        if (!isAlive) { return; }
        // Bas�lan tu�un y�n�n� al�r.
        moveInput = value.Get<Vector2>();

    }

    void Run()
    {
        // Bas�lan tu�un y�n�ne g�re oyuncuya yeni bir yatay h�z belirler.
        Vector2 playerVelocity =new Vector2 (moveInput.x*hozirontalSpeed, rigidBody.velocity.y);
        rigidBody.velocity = playerVelocity;
        // Oyuncunun bir h�z� var m�, test eder. Hareket halindeyse �sRunning animasyonunu tetikler.
        bool playerHasHorizontalSpeed = Mathf.Abs(rigidBody.velocity.x) > Mathf.Epsilon;
        animator.SetBool("�sRunning", playerHasHorizontalSpeed);
        

    }
    void flipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(rigidBody.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2((Mathf.Sign(rigidBody.velocity.x)), 1f);
        }

    }
    void climb()
    {
        //Oyuncu Ladder objesine de�iyor mu test eder. E�er de�miyorsa gravity normale d�ner.
        if (!boxCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            rigidBody.gravityScale = gravityScaleAtStart;
            animator.SetBool("�sClimbing", false);
            return;
        }
        // Oyuncuya yeni bir yukar� y�nl� h�z belirler.Gravity s�f�r yapar.
        Vector2 climbVelocity = new Vector2(rigidBody.velocity.x, moveInput.y * climbSpeed);
        rigidBody.velocity = climbVelocity;
        rigidBody.gravityScale = 0f;
        // Oyuncunun yukar� y�nl� h�z� var m� test eder, varsa �sClimbing animasyonunu true yapar.
        bool playerHasClimblSpeed = Mathf.Abs(rigidBody.velocity.y) > Mathf.Epsilon;
        animator.SetBool("�sClimbing", playerHasClimblSpeed);
    }
  
   
  
       
    private bool isGrounded()
    {
        // Oyuncu yere de�iyor mu test eder.
        return boxCollider.IsTouchingLayers(LayerMask.GetMask("Platform"));
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {   
        // B�l�m sonu kazanma i�in.
        // Oyuncu Chest tag'l� objeye de�erse �sAlive false oluri winSFX sesini �alar.
        if(collision.tag=="Chest")
        {
            rigidBody.velocity = new Vector2(0, 0);
            animator.SetBool("�sRunning", false);
            moveInput = new Vector2(0, 0);
            isAlive = false;
            
            

        }
    }
   
}
    

