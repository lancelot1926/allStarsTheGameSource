using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;


public class GameHandlerMap : MonoBehaviour
{
    private GameObject MapPlayer;

    public GameObject EnemySpawner;
    public List<Transform> SpawnerLocations;
    public List<GameObject> SpawnedSpawners;


    public List<EnemySpawnerData> bStartersData=new List<EnemySpawnerData>();
    public static GameData gd;


    private void Awake()
    {
        MapPlayer = GameObject.Find("MapPlayer");
        gd = new GameData(PlayerPartyScript.LastScene);
        SaveSystem.Init();
    }
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(PlayerPartyScript.LastScene);
        if (PlayerPartyScript.LastScene != "BattleScene"&& PlayerPartyScript.LastScene != "BattleSceneDungeon")
        {
            if (gameObject.scene.name=="MapSceneTwo")
            {
                MapPlayer.transform.position = GameObject.Find("FromMap").transform.position;
            }
            foreach(Transform x in SpawnerLocations)
            {
                GameObject spawnedSpawn = Instantiate(EnemySpawner, x);
                SpawnedSpawners.Add(spawnedSpawn);
            }

            for (int i = 0; i < SpawnerLocations.Count; i++)
            {

                bool had = true;
                EnemySpawnerData esd = new EnemySpawnerData(SpawnerLocations[i].name, SpawnerLocations[i].childCount, had);
                if (bStartersData.Contains(esd) == false)
                {
                    bStartersData.Add(esd);
                }

                /*if (bStartersData[i].baseName == SpawnerLocations[i].name)
                {
                    bStartersData.Remove(bStartersData[i]);
                }*/
            }

        }

        if (PlayerPartyScript.LastScene == "BattleScene"|| PlayerPartyScript.LastScene == "BattleSceneDungeon")
        {
            bStartersData = FileHandler.ReadFromListJson<EnemySpawnerData>("SaveMuch.json");
            string saveString = SaveSystem.Load();
            if (saveString != null)
            {
                SaveObject saveObject = JsonUtility.FromJson<SaveObject>(saveString);

                MapPlayer.transform.position = saveObject.playerPosition;
                
            }

            for (int b = 0; b < bStartersData.Count; b++)
            {
                if (bStartersData[b].hadASpawner == true)
                {
                    Instantiate(EnemySpawner, GameObject.Find(bStartersData[b].baseName).transform);
                }
            }
            /*
            foreach(EnemySpawner2 es2 in ess)
            {
                BattleStarters.Add(es2.gameObject);
            }

            /*foreach(EnemySpawner2 d in bStartersData)
            {

                BattleStarters.Add(d.gameObject);
            }*/

            //EnemySpawnerData data= SaveSystemBinaryStyle.LoadGameMap();

            //BattleStarters = data.spawners;

        }
        if(PlayerPartyScript.LastScene == "LoseScene"|| PlayerPartyScript.LastScene == "StartScene")
        {
            bStartersData = FileHandler.ReadFromListJson<EnemySpawnerData>("GeneralSave.json");
            string saveString = SaveSystem.Load();
            if (saveString != null)
            {
                SaveObject saveObject = JsonUtility.FromJson<SaveObject>(saveString);

                MapPlayer.transform.position = saveObject.playerPosition;

            }
            for (int b = 0; b < bStartersData.Count; b++)
            {
                if (bStartersData[b].hadASpawner == true)
                {
                    Instantiate(EnemySpawner, GameObject.Find(bStartersData[b].baseName).transform);
                }
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void GeneralSave()
    {
        if (gameObject.scene.name == "MapScene")
        {
            PlayerPartyScript.LastScene = "MapScene";
        }
        if (gameObject.scene.name == "MapSceneTwo")
        {
            PlayerPartyScript.LastScene = "MapSceneTwo";
            
        }
        gd.LastScene = PlayerPartyScript.LastScene;
        FileHandler.SaveToJson<GameData>(gd, "GameData.json");
        FileHandler.SaveToJson<EnemySpawnerData>(bStartersData, "GeneralSave.json");
        Vector3 playerP = MapPlayer.transform.position;

        SaveObject saveObject = new SaveObject
        {
            playerPosition = playerP,

        };
        string json = JsonUtility.ToJson(saveObject);
        SaveSystem.Save(json);
    }
    public void Save()
    {
       
        
        
        
        FileHandler.SaveToJson<EnemySpawnerData>(bStartersData, "SaveMuch.json");
        

        Vector3 playerP = MapPlayer.transform.position;

        SaveObject saveObject = new SaveObject
        {
            playerPosition = playerP,

        };
        string json = JsonUtility.ToJson(saveObject);
        SaveSystem.Save(json);
    }


    public void Load()
    {
        bStartersData=FileHandler.ReadFromListJson<EnemySpawnerData>("SaveMuch.json");
        //bStartersData = FileHandler.ReadFromListJson<EnemySpawnerData>("SaveMuch.json");
        
        /*foreach (EnemySpawnerData d in bStartersData)
        {
            es2Data.Add(d.spawner);
        }

        foreach (EnemySpawner2 es2 in es2Data)
        {
            BattleStarters.Add(es2.gameObject);
        }*/


        /*string saveString = SaveSystem.Load();
        if (saveString != null)
        {
            SaveObject saveObject = JsonUtility.FromJson<SaveObject>(saveString);

            MapPlayer.transform.position = saveObject.playerPosition;
            //BattleStarters = saveObject.enemySpawners;
        }
       */


    }


    
    private class SaveObject
    {
        
        public Vector3 playerPosition;
        //public List<GameObject> enemySpawners;
    }
}
