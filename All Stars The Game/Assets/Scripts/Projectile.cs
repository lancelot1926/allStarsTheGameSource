using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Projectile : MonoBehaviour
{
    private Vector3 shootDir;
    static private CharacterBattle tar;
    private Action onArriving;
    public State state;
    public AttackManager aprefMan;
    private MovementTryOut moveTry;
    [SerializeField]
    public GameObject hitEffect;
    public enum State
    {
        Fired,
        Stopped,
    }
    

    public void Setup(Vector3 shootDir,CharacterBattle target,AttackManager apref)
    {
        this.shootDir = shootDir;
        tar = target;
        aprefMan = apref;
        
    }


    private void Start()
    {
        state = State.Fired;
        
    }
    private void Update()
    {

        switch (state)
        {
            case State.Stopped:
                Destroy(gameObject);
                break;

            case State.Fired:

                float moveSpeed = 3;
                transform.position += (tar.transform.position - transform.position) * moveSpeed * Time.deltaTime;



                float reachedDistance = 1f;
                if (Vector3.Distance(transform.position, tar.transform.position) < reachedDistance)
                {
                    //state = State.Stopped;
                    // Arrived at Slide Target Position
                    //transform.position = slideTargetPosition;
                    

                    

                }


                break;
        }



        
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (tar.GetComponent<CharacterBattle>() == collision.GetComponent<CharacterBattle>())
        {
            Debug.Log("tttttt");
            Instantiate(hitEffect, tar.transform);
            tar.ProjectileArrival(transform.GetComponentInParent<CharacterBattle>(), tar, aprefMan);
            state = State.Stopped;
        }
    }
}
