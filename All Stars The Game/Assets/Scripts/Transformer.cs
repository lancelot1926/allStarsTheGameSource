using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transformer : MonoBehaviour
{
    public GameObject NormalPhase;
    public GameObject SuperPhase;


    // Start is called before the first frame update
    void Start()
    {
        if (transform.GetChild(0).GetComponent<UnitStatsss>().CanTransform == true)
        {
            NormalPhase = transform.GetChild(0).gameObject;
            SuperPhase = transform.GetChild(1).gameObject;
            SuperPhase.GetComponent<UnitStatsss>().HealthBar = NormalPhase.GetComponent<UnitStatsss>().HealthBar;
            SuperPhase.GetComponent<UnitStatsss>().StatInfo = NormalPhase.GetComponent<UnitStatsss>().StatInfo;
            SuperPhase.GetComponent<UnitStatsss>().WsInfo = NormalPhase.GetComponent<UnitStatsss>().WsInfo;
        }
        
    }

    
    public void TransformFunc(Action onTransformComplete) 
    {
        if (SuperPhase.activeInHierarchy)
        {
            SuperPhase.SetActive(false);
            NormalPhase.SetActive(true);

        }

        if (NormalPhase.activeInHierarchy)
        {
            NormalPhase.SetActive(false);
            SuperPhase.SetActive(true);
        }

        onTransformComplete();
    }
    
    
    
}
