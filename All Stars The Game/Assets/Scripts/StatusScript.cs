using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusScript : MonoBehaviour
{
    public GameObject immobileActive;
    public GameObject specialActive;
    public GameObject passiveDamageActive;
    public Text immobileType;
    public Text specialType;
    public Text passiveDamageType;
    public Text immobileTurnCount;
    public Text specialTurnCount;
    public Text passiveDamageTurnCount;
    


    public void SetInfo(bool immobile,bool special,bool passiveD)
    {
        if (immobile == true)
        {
            immobileActive.SetActive(true);
        }
        else
        {
            immobileActive.SetActive(false);
        }
        if (special == true)
        {
            specialActive.SetActive(true);
        }
        else
        {
            specialActive.SetActive(false);
        }
        if (passiveD == true)
        {
            passiveDamageActive.SetActive(true);
        }
        else
        {
            passiveDamageActive.SetActive(false);
        }
    }

    public void SetStatusText(string immobilType,string speciType,string passiveType)
    {
        immobileType.text = immobilType;
        specialType.text = speciType;
        passiveDamageType.text = passiveType;
    }

    public void SetTurnCountText(int immobileTurn,int specialTurn,int passiveDTurn)
    {
        immobileTurnCount.text = immobileTurn.ToString();
        specialTurnCount.text = specialTurn.ToString();
        passiveDamageTurnCount.text = passiveDTurn.ToString();
    }

    
}
