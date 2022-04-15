using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
public class GameHandler : MonoBehaviour
{
    private BattleHandler battleHandle;
    private GameObject MapPlayer;

    private void Awake()
    {
        MapPlayer = GameObject.Find("MapPlayer");
        MapPlayer.SetActive(false);
    }
    void Start()
    {
        battleHandle = GameObject.Find("BattleHandler").GetComponent<BattleHandler>();
        
    }

    // Update is called once per frame
    void Update()
    {
        battleHandle.CheckWinLose();

        if (battleHandle.playerWon==true)
        {
            StartCoroutine(EventDelayer(5f, () =>
            {
                if (PlayerPartyScript.LastScene == "MapScene")
                {
                    PlayerPartyScript.LastScene = "BattleScene";
                    MapPlayer.SetActive(true);
                    SceneManager.LoadScene("MapScene");
                }
                if (PlayerPartyScript.LastScene == "MapSceneTwo")
                {
                    PlayerPartyScript.LastScene = "BattleSceneDungeon";
                    MapPlayer.SetActive(true);
                    SceneManager.LoadScene("MapSceneTwo");
                }

                if (gameObject.scene.name=="BattleSceneBoss")
                {
                    
                    SceneManager.LoadScene("EndScene");
                }

            }));
        }
        
        if (battleHandle.enemyWon==true)
        {
            StartCoroutine(EventDelayer(5f, () =>
            {
                SceneManager.LoadScene("LoseScene");
            }));
        }
    }



    IEnumerator EventDelayer(float delay, Action onDelayComplete)
    {
        yield return new WaitForSeconds(delay);
        onDelayComplete();
    }


}
