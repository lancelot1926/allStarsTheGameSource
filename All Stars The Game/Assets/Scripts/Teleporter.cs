using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleporter : MonoBehaviour
{
    [SerializeField]
    private GameHandlerMap ghm;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (ghm.SpawnedSpawners.Count == 0)
        {
            PlayerPartyScript.LastScene = "MapScene";
            SceneManager.LoadScene("MapSceneTwo");
        }
        
    }
}
