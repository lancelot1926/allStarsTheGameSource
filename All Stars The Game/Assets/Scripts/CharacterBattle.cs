using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBattle : MonoBehaviour
{
    public bool Attacking;

    private AnimationHandler animHandler;
    private AttackManager atManager;
    private CharacterBattle tarGet;
    private List<CharacterBattle> tarGetList;
    private Vector3 targetPosition;
    Vector3 startingPosition;
    public bool projectileArrived;
    private Action onArriving;
    private State state;

    private BattleHandler battleHandle;
    private AttackManager aPrefabManager;

    private int EndDamage;
    private int EndHeal;
    private int EndBuff;
    

    private Action<Action,AnimationHandler,CharacterBattle> CalledAttack;
    private Action<Action, AnimationHandler, CharacterBattle> CalledEvent;

    public enum State
    {
        Idle,
        Busy,
        Running,
        Teleporting,
        Guarding,
    }

    private void Start()
    {
        animHandler=GetComponent<AnimationHandler>();
        
        battleHandle = GameObject.Find("BattleHandler").GetComponent<BattleHandler>();

        startingPosition = GetPosition();

    }


    private void Update()
    {
        switch (state)
        {
            case State.Idle:
                break;
            case State.Busy:
                break;
            case State.Running:
                
                float runSpeed = 3f;
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

        if (transform.GetComponent<UnitStatsss>().GuardUp)
        {
            transform.GetComponent<AnimationHandler>().GuardAnim();
        }
    }

    public void EventExecuter(CharacterBattle target, Action<Action, AnimationHandler, CharacterBattle> choEvent, AttackManager ePrefab, AnimationHandler aHandler, Action onEventComplete)
    {
        CalledEvent = choEvent;
        AttackManager eventPrefab = Instantiate(ePrefab, transform);
        AttackManager ePrefabManager = eventPrefab;

        if(ePrefabManager.IsItSpecial== true)
        {
            SteadyEvent(target, aHandler, ePrefabManager);
        }

        


        if (transform.GetComponent<UnitStatsss>().GuardUp)
        {
            transform.GetComponent<UnitStatsss>().GuardUp = false;
        }

        StartCoroutine(EventDelayer(3f, () =>
        {

            Destroy(eventPrefab);
        }));

        onEventComplete();

    }

    public void Attack(CharacterBattle target,Action<Action,AnimationHandler,CharacterBattle> choAttack,AttackManager atPrefab,AnimationHandler aHandler,Action onAttackComplete)
    {
        
        
        CalledAttack = choAttack;
        AttackManager attackPrefab=Instantiate(atPrefab, transform);
        AttackManager aPrefabManager=attackPrefab;
        atManager = attackPrefab;
        tarGet = target;
        if (aPrefabManager.Melee && aPrefabManager.CanTP == false)
        {
            RunAttack(target, aHandler,aPrefabManager);
            aPrefabManager.AttackEffect.GetComponent<RangedEffect>().Setup(this, atManager);
        }
        if (aPrefabManager.Melee && aPrefabManager.CanTP == false && aPrefabManager.Jump == true)
        {
            JumpAttack(target, aHandler, aPrefabManager);
            aPrefabManager.AttackEffect.GetComponent<RangedEffect>().Setup(this, atManager);
        }

        if (aPrefabManager.Melee == false && aPrefabManager.CanTP == false&&aPrefabManager.Jump==false)
        {
            SteadyAttack(target,aHandler, aPrefabManager);
            if (aPrefabManager.Projectile == true)
            {
                Vector3 shootDir = (target.transform.position - transform.position).normalized;
                aPrefabManager.AttackEffect.GetComponent<Projectile>().Setup(shootDir, target,atManager);
                aPrefabManager.AttackEffect.GetComponent<Projectile>().hitEffect.GetComponent<RangedEffect>().Setup(this, atManager);
            }
            else
            {
                aPrefabManager.AttackEffect.GetComponent<RangedEffect>().Setup(this, atManager);
            }
        }
        if (aPrefabManager.Melee == false && aPrefabManager.CanTP == false&&aPrefabManager.Jump==true)
        {
            JumpRangedAttack(target, aHandler, aPrefabManager);
            if (aPrefabManager.Projectile == true)
            {
                Vector3 shootDir = (target.transform.position - transform.position).normalized;
                aPrefabManager.AttackEffect.GetComponent<Projectile>().Setup(shootDir, target, atManager);
                aPrefabManager.AttackEffect.GetComponent<Projectile>().hitEffect.GetComponent<RangedEffect>().Setup(this, atManager);
            }
            else
            {
                aPrefabManager.AttackEffect.GetComponent<RangedEffect>().Setup(this, atManager);
            }
        }

        if (aPrefabManager.Melee&&aPrefabManager.CanTP)
        {
            TpAttack(target, aHandler,aPrefabManager);
            aPrefabManager.AttackEffect.GetComponent<RangedEffect>().Setup(this, atManager);
        }

        if (target.GetComponent<UnitStatsss>().GuardUp)
        {
            target.GetComponent<UnitStatsss>().GuardUp = false;
        }

        if (transform.GetComponent<UnitStatsss>().GuardUp)
        {
            transform.GetComponent<UnitStatsss>().GuardUp = false;
        }

        StartCoroutine(EventDelayer(3f, () =>
        {
            
            //Destroy(attackPrefab);

        }));

        onAttackComplete();
    }

    public void MultipleAttack(List<CharacterBattle> targetTeam, Action<Action, AnimationHandler, CharacterBattle> choAttack, AttackManager atPrefab, AnimationHandler aHandler, Action onAttackComplete)
    {


        CalledAttack = choAttack;
        AttackManager attackPrefab = Instantiate(atPrefab, transform);
        AttackManager aPrefabManager = attackPrefab;
        atManager = attackPrefab;
        tarGetList = targetTeam;

        for (int x = 0; x < targetTeam.Count; x++)
        {
            CharacterBattle target = targetTeam[x];
            
            if (aPrefabManager.Melee == false && aPrefabManager.CanTP == false)
            {
                SteadyAttack(target, aHandler, aPrefabManager);
                aPrefabManager.AttackEffect.GetComponent<RangedEffect>().Setup(this,atManager);
            }
            if (aPrefabManager.Melee == false && aPrefabManager.CanTP == false&&aPrefabManager.Jump==true)
            {
                JumpRangedAttack(target, aHandler, aPrefabManager);
                aPrefabManager.AttackEffect.GetComponent<RangedEffect>().Setup(this, atManager);
            }



            if (target.GetComponent<UnitStatsss>().GuardUp)
            {
                target.GetComponent<UnitStatsss>().GuardUp = false;
            }

            if (transform.GetComponent<UnitStatsss>().GuardUp)
            {
                transform.GetComponent<UnitStatsss>().GuardUp = false;
            }
        }

        

        StartCoroutine(EventDelayer(3f, () =>
        {

            Destroy(attackPrefab);
        }));

        onAttackComplete();
    }

    public void Guard()
    {
        transform.GetComponent<UnitStatsss>().GuardUp = true;
    }

    public void SteadyEvent(CharacterBattle self, AnimationHandler aHandler,AttackManager ePrefabManager)
    {
        CalledEvent(() =>
        {
            StartCoroutine(EventDelayer(ePrefabManager.maxDelay, () =>
            {
                animHandler.IdleAnim();
            }));
        },aHandler,self);
    }
    public void SteadyAttack(CharacterBattle target, AnimationHandler aHandler, AttackManager aPrefabManager) 
    {
        CalledAttack(() =>
        {
            if (aPrefabManager.Projectile == false)
            {
                /*DamageCalculator(transform.GetComponent<UnitStatsss>(), aPrefabManager, target.GetComponent<UnitStatsss>());
                target.GetComponent<UnitStatsss>().RecieveDamage(EndDamage);
                Debug.Log(EndDamage);
                target.GetComponent<AnimationHandler>().HitAnim();
                StartCoroutine(EventDelayer(aPrefabManager.AttackDelay, () =>
                {
                    if (target.GetComponent<UnitStatsss>().IsDead == false)
                    {
                        target.GetComponent<AnimationHandler>().IdleAnim();
                    }
                    animHandler.IdleAnim();
                }));*/
            }
            
        }, aHandler,target);
    }


    public void RunAttack(CharacterBattle target, AnimationHandler aHandler,AttackManager aPrefabManager)
    {
        Vector3 targetPosition = target.GetPosition() + (GetPosition() - target.GetPosition()).normalized * 20f;
        Vector3 startingPosition = GetPosition();

        RunToPosition(targetPosition, () =>
        {
            state = State.Busy;
            CalledAttack(() => {
                DamageCalculator(transform.GetComponent<UnitStatsss>(), aPrefabManager, target.GetComponent<UnitStatsss>());
                target.GetComponent<UnitStatsss>().RecieveDamage(EndDamage);
                Debug.Log(EndDamage);
                target.GetComponent<AnimationHandler>().HitAnim();
                StartCoroutine(RunBackToPosition(aPrefabManager.AttackDelay, startingPosition, () =>
                {
                    state = State.Idle;
                    if (target.GetComponent<UnitStatsss>().IsDead == false)
                    {
                        target.GetComponent<AnimationHandler>().IdleAnim();
                    }
                    animHandler.IdleAnim();
                }));
            }, aHandler,target);
        });
    }
    public void JumpAttack(CharacterBattle target, AnimationHandler aHandler, AttackManager aPrefabManager)
    {
        Vector3 jumpPosition = GameObject.Find("JumpPoint").transform.position + (GetPosition() - GameObject.Find("JumpPoint").transform.position).normalized * 20f;
        Vector3 targetPosition = target.GetPosition() + (GetPosition() - target.GetPosition()).normalized * 20f;
        Vector3 startingPosition = GetPosition();

        JumpToPosition(jumpPosition, () =>
        {
            JumpToPosition(targetPosition, () =>
            {
                state = State.Busy;
                CalledAttack(() => {
                    DamageCalculator(transform.GetComponent<UnitStatsss>(), aPrefabManager, target.GetComponent<UnitStatsss>());
                    target.GetComponent<UnitStatsss>().RecieveDamage(EndDamage);
                    Debug.Log(EndDamage);
                    target.GetComponent<AnimationHandler>().HitAnim();
                    StartCoroutine(JumpBackToPosition(aPrefabManager.AttackDelay, jumpPosition, () =>
                    {
                        JumpToPosition(startingPosition, () => {
                            state = State.Idle;
                            if (target.GetComponent<UnitStatsss>().IsDead == false)
                            {
                                target.GetComponent<AnimationHandler>().IdleAnim();
                            }
                            animHandler.IdleAnim();
                        });
                        
                    }));
                }, aHandler, target);
            });
            
        });
    }

    public void JumpRangedAttack(CharacterBattle target, AnimationHandler aHandler, AttackManager aPrefabManager)
    {
        Vector3 jumpPosition = GameObject.Find("JumpPoint").transform.position + (GetPosition() - GameObject.Find("JumpPoint").transform.position).normalized * 20f;
        
        Vector3 startingPosition = GetPosition();

        JumpToPosition(jumpPosition, () =>
        {
            state = State.Busy;
            CalledAttack(() => {
                if (aPrefabManager.Projectile == false)
                {
                    /*DamageCalculator(transform.GetComponent<UnitStatsss>(), aPrefabManager, target.GetComponent<UnitStatsss>());
                    target.GetComponent<UnitStatsss>().RecieveDamage(EndDamage);
                    Debug.Log(EndDamage);
                    target.GetComponent<AnimationHandler>().HitAnim();
                    StartCoroutine(EventDelayer(aPrefabManager.AttackDelay, () =>
                    {
                        JumpToPosition(startingPosition, () => {
                            state = State.Idle;
                            if (target.GetComponent<UnitStatsss>().IsDead == false)
                            {
                                target.GetComponent<AnimationHandler>().IdleAnim();
                            }
                            animHandler.IdleAnim();
                        });
                    }));*/
                }


                
            }, aHandler, target);

        });
    }


    public void TpAttack(CharacterBattle target,AnimationHandler aHandler, AttackManager aPrefabManager)
    {
        Vector3 targetPosition = target.GetPosition() + (GetPosition() - target.GetPosition()).normalized * 20f;
        Vector3 startingPosition = GetPosition();

        Dissapear(()=> {
            StartCoroutine(TPtoPosition(aPrefabManager.TpDelay, targetPosition,()=> {
                Appear(()=> {
                    StartCoroutine(EventDelayer(aPrefabManager.TpDelay, ()=> {
                        CalledAttack(() => {
                            DamageCalculator(transform.GetComponent<UnitStatsss>(), aPrefabManager, target.GetComponent<UnitStatsss>());
                            target.GetComponent<UnitStatsss>().RecieveDamage(EndDamage);
                            Debug.Log(EndDamage);
                            target.GetComponent<AnimationHandler>().HitAnim();
                            StartCoroutine(EventDelayer(aPrefabManager.AttackDelay, ()=> {
                                if (target.GetComponent<UnitStatsss>().IsDead == false)
                                {
                                    target.GetComponent<AnimationHandler>().IdleAnim();
                                }
                                Dissapear(() => {
                                    StartCoroutine(TPtoPosition(aPrefabManager.TpDelay, startingPosition, () => {
                                        Appear(()=> {
                                            StartCoroutine(EventDelayer(aPrefabManager.TpDelay, ()=> {
                                                animHandler.IdleAnim();
                                            }));
                                        });    
                                    }));
                                });
                            }));                                               
                        },aHandler,target);
                    }));                                      
                });
            }));
        });   
    }


    private void DamageCalculator(UnitStatsss owner, AttackManager attack,UnitStatsss target)
    {
        float attackMultipType;
        float attackMulip = UnityEngine.Random.Range(owner.minAttackMultip, owner.MaxAttackMultip);
        float defenceMultip = UnityEngine.Random.Range(owner.minDefenceMultip, owner.MaxDefenceMultip);
        
        
        if (attack.Melee)
        {
            attackMultipType = owner.attackDamage;
        }
        else
        {
            attackMultipType = owner.magicDamage;
        }
        float damage =attack.Damage + attackMultipType * attackMulip;
        float defence = owner.defence * defenceMultip;

        /*if (target.weakTo.Contains(attack.Element))
        {
            EndDamage = ((int)Mathf.Max(0, 2*damage - defence));
        }
        
        if (target.strongTo.Contains(attack.Element))
        {
            EndDamage = ((int)Mathf.Max(0, damage/2 - defence));
            
        (int)Mathf.Max(0, damage - defence);
        }*/

        EndDamage = (int)damage;
    }

    private void HealCalculator(UnitStatsss owner, AttackManager attack, UnitStatsss target)
    {
        float attackMultipType=owner.magicDamage;
        float healMultip = UnityEngine.Random.Range(owner.minAttackMultip, owner.MaxAttackMultip);      
        float heal = attack.Damage + attackMultipType * healMultip;
        
        EndHeal = (int)heal;
    }

    private void BuffCalculator(UnitStatsss owner, AttackManager attack, UnitStatsss target)
    {
        float attackMultipType = owner.magicDamage;
        float buffMultip= UnityEngine.Random.Range(owner.minAttackMultip, owner.MaxAttackMultip);
        float buff=attack.Damage + attackMultipType * buffMultip;

        EndBuff = (int)buff;
    }

    private void RunToPosition(Vector3 targetPosition,Action onArriving)
    {
        
        this.targetPosition = targetPosition;
        this.onArriving = onArriving;
        state = State.Running;
        animHandler.Running();
    }

    private void JumpToPosition(Vector3 targetPosition, Action onArriving)
    {

        this.targetPosition = targetPosition;
        this.onArriving = onArriving;
        state = State.Running;
        animHandler.Jumping();
    }

    IEnumerator RunBackToPosition(float delay, Vector3 targetPosition, Action onArriving)
    {
        yield return new WaitForSeconds(delay);
        this.targetPosition = targetPosition;
        this.onArriving = onArriving;
        state = State.Running;
        animHandler.RunningBack();
    }
    IEnumerator JumpBackToPosition(float delay, Vector3 targetPosition, Action onArriving)
    {
        yield return new WaitForSeconds(delay);
        this.targetPosition = targetPosition;
        this.onArriving = onArriving;
        state = State.Running;
        animHandler.Jumping();
    }

    IEnumerator TPtoPosition(float delay,Vector3 targetPosition, Action onTpComplete)
    {
        yield return new WaitForSeconds(delay);
        transform.position += (targetPosition - GetPosition());
        onTpComplete();
        
        
    }

    IEnumerator EventDelayer(float delay,Action onActionComplete)
    {
        yield return new WaitForSeconds(delay);
        onActionComplete();
    }
    
    private void Dissapear(Action onTpComplete)
    {
        
        animHandler.DissapearAnim();
        onTpComplete();
    }
    
    private void Appear(Action onTpComplete)
    {
        animHandler.AppearAnim();
        onTpComplete();
    }
    
    
    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void SpawnTheProjectile()
    {
        atManager.EfectSpawner(tarGet, animHandler);
    }

    public void SpawnTheEffect()
    {
        if (atManager.IsItAoe == false)
        {
            atManager.EfectSpawner(tarGet, animHandler);
        }
        

        if (atManager.IsItAoe == true)
        {
            if (atManager.Melee == false)
            {
                for (int x = 0; x < tarGetList.Count; x++)
                {
                    CharacterBattle target = tarGetList[x];

                    atManager.EfectSpawner(target, animHandler);
                }
            }
            
        }




    }

    public void ProjectileArrival(CharacterBattle owner,CharacterBattle target,AttackManager apref)
    {
        if (apref.Jump == false)
        {
            DamageCalculator(owner.GetComponent<UnitStatsss>(), apref, target.GetComponent<UnitStatsss>());
            target.GetComponent<UnitStatsss>().RecieveDamage(EndDamage);
            Debug.Log(EndDamage);
            target.GetComponent<AnimationHandler>().HitAnim();
            StartCoroutine(EventDelayer(apref.AttackDelay, () =>
            {
                if (target.GetComponent<UnitStatsss>().IsDead == false)
                {
                    target.GetComponent<AnimationHandler>().IdleAnim();
                }
                owner.GetComponent<AnimationHandler>().IdleAnim();
            }));
        }
        if (apref.Jump == true)
        {
            DamageCalculator(owner.GetComponent<UnitStatsss>(), apref, target.GetComponent<UnitStatsss>());
            target.GetComponent<UnitStatsss>().RecieveDamage(EndDamage);
            Debug.Log(EndDamage);
            target.GetComponent<AnimationHandler>().HitAnim();
            StartCoroutine(EventDelayer(apref.AttackDelay, () =>
            {
                owner.JumpToPosition(owner.startingPosition, () => {
                    state = State.Idle;
                    if (target.GetComponent<UnitStatsss>().IsDead == false)
                    {
                        target.GetComponent<AnimationHandler>().IdleAnim();
                    }
                    owner.animHandler.IdleAnim();
                });
            }));
        }

               

    }

    public void EffectsResolving(CharacterBattle owner, CharacterBattle target, AttackManager apref)
    {
        if (apref.Jump == false)
        {
            if (apref.IsItAttack == true) 
            {
                DamageCalculator(owner.GetComponent<UnitStatsss>(), apref, target.GetComponent<UnitStatsss>());
                target.GetComponent<UnitStatsss>().RecieveDamage(EndDamage);
                Debug.Log(EndDamage);
                target.GetComponent<AnimationHandler>().HitAnim();
                StartCoroutine(EventDelayer(apref.AttackDelay, () =>
                {
                    if (target.GetComponent<UnitStatsss>().IsDead == false)
                    {
                        target.GetComponent<AnimationHandler>().IdleAnim();
                    }
                    owner.GetComponent<AnimationHandler>().IdleAnim();
                }));
            }
            if (apref.IsItHeal == true)
            {
                HealCalculator(owner.GetComponent<UnitStatsss>(), apref, target.GetComponent<UnitStatsss>());
                target.GetComponent<UnitStatsss>().RecieveHeal(EndHeal);
                Debug.Log(EndHeal);
                if (gameObject.tag == "Player")
                {
                    target.GetComponent<AnimationHandler>().BuffAnim();
                }
                StartCoroutine(EventDelayer(apref.AttackDelay, () =>
                {
                    target.GetComponent<AnimationHandler>().IdleAnim();
                    owner.GetComponent<AnimationHandler>().IdleAnim();
                }));
            }
            if (apref.IsItBuff == true)
            {
                BuffCalculator(owner.GetComponent<UnitStatsss>(), apref, target.GetComponent<UnitStatsss>());
                target.GetComponent<UnitStatsss>().RecieveBuff(EndBuff);
                Debug.Log(EndBuff);
                if (gameObject.tag == "Player")
                {
                    target.GetComponent<AnimationHandler>().BuffAnim();
                }
                StartCoroutine(EventDelayer(apref.AttackDelay, () =>
                {
                    target.GetComponent<AnimationHandler>().IdleAnim();
                    owner.GetComponent<AnimationHandler>().IdleAnim();
                }));
            }
        }
        if (apref.Jump == true)
        {
            if(apref.IsItAttack == true) 
            {
                DamageCalculator(owner.GetComponent<UnitStatsss>(), apref, target.GetComponent<UnitStatsss>());
                target.GetComponent<UnitStatsss>().RecieveDamage(EndDamage);
                Debug.Log(EndDamage);
                target.GetComponent<AnimationHandler>().HitAnim();
                StartCoroutine(EventDelayer(apref.AttackDelay, () =>
                {
                    owner.JumpToPosition(owner.startingPosition, () => {
                        state = State.Idle;
                        if (target.GetComponent<UnitStatsss>().IsDead == false)
                        {
                            target.GetComponent<AnimationHandler>().IdleAnim();
                        }
                        owner.animHandler.IdleAnim();
                    });
                }));
            }
            if (apref.IsItHeal == true)
            {
                HealCalculator(owner.GetComponent<UnitStatsss>(), apref, target.GetComponent<UnitStatsss>());
                target.GetComponent<UnitStatsss>().RecieveHeal(EndHeal);
                Debug.Log(EndHeal);
                if (gameObject.tag == "Player")
                {
                    target.GetComponent<AnimationHandler>().BuffAnim();
                }
                
                StartCoroutine(EventDelayer(apref.AttackDelay, () =>
                {
                    owner.JumpToPosition(owner.startingPosition, () => {
                        state = State.Idle;
                        target.GetComponent<AnimationHandler>().IdleAnim();
                        owner.animHandler.IdleAnim();
                    });
                }));
            }
            if (apref.IsItBuff == true)
            {
                BuffCalculator(owner.GetComponent<UnitStatsss>(), apref, target.GetComponent<UnitStatsss>());
                target.GetComponent<UnitStatsss>().RecieveBuff(EndBuff);
                Debug.Log(EndBuff);
                if (gameObject.tag == "Player")
                {
                    target.GetComponent<AnimationHandler>().BuffAnim();
                }
                StartCoroutine(EventDelayer(apref.AttackDelay, () =>
                {
                    owner.JumpToPosition(owner.startingPosition, () => {
                        state = State.Idle;
                        target.GetComponent<AnimationHandler>().IdleAnim();
                        owner.animHandler.IdleAnim();
                    });
                }));
            }

        }

    }

    

    private void OnMouseDown()
    {
        if (gameObject.tag == "Enemy")
        {
            Debug.Log("pressed");
            battleHandle.chosenEnemy = gameObject.GetComponent<CharacterBattle>();
            battleHandle.chosenEnemyUnitStats = gameObject.GetComponent<UnitStatsss>();

        }

        if (gameObject.tag == "Player")
        {
            Debug.Log("pressed");
            battleHandle.chosenPlayer = gameObject.GetComponent<CharacterBattle>();
            battleHandle.chosenPlayerUnitStats = gameObject.GetComponent<UnitStatsss>();
        }


    }

}

