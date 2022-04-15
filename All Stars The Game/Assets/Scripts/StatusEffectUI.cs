using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StatusEffectUI : MonoBehaviour
{
    public GameObject statusInfo;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseOver()
    {
        statusInfo.SetActive(true);
    }
    private void OnMouseExit()
    {
        statusInfo.SetActive(false);
    }
}
