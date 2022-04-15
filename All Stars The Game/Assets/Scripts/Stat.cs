using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Stat : ScriptableObject
{
    public int charMaxHp;
    public int charCurrentHp;
    public int charMaxMp;
    public int CharCurrentMp;
    public float charAttackDamage;
    public float charMagicDamage;
    public float charDefence;
    public int charSpeed;
    public int maxAttackMultiplier;
    public int minAttackMultiplier;
    public int maxDefenceMultiplier;
    public int minDefenceMultiplier;
    public bool isDed;
}
