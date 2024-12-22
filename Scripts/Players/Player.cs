using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.SceneManagement;


public class Player : MonoBehaviour
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
    MainMenu mainMenu;
    GameObject arrowReady;
    

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = rigidBody.gravityScale;
        playerPos = transform.position;
        audioManager = GameObject.FindObjectOfType<AudioManager>();
        arrowReady = GameObject.FindGameObjectWithTag("Arrow Ready");
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive) { return;}
        Run();
        flipSprite();
        climb();
        dying();
        
    }

    void OnSave(InputValue value)
    {
        // Save Almak i�in kullan�l�r.
        if (!isAlive) { return; }
        if (savePointBoxCollider.IsTouchingLayers(LayerMask.GetMask("Player")))
        {


            if (value.isPressed)
            {

                StartCoroutine(Saving(2));


            }
        }
    }
    IEnumerator Saving(int saveTime)
    {
        isAlive = false;
        animator.SetBool("Saving", true);
        yield return new WaitForSeconds(saveTime);
        playerPos = transform.position;
        animator.SetBool("Saving",false);
        isAlive= true;
        audioManager.playSFX(audioManager.save);

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
   

    void OnFire(InputValue value)
    {
       
        if (!isAlive) { return; }
        // E�er cooldown s�resi dolmad�ysa, i�lem yapma.
        if (cooldown.isCoolingDown) { return; }

        // Attack animasyonunu tetikler, bow pozisyonunda arrow �retir, cooldown ba�lat�r.
        animator.SetTrigger("Attack");
        Instantiate(arrow, bow.position,transform.rotation);
        StartCoroutine(arrowReady�mage());
        cooldown.startCoolDown();
    }
    IEnumerator arrowReady�mage()
    {
        arrowReady.SetActive(false);
        yield return new WaitForSeconds(2);
        arrowReady.SetActive(true);
    }
    void OnPause(InputValue value)
    {
        // Oyunu durdurmak ve pause menu a�mak i�in kullan�l�r.
        if (!isAlive) { return; }
        if (value.isPressed)
        {
            FindObjectOfType<MainMenu>().pauseGame();
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
            Vector2 playerVelocity = new Vector2(moveInput.x * hozirontalSpeed, rigidBody.velocity.y);
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
    
  
   
   public void dying()
    {
        if (!isAlive) { return; }
        // Oyuncunun colliderlar� enemies,Hazards,Water tagli objelere de�erse karakteri �ld�r�r.
        if ((capsuleCollider.IsTouchingLayers(LayerMask.GetMask("enemies","Hazards","Water")))|| (boxCollider.IsTouchingLayers(LayerMask.GetMask("enemies", "Hazards", "Water"))))
        {
        // isAlive false yapar, Death Animasyonunu tetikler,  yeni bir h�z belirler.
            isAlive = false;
            animator.SetTrigger("Death");
            audioManager.playSFX(audioManager.death);
            rigidBody.velocity = new Vector2(0,10);
            StartCoroutine(Respawn(1f));
            FindObjectOfType<GameSession>().playerLivesProcess();


        }
        
}
    private bool isGrounded()
    {
        // Oyuncu yere de�iyor mu test eder.
        return boxCollider.IsTouchingLayers(LayerMask.GetMask("Platform"));
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {   
        // B�l�m sonu kazanma i�in.
        // Oyuncu Chest tag'l� objeye de�erse �sAlive false olur.
        if(collision.tag=="Chest")
        {
            isAlive = false;
            audioManager.playSFX(audioManager.win);
        }
    }
    IEnumerator Respawn(float respawnTime)
    {
        capsuleCollider.isTrigger = true;
        boxCollider.isTrigger = true;
        yield return new WaitForSeconds(respawnTime);
        rigidBody.velocity = new Vector2(0, 0);
        moveInput = new Vector2(0, 0);
        capsuleCollider.isTrigger = false;
        boxCollider.isTrigger=false;
        transform.position = playerPos;
        yield return new WaitForSeconds(0.5f);
        isAlive = true;
    }
}

    

