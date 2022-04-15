using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEffect : MonoBehaviour
{
    static private CharacterBattle tar;
    static private List<CharacterBattle> tarList;
    public AttackManager aprefMan;
    static private CharacterBattle Owner;
    public void Setup(CharacterBattle owner,AttackManager apref)
    {
        Owner = owner;
        aprefMan = apref;

    }
    public void MultiSetup(CharacterBattle owner, List<CharacterBattle> targetList, AttackManager apref)
    {
        Owner = owner;
        tarList = targetList;
        aprefMan = apref;

    }



    // Start is called before the first frame update
    void Start()
    {
        if (aprefMan.Projectile == false&&aprefMan.Melee==false)
        {
            transform.GetComponentInParent<CharacterBattle>().EffectsResolving(Owner, transform.GetComponentInParent<CharacterBattle>(), aprefMan);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
