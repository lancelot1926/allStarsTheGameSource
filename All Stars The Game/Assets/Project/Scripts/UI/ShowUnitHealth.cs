using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowUnitHealth : ShowUnitStat
{
    protected override float NewStatValue()
    {
        return unit.GetComponent<UnitStats>().health;
    }
}
