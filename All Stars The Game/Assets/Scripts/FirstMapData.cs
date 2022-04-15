using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class FirstMapData
{
    public GameObject Spawner;
    public List<GameObject> SpawnerList;


    public FirstMapData(GameObject spawner)
    {
        spawner=Spawner;
        SpawnerList.Add(Spawner);
    }
}
