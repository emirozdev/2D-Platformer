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
        // Save Almak için kullanýlýr.
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
        // Eðer oyuncu hayatta deðilse, iþlem yapma
        if (!isAlive) return;

        // Eðer zýplama tuþuna basýlmýþsa ve (oyuncu yerdeyse veya çift zýplama hakký varsa)
        if (value.isPressed && isGrounded())
        {
            // Yukarý doðru hýz uygula
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, verticalSpeed);
            
           
        }
    }
   

    void OnFire(InputValue value)
    {
       
        if (!isAlive) { return; }
        // Eðer cooldown süresi dolmadýysa, iþlem yapma.
        if (cooldown.isCoolingDown) { return; }

        // Attack animasyonunu tetikler, bow pozisyonunda arrow üretir, cooldown baþlatýr.
        animator.SetTrigger("Attack");
        Instantiate(arrow, bow.position,transform.rotation);
        StartCoroutine(arrowReadyÝmage());
        cooldown.startCoolDown();
    }
    IEnumerator arrowReadyÝmage()
    {
        arrowReady.SetActive(false);
        yield return new WaitForSeconds(2);
        arrowReady.SetActive(true);
    }
    void OnPause(InputValue value)
    {
        // Oyunu durdurmak ve pause menu açmak için kullanýlýr.
        if (!isAlive) { return; }
        if (value.isPressed)
        {
            FindObjectOfType<MainMenu>().pauseGame();
        }

    }
        void OnMove(InputValue value)
        {

            if (!isAlive) { return; }
            // Basýlan tuþun yönünü alýr.
            moveInput = value.Get<Vector2>();

        }
    

        void Run()
        {
            // Basýlan tuþun yönüne göre oyuncuya yeni bir yatay hýz belirler.
            Vector2 playerVelocity = new Vector2(moveInput.x * hozirontalSpeed, rigidBody.velocity.y);
            rigidBody.velocity = playerVelocity;
            // Oyuncunun bir hýzý var mý, test eder. Hareket halindeyse ÝsRunning animasyonunu tetikler.
            bool playerHasHorizontalSpeed = Mathf.Abs(rigidBody.velocity.x) > Mathf.Epsilon;
            animator.SetBool("ÝsRunning", playerHasHorizontalSpeed);


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
            //Oyuncu Ladder objesine deðiyor mu test eder. Eðer deðmiyorsa gravity normale döner.
            if (!boxCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")))
            {
                rigidBody.gravityScale = gravityScaleAtStart;
                animator.SetBool("ÝsClimbing", false);
                return;
            }
            // Oyuncuya yeni bir yukarý yönlü hýz belirler.Gravity sýfýr yapar.
            Vector2 climbVelocity = new Vector2(rigidBody.velocity.x, moveInput.y * climbSpeed);
            rigidBody.velocity = climbVelocity;
            rigidBody.gravityScale = 0f;
            // Oyuncunun yukarý yönlü hýzý var mý test eder, varsa ÝsClimbing animasyonunu true yapar.
            bool playerHasClimblSpeed = Mathf.Abs(rigidBody.velocity.y) > Mathf.Epsilon;
            animator.SetBool("ÝsClimbing", playerHasClimblSpeed);
        }
    
  
   
   public void dying()
    {
        if (!isAlive) { return; }
        // Oyuncunun colliderlarý enemies,Hazards,Water tagli objelere deðerse karakteri öldürür.
        if ((capsuleCollider.IsTouchingLayers(LayerMask.GetMask("enemies","Hazards","Water")))|| (boxCollider.IsTouchingLayers(LayerMask.GetMask("enemies", "Hazards", "Water"))))
        {
        // isAlive false yapar, Death Animasyonunu tetikler,  yeni bir hýz belirler.
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
        // Oyuncu yere deðiyor mu test eder.
        return boxCollider.IsTouchingLayers(LayerMask.GetMask("Platform"));
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {   
        // Bölüm sonu kazanma için.
        // Oyuncu Chest tag'lý objeye deðerse ÝsAlive false olur.
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

    

