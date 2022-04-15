using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AttackManager : ScriptableObject
{

    public bool IsItAttack;
    public bool IsItSpecial;
    public bool IsItHeal;
    public bool IsItBuff;
    public bool Melee;
    public bool CanTP;
    public bool Jump;
    public string Element;
    public int Damage;
    public int ManaCost;
    public float TpDelay;
    public float AttackDelay;
    public float maxDelay;
    public string AttackName;
    public string EventNameE;
    public bool Projectile;
    public bool IsItAoe;
    
    [SerializeField] private int AttackNum;
    [SerializeField] private string EventName;
    public AttackManager ChosenMeleeAoe;
    public GameObject AttackEffect;
    private GameObject summonedEffect;
    
    public void ExecuteAttack(Action onAttackComplete,AnimationHandler aHandler,CharacterBattle target)
    {
        switch (AttackNum)
        {
            case 1:
                aHandler.Attack1();
                break;
            case 2:
                aHandler.Attack2();
                break;
            case 3:
                aHandler.Attack3();
                break;
            case 4:
                aHandler.Attack4();
                break;
            case 5:
                aHandler.Attack5();
                break;
            case 6:
                aHandler.Attack6();
                break;
            case 7:
                aHandler.Attack7();
                break;
            case 8:
                aHandler.Attack8();
                break;
            case 9:
                aHandler.Attack9();
                break;
            case 10:
                aHandler.Attack10();
                break;
            case 11:
                aHandler.Attack11();
                break;
            case 12:
                aHandler.Attack12();
                break;
            case 13:
                aHandler.Attack13();
                break;
            case 14:
                aHandler.Attack14();
                break;
            

        }
        if (Projectile == false)
        {
            //GameObject summonedEffect = Instantiate(AttackEffect, target.transform);
        }
        if (Melee == true)
        {
            //GameObject summonedEffect = Instantiate(AttackEffect, aHandler.transform);
            
        }
        

        onAttackComplete();
    }


    public void ExecuteEvent(Action onEventComplete,AnimationHandler aHandler,CharacterBattle target)
    {
        switch (EventName)
        {
            case "Transformation":
                aHandler.TransformAnim();
                break;

            case "Tag Attack":
                break;

            case "Revive":
                break;
        }
        GameObject summonedEffect = Instantiate(AttackEffect, target.transform);

        onEventComplete();
    }
    


    public void StatusEffectActivation(UnitStatsss target)
    {
        if (IsItAttack == true)
        {
            if (target.strongTo.Contains(Element) == false && IsItAttack == true)
            {
                target.takenDamageType = Element;
                switch (Element)
                {
                    case "Fire":
                        if (target.takingPassiveDamage == false && target.frozen == false)
                        {
                            if (target.weakTo.Contains(Element))
                            {
                                target.takingPassiveDamage = true;
                                target.passiveDamageType = "Burning";
                                target.burning = true;
                                target.turnCountPassiveDamaege = PassiveDamageTurnCount();
                            }
                            else
                            {
                                if (RollADice() == true)
                                {
                                    target.takingPassiveDamage = true;
                                    target.passiveDamageType = "Burning";
                                    target.burning = true;
                                    target.turnCountPassiveDamaege = PassiveDamageTurnCount();
                                }
                            }

                        }

                        if (target.wet == true)
                        {
                            target.wet = false;
                            target.takingPassiveDamage = false;
                            target.turnCountPassiveDamaege = 0;
                            target.specialStatusType = "Blind";
                            target.blinded = true;
                            target.underSpecialStatus = true;
                            target.turnCountSpecialStatus = StunTurnCount();
                        }


                        if (target.frozen == true)
                        {
                            target.frozen = false;
                            target.immobilized = false;
                            target.turnCountImmobilization = 0;
                            target.wet = true;
                            target.takingPassiveDamage = true;
                            target.passiveDamageType = "Wet";
                            target.turnCountPassiveDamaege = PassiveDamageTurnCount();

                        }

                        break;

                    case "Ice":
                        if (target.immobilized == false && target.wet == false && target.burning == false)
                        {
                            if (target.weakTo.Contains(Element))
                            {
                                target.immobilizationType = "Frozen";
                                target.frozen = true;
                                target.immobilized = true;
                                target.turnCountImmobilization = StunTurnCount();
                            }
                            else
                            {
                                if (RollADice() == true)
                                {
                                    target.immobilizationType = "Frozen";
                                    target.frozen = true;
                                    target.immobilized = true;
                                    target.turnCountImmobilization = StunTurnCount();
                                }
                            }
                        }

                        if (target.wet == true)
                        {
                            target.wet = false;
                            target.takingPassiveDamage = false;
                            target.turnCountPassiveDamaege = 0;
                            target.specialStatusType = "Frozen";
                            target.frozen = true;
                            target.immobilized = true;
                            target.turnCountImmobilization = StunTurnCount();
                        }

                        if (target.burning == true)
                        {
                            target.burning = false;
                            target.takingPassiveDamage = true;
                            target.wet = true;
                            target.passiveDamageType = "Wet";
                            target.turnCountPassiveDamaege = PassiveDamageTurnCount();
                        }



                        break;

                    case "Poison":
                        if (target.takingPassiveDamage == false)
                        {
                            if (target.weakTo.Contains(Element))
                            {
                                target.takingPassiveDamage = true;
                                target.passiveDamageType = "Poisoned";
                                target.poisoned = true;
                                target.turnCountPassiveDamaege = PassiveDamageTurnCount();
                            }
                            else
                            {
                                if (RollADice() == true)
                                {
                                    target.takingPassiveDamage = true;
                                    target.passiveDamageType = "Poisoned";
                                    target.poisoned = true;
                                    target.turnCountPassiveDamaege = PassiveDamageTurnCount();
                                }
                            }
                        }

                        break;

                    case "Dark":
                        if (target.takingPassiveDamage == false)
                        {
                            if (target.weakTo.Contains(Element))
                            {
                                target.takingPassiveDamage = true;
                                target.passiveDamageType = "Cursed";
                                target.cursed = true;
                                target.turnCountPassiveDamaege = PassiveDamageTurnCount();
                            }
                            else
                            {
                                if (RollADice() == true)
                                {
                                    target.takingPassiveDamage = true;
                                    target.passiveDamageType = "Cursed";
                                    target.cursed = true;
                                    target.turnCountPassiveDamaege = PassiveDamageTurnCount();
                                }
                            }
                        }

                        if (target.takingPassiveDamage == true)
                        {
                            target.turnCountPassiveDamaege += 2;
                            target.RecieveDamage(50);
                        }


                        if (target.immobilized == false)
                        {
                            if (target.weakTo.Contains(Element))
                            {
                                target.immobilized = true;
                                target.immobilizationType = "Feared";
                                target.feared = true;
                                target.turnCountImmobilization = StunTurnCount();
                            }
                            else
                            {
                                if (RollADice() == true)
                                {
                                    target.immobilized = true;
                                    target.immobilizationType = "Feared";
                                    target.feared = true;
                                    target.turnCountImmobilization = StunTurnCount();
                                }
                            }

                        }

                        break;

                    case "Physical":
                        if (target.takingPassiveDamage == false)
                        {
                            if (target.weakTo.Contains(Element))
                            {
                                target.takingPassiveDamage = true;
                                target.passiveDamageType = "Bleeding";
                                target.bleeding = true;
                                target.turnCountPassiveDamaege = PassiveDamageTurnCount();
                            }
                            else
                            {
                                if (RollADice() == true)
                                {
                                    target.takingPassiveDamage = true;
                                    target.passiveDamageType = "Bleeding";
                                    target.bleeding = true;
                                    target.turnCountPassiveDamaege = PassiveDamageTurnCount();
                                }
                            }
                        }

                        if (target.immobilized == false)
                        {
                            if (target.weakTo.Contains(Element))
                            {
                                target.immobilized = true;
                                target.immobilizationType = "Stunned";
                                target.stunned = true;
                                target.turnCountImmobilization = StunTurnCount();
                            }
                            else
                            {
                                if (RollADice() == true)
                                {
                                    target.immobilized = true;
                                    target.immobilizationType = "Stunned";
                                    target.stunned = true;
                                    target.turnCountImmobilization = StunTurnCount();
                                }
                            }
                        }

                        break;

                    case "Wind":
                        if (target.burning == true)
                        {
                            target.RecieveDamage(50);
                        }

                        if (target.wet == true)
                        {
                            target.wet = false;
                            target.takingPassiveDamage = false;
                            target.turnCountPassiveDamaege = 0;
                            target.immobilizationType = "Frozen";
                            target.frozen = true;
                            target.immobilized = true;
                            target.turnCountImmobilization = StunTurnCount();
                        }

                        break;

                    case "Water":
                        if (target.takingPassiveDamage == false)
                        {
                            if (target.weakTo.Contains(Element))
                            {
                                target.takingPassiveDamage = true;
                                target.passiveDamageType = "Wet";
                                target.wet = true;
                                target.turnCountPassiveDamaege = PassiveDamageTurnCount();
                            }
                            else
                            {
                                if (RollADice() == true)
                                {
                                    target.takingPassiveDamage = true;
                                    target.passiveDamageType = "Wet";
                                    target.wet = true;
                                    target.turnCountPassiveDamaege = PassiveDamageTurnCount();
                                }
                            }
                        }

                        if (target.burning == true)
                        {
                            target.burning = false;
                            target.takingPassiveDamage = false;
                            target.turnCountPassiveDamaege = 0;
                            target.specialStatusType = "Blinded";
                            target.blinded = true;
                            target.underSpecialStatus = true;
                            target.turnCountSpecialStatus = StunTurnCount();
                        }

                        if (target.shocked == true)
                        {
                            target.RecieveDamage(50);
                        }

                        break;

                    case "Rock":
                        if (target.immobilized == false)
                        {
                            if (target.weakTo.Contains(Element))
                            {
                                target.immobilized = true;
                                target.immobilizationType = "Petrified";
                                target.petrified = true;
                                target.turnCountImmobilization = StunTurnCount();
                            }
                            else
                            {
                                if (RollADice() == true)
                                {
                                    target.immobilized = true;
                                    target.immobilizationType = "Petrified";
                                    target.petrified = true;
                                    target.turnCountImmobilization = StunTurnCount();
                                }
                            }
                        }


                        break;

                    case "Electric":
                        if (target.immobilized == false && target.wet == false)
                        {
                            if (target.weakTo.Contains(Element))
                            {
                                target.immobilized = true;
                                target.immobilizationType = "Shocked";
                                target.shocked = true;
                                target.turnCountImmobilization = StunTurnCount();
                            }
                            else
                            {
                                if (RollADice() == true)
                                {
                                    target.immobilized = true;
                                    target.immobilizationType = "Shocked";
                                    target.shocked = true;
                                    target.turnCountImmobilization = StunTurnCount();
                                }
                            }
                        }

                        if (target.wet == true)
                        {
                            target.wet = false;
                            target.takingPassiveDamage = false;
                            target.turnCountPassiveDamaege = 0;
                            target.immobilizationType = "Shocked";
                            target.shocked = true;
                            target.immobilized = true;
                            target.turnCountImmobilization = StunTurnCount();
                        }


                        break;

                    case "Sleep":
                        if (target.underSpecialStatus == false)
                        {
                            if (target.weakTo.Contains(Element))
                            {
                                target.underSpecialStatus = true;
                                target.specialStatusType = "Sleep";
                                target.sleep = true;
                                target.turnCountSpecialStatus = StunTurnCount();
                            }
                            else
                            {
                                if (RollADice() == true)
                                {
                                    target.underSpecialStatus = true;
                                    target.specialStatusType = "Sleep";
                                    target.sleep = true;
                                    target.turnCountSpecialStatus = StunTurnCount();
                                }
                            }
                        }

                        break;

                    case "Charm":
                        if (target.underSpecialStatus == false)
                        {
                            if (target.weakTo.Contains(Element))
                            {
                                target.underSpecialStatus = true;
                                target.specialStatusType = "Charmed";
                                target.charmed = true;
                                target.turnCountSpecialStatus = StunTurnCount();
                            }
                            else
                            {
                                if (RollADice() == true)
                                {
                                    target.underSpecialStatus = true;
                                    target.specialStatusType = "Charmed";
                                    target.charmed = true;
                                    target.turnCountSpecialStatus = StunTurnCount();
                                }
                            }
                        }

                        break;

                    case "Confuse":
                        if (target.underSpecialStatus == false)
                        {
                            if (target.weakTo.Contains(Element))
                            {
                                target.underSpecialStatus = true;
                                target.specialStatusType = "Confused";
                                target.confused = true;
                                target.turnCountSpecialStatus = StunTurnCount();
                            }
                            else
                            {
                                if (RollADice() == true)
                                {
                                    target.underSpecialStatus = true;
                                    target.specialStatusType = "Confused";
                                    target.confused = true;
                                    target.turnCountSpecialStatus = StunTurnCount();
                                }
                            }
                        }

                        break;

                    case "Rage":
                        if (target.underSpecialStatus == false)
                        {
                            if (target.weakTo.Contains(Element))
                            {
                                target.underSpecialStatus = true;
                                target.specialStatusType = "Raged";
                                target.raged = true;
                                target.turnCountSpecialStatus = StunTurnCount();
                            }
                            else
                            {
                                if (RollADice() == true)
                                {
                                    target.underSpecialStatus = true;
                                    target.specialStatusType = "Raged";
                                    target.raged = true;
                                    target.turnCountSpecialStatus = StunTurnCount();
                                }
                            }
                        }

                        break;

                    case "Fear":
                        if (target.immobilized == false)
                        {
                            if (target.weakTo.Contains(Element))
                            {
                                target.immobilized = true;
                                target.immobilizationType = "Feared";
                                target.feared = true;
                                target.turnCountImmobilization = StunTurnCount();

                            }
                            else
                            {
                                if (RollADice() == true)
                                {
                                    target.immobilized = true;
                                    target.immobilizationType = "Feared";
                                    target.feared = true;
                                    target.turnCountImmobilization = StunTurnCount();
                                }
                            }
                        }

                        break;

                    case "Blind":
                        if (target.underSpecialStatus == false)
                        {
                            if (target.weakTo.Contains(Element))
                            {
                                target.underSpecialStatus = true;
                                target.specialStatusType = "Blinded";
                                target.blinded = true;
                                target.turnCountSpecialStatus = StunTurnCount();
                            }
                            else
                            {
                                if (RollADice() == true)
                                {
                                    target.underSpecialStatus = true;
                                    target.specialStatusType = "Blinded";
                                    target.blinded = true;
                                    target.turnCountSpecialStatus = StunTurnCount();
                                }
                            }
                        }

                        break;

                }
            }
        }
        if (IsItBuff == true)
        {
            target.takenBuffType = Element;
        }
        
        
    }
    public void StatuEffecttActivationForTeam(List<CharacterBattle> targetList)
    {
        for(int i = 0; i < targetList.Count; i++)
        {
            StatusEffectActivation(targetList[i].GetComponent<UnitStatsss>());
        }
    }

    public bool RollADice()
    {
        bool decision;
        
        List<int> DiceNums = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        List<int> PozitiveNums = new List<int> { 2, 6, 10 };
        int rollenDice = DiceNums[UnityEngine.Random.Range(0, DiceNums.Count)];

        if (PozitiveNums.Contains(rollenDice))
        {
            decision = true;
        }
        else
        {
            decision = false;
        }

        return decision;
    }

    public int StunTurnCount()
    {
        int turnCount;
        List<int> numbers = new List<int> { 1, 2, 3};
        turnCount = numbers[UnityEngine.Random.Range(0, numbers.Count)];
        return turnCount;
    }

    public int PassiveDamageTurnCount()
    {
        int turnCount;
        List<int> numbers = new List<int> { 3, 4, 5, 6 };
        turnCount = numbers[UnityEngine.Random.Range(0, numbers.Count)];
        return turnCount;
    }

    public void EfectSpawner(CharacterBattle target,AnimationHandler aHandler)
    {
        if (Projectile == false)
        {
            GameObject summonedEffect = Instantiate(AttackEffect, target.transform);
        }
        if (Projectile == true)
        {
            GameObject summonedEffect = Instantiate(AttackEffect, aHandler.transform);

        }

    }

    IEnumerator EventDelayer(float delay, Action onDelayComplete)
    {
        yield return new WaitForSeconds(delay);
        onDelayComplete();
    }

}
