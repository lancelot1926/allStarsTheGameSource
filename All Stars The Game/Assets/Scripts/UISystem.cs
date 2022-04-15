using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISystem : MonoBehaviour
{
    public GameObject ActionHud;
    public GameObject MagicAttackHud;
    public GameObject SpecialAttackHud;
    public GameObject DeadPlayerListHud;
    public GameObject OnSpriteHealthBar;
    public GameObject ItemsHud;
    public GameObject ItemsHud2;
    public GameObject ActiveCharHud;
    private BattleHandler battleHandle;
    private GameObject actHud;
    private Vector3 charVector;
    private int indexCarrier;
    public GameObject PlayerStatsHud;
    public GameObject EnemyStatusHud;
    public GameObject EnemyStatusHudBase;

    [SerializeField]
    private List<Button> MagicAttackButtons;

    [SerializeField]
    private List<Button> SpecialAttackButtons;

    [SerializeField]
    private List<Button> DeadPlayerButtons;

    [SerializeField]
    private List<Button> ItemButtons;

    [SerializeField]
    private List<Button> ActiveCharButtons;

    
    public List<GameObject> EnemyStatInfoList;

    public List<Transform> PlayerHudBases;

    public List<GameObject> spawnedPlayerStats;
    public List<GameObject> IconsList;
    
    // Start is called before the first frame update
    void Start()
    {
        battleHandle = GameObject.Find("BattleHandler").GetComponent<BattleHandler>();

        
    }

    // Update is called once per frame
    void Update()
    {
        SetMagicAttacks();
        SetSpecials();
        
        SetMagicAttacksButtons();
        SetSpecialsButtons();

        SetDeadPlayers();
        SetDeadPlayerButtons();

        SetItems();
        SetItemButtons();

        SetActivePlayers();
        SetActiveCharButtons();
        
        

        
    }

    public void SetHud()
    {
        ActionHud.transform.position = new Vector2(battleHandle.charBattle.gameObject.transform.position.x, battleHandle.charBattle.gameObject.transform.position.y);
    }

    public void SetHealthBar()
    {
        
        OnSpriteHealthBar.transform.position= new Vector2(battleHandle.enemyBattle.gameObject.transform.position.x, battleHandle.enemyBattle.gameObject.transform.position.y);
    }

    private void SetMagicAttacks()
    {
        for(int i = 0; i < MagicAttackButtons.Count; i++)
        {
            if (battleHandle.playerUnitStats!=null)
            {
                if (i >= battleHandle.playerUnitStats.MagicAttackList.Count)
                {
                    MagicAttackButtons[i].gameObject.SetActive(false);
                    continue;
                }

                string attackName = battleHandle.playerUnitStats.MagicAttackList[i].AttackName;
                
                MagicAttackButtons[i].gameObject.SetActive(true);
                MagicAttackButtons[i].GetComponentInChildren<Text>().text = attackName;

               
                /*string element = battleHandle.playerUnitStats.MagicAttackList[i].Element;
                MagicAttackButtons[i].GetComponentInChildren<IconSetterForAttack>().SetTheIcon(element);*/


            }

           
        }
    }
    public void SetIconsForAttacks()
    {
        for (int i = 0; i < MagicAttackButtons.Count; i++)
        {
            string element = battleHandle.playerUnitStats.MagicAttackList[i].Element;
            MagicAttackButtons[i].GetComponentInChildren<IconSetterForAttack>().SetTheIcon(element);


        }
    }
    public void RemoveIconsForAttacks()
    {
        for (int i = 0; i < MagicAttackButtons.Count; i++)
        {
            
            MagicAttackButtons[i].GetComponentInChildren<IconSetterForAttack>().RemoveIcon();


        }
    }

    private void SetMagicAttacksButtons()
    {
        for (int i = 0; i < MagicAttackButtons.Count; i++)
        {
            Button button = MagicAttackButtons[i];
            int buttonindex = i;
            button.onClick.AddListener(() =>{
                battleHandle.SelectedAttack = battleHandle.playerUnitStats.MagicAttackList[buttonindex].ExecuteAttack;
                AttackManager aprefab = battleHandle.playerUnitStats.MagicAttackList[buttonindex];
                battleHandle.MagicAttack(aprefab);
                MagicAttackHud.gameObject.SetActive(false);

            });
        }
    }

    private void SetSpecialsButtons()
    {
        for (int i = 0; i < SpecialAttackButtons.Count; i++)
        {
            Button button = SpecialAttackButtons[i];
            int buttonindex = i;
            button.onClick.AddListener(() => {
                if (battleHandle.playerUnitStats.SpecialAttackList[buttonindex].EventNameE== "Transformation")
                {
                    battleHandle.ChosenEvent = battleHandle.playerUnitStats.SpecialAttackList[buttonindex].ExecuteEvent;
                    AttackManager eprefab = battleHandle.playerUnitStats.SpecialAttackList[buttonindex];
                    battleHandle.Specials(eprefab,eprefab,eprefab);
                    SpecialAttackHud.gameObject.SetActive(false);
                }

                if (battleHandle.playerUnitStats.SpecialAttackList[buttonindex].EventNameE == "Revive")
                {
                    DeadPlayerListHud.gameObject.SetActive(true);
                    indexCarrier = buttonindex;
                    
                }

                if (battleHandle.playerUnitStats.SpecialAttackList[buttonindex].EventNameE=="Tag Attack")
                {
                    AttackManager s1prefab = battleHandle.playerUnitStats.AllAttackList[Random.Range(0, battleHandle.playerUnitStats.AllAttackList.Count)];
                    AttackManager s2prefab = battleHandle.chosenPlayerUnitStats.AllAttackList[Random.Range(0, battleHandle.chosenPlayerUnitStats.AllAttackList.Count)];
                    battleHandle.SelectedAttack = s1prefab.ExecuteAttack;
                    battleHandle.SelectedAttack2 = s2prefab.ExecuteAttack;
                    AttackManager tprefab = battleHandle.playerUnitStats.SpecialAttackList[buttonindex];
                    
                    battleHandle.Specials(tprefab,s1prefab,s2prefab);
                    SpecialAttackHud.gameObject.SetActive(false);
                }

                /*battleHandle.SelectedAttack = battleHandle.playerUnitStats.SpecialAttackList[buttonindex].ExecuteAttack;
                AttackManager aprefab = battleHandle.playerUnitStats.SpecialAttackList[buttonindex];
                battleHandle.MagicAttack(aprefab);
                SpecialAttackHud.gameObject.SetActive(false);*/

            });
        }
    }

    private void SetSpecials()
    {
        for (int i = 0; i < SpecialAttackButtons.Count; i++)
        {
            if (battleHandle.playerUnitStats != null)
            {
                if (i >= battleHandle.playerUnitStats.SpecialAttackList.Count)
                {
                    SpecialAttackButtons[i].gameObject.SetActive(false);
                    continue;
                }

                string attackName = battleHandle.playerUnitStats.SpecialAttackList[i].AttackName;

                SpecialAttackButtons[i].gameObject.SetActive(true);
                SpecialAttackButtons[i].GetComponentInChildren<Text>().text = attackName;

            }


        }
    }
    private void SetDeadPlayerButtons()
    {
        for (int i = 0; i < DeadPlayerButtons.Count; i++)
        {
            Button button = DeadPlayerButtons[i];
            int buttonindex = i;
            button.onClick.AddListener(() => {


                battleHandle.chosenPlayer = BattleHandler.DeadCharacterList[buttonindex];
                battleHandle.ChosenEvent = battleHandle.playerUnitStats.SpecialAttackList[indexCarrier].ExecuteEvent;
                AttackManager xprefab = battleHandle.playerUnitStats.SpecialAttackList[indexCarrier];
                battleHandle.Specials(xprefab, xprefab, xprefab);

                DeadPlayerListHud.gameObject.SetActive(false);
                SpecialAttackHud.gameObject.SetActive(false);

            });
        }

    }

    private void SetDeadPlayers()
    {
        for (int i = 0; i < DeadPlayerButtons.Count; i++)
        {
            if (i >= BattleHandler.DeadCharacterList.Count)
            {
                DeadPlayerButtons[i].gameObject.SetActive(false);
                continue;
            }

            string playerName = BattleHandler.DeadCharacterList[i].GetComponentInChildren<UnitStatsss>().unitName;

            DeadPlayerButtons[i].gameObject.SetActive(true);
            DeadPlayerButtons[i].GetComponentInChildren<Text>().text = playerName;
        }

    }


    private void SetItems()
    {
        for (int i = 0; i < ItemButtons.Count; i++)
        {
            if (i >= PlayerPartyScript.Items.Count)
            {
                ItemButtons[i].gameObject.SetActive(false);
                continue;
            }

            if (PlayerPartyScript.Items.Count > 10)
            {
                ItemsHud.gameObject.SetActive(true);
            }

            string itemName = PlayerPartyScript.Items[i].Name;
            string itemCount = PlayerPartyScript.Items[i].UsageCount.ToString();

            ItemButtons[i].gameObject.SetActive(true);
            ItemButtons[i].transform.GetChild(0).GetComponent<Text>().text = itemName;
            ItemButtons[i].transform.GetChild(1).GetComponent<Text>().text = itemCount;
        }

    }


    private void SetItemButtons()
    {
        for (int i = 0; i < ItemButtons.Count; i++)
        {
            Button button = ItemButtons[i];
            int buttonindex = i;
            button.onClick.AddListener(() => {


                indexCarrier = buttonindex;

                ActiveCharHud.gameObject.SetActive(true);
                

            });
        }

    }


    private void SetActivePlayers()
    {
        for (int i = 0; i <ActiveCharButtons.Count; i++)
        {
            if (i >= BattleHandler.ActiveCharacterList.Count)
            {
                ActiveCharButtons[i].gameObject.SetActive(false);
                continue;
            }

            string playerName = BattleHandler.ActiveCharacterList[i].GetComponent<UnitStatsss>().unitName;
            

            ActiveCharButtons[i].gameObject.SetActive(true);
            ActiveCharButtons[i].GetComponentInChildren<Text>().text = playerName;

        }

    }



    private void SetActiveCharButtons()
    {
        for (int i = 0; i < ActiveCharButtons.Count; i++)
        {
            Button button = ActiveCharButtons[i];
            int buttonindex = i;
            button.onClick.AddListener(() => {


                battleHandle.chosenPlayer = BattleHandler.ActiveCharacterList[buttonindex];
                Items item =PlayerPartyScript.Items[indexCarrier];
                battleHandle.ItemUse(item);

                ActiveCharHud.gameObject.SetActive(false);
                ItemsHud.gameObject.SetActive(false);
                ItemsHud2.gameObject.SetActive(false);



            });
        }

    }






    public void EnableMagicMenu()
    {
        MagicAttackHud.gameObject.SetActive(!MagicAttackHud.gameObject.activeSelf);
    }
    public void EnableSpecialsMenu()
    {
        SpecialAttackHud.gameObject.SetActive(!SpecialAttackHud.gameObject.activeSelf);
    }
    public void EnableItemsMenu()
    {
        ItemsHud.gameObject.SetActive(!ItemsHud.gameObject.activeSelf);
    }

    public void SetPlayerHuds()
    {

        for (int s = 0; s < battleHandle.spawnedPlayer.Count; s++)
        {
            GameObject spawnedStat = Instantiate(PlayerStatsHud, PlayerHudBases[s]);
            spawnedPlayerStats.Add(spawnedStat);
        }

        for(int s = 0; s < spawnedPlayerStats.Count; s++)
        {
            spawnedPlayerStats[s].transform.GetChild(1).GetComponent<Image>().sprite = battleHandle.spawnedPlayer[s].GetComponentInChildren<UnitStatsss>().PlayerStatssIcon;
            battleHandle.spawnedPlayer[s].GetComponentInChildren<UnitStatsss>().HealthBar = spawnedPlayerStats[s].transform.GetChild(2).GetComponent<HealthBarScript>();
            battleHandle.spawnedPlayer[s].GetComponentInChildren<UnitStatsss>().StatInfo = spawnedPlayerStats[s].transform.GetChild(3).GetComponent<StatusScript>();
            battleHandle.spawnedPlayer[s].GetComponentInChildren<UnitStatsss>().WsInfo = spawnedPlayerStats[s].transform.GetChild(4).GetComponent<WsInfoScript>();
        }



    }

    public void SetEnemyStatusHud()
    {
        for (int s = 0; s < battleHandle.spawnedEnemy.Count; s++)
        {
            GameObject spawnedStatHud = Instantiate(EnemyStatusHud, EnemyStatusHudBase.transform);
            EnemyStatInfoList.Add(spawnedStatHud);
            //battleHandle.spawnedEnemy[s].GetComponentInChildren<UnitStatsss>().StatInfo = EnemyStatInfoLıst[s].GetComponent<StatusScript>();
        }
        for (int s = 0; s < battleHandle.spawnedEnemy.Count; s++)
        {
            battleHandle.spawnedEnemy[s].GetComponent<UnitStatsss>().StatInfo = EnemyStatInfoList[s].GetComponent<StatusScript>();
            battleHandle.spawnedEnemy[s].GetComponent<UnitStatsss>().WsInfo = EnemyStatInfoList[s].GetComponentInChildren<WsInfoScript>();


        }
    }

}
