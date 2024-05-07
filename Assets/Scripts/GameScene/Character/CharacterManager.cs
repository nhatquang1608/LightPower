using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public static event Action<bool> OnSetButtonControl;
    public static float moveInput;
    public static bool jumpInput;
    public bool canControl;
    private float moveSpeed = 5f;
    private float jumpForce = 30f;
    private float surfaceDistance = 0.2f;

    [SerializeField] private bool isOnGround = true;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;

    [SerializeField] private Transform surfaceCheck;
    [SerializeField] private LayerMask surfaceMask;
    [SerializeField] private Transform playerBulb;
    [SerializeField] private Transform playerPower;

    private void Update()
    {
        if(Physics2D.OverlapCircle(surfaceCheck.position, surfaceDistance, surfaceMask))
        {
            isOnGround = true;
        }
        else
        {
            isOnGround = false;
        }

        if(!canControl) return;
        Move();
        Jump();
    }

    public void SetCanControl(bool can)
    {
        if(can)
        {
            canControl = true;
            animator.SetBool("sleep", false);
            animator.SetBool("idle", true);
            playerPower = GameObject.FindGameObjectWithTag("Power").transform;
            playerPower.position = playerBulb.position;
            playerPower.SetParent(transform);
            OnSetButtonControl?.Invoke(true);
        }
        else
        {
            canControl = false;
            animator.SetBool("sleep", true);
            animator.SetBool("idle", false);
            animator.SetBool("walk", false);

            if(playerPower)
            {
                playerPower.SetParent(null);
                playerPower = null;
            }
            OnSetButtonControl?.Invoke(false);
        }
    }

    private void Move()
    {
        // moveInput = Input.GetAxisRaw("Horizontal");
        Vector2 moveVelocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        rb.velocity = moveVelocity;
        if(isOnGround)
        {
            if(moveInput != 0)
            {
                animator.SetBool("walk", true);
                if(moveInput > 0)
                {
                    transform.localScale = new Vector3(1.5f, 1.5f, 1);
                }
                else
                {
                    transform.localScale = new Vector3(-1.5f, 1.5f, 1);
                }
            }
            else
            {
                animator.SetBool("idle", true);
                animator.SetBool("walk", false);
            }
        }
        else
        {
            animator.SetBool("idle", false);
            animator.SetBool("walk", false);
            animator.SetTrigger("jump");
        }
    }

    private void Jump()
    {
        if(jumpInput && isOnGround)
        {
            jumpInput = false;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }
}
