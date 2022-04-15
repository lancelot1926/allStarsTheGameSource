using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnitAction : MonoBehaviour
{
    [SerializeField]
    private GameObject attackPrefab;

    [SerializeField]
    private string targetsTag;

    private GameObject attack;

    private void Awake()
    {
        attack = Instantiate(attackPrefab);
        attack.GetComponent<AttackTarget>().owner = gameObject;

    }

    private GameObject FindRandomTarget()
    {
        GameObject[] possibleTargets = GameObject.FindGameObjectsWithTag(targetsTag);

        if (possibleTargets.Length > 0)
        {
            return possibleTargets[Random.Range(0, possibleTargets.Length)];
        }
        else
        {
            return null;
        }
    }

    public void Act()
    {
        GameObject target = FindRandomTarget();
        attack.GetComponent<AttackTarget>().Hit(target);
    }
}
