using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float horizontalMove = 0f;
    public float verticalMove = 0f;
    public float MoveSpeed;

    private bool playerMoving;
    private Vector2 lastMove;
    private Animator anim;
    private Rigidbody2D rigbody;

    void Start()
    {
        rigbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal");
        verticalMove = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {

        playerMoving = false;
        
        if (horizontalMove > 0.5f || horizontalMove < -0.5f)
        {
            rigbody.velocity = new Vector2(horizontalMove * MoveSpeed, rigbody.velocity.y);
            playerMoving = true;
            lastMove = new Vector2(horizontalMove, 0f);
        }

        if (verticalMove > 0.5f || verticalMove < -0.5f)
        {
            rigbody.velocity = new Vector2(rigbody.velocity.x, verticalMove * MoveSpeed);
            playerMoving = true;
            lastMove = new Vector2(0f, verticalMove);
        }


        if (horizontalMove < 0.5f && horizontalMove > -0.5f)
        {
            rigbody.velocity = new Vector2(0f, rigbody.velocity.y);
        }
        if (verticalMove < 0.5f && verticalMove > -0.5f)
        {
            rigbody.velocity = new Vector2(rigbody.velocity.x, 0f);
        }

        anim.SetFloat("MoveX", horizontalMove);
        anim.SetFloat("MoveY", verticalMove);
        anim.SetFloat("LastMoveX", lastMove.x);
        anim.SetFloat("LastMoveY", lastMove.y);
        anim.SetBool("PlayerMoving", playerMoving);
    }
}
