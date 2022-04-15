using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateEnemyMenuItems : MonoBehaviour
{
    [SerializeField]
    private GameObject targetEnemyUnitPrefab;
    [SerializeField]
    private Sprite menuItemSprite;
    [SerializeField]
    private Vector2 initalPosition;
    [SerializeField]
    private Vector2 itemDimensions;
    [SerializeField]
    private KillEnemy killEnemyScript;


    private void Awake()
    {
        //Find the units menu
        GameObject enemyUnitsMenu = GameObject.Find("EnemyUnitsMenu");
        
        //Calculate the item Position
        GameObject[] existingItems = GameObject.FindGameObjectsWithTag("TargetEnemyUnit");
        Vector2 nextPosition = new Vector2(
            initalPosition.x+(existingItems.Length*itemDimensions.x),
            initalPosition.y
            
            );

        //Instantiate the item
        GameObject targetEnemyUnit = Instantiate(targetEnemyUnitPrefab, enemyUnitsMenu.transform);
        targetEnemyUnit.name = "Target" + gameObject.name;
        targetEnemyUnit.transform.localPosition = nextPosition;
        targetEnemyUnit.transform.localScale = new Vector2(0.7f, 0.7f);
        targetEnemyUnit.GetComponent<Button>().onClick.AddListener(()=> SelectEnemyTarget());
        targetEnemyUnit.GetComponent<Image>().sprite = menuItemSprite;

        //Set the menu item in the killenemy script.
        killEnemyScript.menuItem = targetEnemyUnit;
    }


    public void SelectEnemyTarget()
    {
        GameObject partyData = GameObject.Find("PlayerParty");
        partyData.GetComponent<SelectUnit>().AttackEnemyTarget(gameObject);
    }
}
