using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    public Animator anim;
    public float attackDelay;
    public float tpDelay;
    public string currentAnimaton;



    

    private State state;

    const string Character_Idle = "Idle";
    const string Character_Hit = "Hit_Anim";
    const string Character_Running = "Running";
    const string Character_Running_Back = "Running_Back";
    const string Character_Transform_Super = "Transform_Super";
    const string Character_Attack_1 = "Attack_1";
    const string Character_Attack_2 = "Attack_2";
    const string Character_Attack_3 = "Attack_3";
    const string Character_Attack_4 = "Attack_4";
    const string Character_Attack_5 = "Attack_5";
    const string Character_Attack_6 = "Attack_6";
    const string Character_Attack_7 = "Attack_7";
    const string Character_Attack_8 = "Attack_8";
    const string Character_Attack_9 = "Attack_9";
    const string Character_Attack_10 = "Attack_10";
    const string Character_Attack_11 = "Attack_11";
    const string Character_Attack_12 = "Attack_12";
    const string Character_Attack_13 = "Attack_13";
    const string Character_Attack_14 = "Attack_14";
    const string Character_Dissapear = "Dissapear_Anim";
    const string Character_Appear = "Appear_Anim";
    const string Character_Charge = "Charge";
    const string Character_Guard = "Guard";
    const string Character_Buff = "Buff_Anim";
    const string Item_Use = "Item_Use";
    const string Character_Jumping = "Jumping";
    const string Character_Jumping_Back = "Jumping_Back";
    public enum State{
        ıdle,
        Running,
        arriving,
        Busy,

    }

    public void Start()
    {
        anim = GetComponent<Animator>();
        
        
    }

    public void Update()
    {
    }


    public void Attack1()
    {
        ChangeAnimation(Character_Attack_1);
        
    }

    public void Attack2()
    {
        ChangeAnimation(Character_Attack_2);
        
    }
    public void Attack3()
    {
        ChangeAnimation(Character_Attack_3);
        
    }
    public void Attack4()
    {
        ChangeAnimation(Character_Attack_4);
        
    }
    public void Attack5()
    {
        ChangeAnimation(Character_Attack_5);

    }

    public void Attack6()
    {
        ChangeAnimation(Character_Attack_6);

    }
    public void Attack7()
    {
        ChangeAnimation(Character_Attack_7);

    }
    public void Attack8()
    {
        ChangeAnimation(Character_Attack_8);

    }
    public void Attack9()
    {
        ChangeAnimation(Character_Attack_9);

    }
    public void Attack10()
    {
        ChangeAnimation(Character_Attack_10);

    }
    public void Attack11()
    {
        ChangeAnimation(Character_Attack_11);

    }
    public void Attack12()
    {
        ChangeAnimation(Character_Attack_12);

    }
    public void Attack13()
    {
        ChangeAnimation(Character_Attack_13);

    }
    public void Attack14()
    {
        ChangeAnimation(Character_Attack_14);

    }

    public void Running()
    {             
        ChangeAnimation(Character_Running); 
    }
    public void RunningBack()
    {
        ChangeAnimation(Character_Running_Back);
    }
    public void BuffAnim()
    {
        ChangeAnimation(Character_Buff);
    }
    public void ItemUseAnim()
    {
        ChangeAnimation(Item_Use);
    }
    public void IdleAnim()
    {
        ChangeAnimation(Character_Idle);   
    }
    public void HitAnim()
    {
        ChangeAnimation(Character_Hit);
    }
    public void DissapearAnim()
    {
        tpDelay = 1.3f;
        ChangeAnimation(Character_Dissapear);
    }

    public void AppearAnim()
    {
        tpDelay = 1.3f;
        ChangeAnimation(Character_Appear);
    }

    public void GuardAnim()
    {
        ChangeAnimation(Character_Guard);
    }

    public void TransformAnim()
    {
        ChangeAnimation(Character_Transform_Super);
    }
    public void Jumping()
    {
        ChangeAnimation(Character_Jumping);
    }


    public void ChangeAnimation(string newAnimation)
    {
        if (currentAnimaton == newAnimation) return;

        anim.Play(newAnimation);
        currentAnimaton = newAnimation;
    }

    IEnumerator SetOnAttackComp(float delay,Action onAttackComplete)
    {
        yield return new WaitForSeconds(delay);
        onAttackComplete();
    }
}
