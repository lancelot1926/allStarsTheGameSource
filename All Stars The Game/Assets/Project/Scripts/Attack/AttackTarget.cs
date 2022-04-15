using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTarget : MonoBehaviour
{
    public GameObject owner;

    [SerializeField]
    private string attackAnimation;

    [SerializeField]
    private bool magicAttack;

    [SerializeField]
    private float manaCost;

    [SerializeField]
    private float minAttackMultiplier;

    [SerializeField]
    private float maxAttackMultiplier;

    [SerializeField]
    private float minDefenseMultiplier;

    [SerializeField]
    private float maxDefenseMultiplier;

    public void Hit(GameObject target)
    {
        UnitStats ownerStats = owner.GetComponent<UnitStats>();
        UnitStats targetStats = target.GetComponent<UnitStats>();

        if (ownerStats.mana >= manaCost)
        {
            float attackMultiplier = Random.Range(minAttackMultiplier, maxAttackMultiplier);
            float damage = (magicAttack) ? (attackMultiplier * ownerStats.magic) : (attackMultiplier * ownerStats.attack);

            float defenseMultiplier = Random.Range(minDefenseMultiplier, maxDefenseMultiplier);
            float guard = defenseMultiplier * targetStats.defesne;
            damage = Mathf.Max(0, damage - guard);

            owner.GetComponent<Animator>().Play(attackAnimation);


            targetStats.RecieveDamage(damage);

            ownerStats.mana -= manaCost;
        }
    }
   
}
