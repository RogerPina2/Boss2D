using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
    public Animator animator;
    [Header("Events")] [Space] public UnityEvent OnLandEvent;

    public Rigidbody2D rb;

    private bool facingRight = true;

    public bool isGrounded;
    public bool wallSide;
    [SerializeField] private float jumpForce = 250f;
    [SerializeField] private float rollForce = 1.2f;
    [SerializeField] private Collider2D rollDisableCollider;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();
    }

    public void Move(float move, bool jump, bool roll, bool attack, bool block)
    {
        transform.Translate(move * 5.0f * Time.deltaTime, 0f, 0f);

        if (move > 0 && !facingRight)
            Flip();
        else if (move < 0 && facingRight)
            Flip();

        if (jump && isGrounded)
        {
            rb.AddForce(new Vector2(0f, jumpForce));
            isGrounded = false;
        }

        if (wallSide)
        {
            animator.SetBool("WallSlide", true);
        }

        if (roll && isGrounded)
        {
            if (facingRight)
                rb.velocity = new Vector2(rollForce, rb.velocity.y);
            else
                rb.velocity = new Vector2(-1 * rollForce, rb.velocity.y);

            if (rollDisableCollider != null)
                StartCoroutine(WaitToEnable());
        }

        if (attack)
        {
            //
        }

    }

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
            wallSide = false;
        }

        if (collision.gameObject.tag == "Wall") 
        {
            wallSide = true;
            isGrounded = false;
        }
    }

    private IEnumerator WaitToEnable()
    {
        rollDisableCollider.enabled = false;
        yield return new WaitForSeconds(0.5f);
        rollDisableCollider.enabled = true;
    }
}
