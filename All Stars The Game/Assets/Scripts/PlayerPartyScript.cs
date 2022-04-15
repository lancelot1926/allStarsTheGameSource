using System.Collections.Generic;
using UnityEngine;

public class PlayerPartyScript : MonoBehaviour
{
    public static List<GameObject> playerCharacterList;
    public List<GameObject> playerChaList;

    public static List<GameObject> absolutePlayerCharacterList;
    public List<GameObject> absolutePlayerChaList;

    public static List<GameObject> deadPlayerCharacterList;
    public List<GameObject> deadPlayerChaList;

    public static List<Items> Items;
    public List<Items> items;

    public static string LastScene;

    public static TeamData td;
    public static GameData gd;

    private void Awake()
    {
        
        if (LastScene != "BattleScene")
        {

            absolutePlayerCharacterList = absolutePlayerChaList;
            playerCharacterList = playerChaList;
            deadPlayerCharacterList = deadPlayerChaList;
            Items = items;

        }

        if (LastScene == "BattleScene")
        {
            td = FileHandler.ReadFromJson<TeamData>("TeamData.json");
            playerChaList = td.playerTeamList;
            absolutePlayerChaList = td.absolutePlayerTeamList;
            deadPlayerChaList = td.deadPlayerTeamList;
        }
        td = new TeamData(playerChaList, absolutePlayerChaList, deadPlayerChaList);
        FileHandler.SaveToJson<TeamData>(td, "TeamData.json");
    }

    void Start()
    {
        
        
        
        
        
    }


    void Update()
    {
        

        for (int x = 0; x < playerChaList.Count; x++)
        {
            GameObject playerChar = playerChaList[x];
            if (playerChar.GetComponentInChildren<UnitStatsss>().charStat.isDed == true)
            {
                if (playerChaList.Contains(playerChar) == true)
                {
                    playerChaList.Remove(playerChar);
                }

                if (deadPlayerChaList.Contains(playerChar) == false)
                {
                    deadPlayerChaList.Add(playerChar);
                    deadPlayerCharacterList = deadPlayerChaList;
                }


            }

        }
    }
}
