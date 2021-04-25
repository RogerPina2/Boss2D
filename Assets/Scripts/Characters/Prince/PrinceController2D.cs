using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PrinceController2D : MonoBehaviour
{
    GameManager gm;
    public  Rigidbody2D  rb;
    public  Animator     animator;
    [Header("Events")] [Space] public UnityEvent OnLandEvent;

    // Game Managing
    public bool inGame;

    // Movimentation
    private bool    facingRight = true;
    public  bool    isGrounded;

    [SerializeField] public  float   jumpForce = 300f;

    [SerializeField] private Collider2D rollDisableCollider;
    [SerializeField] private float rollForce = 1.2f;

    void Start()
    {
        gm = GameManager.GetInstance();
    }

    void Update()
    {
        // Game Managing
        if (gm.gameState != GameManager.GameState.GAME)
            return;

        if (Input.GetKeyDown(KeyCode.Escape) && gm.gameState == GameManager.GameState.GAME)
        {
            gm.ChangeState(GameManager.GameState.PAUSE);
        }

        // Morte por queda
        if (transform.position.y < -8)
        {
            Reset();
        }

    }

    private void Awake()
    {
        rb          = GetComponent<Rigidbody2D>();
        animator    = GetComponent<Animator>();

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();
    } 

    public void Move(bool run, float move, bool jump, bool roll)
    {
        // Run
        transform.Translate(move * 4.5f * Time.deltaTime, 0f, 0f);

        if (move > 0 && !facingRight)
            Flip();
        else if (move < 0 && facingRight)
            Flip();

        // Jump
        if (jump && isGrounded) 
        {
            rb.AddForce(new Vector2(0f, jumpForce));
            isGrounded = false;
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

    // private void Combat()
    // {
    //     // Attack
    //     if (attack) {
    //         animator.SetTrigger("Attack" + currentAttack);
    //         attack = false;
    //     }
    // }

    private void Flip()
    {
        facingRight = !facingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
            OnLandEvent.Invoke();
        }
    }

    private IEnumerator WaitToEnable()
    {
        rollDisableCollider.enabled = false;
        yield return new WaitForSeconds(0.5f);
        rollDisableCollider.enabled = true;
    }

    private void Reset()
    {
        if (gm.lifes <= 0 && gm.gameState == GameManager.GameState.GAME)
            gm.ChangeState(GameManager.GameState.ENDGAME);

        Vector3 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        transform.position = new Vector3(-6, 0, 0);
    
        gm.lifes--;
    }
}
