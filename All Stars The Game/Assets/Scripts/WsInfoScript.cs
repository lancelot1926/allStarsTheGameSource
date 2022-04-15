using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WsInfoScript : MonoBehaviour
{
    public List<Transform> StrongplaceHolders;
    public List<Transform> WeakPlaceHolders;
    public List<GameObject> IconsList;
    int sph = 0;
    int wph = 0;

    public void SetICons(List<string> strongs, List<string> weaks) 
    {
        for(int s = 0; s < strongs.Count; s++)
        {
            string element = strongs[s];
            
            switch (element)
            {
                case "Blind":
                    Instantiate(IconsList[0], StrongplaceHolders[sph]);
                    //sph++;
                    break;
                case "Charm":
                    Instantiate(IconsList[1], StrongplaceHolders[sph]);
                    break;
                case "Confuse":
                    Instantiate(IconsList[2], StrongplaceHolders[sph]);
                    break;
                case "Dark":
                    Instantiate(IconsList[3], StrongplaceHolders[sph]);
                    //sph++;
                    break;
                case "Electric":
                    Instantiate(IconsList[4], StrongplaceHolders[sph]);
                    break;
                case "Fire":
                    Instantiate(IconsList[5], StrongplaceHolders[sph]);
                    break;
                case "Ice":
                    Instantiate(IconsList[6], StrongplaceHolders[sph]);
                    break;
                case "Light":
                    Instantiate(IconsList[7], StrongplaceHolders[sph]);
                    break;
                case "Physical":
                    Instantiate(IconsList[8], StrongplaceHolders[sph]);
                    break;
                case "Poison":
                    Instantiate(IconsList[9], StrongplaceHolders[sph]);
                    //sph++;
                    break;
                case "Rage":
                    Instantiate(IconsList[10], StrongplaceHolders[sph]);
                    break;
                case "Rock":
                    Instantiate(IconsList[11], StrongplaceHolders[sph]);
                    //sph++;
                    break;
                case "Sleep":
                    Instantiate(IconsList[12], StrongplaceHolders[sph]);
                    break;
                case "Water":
                    Instantiate(IconsList[13], StrongplaceHolders[sph]);
                    break;
                case "Wind":
                    Instantiate(IconsList[14], StrongplaceHolders[sph]);
                    break;
            }

            sph++;

            /*for (int sph = 0; sph < StrongplaceHolders.Count; sph++)
            {

            }  */
        }

        for (int s = 0; s < weaks.Count; s++)
        {
            string element = weaks[s];
            switch (element)
            {
                case "Blind":
                    Instantiate(IconsList[0], WeakPlaceHolders[wph]);
                    break;
                case "Charm":
                    Instantiate(IconsList[1], WeakPlaceHolders[wph]);
                    break;
                case "Confuse":
                    Instantiate(IconsList[2], WeakPlaceHolders[wph]);
                    break;
                case "Dark":
                    Instantiate(IconsList[3], WeakPlaceHolders[wph]);
                    break;
                case "Electric":
                    Instantiate(IconsList[4], WeakPlaceHolders[wph]);
                    break;
                case "Fire":
                    Instantiate(IconsList[5], WeakPlaceHolders[wph]);
                    break;
                case "Ice":
                    Instantiate(IconsList[6], WeakPlaceHolders[wph]);
                    break;
                case "Light":
                    Instantiate(IconsList[7], WeakPlaceHolders[wph]);
                    break;
                case "Physcial":
                    Instantiate(IconsList[8], WeakPlaceHolders[wph]);
                    break;
                case "Poison":
                    Instantiate(IconsList[9], WeakPlaceHolders[wph]);
                    break;
                case "Rage":
                    Instantiate(IconsList[10], WeakPlaceHolders[wph]);
                    break;
                case "Rock":
                    Instantiate(IconsList[11], WeakPlaceHolders[wph]);
                    break;
                case "Sleep":
                    Instantiate(IconsList[12], WeakPlaceHolders[wph]);
                    break;
                case "Water":
                    Instantiate(IconsList[13], WeakPlaceHolders[wph]);
                    break;
                case "Wind":
                    Instantiate(IconsList[14], WeakPlaceHolders[wph]);
                    break;
            }

            wph++;
        }

        
    }
   
}
