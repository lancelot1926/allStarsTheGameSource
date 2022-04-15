using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestScript : MonoBehaviour
{
    public GameObject CloseChest;
    public GameObject OpenChest;
    public GameObject Drop;
    public bool IsItOpen;
    public bool IsItEmpty=false;
    void Start()
    {
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "MapPlayer")
        {
            IsItOpen = true;
            CloseChest.SetActive(false);
            OpenChest.SetActive(true);
            if (IsItEmpty == false)
            {
                Drop.SetActive(true);
            }
            
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("col");
        if (collision.gameObject.tag == "MapPlayer")
        {
            
            if (IsItOpen&&IsItEmpty==false)
            {
                if (Input.GetKeyDown(KeyCode.Y))
                {
                    Drop.SetActive(false);
                    IsItEmpty = true;
                }
            }
        }
    }

    
}
