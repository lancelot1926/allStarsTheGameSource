using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TurnSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject actionsMenu, enemyUnitsMenu;
    
    private List<UnitStats> unitsStats;

    public GameObject enemyEncounter;



    // Start is called before the first frame update
    void Start()
    {
        unitsStats = new List<UnitStats>();

        //Add player units to the list.
        GameObject[] playerUnits=GameObject.FindGameObjectsWithTag("PlayerUnit");
        foreach(GameObject playerUnit in playerUnits)
        {
            UnitStats currentUnitStats = playerUnit.GetComponent<UnitStats>();
            currentUnitStats.CalculateNextActTurn(0);
            unitsStats.Add(currentUnitStats);
        }

        //Add enemy units to the list.
        GameObject[] enemyUnits = GameObject.FindGameObjectsWithTag("EnemyUnit");
        foreach(GameObject enemyUnit in enemyUnits)
        {
            UnitStats currentUnitStats = enemyUnit.GetComponent<UnitStats>();
            currentUnitStats.CalculateNextActTurn(0);
            unitsStats.Add(currentUnitStats);
        }

        //Sort the list based on the speeds.
        unitsStats.Sort();

        actionsMenu.SetActive(false);
        enemyUnitsMenu.SetActive(false);

        NextTurn();
    }

    public void NextTurn()
    {
        //Check if we beat all enemies
        GameObject[] remainingEnemyUnits = GameObject.FindGameObjectsWithTag("EnemyUnit");
        if (remainingEnemyUnits.Length == 0)
        {
            enemyEncounter.GetComponent<CollectReward>().GetReward();
            SceneManager.LoadScene("Town");
        }

        //Check if game over
        GameObject[] remainingPlayerUnits = GameObject.FindGameObjectsWithTag("PlayerUnit");
        if (remainingPlayerUnits.Length == 0)
        {
            SceneManager.LoadScene("Title");
        }



        UnitStats currentUnitStats = unitsStats[0];
        unitsStats.Remove(currentUnitStats);

        if (!currentUnitStats.IsDead())
        {
            GameObject currentUnit = currentUnitStats.gameObject;

            currentUnitStats.CalculateNextActTurn(currentUnitStats.nextActTurn);
            unitsStats.Add(currentUnitStats);
            unitsStats.Sort();

            if (currentUnit.tag == "PlayerUnit")
            {
                GameObject.Find("PlayerParty").GetComponent<SelectUnit>().SelectCurrentUnit(currentUnit.gameObject);
            }
            else
            {
                currentUnit.GetComponent<EnemyUnitAction>().Act();
            }
        }
        else
        {
            NextTurn();
        }


    }

    public void WaitThenNextTurn()
    {
        StartCoroutine(WaitThenNextTurnRoutine());
    }

    private IEnumerator WaitThenNextTurnRoutine()
    {
        yield return new WaitForSeconds(1.0f);
        NextTurn();
    }
}
