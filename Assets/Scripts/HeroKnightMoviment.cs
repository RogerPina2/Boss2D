using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroKnightMoviment : MonoBehaviour
{
    public CharacterController2D controller;
    public Animator animator;

    public float runSpeed = 40f;   
    float horizontalMove = 0f;
    bool jump;
    bool roll;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Hero Knight Run
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
        animator.SetBool("Grounded", controller.isGrounded);

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
    }

    void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, jump, roll);
        jump = false;
        roll = false;
    }

    public void OnLanding()
    {
        animator.SetBool("Jump", false);
    }
}
