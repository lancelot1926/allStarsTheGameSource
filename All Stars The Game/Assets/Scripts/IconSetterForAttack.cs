using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconSetterForAttack : MonoBehaviour
{
    public List<GameObject> IconsList;
    public GameObject spawnedIcon;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetTheIcon(string element)
    {
        switch (element)
        {
            case "Blind":
                spawnedIcon=Instantiate(IconsList[0], gameObject.transform);
                //sph++;
                break;
            case "Charm":
                spawnedIcon = Instantiate(IconsList[1], gameObject.transform);
                break;
            case "Confuse":
                spawnedIcon = Instantiate(IconsList[2], gameObject.transform);
                break;
            case "Dark":
                spawnedIcon = Instantiate(IconsList[3], gameObject.transform);
                //sph++;
                break;
            case "Electric":
                spawnedIcon = Instantiate(IconsList[4], gameObject.transform);
                break;
            case "Fire":
                spawnedIcon = Instantiate(IconsList[5], gameObject.transform);
                break;
            case "Ice":
                spawnedIcon = Instantiate(IconsList[6], gameObject.transform);
                break;
            case "Light":
                spawnedIcon = Instantiate(IconsList[7], gameObject.transform);
                break;
            case "Physical":
                spawnedIcon = Instantiate(IconsList[8], gameObject.transform);
                break;
            case "Poison":
                spawnedIcon = Instantiate(IconsList[9], gameObject.transform);
                //sph++;
                break;
            case "Rage":
                spawnedIcon = Instantiate(IconsList[10], gameObject.transform);
                break;
            case "Rock":
                spawnedIcon = Instantiate(IconsList[11], gameObject.transform);
                //sph++;
                break;
            case "Sleep":
                spawnedIcon = Instantiate(IconsList[12], gameObject.transform);
                break;
            case "Water":
                spawnedIcon = Instantiate(IconsList[13], gameObject.transform);
                break;
            case "Wind":
                spawnedIcon = Instantiate(IconsList[14], gameObject.transform);
                break;
        }
    }
    public void RemoveIcon()
    {
        Destroy(spawnedIcon);
    }
}
