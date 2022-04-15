using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHandler : MonoBehaviour
{
    public UnitStatsss CurrentTurnChar;
    public List<GameObject> playerChar;
    public List<Transform> playerTeamBase;
    public List<GameObject> spawnedPlayer;
    public CharacterBattle charBattle;
    public UnitStatsss playerUnitStats;
    private AnimationHandler aHandlerPlayer;
    public CharacterBattle TargetedPlayerForAnAttack;
    private Transformer transformer;
    private UISystem uiSystem;

    public List<GameObject> enemyChar;
    public List<Transform> enemyTeamBase;
    public List<GameObject> spawnedEnemy;
    public CharacterBattle enemyBattle;
    public CharacterBattle chosenEnemy;
    public CharacterBattle chosenPlayer;
    public UnitStatsss chosenEnemyUnitStats;
    public UnitStatsss chosenPlayerUnitStats;
    private UnitStatsss enemyUnitStats;
    public List<CharacterBattle> enemyCharBattleList;

    private AnimationHandler aHandlerEnemy;

    public static List<CharacterBattle> DeadCharacterList;
    public List<CharacterBattle> DeadChaList;

    public static List<CharacterBattle> ActiveCharacterList;
    public List<CharacterBattle> ActiveChaList;

    public Action<Action,AnimationHandler,CharacterBattle> SelectedAttack;
    public Action<Action, AnimationHandler, CharacterBattle> SelectedAttack2;
    public Action<Action, AnimationHandler, CharacterBattle> RandomSelectedAttack;
    public Action<Action, AnimationHandler, CharacterBattle> ChosenEvent;

    public List<UnitStatsss> PlayerUnitStatsList;
    public List<UnitStatsss> EnemyUnitStatsList;
    public List<UnitStatsss> EveryCharList;
    public List<CharacterBattle> EveryCharBattleList;
    public int playerIndex;
    public int enemyIndex;
    public int turnIndex;

    [SerializeField]
    private List<GameObject> statusEffectVFX;
    private float cooldown;
    public bool Busy;
    private float timeForNextAttack;
    public bool playerWon;
    public bool enemyWon;
    private State state;
    private List<GameObject> spawnedBunshinList;
    public Vector3 shootDir;

    bool calledOnce;
    bool calledOnce2;
    bool calledOnce3;
    bool calledOnce4;
    bool calledOnce5;
    bool calledOnce6;

    bool debugOnce;
    bool debugOnce2;
    bool debugOnce3;
    bool debugOnce4;
    bool debugOnce5;
    bool debugOnce6;
    bool debugOnce7;
    bool debugOnce8;
    bool debugOnce9;
    
    private enum State
    {
        BattleStart,
        TurnStart,
        CheckingPassive,
        CheckingLimitattionsImmobile,
        CheckingLimitationsSpecial,
        PlayerTurn,
        EnemyTurn,
        Busy,
        EndTurn,
        BattleEnd,
    }

    private void Awake()
    {
        
        playerChar = PlayerPartyScript.playerCharacterList;
    }
    private void Start()
    {
        ActiveCharacterList = ActiveChaList;
        DeadCharacterList = DeadChaList;
        enemyWon = false;
        playerWon = false;
        uiSystem = GameObject.Find("UI").GetComponent<UISystem>();
        Busy = false;
        SpawnPlayer();
        
        SpawnEnemy();
        
        state = State.Busy;

        EveryCharList = new List<UnitStatsss>();
        spawnedBunshinList = new List<GameObject>();

        enemyCharBattleList = new List<CharacterBattle>();
        EnemyUnitStatsList = new List<UnitStatsss>();
        foreach (GameObject enemyUnit in spawnedEnemy)
        {
            CharacterBattle currentEnemyCharBattle = enemyUnit.GetComponent<CharacterBattle>();
            enemyCharBattleList.Add(currentEnemyCharBattle);
            UnitStatsss currentEnemyUnitStats = enemyUnit.GetComponent<UnitStatsss>();
            EnemyUnitStatsList.Add(currentEnemyUnitStats);
            EveryCharList.Add(currentEnemyUnitStats);
            EveryCharBattleList.Add(currentEnemyCharBattle);
        }
        
        PlayerUnitStatsList = new List<UnitStatsss>();
        foreach (GameObject playerUnit in spawnedPlayer)
        {
            UnitStatsss currentPlayerUnitStats = playerUnit.GetComponentInChildren<UnitStatsss>();
            PlayerUnitStatsList.Add(currentPlayerUnitStats);
            ActiveChaList.Add(currentPlayerUnitStats.GetComponent<CharacterBattle>());
            EveryCharList.Add(currentPlayerUnitStats);
            EveryCharBattleList.Add(currentPlayerUnitStats.GetComponent<CharacterBattle>());
        }


        EnemyUnitStatsList.Sort((x,y) => y.speed.CompareTo(x.speed));
        PlayerUnitStatsList.Sort((x, y) => y.speed.CompareTo(x.speed));
        EveryCharList.Sort((x, y) => y.speed.CompareTo(x.speed));

        foreach (UnitStatsss stat in EveryCharList)
        {
            Debug.Log(stat);
        }
        
        cooldown = 0;
        timeForNextAttack = cooldown;
        
        uiSystem.SetPlayerHuds();
        uiSystem.SetEnemyStatusHud();
        state = State.BattleStart;
        debugOnce = false;
        debugOnce2 = false;
        debugOnce3 = false;
        debugOnce4 = false;
        debugOnce5 = false;
        debugOnce6 = false;
        debugOnce7 = false;
        debugOnce8 = false;
        debugOnce9 = false;

        calledOnce = false;
        calledOnce2 = false;
        calledOnce3 = false;
        calledOnce4 = false;
        calledOnce5 = false;
        calledOnce6 = false;

    }

    private void TurnSystem()
    {
        switch (state)
        {
            case State.BattleStart:
                if (debugOnce == false)
                {
                    Debug.Log(state);
                    debugOnce = true;
                }
                StartCoroutine(EventDelayer(1f, () => {
                    state = State.TurnStart;
                }));
                break;
            case State.TurnStart:
                for (int x = 0; x < enemyCharBattleList.Count; x++)
                {
                    if (enemyCharBattleList[x].GetComponent<UnitStatsss>().GuardUp == false)
                    {
                        enemyCharBattleList[x].GetComponent<AnimationHandler>().IdleAnim();
                    }
                    
                }
                EnemyUnitSetter();
                PlayerUnitSetter();
                if (calledOnce6 == false)
                {
                    uiSystem.SetIconsForAttacks();
                    calledOnce6 = true;
                }
                if (debugOnce2 == false)
                {
                    Debug.Log(state);
                    debugOnce2 = true;
                }
                if (turnIndex < EveryCharList.Count)
                {
                    UnitStatsss currentChar = EveryCharList[turnIndex];
                    CurrentTurnChar = currentChar;
                    if (calledOnce == false)
                    {
                        Debug.Log(CurrentTurnChar.unitName);
                        calledOnce = true;
                    }
                }
                else
                {
                    turnIndex = 0;
                }
                debugOnce7 = false;
                StartCoroutine(EventDelayer(1f, () => {
                    state = State.CheckingPassive;
                }));
                
                break;
            case State.CheckingPassive:
                if (debugOnce3 == false)
                {
                    Debug.Log(state);
                    debugOnce3 = true;
                }
                PassiveDamage();
                StartCoroutine(EventDelayer(2f, () => {
                    state = State.CheckingLimitattionsImmobile;
                }));
                break;
            case State.CheckingLimitattionsImmobile:
                if (debugOnce4 == false)
                {
                    Debug.Log(state);
                    debugOnce4 = true;
                }
                if (calledOnce4 == false)
                {
                    Immobilization();
                    calledOnce4 = true;
                }
                
                break;
            case State.CheckingLimitationsSpecial:
                if (debugOnce8 == false)
                {
                    Debug.Log(state);
                    debugOnce8 = true;
                }
                if (calledOnce5 == false)
                {
                    SpecialStatusEffect();
                    calledOnce5 = true;
                }
                break;
            case State.PlayerTurn:
                if (debugOnce5 == false)
                {
                    Debug.Log(state);
                    debugOnce5 = true;
                }
                if (calledOnce2 == false)
                {
                    uiSystem.SetHud();
                    uiSystem.ActionHud.gameObject.SetActive(true);
                    calledOnce2 = true;
                }
                
                break;
            case State.EnemyTurn:
                if (debugOnce6 == false)
                {
                    Debug.Log(state);
                    debugOnce6 = true;
                }
                if (calledOnce3 == false)
                {
                    EnemyAttack();
                    calledOnce3 = true;
                }
                
                break;
            case State.Busy:
                
                break;
            case State.EndTurn:
                if (debugOnce7 == false)
                {
                    Debug.Log(state);
                    debugOnce7 = true;
                }
               
                uiSystem.RemoveIconsForAttacks();
                debugOnce = false;
                debugOnce2 = false;
                debugOnce3 = false;
                debugOnce4 = false;
                debugOnce5 = false;
                debugOnce6 = false;
                debugOnce8 = false;
                calledOnce = false;
                calledOnce2 = false;
                calledOnce3 = false;
                calledOnce4 = false;
                calledOnce5 = false;
                calledOnce6 = false;
                StartCoroutine(EventDelayer(2f, () => {
                    state = State.TurnStart;
                }));
                break;

        }
    }

    private void Update()
    {
        TurnSystem();


        if (Input.GetKeyDown(KeyCode.L))
        {
            playerWon = true;
        }
        //PassiveDamage();
        /*      


       

        if (chosenEnemy == null&&enemyCharBattleList.Count!=0)
        {
            chosenEnemy = enemyCharBattleList[UnityEngine.Random.Range(0, enemyCharBattleList.Count)];
        }
        if (chosenEnemy.GetComponent<UnitStatsss>().IsDead == true&& enemyCharBattleList.Count != 0)
        {
            chosenEnemy = enemyCharBattleList[UnityEngine.Random.Range(0, enemyCharBattleList.Count)];
        }
        


        /*if (Input.GetKeyDown(KeyCode.B))
        {
            
            GameObject bunshin = Instantiate(playerUnitStats.Clone,playerUnitStats.transform.parent);
            bunshin.transform.position = new Vector2(playerUnitStats.transform.position.x + 20, playerUnitStats.transform.position.y + 20);
            StartCoroutine(EventDelayer(1f,()=> {
                SelectedAttack = playerUnitStats.NormalAttack.ExecuteAttack;
                bunshin.GetComponent<CharacterBattle>().Attack(chosenEnemy, SelectedAttack, playerUnitStats.NormalAttack, bunshin.GetComponent<AnimationHandler>(), () => { });

                charBattle.Attack(chosenEnemy, SelectedAttack, playerUnitStats.NormalAttack, aHandlerPlayer, () => {
                    StartCoroutine(EventDelayer(7f, () => {
                        Destroy(bunshin);
                    }));
                });
            }));
        }*/


    }

    private void SpawnEnemy()
    {
        if(gameObject.scene.name=="BattleScene"|| gameObject.scene.name == "BattleSceneDungeon")
        {
            int deleteCount = UnityEngine.Random.Range(0, 5);

            enemyTeamBase.RemoveRange(0, deleteCount);

            foreach (Transform enemyBase in enemyTeamBase)
            {
                GameObject spawnedE = Instantiate(enemyChar[UnityEngine.Random.Range(0, enemyChar.Count)], enemyBase);
                spawnedEnemy.Add(spawnedE);

            }
        }
        if (gameObject.scene.name == "BattleSceneBoss")
        {
            foreach (Transform enemyBase in enemyTeamBase)
            {
                GameObject spawnedE = Instantiate(enemyChar[0], enemyBase);
                spawnedEnemy.Add(spawnedE);

            }
        }

        
    }

    private void SpawnPlayer()
    {       
        for(int i = 0; i < playerChar.Count; i++)
        {
            GameObject spawnedP =Instantiate(playerChar[i], playerTeamBase[i]);
            spawnedPlayer.Add(spawnedP);

        }
        
    }

    private void EnemyUnitSetter()
    {
        if (enemyIndex <EnemyUnitStatsList.Count)
        {
            UnitStatsss currentUnit = EnemyUnitStatsList[enemyIndex];
            enemyBattle = currentUnit.GetComponent<CharacterBattle>();
            enemyUnitStats = enemyBattle.GetComponent<UnitStatsss>();
            aHandlerEnemy = enemyBattle.GetComponent<AnimationHandler>();
            
        }
        else
        {
            enemyIndex = 0;
        }
    }

    private void PlayerUnitSetter()
    {
        if (playerIndex<PlayerUnitStatsList.Count)
        {
            UnitStatsss currentUnit = PlayerUnitStatsList[playerIndex];
            charBattle = currentUnit.GetComponent<CharacterBattle>();
            playerUnitStats = charBattle.GetComponent<UnitStatsss>();
            aHandlerPlayer = charBattle.GetComponent<AnimationHandler>();
            transformer = charBattle.GetComponentInParent<Transformer>();
            
        }
        else
        {
            playerIndex = 0;
        }
    }


    private void TurnSetter()
    {

        StartCoroutine(EventDelayer(3f, () => {

            if (turnIndex < EveryCharList.Count)
            {
                UnitStatsss currentChar = EveryCharList[turnIndex];
                CurrentTurnChar = currentChar;
                PassiveDamage();
                Immobilization();
                SpecialStatusEffect();



                if (currentChar.tag == "Player")
                {
                    state = State.PlayerTurn;

                }

                if (currentChar.tag == "Enemy")
                {
                    state = State.EnemyTurn;
                }


            }
            else
            {
                turnIndex = 0;
            }

            
        }));

    }

    private void PassiveDamageReloader(UnitStatsss victim)
    {
        if (victim.turnCountPassiveDamaege > 0)
        {
            switch (victim.passiveDamageType)
            {
                case "Burning":
                    StartCoroutine(EventDelayer(9f, () =>
                    {
                        victim.burning = true;
                    }));
                    break;

                case "Poisoned":
                    StartCoroutine(EventDelayer(9f, () =>
                    {
                        victim.poisoned = true;
                    }));

                    break;

                case "Cursed":
                    StartCoroutine(EventDelayer(9f, () =>
                    {
                        victim.cursed = true;
                    }));

                    break;

                case "Bleeding":
                    StartCoroutine(EventDelayer(9f, () =>
                    {
                        victim.bleeding = true;
                    }));

                    break;

            }
        }
    }
    
    private void PassiveDamage()
    {
        
        int passiveDamage =CurrentTurnChar.PassiveDamageCalculator(CurrentTurnChar);
        if (CurrentTurnChar.takingPassiveDamage==true)
        {
            if (CurrentTurnChar.burning == true)
            {
                CurrentTurnChar.GetComponent<UnitStatsss>().RecieveDamage(passiveDamage);
                Debug.Log(passiveDamage);
                CurrentTurnChar.GetComponent<AnimationHandler>().HitAnim();
                GameObject passiveEffect=Instantiate(statusEffectVFX[0], CurrentTurnChar.gameObject.transform);
                CurrentTurnChar.GetComponent<UnitStatsss>().turnCountPassiveDamaege -= 1;
                StartCoroutine(EventDelayer(3f, () =>
                {
                    CurrentTurnChar.GetComponent<AnimationHandler>().IdleAnim();
                    Destroy(passiveEffect);
                }));
            }
            CurrentTurnChar.burning = false;


            if (CurrentTurnChar.wet == true)
            {
                GameObject passiveEffect = Instantiate(statusEffectVFX[1], CurrentTurnChar.gameObject.transform);
                CurrentTurnChar.GetComponent<UnitStatsss>().turnCountPassiveDamaege -= 1;

            }
            //currentChar.wet = false;


            if (CurrentTurnChar.poisoned == true)
            {

                CurrentTurnChar.GetComponent<UnitStatsss>().RecieveDamage(passiveDamage);
                CurrentTurnChar.GetComponent<AnimationHandler>().HitAnim();
                GameObject passiveEffect = Instantiate(statusEffectVFX[2], CurrentTurnChar.gameObject.transform);
                CurrentTurnChar.GetComponent<UnitStatsss>().turnCountPassiveDamaege -= 1;
                StartCoroutine(EventDelayer(3f, () =>
                {
                    Destroy(passiveEffect);
                }));
            }
            CurrentTurnChar.poisoned = false;


            if (CurrentTurnChar.cursed == true)
            {

                CurrentTurnChar.GetComponent<UnitStatsss>().RecieveDamage(passiveDamage);
                CurrentTurnChar.GetComponent<AnimationHandler>().HitAnim();
                GameObject passiveEffect = Instantiate(statusEffectVFX[3], CurrentTurnChar.gameObject.transform);
                CurrentTurnChar.GetComponent<UnitStatsss>().turnCountPassiveDamaege -= 1;
                StartCoroutine(EventDelayer(3f, () =>
                {
                    Destroy(passiveEffect);
                }));
            }
            CurrentTurnChar.cursed = false;



            if (CurrentTurnChar.bleeding == true)
            {

                CurrentTurnChar.GetComponent<UnitStatsss>().RecieveDamage(passiveDamage);
                CurrentTurnChar.GetComponent<AnimationHandler>().HitAnim();
                GameObject passiveEffect = Instantiate(statusEffectVFX[4], CurrentTurnChar.gameObject.transform);
                CurrentTurnChar.GetComponent<UnitStatsss>().turnCountPassiveDamaege -= 1;
                StartCoroutine(EventDelayer(3f, () =>
                {
                    Destroy(passiveEffect);
                }));
            }
            CurrentTurnChar.bleeding = false;


            if (CurrentTurnChar.GetComponent<UnitStatsss>().turnCountPassiveDamaege == 0)
            {
                CurrentTurnChar.takingPassiveDamage = false;
                switch (CurrentTurnChar.passiveDamageType)
                {
                    case "Burning":
                        CurrentTurnChar.burning = false;
                        break;

                    case "Poisoned":
                        CurrentTurnChar.poisoned = false;

                        break;

                    case "Cursed":
                        CurrentTurnChar.cursed = false;

                        break;

                    case "Bleeding":
                        CurrentTurnChar.bleeding = false;

                        break;

                }
            }


        }
        


    }

    private void Immobilization()
    {
        

        if (CurrentTurnChar.immobilized == true)
        {
            if (CurrentTurnChar.underSpecialStatus == true)
            {
                CurrentTurnChar.turnCountSpecialStatus -= 1;
            }
            if (CurrentTurnChar.petrified == true)
            {
                if (CurrentTurnChar.tag == "Player")
                {
                    PassiveDamageReloader(CurrentTurnChar);
                    StartCoroutine(EventDelayer(2.5f, () =>
                    {
                        playerIndex++;
                        turnIndex++;
                        CurrentTurnChar.GetComponent<UnitStatsss>().turnCountImmobilization -= 1;
                        state = State.EndTurn;

                    }));
                    

                }


                if (CurrentTurnChar.tag == "Enemy")
                {
                    PassiveDamageReloader(CurrentTurnChar);
                    StartCoroutine(EventDelayer(2.5f, () =>
                    {
                        enemyIndex++;
                        turnIndex++;
                        CurrentTurnChar.GetComponent<UnitStatsss>().turnCountImmobilization -= 1;
                        state = State.EndTurn;

                    }));
                    
                }


            }

            if (CurrentTurnChar.feared == true)
            {
                if (CurrentTurnChar.tag == "Player")
                {
                    PassiveDamageReloader(CurrentTurnChar);
                    StartCoroutine(EventDelayer(2.5f, () =>
                    {
                        playerIndex++;
                        turnIndex++;
                        CurrentTurnChar.GetComponent<UnitStatsss>().turnCountImmobilization -= 1;
                        state = State.EndTurn;
                    }));
                    
                }


                if (CurrentTurnChar.tag == "Enemy")
                {
                    PassiveDamageReloader(CurrentTurnChar);
                    StartCoroutine(EventDelayer(2.5f, () =>
                    {
                        enemyIndex++;
                        turnIndex++;
                        CurrentTurnChar.GetComponent<UnitStatsss>().turnCountImmobilization -= 1;
                        state = State.EndTurn;
                    }));
                    
                }


            }

            if (CurrentTurnChar.stunned == true)
            {
                if (CurrentTurnChar.tag == "Player")
                {
                    PassiveDamageReloader(CurrentTurnChar);
                    StartCoroutine(EventDelayer(2.5f, () =>
                    {
                        playerIndex++;
                        turnIndex++;
                        CurrentTurnChar.GetComponent<UnitStatsss>().turnCountImmobilization -= 1;
                        state = State.EndTurn;
                    }));
                    
                }


                if (CurrentTurnChar.tag == "Enemy")
                {
                    PassiveDamageReloader(CurrentTurnChar);
                    StartCoroutine(EventDelayer(2.5f, () =>
                    {
                        enemyIndex++;
                        turnIndex++;
                        CurrentTurnChar.GetComponent<UnitStatsss>().turnCountImmobilization -= 1;
                        state = State.EndTurn;
                    }));
                    
                }


            }

            if (CurrentTurnChar.shocked == true)
            {
                if (CurrentTurnChar.tag == "Player")
                {
                    PassiveDamageReloader(CurrentTurnChar);
                    StartCoroutine(EventDelayer(2.5f, () =>
                    {
                        playerIndex++;
                        turnIndex++;
                        CurrentTurnChar.GetComponent<UnitStatsss>().turnCountImmobilization -= 1;
                        state = State.EndTurn;
                    }));
                    
                }


                if (CurrentTurnChar.tag == "Enemy")
                {
                    PassiveDamageReloader(CurrentTurnChar);
                    StartCoroutine(EventDelayer(2.5f, () =>
                    {
                        enemyIndex++;
                        turnIndex++;
                        CurrentTurnChar.GetComponent<UnitStatsss>().turnCountImmobilization -= 1;
                        state = State.EndTurn;
                    }));
                    
                }


            }

            if (CurrentTurnChar.sleep == true)
            {
                if (CurrentTurnChar.tag == "Player")
                {
                    PassiveDamageReloader(CurrentTurnChar);

                    StartCoroutine(EventDelayer(2.5f, () =>
                    {
                        playerIndex++;
                        turnIndex++;
                        CurrentTurnChar.GetComponent<UnitStatsss>().turnCountImmobilization -= 1;
                        state = State.EndTurn;

                    }));
                    
                }


                if (CurrentTurnChar.tag == "Enemy")
                {
                    PassiveDamageReloader(CurrentTurnChar);
                    StartCoroutine(EventDelayer(2.5f, () =>
                    {
                        enemyIndex++;
                        turnIndex++;
                        CurrentTurnChar.GetComponent<UnitStatsss>().turnCountImmobilization -= 1;
                        state = State.EndTurn;
                    }));
                    
                }


            }

            if (CurrentTurnChar.frozen == true)
            {
                if (CurrentTurnChar.tag == "Player")
                {
                    PassiveDamageReloader(CurrentTurnChar);
                    StartCoroutine(EventDelayer(2.5f, () =>
                    {
                        playerIndex++;
                        turnIndex++;
                        CurrentTurnChar.GetComponent<UnitStatsss>().turnCountImmobilization -= 1;
                        state = State.EndTurn;
                    }));
                    
                }


                if (CurrentTurnChar.tag == "Enemy")
                {
                    PassiveDamageReloader(CurrentTurnChar);
                    StartCoroutine(EventDelayer(2.5f, () =>
                    {
                        enemyIndex++;
                        turnIndex++;
                        CurrentTurnChar.GetComponent<UnitStatsss>().turnCountImmobilization -= 1;
                        state = State.EndTurn;
                    }));
                    
                }


            }

            if (CurrentTurnChar.turnCountImmobilization == 0)
            {
                CurrentTurnChar.immobilized = false;
                switch (CurrentTurnChar.immobilizationType)
                {
                    case "Petrified":
                        CurrentTurnChar.petrified = false;
                        break;

                    case "Stunned":
                        CurrentTurnChar.stunned = false;

                        break;

                    case "Feared":
                        CurrentTurnChar.feared = false;

                        break;

                    case "Shocked":
                        CurrentTurnChar.shocked = false;

                        break;
                    case "Sleep":
                        CurrentTurnChar.sleep = false;

                        break;
                    case "Frozen":
                        CurrentTurnChar.frozen = false;
                        break;

                }
            }

           

        }
        if (CurrentTurnChar.immobilized == false)
        {
            StartCoroutine(EventDelayer(2.5f, () => {
                state = State.CheckingLimitationsSpecial;
            }));
        }
    }


    private void SpecialStatusEffect()
    {
        
        CharacterBattle currentCharBattle = CurrentTurnChar.GetComponent<CharacterBattle>();
        AttackManager randomAttack= CurrentTurnChar.AllAttackList[UnityEngine.Random.Range(0, CurrentTurnChar.AllAttackList.Count)];
        RandomSelectedAttack = randomAttack.ExecuteAttack;
        //CharacterBattle randomPlayer= PlayerUnitStatsList[UnityEngine.Random.Range(0, PlayerUnitStatsList.Count)].GetComponent<CharacterBattle>();
        //CharacterBattle randomEnemy = enemyCharBattleList[UnityEngine.Random.Range(0, enemyCharBattleList.Count)];
        //CharacterBattle randomUnit=EveryCharList[UnityEngine.Random.Range(0, EveryCharList.Count)].GetComponent<CharacterBattle>();

        int ActionNum;
        List<int> numbers = new List<int> { 1, 2};
        ActionNum = numbers[UnityEngine.Random.Range(0, numbers.Count)];

        if (CurrentTurnChar.immobilized == false)
        {
            if (CurrentTurnChar.underSpecialStatus == true)
            {
                if (CurrentTurnChar.charmed == true)
                {
                    if (CurrentTurnChar.tag == "Player")
                    {
                        
                        switch (ActionNum)
                        {
                            case 1:
                                if (Busy == false)
                                {
                                    if (PlayerUnitStatsList.Count != 0)
                                    {
                                        if (randomAttack.IsItAoe == false)
                                        {
                                            Busy = true;
                                            cooldown = randomAttack.maxDelay;
                                            CharacterBattle randomPlayer = PlayerUnitStatsList[UnityEngine.Random.Range(0, PlayerUnitStatsList.Count)].GetComponent<CharacterBattle>();
                                            currentCharBattle.Attack(randomPlayer, RandomSelectedAttack, randomAttack, CurrentTurnChar.GetComponent<AnimationHandler>(), () => {
                                                randomAttack.StatusEffectActivation(randomPlayer.GetComponent<UnitStatsss>());
                                                PassiveDamageReloader(charBattle.GetComponent<UnitStatsss>());
                                                StartCoroutine(EventDelayer(3f, () =>
                                                {
                                                    playerIndex++;
                                                    turnIndex++;
                                                    state = State.EndTurn;
                                                }));
                                            });
                                        }
                                        if (randomAttack.IsItAoe == true)
                                        {
                                            Busy = true;
                                            cooldown = randomAttack.maxDelay;
                                            currentCharBattle.MultipleAttack(ActiveChaList, RandomSelectedAttack, randomAttack, CurrentTurnChar.GetComponent<AnimationHandler>(), () => {
                                                randomAttack.StatuEffecttActivationForTeam(ActiveChaList);
                                                PassiveDamageReloader(charBattle.GetComponent<UnitStatsss>());
                                                StartCoroutine(EventDelayer(3f, () =>
                                                {
                                                    playerIndex++;
                                                    turnIndex++;
                                                    state = State.EndTurn;
                                                }));
                                            });
                                        }

                                        
                                    }
                                    timeForNextAttack = cooldown;
                                    CurrentTurnChar.turnCountSpecialStatus -= 1;
                                    StartCoroutine(EventDelayer(cooldown, () =>
                                    {
                                       Busy = false;
                                    }));
                                }

                                break;

                            case 2:
                                if (Busy == false)
                                {
                                    if (enemyCharBattleList.Count != 0)
                                    {

                                        Busy = true;
                                        CharacterBattle randomEnemy = enemyCharBattleList[UnityEngine.Random.Range(0, enemyCharBattleList.Count)];
                                        PlayerPartyScript.Items[UnityEngine.Random.Range(0, PlayerPartyScript.Items.Count)].ExecuteItemEffect(randomEnemy.GetComponent<UnitStatsss>());
                                        charBattle.GetComponent<AnimationHandler>().ItemUseAnim();
                                        PassiveDamageReloader(charBattle.GetComponent<UnitStatsss>());
                                        StartCoroutine(EventDelayer(3f, () =>
                                        {
                                            charBattle.GetComponent<AnimationHandler>().IdleAnim();
                                            playerIndex++;
                                            turnIndex++;
                                            state = State.EndTurn;
                                        }));
                                        CurrentTurnChar.turnCountSpecialStatus -= 1;
                                        StartCoroutine(EventDelayer(2f, () =>
                                        {
                                            Busy = false;

                                        }));
                                        

                                    }
                                }
                                    break;
                        }
                        
                    }

                    if (CurrentTurnChar.tag == "Enemy")
                    {
                        
                        if (Busy == false)
                        {
                            if (enemyCharBattleList.Count != 0)
                            {
                                Busy = true;
                                cooldown = randomAttack.maxDelay;
                                CharacterBattle randomEnemy = enemyCharBattleList[UnityEngine.Random.Range(0, enemyCharBattleList.Count)];
                                currentCharBattle.Attack(randomEnemy, RandomSelectedAttack, randomAttack, CurrentTurnChar.GetComponent<AnimationHandler>(), () => {
                                    randomAttack.StatusEffectActivation(randomEnemy.GetComponent<UnitStatsss>());
                                    PassiveDamageReloader(enemyBattle.GetComponent<UnitStatsss>());
                                    StartCoroutine(EventDelayer(3f, () =>
                                    {
                                        enemyIndex++;
                                        turnIndex++;
                                        state = State.EndTurn;
                                    }));
                                });
                            }
                            timeForNextAttack = cooldown;
                            CurrentTurnChar.turnCountSpecialStatus -= 1;
                            StartCoroutine(EventDelayer(cooldown, () =>
                            {
                                Busy = false;
                            }));
                        }
                        
                        /*switch (ActionNum)
                        {
                            case 1:
                                currentCharBattle.Attack(randomEnemy, SelectedAttack, randomAttack, currentChar.GetComponent<AnimationHandler>(), () => {
                                    enemyIndex++;
                                    turnIndex++;
                                });

                                break;
                        }*/


                    }
                }


                if (CurrentTurnChar.confused == true)
                {
                    if (CurrentTurnChar.tag == "Player")
                    {
                        state = State.PlayerTurn;
                    }
                    
                    if (CurrentTurnChar.tag == "Enemy")
                    {
                        if (RollADice() == true)
                        {
                            
                            if (Busy == false)
                            {
                                if (EveryCharBattleList.Count != 0)
                                {
                                    Busy = true;
                                    cooldown = randomAttack.maxDelay;
                                    CharacterBattle randomUnit = EveryCharList[UnityEngine.Random.Range(0, EveryCharList.Count)].GetComponent<CharacterBattle>();
                                    currentCharBattle.Attack(randomUnit, RandomSelectedAttack, randomAttack, CurrentTurnChar.GetComponent<AnimationHandler>(), () => {
                                        randomAttack.StatusEffectActivation(randomUnit.GetComponent<UnitStatsss>());
                                        PassiveDamageReloader(enemyBattle.GetComponent<UnitStatsss>());
                                        StartCoroutine(EventDelayer(3f, () =>
                                        {
                                            enemyIndex++;
                                            turnIndex++;
                                            state = State.EndTurn;
                                        }));
                                    });
                                }
                                timeForNextAttack = cooldown;
                                CurrentTurnChar.turnCountSpecialStatus -= 1;
                                StartCoroutine(EventDelayer(cooldown, () =>
                                {
                                    Busy = false;
                                }));
                            }
                        }
                    }
                }


                if (CurrentTurnChar.blinded == true)
                {
                    if (CurrentTurnChar.tag == "Player")
                    {
                        state = State.PlayerTurn;
                    }
                    if (CurrentTurnChar.tag == "Enemy")
                    {
                        PassiveDamageReloader(enemyBattle.GetComponent<UnitStatsss>());
                        StartCoroutine(EventDelayer(1.5f, () =>
                        {
                            enemyIndex++;
                            turnIndex++;
                            state = State.EndTurn;
                        }));
                        CurrentTurnChar.turnCountSpecialStatus -= 1;
                    }
                }



                if (CurrentTurnChar.raged == true)
                {
                    float attackDamagePP = CurrentTurnChar.attackDamage * 30 / 100;
                    if (CurrentTurnChar.tag == "Player")
                    {
                        if (Busy == false)
                        {
                            if (EveryCharBattleList.Count != 0)
                            {
                                
                                CurrentTurnChar.attackDamage += attackDamagePP;
                                if (randomAttack.IsItAoe == false)
                                {
                                    Busy = true;
                                    cooldown = randomAttack.maxDelay;
                                    CharacterBattle randomUnit = EveryCharList[UnityEngine.Random.Range(0, EveryCharList.Count)].GetComponent<CharacterBattle>();
                                    currentCharBattle.Attack(randomUnit, RandomSelectedAttack, randomAttack, CurrentTurnChar.GetComponent<AnimationHandler>(), () => {
                                        randomAttack.StatusEffectActivation(randomUnit.GetComponent<UnitStatsss>());
                                        PassiveDamageReloader(charBattle.GetComponent<UnitStatsss>());
                                        StartCoroutine(EventDelayer(3f, () =>
                                        {
                                            playerIndex++;
                                            turnIndex++;
                                            state = State.EndTurn;
                                        }));
                                    });
                                }
                                if (randomAttack.IsItAoe == true)
                                {
                                    Busy = true;
                                    cooldown = randomAttack.maxDelay;
                                    currentCharBattle.MultipleAttack(EveryCharBattleList, RandomSelectedAttack, randomAttack, CurrentTurnChar.GetComponent<AnimationHandler>(), () => {
                                        randomAttack.StatuEffecttActivationForTeam(EveryCharBattleList);
                                        PassiveDamageReloader(charBattle.GetComponent<UnitStatsss>());
                                        StartCoroutine(EventDelayer(3f, () =>
                                        {
                                            playerIndex++;
                                            turnIndex++;
                                            state = State.EndTurn;
                                        }));
                                    });
                                }
                                
                            }
                            timeForNextAttack = cooldown;
                            CurrentTurnChar.turnCountSpecialStatus -= 1;
                            
                            StartCoroutine(EventDelayer(cooldown, () =>
                            {
                                CurrentTurnChar.attackDamage -= attackDamagePP;
                                Busy = false;
                            }));
                        }
                    }

                    if (CurrentTurnChar.tag == "Enemy")
                    {
                        if (Busy == false)
                        {
                            if (EveryCharBattleList.Count != 0)
                            {
                                CurrentTurnChar.attackDamage += attackDamagePP;
                                Busy = true;
                                cooldown = randomAttack.maxDelay;
                                CharacterBattle randomUnit = EveryCharList[UnityEngine.Random.Range(0, EveryCharList.Count)].GetComponent<CharacterBattle>();
                                currentCharBattle.Attack(randomUnit, RandomSelectedAttack, randomAttack, CurrentTurnChar.GetComponent<AnimationHandler>(), () => {
                                    randomAttack.StatusEffectActivation(randomUnit.GetComponent<UnitStatsss>());
                                    PassiveDamageReloader(enemyBattle.GetComponent<UnitStatsss>());
                                    StartCoroutine(EventDelayer(3f, () =>
                                    {
                                        enemyIndex++;
                                        turnIndex++;
                                        state = State.EndTurn;
                                    }));
                                });
                            }
                            timeForNextAttack = cooldown;
                            CurrentTurnChar.turnCountSpecialStatus -= 1;
                            StartCoroutine(EventDelayer(cooldown, () =>
                            {
                                CurrentTurnChar.attackDamage -= attackDamagePP;
                                Busy = false;
                            }));
                        }
                    }
                }



            }
            else
            {
                StartCoroutine(EventDelayer(2f, () => {
                    if (CurrentTurnChar.tag == "Player")
                    {
                        state = State.PlayerTurn;
                    }
                    if (CurrentTurnChar.tag == "Enemy")
                    {
                        state = State.EnemyTurn;
                    }
                }));
            }
            
        }
        
        if (CurrentTurnChar.turnCountSpecialStatus == 0)
        {
            CurrentTurnChar.underSpecialStatus = false;
            switch (CurrentTurnChar.specialStatusType)
            {
                case "Charmed":
                    CurrentTurnChar.charmed = false;
                    break;

                case "Raged":
                    CurrentTurnChar.raged = false;

                    break;

                case "Blinded":
                    CurrentTurnChar.blinded = false;

                    break;

                case "Confused":
                    CurrentTurnChar.confused = false;

                    break;


            }

        }
    }


    public void NormalAttack()
    {
        if (chosenEnemy == null && enemyCharBattleList.Count != 0)
        {
            chosenEnemy = enemyCharBattleList[UnityEngine.Random.Range(0, enemyCharBattleList.Count)];
        }

        if (state == State.PlayerTurn && Busy == false && chosenEnemy != null) 
        {
            uiSystem.ActionHud.gameObject.SetActive(false);
            if (charBattle.GetComponent<UnitStatsss>().underSpecialStatus == false)
            {
                
                cooldown = playerUnitStats.NormalAttack.maxDelay;
                Busy = true;
                SelectedAttack = playerUnitStats.NormalAttack.ExecuteAttack;
                charBattle.Attack(chosenEnemy, SelectedAttack, playerUnitStats.NormalAttack, aHandlerPlayer, () => {
                    playerUnitStats.NormalAttack.StatusEffectActivation(chosenEnemy.GetComponent<UnitStatsss>());
                    PassiveDamageReloader(charBattle.GetComponent<UnitStatsss>());
                    StartCoroutine(EventDelayer(cooldown, () =>
                    {
                        playerIndex++;
                        turnIndex++;
                        state = State.EndTurn;
                    }));

                });
            }

            if (charBattle.GetComponent<UnitStatsss>().underSpecialStatus == true)
            {
                CharacterBattle randomE = EveryCharList[UnityEngine.Random.Range(0, EveryCharList.Count)].GetComponent<CharacterBattle>();
                if (charBattle.GetComponent<UnitStatsss>().confused == true)
                {
                    if (RollADice() == true)
                    {
                        cooldown = playerUnitStats.NormalAttack.maxDelay;
                        Busy = true;
                        SelectedAttack = playerUnitStats.NormalAttack.ExecuteAttack;
                        charBattle.Attack(randomE, SelectedAttack, playerUnitStats.NormalAttack, aHandlerPlayer, () => {
                            playerUnitStats.NormalAttack.StatusEffectActivation(randomE.GetComponent<UnitStatsss>());
                            PassiveDamageReloader(charBattle.GetComponent<UnitStatsss>());
                            charBattle.GetComponent<UnitStatsss>().turnCountSpecialStatus -= 1;
                            StartCoroutine(EventDelayer(cooldown, () =>
                            {
                                playerIndex++;
                                turnIndex++;
                                state = State.EndTurn;
                            }));

                        });
                    }
                    else
                    {
                        cooldown = playerUnitStats.NormalAttack.maxDelay;
                        Busy = true;
                        SelectedAttack = playerUnitStats.NormalAttack.ExecuteAttack;
                        charBattle.Attack(chosenEnemy, SelectedAttack, playerUnitStats.NormalAttack, aHandlerPlayer, () => {
                            playerUnitStats.NormalAttack.StatusEffectActivation(chosenEnemy.GetComponent<UnitStatsss>());
                            PassiveDamageReloader(charBattle.GetComponent<UnitStatsss>());
                            charBattle.GetComponent<UnitStatsss>().turnCountSpecialStatus -= 1;
                            StartCoroutine(EventDelayer(cooldown, () =>
                            {
                                playerIndex++;
                                turnIndex++;
                                state = State.EndTurn;
                            }));

                        });
                    }


                }

                if (charBattle.GetComponent<UnitStatsss>().blinded == true)
                {
                    Debug.Log("Can't attack while blind");
                }


            }

        } 

        

        StartCoroutine(EventDelayer(cooldown, () =>
        {
            Busy = false;
            //charBattle.Attacking = false;
        }));

    }
    public void MagicAttack(AttackManager apref)
    {
        if (chosenEnemy == null && enemyCharBattleList.Count != 0)
        {
            chosenEnemy = enemyCharBattleList[UnityEngine.Random.Range(0, enemyCharBattleList.Count)];
        }

        if (state == State.PlayerTurn && Busy == false&&chosenEnemy!=null&&charBattle.GetComponent<UnitStatsss>().currentMP>=apref.ManaCost)
        {
            uiSystem.ActionHud.gameObject.SetActive(false);
            charBattle.GetComponent<UnitStatsss>().currentMP -= apref.ManaCost;
            
            if (charBattle.GetComponent<UnitStatsss>().underSpecialStatus == false)
            {
                if (apref.IsItAttack == true)
                {
                    if (apref.IsItAoe == false)
                    {
                        cooldown = apref.maxDelay;
                        Busy = true;
                        charBattle.Attack(chosenEnemy, SelectedAttack, apref, aHandlerPlayer, () => {
                            apref.StatusEffectActivation(chosenEnemy.GetComponent<UnitStatsss>());
                            PassiveDamageReloader(charBattle.GetComponent<UnitStatsss>());

                            StartCoroutine(EventDelayer(cooldown, () =>
                            {
                                playerIndex++;
                                turnIndex++;
                                state = State.EndTurn;
                            }));

                        });
                    }
                    if (apref.IsItAoe == true)
                    {
                        AOEAttack(apref,enemyCharBattleList);
                    }
                }
                if (apref.IsItHeal == true||apref.IsItBuff==true)
                {
                    if (apref.IsItAoe == false)
                    {
                        cooldown = apref.maxDelay;
                        Busy = true;
                        charBattle.Attack(chosenPlayer, SelectedAttack, apref, aHandlerPlayer, () => {
                            apref.StatusEffectActivation(chosenPlayer.GetComponent<UnitStatsss>());
                            PassiveDamageReloader(charBattle.GetComponent<UnitStatsss>());

                            StartCoroutine(EventDelayer(cooldown, () =>
                            {
                                playerIndex++;
                                turnIndex++;
                                state = State.EndTurn;
                            }));

                        });
                    }
                    if (apref.IsItAoe == true)
                    {
                        AOEAttack(apref,ActiveChaList);
                    }
                }

            }

            if (charBattle.GetComponent<UnitStatsss>().underSpecialStatus == true)
            {
                CharacterBattle randomE = EveryCharList[UnityEngine.Random.Range(0, EveryCharList.Count)].GetComponent<CharacterBattle>();
                if (charBattle.GetComponent<UnitStatsss>().confused == true)
                {
                    if (apref.IsItAttack == true)
                    {
                        if (apref.IsItAoe == false)
                        {
                            if (RollADice() == true)
                            {
                                cooldown = apref.maxDelay;
                                Busy = true;
                                charBattle.Attack(randomE, SelectedAttack, apref, aHandlerPlayer, () => {
                                    apref.StatusEffectActivation(randomE.GetComponent<UnitStatsss>());
                                    PassiveDamageReloader(charBattle.GetComponent<UnitStatsss>());
                                    charBattle.GetComponent<UnitStatsss>().turnCountSpecialStatus -= 1;
                                    StartCoroutine(EventDelayer(cooldown, () =>
                                    {
                                        playerIndex++;
                                        turnIndex++;
                                        state = State.EndTurn;
                                    }));

                                });

                            }
                            else
                            {
                                cooldown = apref.maxDelay;
                                Busy = true;
                                charBattle.Attack(chosenEnemy, SelectedAttack, apref, aHandlerPlayer, () => {
                                    apref.StatusEffectActivation(chosenEnemy.GetComponent<UnitStatsss>());
                                    PassiveDamageReloader(charBattle.GetComponent<UnitStatsss>());
                                    charBattle.GetComponent<UnitStatsss>().turnCountSpecialStatus -= 1;
                                    StartCoroutine(EventDelayer(cooldown, () =>
                                    {
                                        playerIndex++;
                                        turnIndex++;
                                        state = State.EndTurn;
                                    }));

                                });
                            }
                        }
                        if (apref.IsItAoe == true)
                        {
                            AOEAttack(apref,EveryCharBattleList);
                        }
                    }
                    if (apref.IsItHeal == true || apref.IsItBuff == true)
                    {
                        if (apref.IsItAoe == false)
                        {
                            if (RollADice() == true)
                            {
                                cooldown = apref.maxDelay;
                                Busy = true;
                                charBattle.Attack(randomE, SelectedAttack, apref, aHandlerPlayer, () => {
                                    apref.StatusEffectActivation(randomE.GetComponent<UnitStatsss>());
                                    PassiveDamageReloader(charBattle.GetComponent<UnitStatsss>());
                                    charBattle.GetComponent<UnitStatsss>().turnCountSpecialStatus -= 1;
                                    StartCoroutine(EventDelayer(cooldown, () =>
                                    {
                                        playerIndex++;
                                        turnIndex++;
                                        state = State.EndTurn;
                                    }));

                                });

                            }
                            else
                            {
                                cooldown = apref.maxDelay;
                                Busy = true;
                                charBattle.Attack(chosenPlayer, SelectedAttack, apref, aHandlerPlayer, () => {
                                    apref.StatusEffectActivation(chosenPlayer.GetComponent<UnitStatsss>());
                                    PassiveDamageReloader(charBattle.GetComponent<UnitStatsss>());
                                    charBattle.GetComponent<UnitStatsss>().turnCountSpecialStatus -= 1;
                                    StartCoroutine(EventDelayer(cooldown, () =>
                                    {
                                        playerIndex++;
                                        turnIndex++;
                                        state = State.EndTurn;
                                    }));

                                });
                            }
                        }
                        if (apref.IsItAoe == true)
                        {
                            AOEAttack(apref,EveryCharBattleList);
                        }
                    }

                }

                if (charBattle.GetComponent<UnitStatsss>().blinded == true)
                {
                    Debug.Log("Can't attack while blind");
                }

                
            }
        }
        

        StartCoroutine(EventDelayer(cooldown, () =>
        {
            Busy = false;
        }));

    }

    public void Specials(AttackManager apref, AttackManager s1pref, AttackManager s2pref)
    {
        if (chosenEnemy == null && enemyCharBattleList.Count != 0)
        {
            chosenEnemy = enemyCharBattleList[UnityEngine.Random.Range(0, enemyCharBattleList.Count)];
        }
        if (state == State.PlayerTurn && Busy == false && chosenEnemy != null)
        {
            uiSystem.ActionHud.gameObject.SetActive(false);
            if (apref.EventNameE == "Transformation")
            {
                cooldown = apref.maxDelay;
                Busy = true;
                charBattle.EventExecuter(charBattle, ChosenEvent, apref, aHandlerPlayer, () => {
                    StartCoroutine(EventDelayer(cooldown, () =>
                    {
                        transformer.TransformFunc(() => {
                            PlayerUnitStatsList.Add(transformer.SuperPhase.GetComponent<UnitStatsss>());
                            PlayerUnitStatsList.Remove(transformer.NormalPhase.GetComponent<UnitStatsss>());
                            ActiveChaList.Add(transformer.SuperPhase.GetComponent<CharacterBattle>());
                            ActiveChaList.Remove(transformer.NormalPhase.GetComponent<CharacterBattle>());
                            EveryCharList.Add(transformer.SuperPhase.GetComponent<UnitStatsss>());
                            EveryCharList.Remove(transformer.NormalPhase.GetComponent<UnitStatsss>());
                            PlayerUnitStatsList.Sort((x, y) => y.speed.CompareTo(x.speed));
                            EveryCharList.Sort((x, y) => y.speed.CompareTo(x.speed));
                            PassiveDamageReloader(charBattle.GetComponent<UnitStatsss>());
                            StartCoroutine(EventDelayer(cooldown, () =>
                            {
                                playerIndex++;
                                turnIndex++;
                                state = State.EndTurn;
                            }));
                        });
                    }));
                });
               
            }

            if (apref.EventNameE == "Tag Attack"&&chosenPlayer!=null)
            {
                cooldown = apref.maxDelay;
                Busy = true;

                if (s1pref.IsItAoe == false)
                {
                    charBattle.Attack(chosenEnemy, SelectedAttack, s1pref, aHandlerPlayer, () => {

                    });
                    apref.StatusEffectActivation(chosenEnemy.GetComponent<UnitStatsss>());
                }
                if (s1pref.IsItAoe == true)
                {
                    charBattle.MultipleAttack(enemyCharBattleList, SelectedAttack, s1pref, aHandlerPlayer, () => { });
                    apref.StatuEffecttActivationForTeam(enemyCharBattleList);
                }
                if (s2pref.IsItAoe == false)
                {
                    chosenPlayer.Attack(chosenEnemy, SelectedAttack2, s2pref, chosenPlayer.GetComponent<AnimationHandler>(), () => {


                    });
                    apref.StatusEffectActivation(chosenEnemy.GetComponent<UnitStatsss>());
                }
                if (s2pref.IsItAoe == true)
                {
                    chosenPlayer.MultipleAttack(enemyCharBattleList, SelectedAttack2, s2pref, chosenPlayer.GetComponent<AnimationHandler>(), () => { });
                    apref.StatuEffecttActivationForTeam(enemyCharBattleList);
                }

                
                
                PassiveDamageReloader(charBattle.GetComponent<UnitStatsss>());
                StartCoroutine(EventDelayer(Math.Max(s1pref.maxDelay,s2pref.maxDelay), () =>
                {
                    playerIndex++;
                    turnIndex++;
                    state = State.EndTurn;
                }));
            }

            if (apref.EventNameE == "Revive"&& chosenPlayer.GetComponent<UnitStatsss>().charStat.isDed==true)
            {
                cooldown = apref.maxDelay;
                Busy = true;
                charBattle.EventExecuter(chosenPlayer, ChosenEvent, apref, aHandlerPlayer, () => {
                    StartCoroutine(EventDelayer(cooldown, () =>
                    {
                        chosenPlayer.GetComponent<UnitStatsss>().Revive();
                        PassiveDamageReloader(charBattle.GetComponent<UnitStatsss>());
                        StartCoroutine(EventDelayer(3f, () =>
                        {
                            playerIndex++;
                            turnIndex++;
                            state = State.EndTurn;
                        }));
                    }));

                });
            }


            StartCoroutine(EventDelayer(cooldown, () =>
            {
                Busy = false;
                
            }));

        }
    }
    public void ItemUse(Items item)
    {
        if (state == State.PlayerTurn && Busy == false)
        {
            if (charBattle.GetComponent<UnitStatsss>().underSpecialStatus == false)
            {
                Busy = true;
                charBattle.GetComponent<AnimationHandler>().ItemUseAnim();
                chosenPlayer.GetComponent<AnimationHandler>().BuffAnim();
                item.ExecuteItemEffect(chosenPlayer.GetComponent<UnitStatsss>());
                PassiveDamageReloader(charBattle.GetComponent<UnitStatsss>());
                StartCoroutine(EventDelayer(3f, () =>
                {
                    charBattle.GetComponent<AnimationHandler>().IdleAnim();
                    chosenPlayer.GetComponent<AnimationHandler>().IdleAnim();
                    playerIndex++;
                    turnIndex++;
                    state = State.EndTurn;
                }));
            }

            if (charBattle.GetComponent<UnitStatsss>().underSpecialStatus == true)
            {
                if (charBattle.GetComponent<UnitStatsss>().confused == true)
                {
                    if (RollADice() == true)
                    {
                        Busy = true;
                        charBattle.GetComponent<AnimationHandler>().ItemUseAnim();
                        UnitStatsss ranChar = EveryCharList[UnityEngine.Random.Range(0, EveryCharList.Count)];
                        if (ranChar.tag == "Player")
                        {
                            ranChar.GetComponent<AnimationHandler>().BuffAnim();
                        }
                        item.ExecuteItemEffect(ranChar);
                        PassiveDamageReloader(charBattle.GetComponent<UnitStatsss>());
                        StartCoroutine(EventDelayer(3f, () =>
                        {
                            charBattle.GetComponent<AnimationHandler>().IdleAnim();
                            playerIndex++;
                            turnIndex++;
                            state = State.EndTurn;
                        }));

                    }
                    else
                    {
                        Busy = true;
                        charBattle.GetComponent<AnimationHandler>().ItemUseAnim();
                        chosenPlayer.GetComponent<AnimationHandler>().BuffAnim();
                        item.ExecuteItemEffect(chosenPlayer.GetComponent<UnitStatsss>());
                        PassiveDamageReloader(charBattle.GetComponent<UnitStatsss>());
                        StartCoroutine(EventDelayer(3f, () =>
                        {
                            charBattle.GetComponent<AnimationHandler>().IdleAnim();
                            playerIndex++;
                            turnIndex++;
                            state = State.EndTurn;
                        }));
                    }
                }

                


            }
            

            StartCoroutine(EventDelayer(3.5f, () =>
            {
                Busy = false;

            }));

        }
    }

    public void AOEAttack(AttackManager apref, List<CharacterBattle> targetList)
    {
        if (chosenEnemy == null && enemyCharBattleList.Count != 0)
        {
            chosenEnemy = enemyCharBattleList[UnityEngine.Random.Range(0, enemyCharBattleList.Count)];
        }
        if (state == State.PlayerTurn && Busy == false && chosenEnemy != null)
        {
            if (charBattle.GetComponent<UnitStatsss>().underSpecialStatus == false)
            {
                if (apref.IsItAttack == true)
                {
                    if (apref.Melee == false)
                    {
                        cooldown = apref.maxDelay;
                        Busy = true;
                        charBattle.MultipleAttack(targetList, SelectedAttack, apref, aHandlerPlayer, () => {
                            apref.StatuEffecttActivationForTeam(targetList);
                            PassiveDamageReloader(charBattle.GetComponent<UnitStatsss>());
                            StartCoroutine(EventDelayer(cooldown, () =>
                            {
                                playerIndex++;
                                turnIndex++;
                                state = State.EndTurn;
                            }));

                        });
                    }
                }

                if (apref.IsItHeal == true || apref.IsItBuff == true)
                {
                    if (apref.Melee == false)
                    {
                        cooldown = apref.maxDelay;
                        Busy = true;
                        charBattle.MultipleAttack(targetList, SelectedAttack, apref, aHandlerPlayer, () => {
                            apref.StatuEffecttActivationForTeam(targetList);
                            PassiveDamageReloader(charBattle.GetComponent<UnitStatsss>());
                            StartCoroutine(EventDelayer(cooldown, () =>
                            {
                                playerIndex++;
                                turnIndex++;
                                state = State.EndTurn;
                            }));

                        });
                    }
                }

                if (apref.Melee == true)
                {
                    
                    cooldown = apref.maxDelay;
                    Busy = true;
                    AttackManager aMan = apref.ChosenMeleeAoe;
                    for (int i = 1; i < targetList.Count; i++)
                    {
                        GameObject bunshin = Instantiate(charBattle.GetComponent<UnitStatsss>().Clone, charBattle.transform.parent);
                        spawnedBunshinList.Add(bunshin);
                    }
                    spawnedBunshinList[0].transform.position = new Vector2(charBattle.transform.position.x + 20, charBattle.transform.position.y);
                    for (int i = 1; i < spawnedBunshinList.Count; i++)
                    {
                        spawnedBunshinList[i].transform.position = new Vector2(spawnedBunshinList[i - 1].transform.position.x + 20, spawnedBunshinList[i - 1].transform.position.y);
                    }
                    StartCoroutine(EventDelayer(1f, () =>
                    {
                        charBattle.Attack(targetList[0], SelectedAttack, aMan, aHandlerPlayer, () => { });
                        for (int i = 1; i < enemyCharBattleList.Count; i++)
                        {
                            
                            spawnedBunshinList[i - 1].GetComponent<CharacterBattle>().Attack(targetList[i], SelectedAttack, aMan, spawnedBunshinList[i - 1].GetComponent<AnimationHandler>(), () => { });
                        }

                        StartCoroutine(EventDelayer(7f, () => {
                            for (int i = 0; i < spawnedBunshinList.Count; i++)
                            {
                                Destroy(spawnedBunshinList[i].gameObject);
                            }

                            StartCoroutine(EventDelayer(1f, () => {

                                spawnedBunshinList.Clear();
                            }));

                        }));

                    }));
                    apref.StatuEffecttActivationForTeam(targetList);
                    PassiveDamageReloader(charBattle.GetComponent<UnitStatsss>());
                    StartCoroutine(EventDelayer(cooldown, () =>
                    {
                        playerIndex++;
                        turnIndex++;
                        state = State.EndTurn;
                    }));
                }
            }

            if (charBattle.GetComponent<UnitStatsss>().underSpecialStatus == true)
            {
                if (charBattle.GetComponent<UnitStatsss>().confused == true)
                {
                    if (apref.IsItAttack == true)
                    {
                        if (apref.Melee == false)
                        {
                            if (RollADice() == true)
                            {
                                cooldown = apref.maxDelay;
                                Busy = true;
                                charBattle.MultipleAttack(EveryCharBattleList, SelectedAttack, apref, aHandlerPlayer, () => {
                                    PassiveDamageReloader(charBattle.GetComponent<UnitStatsss>());
                                    charBattle.GetComponent<UnitStatsss>().turnCountSpecialStatus -= 1;
                                    StartCoroutine(EventDelayer(cooldown, () =>
                                    {
                                        playerIndex++;
                                        turnIndex++;
                                        state = State.EndTurn;
                                    }));

                                });

                            }
                            else
                            {
                                cooldown = apref.maxDelay;
                                Busy = true;
                                charBattle.MultipleAttack(targetList, SelectedAttack, apref, aHandlerPlayer, () => {
                                    apref.StatuEffecttActivationForTeam(targetList);
                                    PassiveDamageReloader(charBattle.GetComponent<UnitStatsss>());
                                    charBattle.GetComponent<UnitStatsss>().turnCountSpecialStatus -= 1;
                                    StartCoroutine(EventDelayer(cooldown, () =>
                                    {
                                        playerIndex++;
                                        turnIndex++;
                                        state = State.EndTurn;
                                    }));

                                });
                            }
                        }
                        if (apref.Melee == true)
                        {
                            cooldown = apref.maxDelay;
                            Busy = true;
                            AttackManager aMan = apref.ChosenMeleeAoe;
                            for (int i = 1; i < EveryCharBattleList.Count; i++)
                            {
                                GameObject bunshin = Instantiate(charBattle.GetComponent<UnitStatsss>().Clone, charBattle.transform.parent);
                                spawnedBunshinList.Add(bunshin);
                            }
                            spawnedBunshinList[0].transform.position = new Vector2(charBattle.transform.position.x + 20, charBattle.transform.position.y);
                            for (int i = 1; i < spawnedBunshinList.Count; i++)
                            {
                                spawnedBunshinList[i].transform.position = new Vector2(spawnedBunshinList[i - 1].transform.position.x + 20, spawnedBunshinList[i - 1].transform.position.y);
                            }
                            StartCoroutine(EventDelayer(1f, () =>
                            {
                                charBattle.Attack(EveryCharBattleList[0], SelectedAttack, aMan, aHandlerPlayer, () => { });
                                for (int i = 1; i < enemyCharBattleList.Count; i++)
                                {

                                    spawnedBunshinList[i - 1].GetComponent<CharacterBattle>().Attack(EveryCharBattleList[i], SelectedAttack, aMan, spawnedBunshinList[i - 1].GetComponent<AnimationHandler>(), () => { });
                                }

                                StartCoroutine(EventDelayer(7f, () => {
                                    for (int i = 0; i < spawnedBunshinList.Count; i++)
                                    {
                                        Destroy(spawnedBunshinList[i].gameObject);
                                    }

                                    StartCoroutine(EventDelayer(1f, () => {

                                        spawnedBunshinList.Clear();
                                    }));

                                }));

                            }));
                            apref.StatuEffecttActivationForTeam(EveryCharBattleList);
                            PassiveDamageReloader(charBattle.GetComponent<UnitStatsss>());
                            charBattle.GetComponent<UnitStatsss>().turnCountSpecialStatus -= 1;
                            StartCoroutine(EventDelayer(cooldown, () =>
                            {
                                playerIndex++;
                                turnIndex++;
                                state = State.EndTurn;
                            }));
                        }
                    }
                    if(apref.IsItHeal == true || apref.IsItBuff == true)
                    {
                        cooldown = apref.maxDelay;
                        Busy = true;
                        charBattle.MultipleAttack(EveryCharBattleList, SelectedAttack, apref, aHandlerPlayer, () => {
                            apref.StatuEffecttActivationForTeam(EveryCharBattleList);
                            PassiveDamageReloader(charBattle.GetComponent<UnitStatsss>());
                            charBattle.GetComponent<UnitStatsss>().turnCountSpecialStatus -= 1;
                            StartCoroutine(EventDelayer(cooldown, () =>
                            {
                                playerIndex++;
                                turnIndex++;
                                state = State.EndTurn;
                            }));

                        });
                    }
                }

                if (charBattle.GetComponent<UnitStatsss>().blinded == true)
                {
                    Debug.Log("Can't attack while blind");
                }


            }
            
        }


        StartCoroutine(EventDelayer(cooldown, () =>
        {
            Busy = false;
        }));

        
    }

    public void GuardState()
    {
        uiSystem.ActionHud.gameObject.SetActive(false);
        charBattle.Guard();
        PassiveDamageReloader(charBattle.GetComponent<UnitStatsss>());
        StartCoroutine(EventDelayer(3f, () =>
        {
            playerIndex++;
            turnIndex++;
            state = State.EndTurn;
        }));

    }


    

    private void EnemyAttack()
    {
        AttackManager randomAttack = enemyBattle.GetComponent<UnitStatsss>().AllAttackList[UnityEngine.Random.Range(0, enemyBattle.GetComponent<UnitStatsss>().AllAttackList.Count)];

        if (PlayerUnitStatsList.Count != 0 && TargetedPlayerForAnAttack == null)
        {
            TargetedPlayerForAnAttack = PlayerUnitStatsList[UnityEngine.Random.Range(0, PlayerUnitStatsList.Count)].GetComponent<CharacterBattle>();
        }
        if (state==State.EnemyTurn)
        {
            Busy = true;
            cooldown = randomAttack.maxDelay;
            SelectedAttack = randomAttack.ExecuteAttack;
            enemyBattle.Attack(TargetedPlayerForAnAttack, SelectedAttack, randomAttack, aHandlerEnemy, () => {
                randomAttack.StatusEffectActivation(TargetedPlayerForAnAttack.GetComponent<UnitStatsss>());
                PassiveDamageReloader(enemyBattle.GetComponent<UnitStatsss>());
                StartCoroutine(EventDelayer(cooldown, () =>
                {
                    enemyIndex++;
                    turnIndex++;
                    TargetedPlayerForAnAttack = null;
                    state = State.EndTurn;
                }));


            });
            
        }

        StartCoroutine(EventDelayer(cooldown, () =>
        {
            Busy = false;
        }));



    }

    public void CheckWinLose()
    {
        if (PlayerUnitStatsList.Count <= 0)
        {
            enemyWon = true;
        }
        if (EnemyUnitStatsList.Count <= 0)
        {
            playerWon = true;
        }
    }

    IEnumerator EventDelayer(float delay,Action onDelayComplete)
    {
        yield return new WaitForSeconds(delay);
        onDelayComplete();
    }

    public bool RollADice()
    {
        bool decision;

        List<int> DiceNums = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        List<int> PozitiveNums = new List<int> { 2, 6, 10, 8 };
        int rollenDice = DiceNums[UnityEngine.Random.Range(0, DiceNums.Count)];

        if (PozitiveNums.Contains(rollenDice))
        {
            decision = true;
        }
        else
        {
            decision = false;
        }

        return decision;
    }


    public void ResetAttacking(List<CharacterBattle> charSet)
    {
        for(int i = 0; i < charSet.Count; i++)
        {
            charSet[i].Attacking = false;
        }
    }
}
