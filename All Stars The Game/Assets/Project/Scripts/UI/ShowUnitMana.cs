using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowUnitMana : ShowUnitStat
{
    protected override float NewStatValue()
    {
        return unit.GetComponent<UnitStats>().mana;
    }
}
