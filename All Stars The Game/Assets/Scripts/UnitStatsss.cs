using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitStatsss : MonoBehaviour
{
    private BattleHandler battleHandle;
    
    public string unitName;
    public int maxHP;
    public int currentHP;
    public int maxMP;
    public int currentMP;
    public float attackDamage;
    public float magicDamage;
    public float defence;
    public int speed;
    public float MaxAttackMultip;
    public float minAttackMultip;
    public float MaxDefenceMultip;
    public float minDefenceMultip;
    public List<string> weakTo;
    public List<string> strongTo;
    public bool IsDead;
    public bool GuardUp;
    public bool CanTransform;
    public string takenDamageType;
    public string takenBuffType;

    public int turnCountPassiveDamaege;
    public bool takingPassiveDamage;
    public string passiveDamageType;
    public bool burning;
    public bool wet;
    public bool poisoned;
    public bool cursed;
    public bool bleeding;

    public int turnCountImmobilization;
    public bool immobilized;
    public string immobilizationType;
    public bool stunned;
    public bool frozen;
    public bool shocked;
    public bool petrified;
    public bool sleep;
    public bool feared;

    public int turnCountSpecialStatus;
    public bool underSpecialStatus;
    public string specialStatusType;
    public bool charmed;
    public bool confused;
    public bool blinded;
    public bool raged;

    public int passiveDamage;
    public int storedBuff;

    public Sprite PlayerStatssIcon;
    public HealthBarScript HealthBar;
    public StatusScript StatInfo;
    public WsInfoScript WsInfo;

    public List<AttackManager> MagicAttackList;
    public List<AttackManager> SpecialAttackList;
    public List<AttackManager> AllAttackList;

    public AttackManager NormalAttack;
    public AttackManager TestMultipAttack;
    public Stat charStat;
    public GameObject Clone;
    private void Start()
    {
        battleHandle = GameObject.Find("BattleHandler").GetComponent<BattleHandler>();
        if (gameObject.tag == "Player")
        {
            currentHP = charStat.charCurrentHp;
            maxHP = charStat.charMaxHp;
            currentMP = charStat.CharCurrentMp;
            maxMP = charStat.charMaxMp;

        }

        if (gameObject.tag == "Enemy")
        {
            maxHP = charStat.charMaxHp;
            currentHP = maxHP;
            attackDamage = charStat.charAttackDamage;
            magicDamage = charStat.charMagicDamage;
            defence = charStat.charDefence;
            speed = charStat.charSpeed;
            MaxAttackMultip = charStat.maxAttackMultiplier;
            minAttackMultip = charStat.minAttackMultiplier;
            MaxDefenceMultip = charStat.maxDefenceMultiplier;
            minDefenceMultip = charStat.minDefenceMultiplier;
            

        }

        Clone = transform.gameObject;
        if (gameObject.tag != "PlayerClone")
        {

            HealthBar.SetHealth(currentHP, maxHP);
        }
        
        if (gameObject.tag == "Player")
        {
            
            HealthBar.SetMana(currentMP, maxMP);
        }
        WsInfo.SetICons(strongTo, weakTo);
    }
    private void Update()
    {
        charStat.charCurrentHp = currentHP;
        charStat.CharCurrentMp=currentMP;
        if (gameObject.tag == "Player")
        {
            currentHP = charStat.charCurrentHp;
            maxHP = charStat.charMaxHp;
            currentMP = charStat.CharCurrentMp;
            maxMP = charStat.charMaxMp;
            HealthBar.SetTexts(currentHP, currentMP);
           
            
        }
        if (gameObject.tag == "Player")
        {

            HealthBar.SetMana(currentMP, maxMP);
        }
        
        
        StatInfo.SetInfo(immobilized, underSpecialStatus, takingPassiveDamage);
        StatInfo.SetStatusText(immobilizationType, specialStatusType, passiveDamageType);
        StatInfo.SetTurnCountText(turnCountImmobilization, turnCountSpecialStatus, turnCountPassiveDamaege);
        if (unitName == "Sonic")
        {
            if (gameObject.GetComponent<SpriteRenderer>().flipX == false)
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = true;
            }
        }
    }

    public void RecieveDamage(int damage)
    {
        currentHP -= damage;
        HealthBar.SetHealth(currentHP, maxHP);
        

        if (currentHP<=0)
        {
            IsDead = true;
            if (gameObject.tag == "Player")
            {
                BattleHandler.DeadCharacterList.Add(gameObject.GetComponent<CharacterBattle>());
                battleHandle.PlayerUnitStatsList.Remove(gameObject.GetComponent<UnitStatsss>());
                BattleHandler.ActiveCharacterList.Remove(gameObject.GetComponent<CharacterBattle>());
                charStat.isDed = true;
            }
            if (gameObject.tag == "Enemy")
            {
                battleHandle.EnemyUnitStatsList.Remove(gameObject.GetComponent<UnitStatsss>());
                StartCoroutine(EventDelayer(4.5f, () => {
                    battleHandle.enemyCharBattleList.Remove(gameObject.GetComponent<CharacterBattle>());
                }));
            }
            battleHandle.EveryCharList.Remove(gameObject.GetComponent<UnitStatsss>());

            StartCoroutine(EventDelayer(6f, ()=> {
                if (gameObject.tag == "Enemy")
                {
                    gameObject.SetActive(false);
                    //Destroy(gameObject);
                    battleHandle.chosenEnemy = null;
                    battleHandle.enemyCharBattleList.Remove(gameObject.GetComponent<CharacterBattle>());
                }
                if (gameObject.tag == "Player")
                {
                    gameObject.SetActive(false);
                    
                }
            }));

        }


    }
    public void RecieveHeal(int heal)
    {
        currentHP += heal;
        HealthBar.SetHealth(currentHP, maxHP);


        if (currentHP > maxHP)
        {
            currentHP = maxHP;

        }


    }

    public void RecieveBuff(int buff)
    {
        switch (takenBuffType)
        {
            case "Melee Buff":
                storedBuff = buff / 4;
                attackDamage += storedBuff;
                break;
            case "Magic Buff":
                storedBuff = buff / 2;
                magicDamage += storedBuff;
                break;
            case "Defense Buff":
                storedBuff = buff / 4;
                defence += storedBuff;
                break;
            case "Speed Buff":
                storedBuff = buff / 10;
                speed += storedBuff;
                break;
        }
    }


    public int PassiveDamageCalculator(UnitStatsss target)
    {
        if (target.weakTo.Contains(target.takenDamageType))
        {
            passiveDamage = target.maxHP * 15 / 100;
        }
        else
        {
            passiveDamage = target.maxHP * 10 / 100;
        }
        
        return passiveDamage;
    }

    public void Revive()
    {
        if (gameObject.tag == "Player")
        {
            battleHandle.PlayerUnitStatsList.Add(gameObject.GetComponent<UnitStatsss>());
            charStat.isDed = false;
            battleHandle.EveryCharList.Add(gameObject.GetComponent<UnitStatsss>());
            BattleHandler.DeadCharacterList.Remove(gameObject.GetComponent<CharacterBattle>());
            gameObject.SetActive(true);
            currentHP = charStat.charMaxHp;
            
        }
    }

    IEnumerator EventDelayer(float delay, Action onActionComplete)
    {
        yield return new WaitForSeconds(delay);
        onActionComplete();
    }

    private void OnMouseOver()
    {
        if (gameObject.tag == "Enemy")
        {
            HealthBar.gameObject.SetActive(true);
            StatInfo.gameObject.SetActive(true);
        }
        
    }

    private void OnMouseExit()
    {
        if (gameObject.tag == "Enemy")
        {
            HealthBar.gameObject.SetActive(false);
            StatInfo.gameObject.SetActive(false);
        }
        
    }

    
}


