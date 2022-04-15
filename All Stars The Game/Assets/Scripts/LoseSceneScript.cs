using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoseSceneScript : MonoBehaviour
{
    private GameData gd;
    private GameObject MapPlayer;
    // Start is called before the first frame update
    void Start()
    {
        MapPlayer = GameObject.Find("MapPlayer");
        gd =FileHandler.ReadFromJson<GameData>("GameData.json");
        Debug.Log(gd.LastScene);
        if (PlayerPartyScript.LastScene == null)
        {
            PlayerPartyScript.LastScene = gd.LastScene;
        }
        MapPlayer.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartTheGame()
    {
        MapPlayer.SetActive(true);
        SceneManager.LoadScene("MapScene");
        PlayerPartyScript.LastScene = null;
    }

    public void ContinueTheGame()
    {
        
        Debug.Log(PlayerPartyScript.LastScene);
        if (PlayerPartyScript.LastScene != null)
        {
            if (PlayerPartyScript.LastScene == "MapScene")
            {
                MapPlayer.SetActive(true);
                SceneManager.LoadScene("MapScene");
                PlayerPartyScript.LastScene = "StartScene";
            }else if (PlayerPartyScript.LastScene == "MapSceneTwo")
            {
                MapPlayer.SetActive(true);
                SceneManager.LoadScene("MapSceneTwo");
                PlayerPartyScript.LastScene = "StartScene";
            }
        }
        

    }

    public void GoBacktoStart()
    {
        if (PlayerPartyScript.LastScene == "MapScene")
        {
            SceneManager.LoadScene("MapScene");
            foreach (GameObject playerchar in PlayerPartyScript.absolutePlayerCharacterList)
            {

                playerchar.GetComponentInChildren<UnitStatsss>().charStat.isDed = false;
                playerchar.GetComponentInChildren<UnitStatsss>().charStat.charCurrentHp = playerchar.GetComponentInChildren<UnitStatsss>().charStat.charMaxHp;
                PlayerPartyScript.playerCharacterList = PlayerPartyScript.absolutePlayerCharacterList;
                PlayerPartyScript.LastScene = "LoseScene";

            }
        }else if (PlayerPartyScript.LastScene == "MapSceneTwo")
        {
            SceneManager.LoadScene("MapSceneTwo");
            foreach (GameObject playerchar in PlayerPartyScript.absolutePlayerCharacterList)
            {

                playerchar.GetComponentInChildren<UnitStatsss>().charStat.isDed = false;
                playerchar.GetComponentInChildren<UnitStatsss>().charStat.charCurrentHp = playerchar.GetComponentInChildren<UnitStatsss>().charStat.charMaxHp;
                PlayerPartyScript.playerCharacterList = PlayerPartyScript.absolutePlayerCharacterList;
                PlayerPartyScript.LastScene = "LoseScene";

            }
        }
    }


    public void Quit()
    {
        Application.Quit();
    }
}
