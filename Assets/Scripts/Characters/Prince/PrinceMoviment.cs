using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrinceMoviment : MonoBehaviour
{
    GameManager gm;
    public  Animator     animator;
    public  PrinceController2D controller;

    // Run
    [SerializeField] 
    public  float   runSpeed = 40f;
    private float   horizontalMove = 0f;
    private bool    run = true;

    // Jump
    private bool    jump;
 
    // Roll
    private bool    roll;

    // Attack
    private float   timeSinceAttack = 0f;
    private int     currentAttack = 0;
    private bool    attack;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        gm = GameManager.GetInstance();
    }

    // Update is called once per frame
    void Update()
    {
        if (gm.gameState != GameManager.GameState.GAME)
        {
            if (controller.rb.velocity.y != 0) 
                controller.rb.velocity = new Vector2(0f, 0f);
            controller.rb.gravityScale = 0;
            animator.SetFloat("Speed", 0);
            return;
        }
        controller.rb.gravityScale = 1;

        // === Prince Movimentation ===
        animator.SetBool("Grounded", controller.isGrounded);
        animator.SetFloat("AirSpeedY", controller.rb.velocity.y);
        timeSinceAttack += Time.deltaTime;

        // Run
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        // Jump
        if (Input.GetButtonDown("Jump") && !jump) 
        {
            animator.SetBool("Jump", true);
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

    void FixedUpdate() {
        if (gm.gameState != GameManager.GameState.GAME)
            return;
        
        controller.Move(run, horizontalMove * Time.fixedDeltaTime, jump, roll);
        jump = false;
        roll = false;
    }

    public void OnLanding()
    {
        animator.SetBool("Jump", false);
    }

    // Animation Events
    // Called in end of roll animation.
    void AE_ResetRoll()
    {
        roll = false;
    }
}
