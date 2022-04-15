using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawner2 : MonoBehaviour
{
    private GameHandlerMap ghm;
    
    private static bool spawnerExists;
    private void Start()
    {
        ghm = GameObject.Find("GameHandlerMap").GetComponent<GameHandlerMap>();


    }
    private void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "MapPlayer")
        {
            ghm.GeneralSave();
            if (gameObject.name != "Boss")
            {
                foreach (EnemySpawnerData x in ghm.bStartersData)
                {
                    if (x.baseName == gameObject.transform.parent.name)
                    {
                        x.hadASpawner = false;
                        x.spawnerCount--;
                    }
                }
            }
            
            
            
            Destroy(gameObject);
            
            
            ghm.Save();
            if (gameObject.scene.name == "MapScene")
            {
                PlayerPartyScript.LastScene = "MapScene";
                SceneManager.LoadScene("BattleScene");
            }
            if (gameObject.scene.name == "MapSceneTwo")
            {
                PlayerPartyScript.LastScene = "MapSceneTwo";
                SceneManager.LoadScene("BattleSceneDungeon");
            }
            if (gameObject.name == "Boss")
            {
                PlayerPartyScript.LastScene = "MapSceneTwo";
                SceneManager.LoadScene("BattleSceneBoss");
            }
            
            
        }
    }
}
