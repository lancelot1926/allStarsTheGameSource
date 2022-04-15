using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HealthBarScript : MonoBehaviour
{

    public Image fill;
    public Image manaFill;
    public Text HPText;
    public Text MPText;

    public void SetHealth(int currentHealth,int maxHealth)
    {
        fill.fillAmount = (float)currentHealth/(float)maxHealth;
        
        
    }

    public void SetMana(int currentMana, int maxMana)
    {
        manaFill.fillAmount = (float)currentMana / (float)maxMana;
        
    }

    public void SetTexts(int currentHealth,int currentMana)
    {
        HPText.text = currentHealth.ToString();
        MPText.text = currentMana.ToString();
    }
}
