using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MapUI : MonoBehaviour
{
    public int x;
    private int l;
    [SerializeField]
    private GameObject MainMenu;

    [SerializeField]
    private GameObject TeamMenu;

    [SerializeField]
    private GameObject TeamMenuForItems;

    [SerializeField]
    private GameObject ItemMenu;

    [SerializeField]
    private GameObject TeamMenuPartyPickPanel;
    
    [SerializeField]
    private GameObject TeamMenuHealthPanel;

    [SerializeField]
    private Button PlayerCharButton;

    [SerializeField]
    private GameObject PlayerTeamInfo;

    [SerializeField]
    private GameObject ItemPanel;


    [SerializeField]
    private List<Button> PlayerCharPartyButtonsList;

    [SerializeField]
    private List<Button> PlayerCharHealthButtonsList;
    
    [SerializeField]
    private List<Button> ItemsButtonsList;


    public static List<GameObject> PlayableCharacterList;

    [SerializeField]
    private List<GameObject> PlayableCharList;


    [SerializeField]
    private List<GameObject> CharsAreInTeamList;


    void Awake()
    {

    }
    void Start()
    {
        x = PlayerPartyScript.absolutePlayerCharacterList.Count;
        PlayableCharacterList = PlayableCharList;
        SetPlayerChars();
        SetPlayerCharButtons();
    }

    
   

    // Update is called once per frame
    void Update()
    {
        SetStatInfo();
        for (int p = 0; p < PlayableCharList.Count; p++)
        {
            

            if (PlayerPartyScript.deadPlayerCharacterList.Contains(PlayableCharList[p]) == true)
            {
                PlayerCharHealthButtonsList[p].GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
            }
            for (int c = 0; c < PlayerPartyScript.Items.Count; c++)
            {
                ItemsButtonsList[c].GetComponentInChildren<TextMeshProUGUI>().text = PlayerPartyScript.Items[c].Name + " " + PlayerPartyScript.Items[c].UsageCount;
            }
            for(int c = 0; c < PlayableCharList.Count; c++)
            {
                UnitStatsss charStat = PlayableCharList[c].GetComponentInChildren<UnitStatsss>();
                PlayerCharHealthButtonsList[c].GetComponentInChildren<TextMeshProUGUI>().text = charStat.unitName+"  HP:"+ charStat.charStat.charCurrentHp+" MP:"+charStat.charStat.CharCurrentMp;
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MainMenu.SetActive(!MainMenu.activeSelf);
            
        }
    }


    private void SetPlayerChars()
    {
        for(int p = 0; p < PlayableCharList.Count; p++)
        {
            PlayerCharPartyButtonsList.Add(Instantiate(PlayerCharButton, TeamMenuPartyPickPanel.transform));

            PlayerCharHealthButtonsList.Add(Instantiate(PlayerCharButton, TeamMenuHealthPanel.transform));

            PlayerCharPartyButtonsList[p].GetComponentInChildren<TextMeshProUGUI>().text= PlayableCharList[p].name;
           

            if (PlayerPartyScript.playerCharacterList.Contains(PlayableCharList[p]) == true)
            {
                PlayerCharPartyButtonsList[p].GetComponentInChildren<TextMeshProUGUI>().color = Color.green;
            }
            
            

        }

        for(int c = 0; c < PlayerPartyScript.Items.Count; c++)
        {
            ItemsButtonsList.Add(Instantiate(PlayerCharButton, ItemPanel.transform));

            //ItemsButtonsList[c].GetComponentInChildren<TextMeshProUGUI>().text = PlayerPartyScript.Items[c].Name+" "+ PlayerPartyScript.Items[c].UsageCount;
        }

    }


    private void SetPlayerCharButtons()
    {
        for (int x = 0; x < PlayerCharPartyButtonsList.Count; x++)
        {
            int buttonindex = x;
            

            PlayerCharPartyButtonsList[x].onClick.AddListener(() => {

                SetCharFunc(buttonindex);

            });
        }

        for (int y = 0; y < PlayerCharHealthButtonsList.Count; y++)
        {
            int buttonin = y;


            PlayerCharHealthButtonsList[y].onClick.AddListener(() => {

                ItemMenu.SetActive(true);
                l = buttonin;

            });
        }

        for(int i = 0; i < ItemsButtonsList.Count; i++)
        {
            int buttoni = i;
            Items item = PlayerPartyScript.Items[buttoni];
            ItemsButtonsList[i].onClick.AddListener(()=> {
                
                SetHealthFunc(l,item);

            });
        }



    }



    private void SetCharFunc(int buttonindex)
    {
        if (PlayerPartyScript.absolutePlayerCharacterList.Contains(PlayableCharList[buttonindex]) == true && PlayerPartyScript.absolutePlayerCharacterList.Count >= 2)
        {
            PlayerPartyScript.playerCharacterList.Remove(PlayableCharList[buttonindex]);
            PlayerPartyScript.absolutePlayerCharacterList.Remove(PlayableCharList[buttonindex]);
            PlayerCharPartyButtonsList[buttonindex].GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
            PlayerTeamInfo.transform.GetChild(x).gameObject.SetActive(false);
            x--;
            FileHandler.SaveToJson<TeamData>(PlayerPartyScript.td, "TeamData.json");
        }
        else if (PlayerPartyScript.absolutePlayerCharacterList.Contains(PlayableCharList[buttonindex]) == false && PlayerPartyScript.absolutePlayerCharacterList.Count < 4)
        {
            PlayerPartyScript.playerCharacterList.Add(PlayableCharList[buttonindex]);
            PlayerPartyScript.absolutePlayerCharacterList.Add(PlayableCharList[buttonindex]);
            PlayerCharPartyButtonsList[buttonindex].GetComponentInChildren<TextMeshProUGUI>().color = Color.green;
            x++;
            FileHandler.SaveToJson<TeamData>(PlayerPartyScript.td, "TeamData.json");
        }
    }

    
    private void SetHealthFunc(int buttonindex,Items item)
    {
        if (item.UsageCount > 0)
        {
            item.ExecuteItemEffect(PlayableCharList[buttonindex].GetComponentInChildren<UnitStatsss>());
            if (PlayerPartyScript.deadPlayerCharacterList.Contains(PlayableCharList[buttonindex]) == true)
            {


                PlayerPartyScript.deadPlayerCharacterList.Remove(PlayableCharList[buttonindex]);
                if (PlayerPartyScript.absolutePlayerCharacterList.Contains(PlayableCharList[buttonindex]))
                {
                    PlayerPartyScript.playerCharacterList.Add(PlayableCharList[buttonindex]);
                }
                PlayerCharHealthButtonsList[buttonindex].GetComponentInChildren<TextMeshProUGUI>().color = Color.white;


            }
        }

        
        /*else if (PlayerPartyScript.absolutePlayerCharacterList.Contains(PlayableCharList[buttonindex]) == false && PlayerPartyScript.absolutePlayerCharacterList.Count < 4)
        {
            PlayerPartyScript.playerCharacterList.Add(PlayableCharList[buttonindex]);
            PlayerPartyScript.absolutePlayerCharacterList.Add(PlayableCharList[buttonindex]);
            PlayerCharPartyButtonsList[buttonindex].GetComponentInChildren<TextMeshProUGUI>().color = Color.green;
            x++;
            FileHandler.SaveToJson<TeamData>(PlayerPartyScript.td, "TeamData.json");
        }*/
    }


    private void SetStatInfo()
    {
        for (int i = 0; i < PlayerPartyScript.absolutePlayerCharacterList.Count; i++)
        {
            PlayerTeamInfo.transform.GetChild(i+1).gameObject.SetActive(true);
            
        }

        
        PlayerTeamInfo.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = PlayerPartyScript.absolutePlayerCharacterList[0].GetComponentInChildren<UnitStatsss>().unitName + " HP:" + PlayerPartyScript.absolutePlayerCharacterList[0].GetComponentInChildren<UnitStatsss>().charStat.charCurrentHp + " MP:" + PlayerPartyScript.absolutePlayerCharacterList[0].GetComponentInChildren<UnitStatsss>().charStat.CharCurrentMp;
        
        
        if (PlayerTeamInfo.transform.GetChild(2).gameObject.activeSelf == true)
        {
            PlayerTeamInfo.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = PlayerPartyScript.absolutePlayerCharacterList[1].GetComponentInChildren<UnitStatsss>().unitName + " HP:" + PlayerPartyScript.absolutePlayerCharacterList[1].GetComponentInChildren<UnitStatsss>().charStat.charCurrentHp + " MP:" + PlayerPartyScript.absolutePlayerCharacterList[1].GetComponentInChildren<UnitStatsss>().charStat.CharCurrentMp;
        }
        if (PlayerTeamInfo.transform.GetChild(3).gameObject.activeSelf == true)
        {
            PlayerTeamInfo.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = PlayerPartyScript.absolutePlayerCharacterList[2].GetComponentInChildren<UnitStatsss>().unitName + " HP:" + PlayerPartyScript.absolutePlayerCharacterList[2].GetComponentInChildren<UnitStatsss>().charStat.charCurrentHp + " MP:" + PlayerPartyScript.absolutePlayerCharacterList[2].GetComponentInChildren<UnitStatsss>().charStat.CharCurrentMp;
        }
        if (PlayerTeamInfo.transform.GetChild(4).gameObject.activeSelf == true)
        {
            PlayerTeamInfo.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = PlayerPartyScript.absolutePlayerCharacterList[3].GetComponentInChildren<UnitStatsss>().unitName + " HP:" + PlayerPartyScript.absolutePlayerCharacterList[3].GetComponentInChildren<UnitStatsss>().charStat.charCurrentHp + " MP:" + PlayerPartyScript.absolutePlayerCharacterList[3].GetComponentInChildren<UnitStatsss>().charStat.CharCurrentMp;
        }

    }


    public void OpenItemsMenu()
    {
        TeamMenuForItems.SetActive(true);
        MainMenu.SetActive(false);
    }

    public void OpenTeamMenu()
    {
        TeamMenu.SetActive(true);
        MainMenu.SetActive(false);
    }

    public void CloseTeamMenu()
    {
        TeamMenu.SetActive(false);
    }

    public void CloseItemsMenu()
    {
        TeamMenuForItems.SetActive(false);
        ItemMenu.SetActive(false);

    }

    public void Quit()
    {
        Application.Quit();
    }
}
