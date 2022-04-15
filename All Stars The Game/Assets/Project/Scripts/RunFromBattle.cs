using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RunFromBattle : MonoBehaviour
{
    [SerializeField]
    private float runningChance = 0.5f;
    
    
    public void TryRuning()
    {
        float randomNumber = Random.value;
        if (randomNumber < runningChance)
        {
            SceneManager.LoadScene("Town");
        }
        else
        {
            GameObject.Find("TurnSystem").GetComponent<TurnSystem>().NextTurn();
        }
    }
}
