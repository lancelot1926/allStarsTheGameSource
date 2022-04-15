using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectUnit : MonoBehaviour
{
    private GameObject currentUnit;
    private GameObject actionsMenu;
    private GameObject enemyUnitsMenu;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Battle")
        {
            actionsMenu = GameObject.Find("ActionsMenu");
            enemyUnitsMenu = GameObject.Find("EnemyUnitsMenu");
        }
    }


    public void SelectCurrentUnit(GameObject unit)
    {
        currentUnit = unit;
        actionsMenu.SetActive(true);

        currentUnit.GetComponent<PlayerUnitAction>().UpdateHUD();
    }

    public void SelectAttack(bool physical)
    {
        currentUnit.GetComponent<PlayerUnitAction>().SelectAttack(physical);
        actionsMenu.SetActive(false);
        enemyUnitsMenu.SetActive(true);
    }

    public void AttackEnemyTarget (GameObject target)
    {
        actionsMenu.SetActive(false);
        enemyUnitsMenu.SetActive(false);

        currentUnit.GetComponent<PlayerUnitAction>().Act(target);
    }
}
