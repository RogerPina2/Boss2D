using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroKnightMoviment : MonoBehaviour
{
    public CharacterController2D controller;
    public Animator animator;

    public float runSpeed = 40f;   
    private float horizontalMove = 0f;
    private bool jump;
    private bool roll;
    private bool attack = false;
    private int currentAttack = 1;
    private float timeSinceAtack = 0f;
    private bool block = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceAtack += Time.deltaTime;
        animator.SetBool("Grounded", controller.isGrounded);


        // Hero Knight Run
        if (!attack || !block)
        {
            horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
            animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
        }

        // Hero Knight Jump
        if (Input.GetButtonDown("Jump")) {
            jump = true;
            animator.SetBool("Jump", true);
        }
        animator.SetFloat("AirSpeedY", controller.rb.velocity.y);

        // Roll
        if (Input.GetKeyDown(KeyCode.LeftShift) && !roll) {
            roll = true;
            animator.SetBool("Roll", true);
        }

        // Atack
        if (Input.GetMouseButtonDown(0) && timeSinceAtack > 0.25f && !roll)
        {
            attack = true;

            if (currentAttack == 1)
            {
                animator.SetTrigger("Attack1");
            } else if (currentAttack == 2)
            {
                animator.SetTrigger("Attack2");
            } else if (currentAttack == 3)
            {
                animator.SetTrigger("Attack3");
            } else  
            {
                currentAttack = 0;
            }

            timeSinceAtack = 0f;

            currentAttack++;
        }

        // Block
        if (Input.GetMouseButtonDown(1) && !roll)
        {
            block = true;
            animator.SetTrigger("Block");
            // animator.SetBool("IdleBlock", true);
        } else if (Input.GetMouseButtonUp(1)) {
            block = false;
        }
    }

    void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, jump, roll, attack, block);
        jump = false;
        roll = false;
        attack = false;
    }

    public void OnLanding()
    {
        animator.SetBool("Jump", false);
    }
}
