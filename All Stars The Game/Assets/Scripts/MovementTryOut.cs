using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class MovementTryOut : MonoBehaviour
{
    
    public MovementTryOut target;
    public Transform VFXtryout;
    private AnimationHandler animHandler;
    private Vector3 targetPosition;
    private Action onArriving;

    private State state;
    public enum State
    {
        Idle,
        Busy,
        Running,
        Teleporting,
        Guarding,
        Fired,
    }
    // Start is called before the first frame update
    void Start()
    {
        target = target.GetComponent<MovementTryOut>();
        animHandler = GetComponent<AnimationHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Idle:
                break;
            case State.Busy:
                break;
            case State.Running:

                float runSpeed = 5f;
                transform.position += (targetPosition - GetPosition()) * runSpeed * Time.deltaTime;

                float reachedDistance = 1f;
                if (Vector3.Distance(GetPosition(), targetPosition) < reachedDistance)
                {
                    // Arrived at Slide Target Position
                    //transform.position = slideTargetPosition;

                    onArriving();

                }
                break;

            
        }




        if (Input.GetKeyDown(KeyCode.Y))
        {
            if (gameObject.tag == "Player")
            {
                RunAttack(target, animHandler);
            }
                       
        }


        if (Input.GetKeyDown(KeyCode.S))
        {
            if (gameObject.tag == "Player")
            {
                animHandler.GuardAnim();
            }



        }




        if (Input.GetKeyDown(KeyCode.B))
        {
            if (gameObject.tag == "Player")
            {
                animHandler.Attack2();
                Transform projectileTransform = Instantiate(VFXtryout, gameObject.transform);
                Vector3 shootDir = (target.transform.position - transform.position).normalized;
                
                
            }



        }






    }




    private void GoToTarget(Vector3 targetPosition, Action onArriving)
    {

        this.targetPosition = targetPosition;
        this.onArriving = onArriving;
        state = State.Fired;
        
    }

    private void RunToPosition(Vector3 targetPosition, Action onArriving)
    {

        this.targetPosition = targetPosition;
        this.onArriving = onArriving;
        state = State.Running;
        animHandler.Running();
    }

    IEnumerator RunBackToPosition(float delay, Vector3 targetPosition, Action onArriving)
    {
        yield return new WaitForSeconds(delay);
        this.targetPosition = targetPosition;
        this.onArriving = onArriving;
        state = State.Running;
        animHandler.Running();
    }


    IEnumerator EventDelayer(float delay, Action onActionComplete)
    {
        yield return new WaitForSeconds(delay);
        onActionComplete();
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
    public void RunAttack(MovementTryOut target, AnimationHandler aHandler)
    {
        Vector3 jumpPosition= GameObject.Find("JumpPoint").transform.position + (GetPosition() - GameObject.Find("JumpPoint").transform.position).normalized * 20f;
        Vector3 targetPosition = target.GetPosition() + (GetPosition() - target.GetPosition()).normalized * 20f;
        Vector3 startingPosition = GetPosition();

        RunToPosition(jumpPosition, () =>
        {
            RunToPosition(targetPosition, () =>
            {



                state = State.Busy;
                animHandler.Attack4();

                StartCoroutine(RunBackToPosition(4.5f, jumpPosition, () =>
                {
                    RunToPosition(startingPosition, () => {
                        animHandler.IdleAnim();
                    });

                    
                }));
            });


            
        });
    }



    public void VFXTryout(MovementTryOut target,GameObject vfx)
    {
        
        Vector3 targetPosition = target.GetPosition() + (transform.position - target.GetPosition()).normalized * 20f;
        Vector3 startingPosition = GetPosition();
        
        GoToTarget(targetPosition, () =>
        {
            
                
             

                



        });
    }



}





