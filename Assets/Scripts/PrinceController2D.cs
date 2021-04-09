using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrinceController2D : MonoBehaviour
{
    public  Animator     animator;
    public  Rigidbody2D  rb;

    // Movimentation
    private bool    facingRight = true;
    private bool    isGrounded;

    // Run
    [SerializeField] 
    public  float   runSpeed = 40f;
    private float   horizontalMove = 0f;

    // Jump
    [SerializeField] 
    public  float   jumpForce = 250f;
    private bool    jump;
 
    // Roll
    [SerializeField] private Collider2D rollDisableCollider;
    [SerializeField] private float rollForce = 1.2f;
    private bool    roll;

    // Attack
    private float   timeSinceAttack = 0f;
    private int     currentAttack = 0;
    private bool    attack;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Movimentation
        animator.SetBool("Grounded", isGrounded);
        animator.SetFloat("AirSpeedY", rb.velocity.y);
        timeSinceAttack += Time.deltaTime;

        // Run
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        // Jump
        if (Input.GetButtonDown("Jump") && !jump) 
            jump = true;

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

    void FixedUpdate()
    {
        Move();
        Combat();
    }

    private void Move()
    {
        // Run
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

    private void Combat()
    {
        // Attack
        if (attack) {
            animator.SetTrigger("Attack" + currentAttack);
            attack = false;
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
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
            OnLanding();
        }
    }

    // Animation Events
    // Called in end of roll animation.
    void AE_ResetRoll()
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
