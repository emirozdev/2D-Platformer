using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.SceneManagement;

public class Player1 : MonoBehaviour
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
    private bool doubleJump=false;
    public GameObject batTrigger;
    public GameObject breakableWall;
    Vector2 playerPos;
    public BoxCollider2D savePointBoxCollider1;
    public BoxCollider2D savePointBoxCollider2;
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
        audioManager=GameObject.FindObjectOfType<AudioManager>();
        playerPos = transform.position;
        arrowReady = GameObject.FindGameObjectWithTag("Arrow Ready");
        
    }

   
    void Update()
    {
       
        if (!isAlive) { return;}
        Run();
        flipSprite();
        climb();
        dying();
        Shake();
        
    }

    void OnSave(InputValue value)
    {
        if (!isAlive) { return; }
        if (savePointBoxCollider1.IsTouchingLayers(LayerMask.GetMask("Player"))|| savePointBoxCollider2.IsTouchingLayers(LayerMask.GetMask("Player")))
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
        animator.SetBool("Saving", false);
        isAlive = true;
        audioManager.playSFX(audioManager.save);

    }
    void OnPause(InputValue value)
    {
        if (!isAlive) { return; }
        if (value.isPressed)
        {
            FindObjectOfType<MainMenu>().pauseGame();
        }

    }

    void OnJump(InputValue value)
    {
        // Eðer oyuncu hayatta deðilse, iþlem yapma
        if (!isAlive) return;
       

        // Eðer zýplama tuþuna basýlmýþsa ve (oyuncu yerdeyse veya çift zýplama hakký varsa)
        if (value.isPressed && (isGrounded() || doubleJump))
        {
            // Yukarý doðru hýz uygula
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, verticalSpeed);
            // doubleJump tersine çevir.
            doubleJump = !doubleJump;
            // Eðer tuþa basýlmýþsa ve oyuncu yerdeyse doubleJump true olur.
            if (value.isPressed && isGrounded())
            {
                doubleJump = true;
            }
        }
    }
    // Ekran sallanmasýný saðlar.
    void Shake()
    {
        if (!isAlive) { return; }

        if (capsuleCollider.IsTouchingLayers(LayerMask.GetMask("Bats")))
        {
            animator.SetBool("Shaking", true); // Sallanmayý baþlat
            StartCoroutine(StopShakingAfterDelay(5f)); // 5 saniye sonra sallanmayý durdur

        }
    }
    IEnumerator StopShakingAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // 5 saniye bekle
        animator.SetBool("Shaking", false); // Sallanmayý durdur
        Destroy(batTrigger);
        Destroy(breakableWall);
        FindObjectOfType<Spawn>().SpawnBats();
    }

    void OnFire(InputValue value)
    {

        if (!isAlive) { return; }
        // Eðer cooldown süresi dolmadýysa, iþlem yapma.
        if (cooldown.isCoolingDown) { return; }

        // Attack animasyonunu tetikler, bow pozisyonunda arrow üretir, cooldown baþlatýr.
        animator.SetTrigger("Attack");
        Instantiate(arrow, bow.position, transform.rotation);
        StartCoroutine(arrowReadyÝmage());
        cooldown.startCoolDown();
    }
    IEnumerator arrowReadyÝmage()
    {
        arrowReady.SetActive(false);
        yield return new WaitForSeconds(2);
        arrowReady.SetActive(true);
    }
    void OnMove(InputValue value)
    {

        if (!isAlive) { return; }
        moveInput = value.Get<Vector2>();

    }

    void Run()
    {
        Vector2 playerVelocity =new Vector2 (moveInput.x*hozirontalSpeed, rigidBody.velocity.y);
        rigidBody.velocity = playerVelocity;
        bool playerHasHorizontalSpeed = Mathf.Abs(rigidBody.velocity.x) > Mathf.Epsilon;
        animator.SetBool("ÝsRunning", playerHasHorizontalSpeed);
        

    }
    void climb()
    {
        if (!boxCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            rigidBody.gravityScale = gravityScaleAtStart;
            animator.SetBool("ÝsClimbing", false);
            return;
        }
        Vector2 climbVelocity = new Vector2(rigidBody.velocity.x, moveInput.y * climbSpeed);
        rigidBody.velocity = climbVelocity;
        rigidBody.gravityScale = 0f;
        bool playerHasClimblSpeed = Mathf.Abs(rigidBody.velocity.y) > Mathf.Epsilon;
        animator.SetBool("ÝsClimbing", playerHasClimblSpeed);
    }
   
   void flipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(rigidBody.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2((Mathf.Sign(rigidBody.velocity.x)), 1f);
        }

    }
    public void dying()
    {
        if (!isAlive) { return; }
        // Oyuncunun colliderlarý enemies,Hazards,Water tagli objelere deðerse karakteri öldürür.
        if ((capsuleCollider.IsTouchingLayers(LayerMask.GetMask("enemies", "Hazards", "Water"))) || (boxCollider.IsTouchingLayers(LayerMask.GetMask("enemies", "Hazards", "Water"))))
        {
            // isAlive false yapar, Death Animasyonunu tetikler, yeni bir hýz belirler.
            isAlive = false;
            animator.SetTrigger("Death");
            audioManager.playSFX(audioManager.death);
            rigidBody.velocity = new Vector2(0, 10);
            FindObjectOfType<GameSession>().playerLivesProcess();
            StartCoroutine(Respawn(1f));
            


        }
      

    }
    private bool isGrounded()
    {
        return boxCollider.IsTouchingLayers(LayerMask.GetMask("Platform"));
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isAlive) { return; }
        if (collision.tag=="Chest")
        {
            isAlive = false;
            audioManager.playSFX(audioManager.win);
        }
    }
    public void dying2()
    {
        isAlive = false;
        animator.SetTrigger("Death");
        audioManager.playSFX(audioManager.death);
        FindObjectOfType<GameSession>().playerLivesProcess();

    }
    IEnumerator Respawn(float respawnTime)
    {
        capsuleCollider.isTrigger = true;
        boxCollider.isTrigger = true;
        yield return new WaitForSeconds(respawnTime);
        rigidBody.velocity = new Vector2(0, 0);
        moveInput = new Vector2(0, 0);
        capsuleCollider.isTrigger = false;
        boxCollider.isTrigger = false;
        transform.position = playerPos;
        yield return new WaitForSeconds(0.5f);
        isAlive = true;
    }
}
    

