using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Items : ScriptableObject
{
    public string Name;
    public string Type; //Heal,Buff,Recovery,Revive
    public string recoveryType;
    public int Lvl;
    public int UsageCount;


    public void ExecuteItemEffect(UnitStatsss target)
    {
        
        switch (Type)
        {
            case "Potion":
                switch (Lvl)
                {
                    case 1:
                        if (target.charStat.isDed == false&&target.charStat.charCurrentHp<target.charStat.charMaxHp)
                        {
                            if(PlayerPartyScript.LastScene=="BattleScene"|| PlayerPartyScript.LastScene == "BattleSceneDungeon"|| PlayerPartyScript.LastScene == null)
                            {
                                target.charStat.charCurrentHp += 50;
                                if (target.charStat.charCurrentHp > target.charStat.charMaxHp)
                                {
                                    target.charStat.charCurrentHp = target.charStat.charMaxHp;
                                }
                            }
                            if(PlayerPartyScript.LastScene=="MapScene"|| PlayerPartyScript.LastScene == "MapSceneTwo")
                            {
                                target.currentHP += 50;
                                if (target.currentHP > target.maxHP)
                                {
                                    target.currentHP = target.maxHP;
                                }
                            }
                            
                            UsageCount -= 1;
                            Debug.Log("Healed");
                        }
                        
                        break;
                }
                break;
            case "ManaP":
                switch (Lvl)
                {
                    case 1:
                        if (target.charStat.isDed == false && target.charStat.CharCurrentMp < target.charStat.charMaxMp)
                        {
                            if (PlayerPartyScript.LastScene == "BattleScene" || PlayerPartyScript.LastScene == "BattleSceneDungeon" || PlayerPartyScript.LastScene == null)
                            {
                                target.charStat.CharCurrentMp += 50;
                                if (target.charStat.CharCurrentMp > target.charStat.charMaxMp)
                                {
                                    target.charStat.CharCurrentMp = target.charStat.charMaxMp;
                                }
                            }
                            if (PlayerPartyScript.LastScene == "MapScene" || PlayerPartyScript.LastScene == "MapSceneTwo")
                            {
                                target.currentMP += 50;
                                if (target.currentMP > target.maxMP)
                                {
                                    target.currentMP = target.maxMP;
                                }
                            }

                            UsageCount -= 1;
                            Debug.Log("Mana Recovered");
                        }

                        break;
                }
                break;
            case "Revive":
                switch (Lvl)
                {
                    case 1:
                        if (target.charStat.isDed == true)
                        {
                            target.charStat.charCurrentHp= 50;
                            target.charStat.isDed = false;
                            UsageCount -= 1;
                            Debug.Log("Revived");
                        }
                        
                        break;
                }
                break;

            case "Recovery":
                switch (recoveryType)
                {
                    case "Burn":
                        target.burning=false;
                        UsageCount -= 1;
                        break;

                    case "Wet":
                        target.wet = false;
                        UsageCount -= 1;
                        break;

                    case "Poison":
                        target.poisoned = false;
                        UsageCount -= 1;
                        break;

                    case "Curse":
                        target.cursed = false;
                        UsageCount -= 1;
                        break;

                    case "Bleed":
                        target.bleeding = false;
                        UsageCount -= 1;
                        break;

                    case "Stun":
                        target.stunned = false;
                        UsageCount -= 1;
                        break;

                    case "Freeze":
                        target.frozen = false;
                        UsageCount -= 1;
                        break;

                    case "Shock":
                        target.shocked = false;
                        UsageCount -= 1;
                        break;

                    case "Petrify":
                        target.petrified = false;
                        UsageCount -= 1;
                        break;

                    case "Sleep":
                        target.sleep = false;
                        UsageCount -= 1;
                        break;

                    case "Fear":
                        target.feared = false;
                        UsageCount -= 1;
                        break;

                    case "Charm":
                        target.charmed = false;
                        UsageCount -= 1;
                        break;

                    case "Confuse":
                        target.confused = false;
                        UsageCount -= 1;
                        break;

                    case "Blind":
                        target.blinded = false;
                        UsageCount -= 1;
                        break;

                    case "Rage":
                        target.raged = false;
                        UsageCount -= 1;
                        break;


                }
                break;
        }
    }



}
