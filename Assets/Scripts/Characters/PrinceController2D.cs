using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PrinceController2D : MonoBehaviour
{
    private GameManager gm;
    public Animator animator;
    public Rigidbody2D rb;
    public int nextLevel;
    public int gameOverScene;
    public int endingScene;

    // Movimentation
    private bool facingRight = true;

    private bool isGrounded;
    private bool canMove;

    // Run
    [SerializeField]
    public float runSpeed;

    private float horizontalMove = 0f;
    private bool run = true;

    // Jump
    [SerializeField]
    public float jumpForce = 250f;

    private bool jump;

    // Roll
    [SerializeField] private Collider2D rollDisableCollider;

    [SerializeField] private float rollForce;
    private bool roll;

    // Attack
    private float timeSinceAttack = 0f;

    private int currentAttack = 0;
    private bool attack;
    private float attackRadius = 0.5f;

    // Start is called before the first frame update
    private void Start()
    {
        nextLevel = 1;
        gameOverScene = 2;
        endingScene = 3;

        runSpeed = 50f;
        rollForce = 2f;

        animator = GetComponent<Animator>();

        run = true;
        canMove = true;

        gm = GameManager.GetInstance();
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        gm = GameManager.GetInstance();
    }

    // Update is called once per frame
    private void Update()
    {
        if (gm.isPaused)
        {
            canMove = false;
            run = false;
        }
        else
        {
            canMove = true;
            run = true;
        }

        // Morte por queda
        if (transform.position.y < -8)
        {
            Reset();
            Debug.Log($"Vidas: {gm.lifes}");
        }

        // === Prince Movimentation ===
        animator.SetBool("Grounded", isGrounded);
        animator.SetFloat("AirSpeedY", rb.velocity.y);
        timeSinceAttack += Time.deltaTime;

        // Run
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        if (canMove)
        {
            // Jump
            if (Input.GetButtonDown("Jump") && !jump)
            {
                jump = true;
                roll = false;
            }

            // Roll
            if (Input.GetKeyDown(KeyCode.LeftShift) && !roll)
            {
                animator.SetBool("Roll", true);
                roll = true;
            }

            // Attack
            if (Input.GetMouseButtonDown(0) && timeSinceAttack > 0.25f && !roll)
            {
                attack = true;

                currentAttack++;

                if (currentAttack > 3)
                    currentAttack = 1;

                if (timeSinceAttack > 1.0f)
                    currentAttack = 1;

                timeSinceAttack = 0f;
            }
        }
    }

    private void Reset()
    {
        gm.lifes--;
        if (gm.lifes < 0)
        {
            canMove = false;
            run = false;
            SceneManager.LoadScene(gameOverScene);
        }
        else
        {
            Vector3 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
            transform.position = new Vector3(-7, -1, 0);
        }
    }

    private void FixedUpdate()
    {
        Move();
        Combat();
    }

    private void Move()
    {
        if (canMove)
        {
            // Run
            if (run)
                transform.Translate(horizontalMove * Time.deltaTime * 3.5f * Time.fixedDeltaTime, 0f, 0f);

            if (horizontalMove > 0 && !facingRight)
                Flip();
            else if (horizontalMove < 0 && facingRight)
                Flip();
            animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

            // Jump
            if (jump && isGrounded)
            {
                animator.SetBool("Jump", true);
                rb.AddForce(new Vector2(0f, jumpForce));
                isGrounded = false;
                jump = false;
            }

            // Roll
            if (roll && isGrounded)
            {
                if (facingRight)
                    rb.velocity = new Vector2(rollForce, rb.velocity.y);
                else
                    rb.velocity = new Vector2(-1 * rollForce, rb.velocity.y);

                if (rollDisableCollider != null)
                    StartCoroutine(WaitToEnable());
            }
        }
    }

    private void Combat()
    {
        List<string> ignore = new List<string> { "Wall", "Ground", "Player", "Edge", "falling" };
        // Attack
        if (attack)
        {
            this.gameObject.layer = LayerMask.NameToLayer("Default");
            // Debug.Log(this.gameObject.layer);
            animator.SetTrigger("Attack" + currentAttack);
            Collider2D[] swordHit = Physics2D.OverlapCircleAll(new Vector2(transform.position.x + 1f, transform.position.y), attackRadius);
            foreach (Collider2D col in swordHit)
            {
                if (!ignore.Contains(col.gameObject.tag))
                {
                    Debug.Log("Hit object with tag " + col.gameObject.tag + "!");
                    if (col.gameObject.tag == "villager") col.gameObject.GetComponent<Villager>().Die();
                    else if (col.gameObject.tag == "villagerIdle") col.gameObject.GetComponent<VillagerIdle>().Die();
                    else if (col.gameObject.tag == "boss") col.gameObject.GetComponent<FinalBoss>().TakeDamage();
                };
            }
            attack = false;
            this.gameObject.layer = LayerMask.NameToLayer("Player");
            // Debug.Log(this.gameObject.layer);
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void OnLanding()
    {
        animator.SetBool("Jump", false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "falling")
        {
            isGrounded = true;
            OnLanding();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "levelEnd") SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + nextLevel);
        else if (collision.gameObject.tag == "gameEnd") SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + endingScene);
    }

    // Animation Events
    // Called in end of roll animation.
    private void AE_ResetRoll()
    {
        roll = false;
    }

    private IEnumerator WaitToEnable()
    {
        rollDisableCollider.enabled = false;
        yield return new WaitForSeconds(0.5f);
        rollDisableCollider.enabled = true;
    }
}