using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroKnightCombat : MonoBehaviour
{
    public CharacterController2D controller;

    public Animator animator;
    bool attack = false;
    int currentAttack = 1;
    bool block = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) //&& m_timeSinceAttack > 0.25f && !m_rolling
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

            currentAttack++;
        }

        if (Input.GetMouseButtonDown(1)) //&& !m_rolling
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
        controller.Fight(attack, block);
        attack = false;
    }
}
