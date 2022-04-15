using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TeamData
{
    public List<GameObject> playerTeamList;
    public List<GameObject> absolutePlayerTeamList;
    public List<GameObject> deadPlayerTeamList;


    public TeamData(List<GameObject> ptl,List<GameObject> aptl,List<GameObject> dptl)
    {
        playerTeamList = ptl;
        absolutePlayerTeamList = aptl;
        deadPlayerTeamList = dptl;
    }
}
