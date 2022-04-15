using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AIMovement : MonoBehaviour
{
    public float MoveSpeed=1.5f;
    public float verticalMove;
    public float horizontalMove;
    private Vector3 moveDirection;
    private Animator anim;
    public Vector3 StartPoint;
    public Vector3 RoamPosiiton;
    private GameObject Player;
    private State state;

    private enum State
    {
        Roaming,
        Chasing,
        Returning,
    }
    void Start()
    {
        anim = GetComponent<Animator>();
        StartPoint = transform.position;
        RoamPosiiton = GetRoamingPoisition();
        Player = GameObject.FindGameObjectWithTag("MapPlayer");
        state = State.Roaming;
        
    }

    

    private void FixedUpdate()
    {
        switch (state)
        {
            default:
            case State.Roaming:
                transform.position += (RoamPosiiton - transform.position) * MoveSpeed * Time.fixedDeltaTime;
                anim.SetFloat("MoveX", moveDirection.x);
                anim.SetFloat("MoveY", moveDirection.y);
                if (Vector3.Distance(transform.position, RoamPosiiton) < 0.3f)
                {
                    RoamPosiiton = GetRoamingPoisition();
                    /*StartCoroutine(EventDelayer(3f, () => {

                    }));*/
                }
                FindTarget();
                break;
            case State.Chasing:
                transform.position += (Player.transform.position - transform.position) * MoveSpeed * Time.fixedDeltaTime;
                anim.SetFloat("MoveX", Player.GetComponent<PlayerMovement>().horizontalMove);
                anim.SetFloat("MoveY", Player.GetComponent<PlayerMovement>().verticalMove);
                
                float stopChasingDistance = 5f;
                if (Vector3.Distance(transform.position, Player.transform.position) > stopChasingDistance)
                {
                    state = State.Returning;
                }
                break;
            case State.Returning:
                transform.position += (StartPoint - transform.position) * MoveSpeed * Time.fixedDeltaTime;
                gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
                anim.SetFloat("MoveX", StartPoint.x);
                anim.SetFloat("MoveY", StartPoint.y);
                if (Vector3.Distance(transform.position, StartPoint) < 0.3f)
                {
                    gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
                    state = State.Roaming;
                }
                break;

        }


        

    }


    private Vector3 GetRoamingPoisition()
    {
        return StartPoint + GetRandomDir() * UnityEngine.Random.Range(1f, 3f);
    }

    private Vector3 GetRandomDir()
    {
        moveDirection= new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;
        
        return moveDirection;
    }

    private void FindTarget()
    {
        float targetRange= 3f;
        if (Vector3.Distance(transform.position, Player.transform.position) < targetRange)
        {
            state=State.Chasing;
        }
        
    }
    
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        
        
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        RoamPosiiton = GetRoamingPoisition();
    }
}

