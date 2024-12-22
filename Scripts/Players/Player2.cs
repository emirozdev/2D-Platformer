using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.SceneManagement;

public class Player2 : MonoBehaviour
{
    Vector2 moveInput;
    Rigidbody2D rigidBody;
    [SerializeField] float hozirontalSpeed = 1f;
    [SerializeField] float verticalSpeed = 1f;
    [SerializeField] float climbSpeed = 1f;
    [SerializeField] GameObject arrow;
    [SerializeField] Transform bow;
    Animator animator;
    CapsuleCollider2D capsuleCollider;
    BoxCollider2D boxCollider;
    float gravityScaleAtStart;
    bool isAlive = true;
    [SerializeField] Cooldown cooldown;
    private bool doubleJump=false;
    GameObject triangle;
    GameObject breakk;
    [SerializeField] float dashingTime;
    [SerializeField] float dashingPower;
    [SerializeField] float dashingCoolTime;
    private bool canDash = true;
    private bool isDashing=false;
    Bomb Bomb;
    public bool isOnPlatform;
    Vector2 playerPos;
    public BoxCollider2D savePointBoxCollider1;
    public BoxCollider2D savePointBoxCollider2;
    public Rigidbody2D platformRigidbody;
    AudioManager audioManager;
    MainMenu mainMenu;
    GameObject arrowReady;
    GameObject dashReady;



    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = rigidBody.gravityScale;
        audioManager = GameObject.FindObjectOfType<AudioManager>();
        triangle = GameObject.FindWithTag("Bats");
        breakk = GameObject.FindWithTag("Break");
        playerPos = transform.position;
        arrowReady = GameObject.FindGameObjectWithTag("Arrow Ready");
        dashReady = GameObject.FindGameObjectWithTag("DashReady");
    }

    // Update is called once per frame
    void Update()
    {
        if (isDashing) { return; }
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
        if (savePointBoxCollider1.IsTouchingLayers(LayerMask.GetMask("Player")) || savePointBoxCollider2.IsTouchingLayers(LayerMask.GetMask("Player")))
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
    void OnJump(InputValue value)
    {

        // Eðer karakter hayatta deðilse, iþlem yapma
        if (!isAlive) return;
        if (isDashing) { return; }

        // Eðer zýplama tuþuna basýlmýþsa ve (karakter yerdeyse veya çift zýplama hakký varsa)
        if (value.isPressed && (isGrounded() || doubleJump))
        {
            // Yukarý doðru hýz uygula
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, verticalSpeed);
            doubleJump = !doubleJump;
            if (value.isPressed && isGrounded())
            {
                doubleJump = true;
            }
        }
    }
    void Shake()
    {
        if (!isAlive) { return; }

        if (capsuleCollider.IsTouchingLayers(LayerMask.GetMask("Bats")))
        {
            animator.SetBool("Shaking", true); // Sallanmayý baþlat
            Debug.Log("Shaking started");
            StartCoroutine(StopShakingAfterDelay(5f)); // 5 saniye sonra sallanmayý durdur

        }
    }
    IEnumerator StopShakingAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // 5 saniye bekle
        animator.SetBool("Shaking", false); // Sallanmayý durdur
        Debug.Log("Shaking stopped");
        Destroy(triangle);
        Destroy(breakk);
        FindObjectOfType<Spawn>().SpawnBats();
    }
    void OnPause(InputValue value)
    {
        if (!isAlive) { return; }
        if (value.isPressed)
        {
            FindObjectOfType<MainMenu>().pauseGame();
        }

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
    void OnDash(InputValue value)
    {
        if (!isAlive) { return; }
        if(value.isPressed && canDash)
        {
            
            StartCoroutine(Dash());
        }
        

    }
    private IEnumerator Dash()
    {
        dashReady.SetActive(false);
        canDash = false;
        isDashing = true;
        float gravityScaleAtStart = rigidBody.gravityScale;
        rigidBody.gravityScale = 0f;
        Vector2 dashVelocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        rigidBody.velocity = dashVelocity;
       // trailRenderer.emitting = true;
        yield return new WaitForSeconds(dashingTime);
       // trailRenderer.emitting = false;
        rigidBody.gravityScale = gravityScaleAtStart;
        isDashing = false;
        yield return new WaitForSeconds(dashingCoolTime);
        canDash = true;
        dashReady.SetActive(true);
        
    }
    void Run()
    {
        if (isOnPlatform)
        {
            Debug.Log("Platformdayým");
            Vector2 playerVelocity = new Vector2((moveInput.x * hozirontalSpeed) + platformRigidbody.velocity.x, rigidBody.velocity.y);
            rigidBody.velocity = playerVelocity;
            bool playerHasHorizontalSpeed = Mathf.Abs(rigidBody.velocity.x - platformRigidbody.velocity.x) > Mathf.Epsilon;
            animator.SetBool("ÝsRunning", playerHasHorizontalSpeed);



        }
        else {

            Vector2 playerVelocity = new Vector2(moveInput.x * hozirontalSpeed, rigidBody.velocity.y);
            rigidBody.velocity = playerVelocity;
            bool playerHasHorizontalSpeed = Mathf.Abs(rigidBody.velocity.x) > Mathf.Epsilon;
            animator.SetBool("ÝsRunning", playerHasHorizontalSpeed);
        }


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
    void OnMove(InputValue value)
    {
        if (isDashing) { return; }
        if (!isAlive) { return; }
        moveInput =value.Get<Vector2>();
        
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
            // isAlive false yapar, Death Animasyonunu tetikler,  yeni bir hýz belirler.
            isAlive = false;
            animator.SetTrigger("Death");
            audioManager.playSFX(audioManager.death);
            rigidBody.velocity = new Vector2(0, 10);
            StartCoroutine(Respawn(1f));
            FindObjectOfType<GameSession>().playerLivesProcess();


        }
       

    }
    IEnumerator Respawn(float respawnTime)
    {
        capsuleCollider.isTrigger = true;
        boxCollider.isTrigger = true;
        yield return new WaitForSeconds(respawnTime);
        rigidBody.velocity = new Vector2(0, 0);
        moveInput = new Vector2 (0,0);
        capsuleCollider.isTrigger = false;
        boxCollider.isTrigger = false;
        transform.position = playerPos;
        yield return new WaitForSeconds(0.5f);
        isAlive = true;
    }
    private bool isGrounded()
    {
        return boxCollider.IsTouchingLayers(LayerMask.GetMask("Platform"));
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag=="Chest")
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
        rigidBody.velocity = new Vector2(0, 10);
        StartCoroutine(Respawn(1f));
        FindObjectOfType<GameSession>().playerLivesProcess();

    }
}
    

