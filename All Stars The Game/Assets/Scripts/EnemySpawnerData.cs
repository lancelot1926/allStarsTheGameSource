using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class EnemySpawnerData
{
    public string baseName;
    public int spawnerCount;
    public bool hadASpawner;
    //public List<bool> hadASpawnerList;
   
    //public List<EnemySpawner2> spawners;
    //public string[] spawnerNames;

    public EnemySpawnerData(string basename,int spawnercount,bool hadaspawner)
    {
        baseName = basename;
        spawnerCount = spawnercount;
        hadASpawner = hadaspawner;
        
       
    }
    /*public EnemySpawnerData(GameHandlerMap gameHandlerMap )
    {
        spawnerNames = new string[gameHandlerMap.bstartes.Length];
    }*/
}
