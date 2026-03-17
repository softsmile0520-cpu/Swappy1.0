using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using DG.Tweening.Core.Easing;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.Profiling;

public class PointsCalculator : MonoBehaviour
{
    public static PointsCalculator instance;

    // GameOver Results
    public TextMeshProUGUI ModeNameChange;
    public TextMeshProUGUI NoOfPlayers;
    public TextMeshProUGUI RandomMode;
    public TextMeshProUGUI StartPos;

    // Points Elements.
    public TextMeshProUGUI EasyEnemy;
    public TextMeshProUGUI MedimEnemy;
    public TextMeshProUGUI HardEnemy;

    public TextMeshProUGUI EasyEnemyV;
    public TextMeshProUGUI MedimEnemyV;
    public TextMeshProUGUI HardEnemyV;

    public TextMeshProUGUI EasyEnemyT;
    public TextMeshProUGUI MedimEnemyT;
    public TextMeshProUGUI HardEnemyT;

    public TextMeshProUGUI MaxCorePoints;

    public TextMeshProUGUI TotalEnemies;
    public TextMeshProUGUI DefeatedEnemies;

    public TextMeshProUGUI PercOFCorePoints;

    public TextMeshProUGUI TotalCorePoints;

    // Bonus Points Elemnts.
    public TextMeshProUGUI WinnigBonusPerc;
    public TextMeshProUGUI WinnigBonusTotal;
    public TextMeshProUGUI WinnigName;

    public TextMeshProUGUI FinishaGamePerc;
    public TextMeshProUGUI FinishaGameTotal;

    public TextMeshProUGUI ModeName;
    public TextMeshProUGUI ModeNamePerc;
    public TextMeshProUGUI ModeNameTotal;

    public TextMeshProUGUI RandomPerc;
    public TextMeshProUGUI RandomTotal;



    public TextMeshProUGUI PositionPerc;
    public TextMeshProUGUI PositionTotal;

    public TextMeshProUGUI TotalPoints;

    public Image GameOverImage;
    public List<Sprite> GameOverSprites;

    // Player Profile Data
    public PlayerGameResultData PlayerFinalData;

    public Transform content;

    // Zaroori Chezain

    public int OtherPlayers;
    public int DeadPlayers;

    public List<Sprite> BGDisplayesSquare;
    public List<Sprite> BGDisplayesRectangle;

    public float obtainPoints;

    public int finalpos = 0;
    public int PositionJumped = 0;
    public float winningBonus = 0;
    public bool WonGame;

    public float ModePoints = 0;
    public float RandomSelectionPoints;

    public float totalpoints = 0;

    [NonSerialized]
    public PlayerProfileInfoTab playerInfo;

    public PlayerProfileInfoTab PlayerInfoTabPrefab;

    public Transform PlayerProfilePos;

    public GameObject FirstButton;
    //Displaying Trophies Gained
    public GameObject TrophiesGained;

    public GameObject InitialPoints;
    public TextMeshProUGUI XpGained;
    public TextMeshProUGUI CoinsGained;
    public TextMeshProUGUI KudoTotalTrophies;
    public TextMeshProUGUI MasterTotalTrophies;
    public TextMeshProUGUI GoatTotalTrophies;

    public GameObject TrophyDisplayed;
    public TextMeshProUGUI Heading;
    public Image TrophieGainedDisplay;
    public TextMeshProUGUI CoinsForTrophy;
    public TextMeshProUGUI JemsForTrophy;
    public TextMeshProUGUI WhatisTrophyFor;
    public TextMeshProUGUI TrophyName;

    public List<int> KudoTrophiesNewlyGained = new List<int>();
    public List<int> MasterTrophiesNewlyGained = new List<int>();
    public List<int> GoatTrophiesNewlyGained = new List<int>();

    public List<Trophy> AllTrophiesGainedInaMatch = new List<Trophy>();
    public int trophyBeingDisplayed;

    private void Start()
    {

        PlayerProfileData();
        PointsCalculation();
        BonusPointCalculation();
        TrophiesCheck();

        Invoke(nameof(DisplayAd), 1f);
        playerInfo = Instantiate(PlayerInfoTabPrefab, PlayerProfilePos);

    }
    void DisplayAd()
    {
        if (AdManager_Admob.instance.IsInterstitialAdLoaded())
        {
            print("Playing added");
            AdManager_Admob.instance.ShowInterstitialAd();
        }
        else
        {
            print("Ad not loaded");
        }
    }
    public static PointsCalculator ShowUI()
    {
        if (instance == null)
        {
            GameObject obj = Instantiate(Resources.Load("PointsCalculator")) as GameObject;

            obj.gameObject.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);

            instance = obj.GetComponent<PointsCalculator>();
        }

        return instance;
    }

    private void Awake()
    {
        instance = this;
    }
    public void PlayerProfileData()
    {
        //Gamemanager.instance._PlayersList.Sort((a, b) => b.Position.CompareTo(a.Position));

        //for (int i = Gamemanager.instance._PlayersList.Count - 1; i >= 0; i--)
        //{
        //    PlayerGameResultData PlayerData = Instantiate(PlayerFinalData, content);
        //    PlayerData.SetResult(Gamemanager.instance._PlayersList[i]);
        //}

        Gamemanager.instance._PlayersList.Sort((a, b) => a.Position.CompareTo(b.Position)); // Reverse order

        for (int i = 0; i < Gamemanager.instance._PlayersList.Count; i++)
        {
            PlayerGameResultData PlayerData = Instantiate(PlayerFinalData, content);

            PlayerData.SetResult(Gamemanager.instance._PlayersList[i]);
        }
    }

    [ContextMenu("Calculate")]
    public void PointsCalculation()
    {
        ModeNameChange.text = GameConfigration.instance.GameMode.ToString() + " Mode";
        NoOfPlayers.text = Gamemanager.instance._PlayersList.Count + " Players";
        if (GameConfigration.instance.RandomSelected)
        {
            RandomMode.text = "Random Bonus";
        }
        else
        {
            RandomMode.text = "No Random Bonus";
        }
        StartPos.text =  "Start Pos " + (Gamemanager.instance.playerStartingIndex +1) + "";

        int x = TrophiesHandler.Instance.trophyVariables["TotalPlayedGames"] + 1;
        TrophiesHandler.Instance.trophyVariables["TotalPlayedGames"]=x;
        int y = TrophiesHandler.Instance.trophyVariables["TotalPlayedGamesTM"] + 1;
        TrophiesHandler.Instance.trophyVariables["TotalPlayedGamesTM"] = y;

        int easyEnemeyCount = 0;
        int MediumEnemeyCount = 0;
        int HardyEnemeyCount = 0;

        easyEnemeyCount = Gamemanager.instance._PlayersList.Count(a => a.PlayerDifficulty == Difficulty.Easy && a.AiSwappy);

        MediumEnemeyCount = Gamemanager.instance._PlayersList.Count(a => a.PlayerDifficulty == Difficulty.Medium && a.AiSwappy);

        HardyEnemeyCount = Gamemanager.instance._PlayersList.Count(a => a.PlayerDifficulty == Difficulty.Hard && a.AiSwappy);


        EasyEnemy.text = "" + easyEnemeyCount;
        MedimEnemy.text = "" + MediumEnemeyCount;
        HardEnemy.text = "" + HardyEnemeyCount;

        EasyEnemyV.text = "" + easyEnemeyCount;
        MedimEnemyV.text = "" + MediumEnemeyCount;
        HardEnemyV.text = "" + HardyEnemeyCount;

        EasyEnemyT.text = "" + easyEnemeyCount * 30;
        MedimEnemyT.text = "" + MediumEnemeyCount * 60;
        HardEnemyT.text = "" + HardyEnemeyCount * 90;

        float MaxCorePointsV = easyEnemeyCount * 30 + MediumEnemeyCount * 60 + HardyEnemeyCount * 90;
        MaxCorePoints.text = MaxCorePointsV + "";

        OtherPlayers = Gamemanager.instance._PlayersList.Count - 1;

        DeadPlayers = Gamemanager.instance._PlayersList.Count(a => a.Dead && a.AiSwappy);

        if (GameConfigration.instance.GameMode == mode.Classic)
        {
            TrophiesHandler.Instance.trophyVariables["DefeatOppInClassicM"] = TrophiesHandler.Instance.trophyVariables["DefeatOppInClassicM"] + DeadPlayers;
            TrophiesHandler.Instance.trophyVariables["DefeatOppInClassicMTM"] = TrophiesHandler.Instance.trophyVariables["DefeatOppInClassicMTM"] + DeadPlayers;
        }
        if (GameConfigration.instance.GameMode == mode.Fast)
        {
            TrophiesHandler.Instance.trophyVariables["DefeatOppInFastM"] = TrophiesHandler.Instance.trophyVariables["DefeatOppInFastM"] + DeadPlayers;
            TrophiesHandler.Instance.trophyVariables["DefeatOppInFastMTM"] = TrophiesHandler.Instance.trophyVariables["DefeatOppInFastMTM"] + DeadPlayers;
        }
        if (GameConfigration.instance.GameMode == mode.Power)
        {
            TrophiesHandler.Instance.trophyVariables["DefeatOppInPowerM"] = TrophiesHandler.Instance.trophyVariables["DefeatOppInPowerM"] + DeadPlayers;
            TrophiesHandler.Instance.trophyVariables["DefeatOppInPowerM"] = TrophiesHandler.Instance.trophyVariables["DefeatOppInPowerMTM"] + DeadPlayers;
        }
        if (GameConfigration.instance.offline)
        {
            TrophiesHandler.Instance.trophyVariables["DefeatOppInOfflineM"] = TrophiesHandler.Instance.trophyVariables["DefeatOppInOfflineM"] + DeadPlayers;
            TrophiesHandler.Instance.trophyVariables["DefeatOppInOfflineMTM"] = TrophiesHandler.Instance.trophyVariables["DefeatOppInOfflineMTM"] + DeadPlayers;
        }
        if (!GameConfigration.instance.offline)
        {
            TrophiesHandler.Instance.trophyVariables["DefeatOppInOnlineM"] = TrophiesHandler.Instance.trophyVariables["DefeatOppInOnlineM"] + DeadPlayers;
            TrophiesHandler.Instance.trophyVariables["DefeatOppInOnlineMTM"] = TrophiesHandler.Instance.trophyVariables["DefeatOppInOnlineMTM"] + DeadPlayers;
        }
        TotalEnemies.text = OtherPlayers + "";
        DefeatedEnemies.text = DeadPlayers + "";

        float GainingPercent = 0;

        if (DeadPlayers == OtherPlayers)
        {
            WinnigBonusPerc.text = 50 + "";
            winningBonus = Mathf.RoundToInt(MaxCorePointsV / 2);
            WinnigBonusTotal.text = winningBonus + "";
            WonGame = true;
            WinnigName.text = "Winnig";

            GainingPercent = 100;
            PercOFCorePoints.text = GainingPercent + "%";
            GameOverImage.sprite = GameOverSprites[0];
        }
        else
        {
            if (DeadPlayers == 0)
            {
            GameOverImage.sprite = GameOverSprites[2];
            }
            else
            {
                GameOverImage.sprite = GameOverSprites[1];
            }
            WinnigBonusPerc.text = 0 + "";
            winningBonus = 0;
            WinnigBonusTotal.text = winningBonus + "";
            WinnigName.text = "Not Winnig";

            if (DeadPlayers == 0)
            {
                GainingPercent = 0;
                PercOFCorePoints.text = GainingPercent + "%";
            }
            else if (DeadPlayers == OtherPlayers - 1)
            {
                if (OtherPlayers == 3)
                {
                    GainingPercent = (100f / 3f) * 2;
                    PercOFCorePoints.text = Mathf.RoundToInt(GainingPercent) + "%";

                }
                else if (OtherPlayers == 2)
                {
                    GainingPercent = 50;
                    PercOFCorePoints.text = GainingPercent + "%";
                }
            }
            else if (DeadPlayers == OtherPlayers - 2)
            {
                GainingPercent = 100f / 3f;
                PercOFCorePoints.text = Mathf.RoundToInt(GainingPercent) + "%";
            }
        }

        FinishaGamePerc.text = 20 + "";
        FinishaGameTotal.text = 20 + "";

        obtainPoints = Mathf.RoundToInt((GainingPercent / 100f) * MaxCorePointsV);
        TotalCorePoints.text = Mathf.RoundToInt(obtainPoints) + "";
    }

    public void BonusPointCalculation()
    {
        ModeName.text = GameConfigration.instance.GameMode.ToString();

        switch(GameConfigration.instance.GameMode)
        {
            case mode.Classic:

                ModeNamePerc.text = 10 + "";
                ModePoints = (10 / 100f) * obtainPoints;
                ModeNameTotal.text =Mathf.RoundToInt(ModePoints) + "";

                break;

            case mode.Fast:

                ModeNamePerc.text = 30 + "";
                ModePoints = (30 / 100f) * obtainPoints;
                ModeNameTotal.text =Mathf.RoundToInt(ModePoints) + "";

                break;

            case mode.Power:

                ModeNamePerc.text = 0 + "";
                ModePoints = (0 / 100f) * obtainPoints;
                ModeNameTotal.text =Mathf.RoundToInt(ModePoints) + "";

                break;
        }


        if (GameConfigration.instance.RandomSelected == true)
        {
            RandomPerc.text = 10 + "";
            RandomSelectionPoints = (10 / 100f) * obtainPoints;
            RandomTotal.text =Mathf.RoundToInt(RandomSelectionPoints) + "";
        }
        else
        {
            RandomPerc.text = 0 + "";
            RandomSelectionPoints = (0 / 100f) * obtainPoints;
            RandomTotal.text =Mathf.RoundToInt(RandomSelectionPoints) + "";
        }

         finalpos = Gamemanager.instance._PlayersList.IndexOf(Gamemanager.instance.CurrentPlayer);
         PositionJumped = Gamemanager.instance.playerStartingIndex - finalpos;

        int add = 10;
        int sub = 5;

        float PositionIndexPoints = 0;

        if (PositionJumped > 0)
        {
            PositionPerc.text = (add * PositionJumped) + "";
            PositionIndexPoints = ((add * PositionJumped) / 100f) * obtainPoints;
            PositionTotal.text =Mathf.RoundToInt(PositionIndexPoints) + "";
        }
        else if (PositionJumped < 0)
        {
            PositionPerc.text = (sub * PositionJumped) + "";
            PositionIndexPoints = (sub * PositionJumped);
            PositionTotal.text =Mathf.RoundToInt(PositionIndexPoints) + "";
        }

        totalpoints = (obtainPoints + winningBonus + 20 + ModePoints + RandomSelectionPoints) + PositionIndexPoints;

        TotalPoints.text =Mathf.RoundToInt(totalpoints) + "";
    }
    
    public void FirstOkButton()
    {
        GameConfigration.instance.PlayerSound(31);
        TrophiesGained.SetActive(true);
        FirstButton.SetActive(false);
        InitialPointsPanel();
    }

    public void SecondOkButton()
    {
        if (AllTrophiesGainedInaMatch.Count == 0 || AllTrophiesGainedInaMatch.Count == trophyBeingDisplayed)
        {
            TrophiesHandler.Instance.KudoTrophies.Select(i => { i.Newlyreceived = false; return i; }).ToList();
            TrophiesHandler.Instance.MasterTrophies.Select(i => { i.Newlyreceived = false; return i; }).ToList();
            TrophiesHandler.Instance.GoatTrophies.Select(i => { i.Newlyreceived = false; return i; }).ToList();
            Time.timeScale = 1;
            SceneManager.LoadScene(1);
            return;
        }
        InitialPoints.SetActive(false);
        TrophyDisplayed.SetActive(true);

        DisplayingTheTrophiesGained(trophyBeingDisplayed);

        StartCoroutine(WaitandPerform(0.5f, () =>
        {
            TrophiesHandler.Instance.trophyVariables["Coins"] = TrophiesHandler.Instance.trophyVariables["Coins"] + AllTrophiesGainedInaMatch[trophyBeingDisplayed].CoinValue;
            TrophiesHandler.Instance.trophyVariables["Jems"] = TrophiesHandler.Instance.trophyVariables["Jems"] + AllTrophiesGainedInaMatch[trophyBeingDisplayed].JemValue;
            GameConfigration.instance.PlayerSound(31);
            TrophiesHandler.Instance.SaveData(true, playerInfo);
            trophyBeingDisplayed++;
        }));

    }

    public void InitialPointsPanel()
    {
        KudoTrophiesNewlyGained = Enumerable.Range(0, TrophiesHandler.Instance.KudoTrophies.Count)
                         .Where(i => TrophiesHandler.Instance.KudoTrophies[i].Newlyreceived)
                         .ToList();
        MasterTrophiesNewlyGained = Enumerable.Range(0, TrophiesHandler.Instance.MasterTrophies.Count)
                                 .Where(i => TrophiesHandler.Instance.MasterTrophies[i].Newlyreceived)
                                 .ToList();
        GoatTrophiesNewlyGained = Enumerable.Range(0, TrophiesHandler.Instance.GoatTrophies.Count)
                                 .Where(i => TrophiesHandler.Instance.GoatTrophies[i].Newlyreceived)
                                 .ToList();

        AllTrophiesGainedInaMatch.AddRange(KudoTrophiesNewlyGained.Select(index => TrophiesHandler.Instance.KudoTrophies[index]));
        AllTrophiesGainedInaMatch.AddRange(MasterTrophiesNewlyGained.Select(index => TrophiesHandler.Instance.MasterTrophies[index]));
        AllTrophiesGainedInaMatch.AddRange(GoatTrophiesNewlyGained.Select(index => TrophiesHandler.Instance.GoatTrophies[index]));

        XpGained.text ="+"+Mathf.RoundToInt(totalpoints) + "";
        CoinsGained.text = "+" +Mathf.RoundToInt(totalpoints) + "";
        StartCoroutine(WaitandPerform(0.5f, () =>
        {
            TrophiesHandler.Instance.trophyVariables["TotalXp"] = TrophiesHandler.Instance.trophyVariables["TotalXp"] +Mathf.RoundToInt(totalpoints);
            TrophiesHandler.Instance.trophyVariables["TotalXpTM"] = TrophiesHandler.Instance.trophyVariables["TotalXpTM"] +Mathf.RoundToInt(totalpoints);
            TrophiesHandler.Instance.trophyVariables["Coins"] = TrophiesHandler.Instance.trophyVariables["Coins"] +Mathf.RoundToInt(totalpoints);
            TrophiesHandler.Instance.SaveData(true, playerInfo);
            GameConfigration.instance.PlayerSound(31);

        }));
        KudoTotalTrophies.text = KudoTrophiesNewlyGained.Count + "";
        MasterTotalTrophies.text = MasterTrophiesNewlyGained.Count + "";
        GoatTotalTrophies.text = GoatTrophiesNewlyGained.Count + "";

        TrophiesHandler.Instance.trophyVariables["TotalTrophiesTM"] = TrophiesHandler.Instance.trophyVariables["TotalTrophiesTM"] + AllTrophiesGainedInaMatch.Count;
        TrophiesHandler.Instance.trophyVariables["TotalTrophies"] = TrophiesHandler.Instance.KudoTrophies.Count(i => i.received) +
            TrophiesHandler.Instance.MasterTrophies.Count(i => i.received) +
            TrophiesHandler.Instance.GoatTrophies.Count(i => i.received);
    }
    IEnumerator WaitandPerform(float Time, Action action)
    {
        yield return new WaitForSeconds(Time);
        action.Invoke();
    }
    public void DisplayingTheTrophiesGained(int m)
    {
        if (AllTrophiesGainedInaMatch.Count != 0)
        {
            if (AllTrophiesGainedInaMatch[m].tier == 0)
                Heading.text = "Congratulation!\r\nYou earn The Kudo trophy!";
            if (AllTrophiesGainedInaMatch[m].tier == 1)
                Heading.text = "Congratulation!\r\nYou earn The Master trophy!";
            if (AllTrophiesGainedInaMatch[m].tier == 2)
                Heading.text = "Congratulation!\r\nYou earn The Goat trophy!";
        }

        TrophieGainedDisplay.sprite = AllTrophiesGainedInaMatch[m].TrophyImage;
        CoinsForTrophy.text = AllTrophiesGainedInaMatch[m].CoinValue + "";
        JemsForTrophy.text = AllTrophiesGainedInaMatch[m].JemValue + "";
        WhatisTrophyFor.text = AllTrophiesGainedInaMatch[m].WinnigText + "";
        TrophyName.text = AllTrophiesGainedInaMatch[m].TrophyName + "";
    }
    public void TrophiesCheck()
    {
        if (TrophiesHandler.Instance.trophyVariables["TotalTrophies"] >= 5)
        {
            if (!TrophiesHandler.Instance.KudoTrophies[0].received)
            {
                TrophiesHandler.Instance.KudoTrophies[0].Newlyreceived = true;
                TrophiesHandler.Instance.SaveTrophieData(0, TrophiesHandler.Instance.FilePathKudo, TrophiesHandler.Instance.KudoTrophies);
            }

            if (TrophiesHandler.Instance.trophyVariables["TotalTrophies"] >= 25)
            {
                if (!TrophiesHandler.Instance.MasterTrophies[0].received)
                {
                    TrophiesHandler.Instance.MasterTrophies[0].Newlyreceived = true;
                    TrophiesHandler.Instance.SaveTrophieData(0, TrophiesHandler.Instance.FilePathMaster, TrophiesHandler.Instance.MasterTrophies);
                }
            }

            if (TrophiesHandler.Instance.trophyVariables["TotalTrophies"] >= 50)
            {
                if (!TrophiesHandler.Instance.GoatTrophies[0].received)
                {
                    TrophiesHandler.Instance.GoatTrophies[0].Newlyreceived = true;
                    TrophiesHandler.Instance.SaveTrophieData(0, TrophiesHandler.Instance.FilePathGoat, TrophiesHandler.Instance.GoatTrophies);
                }
            }
        }


        if (TrophiesHandler.Instance.trophyVariables["MaxSwappyCombo"] < Gamemanager.instance.maxComboSwappies)
        {
            TrophiesHandler.Instance.trophyVariables["MaxSwappyCombo"] = Gamemanager.instance.maxComboSwappies;
        }


        if (TrophiesHandler.Instance.trophyVariables["TotalDaysPlayed"] >=7)
        {
            if (!TrophiesHandler.Instance.KudoTrophies[1].received)
            {
                TrophiesHandler.Instance.KudoTrophies[1].Newlyreceived = true;
                TrophiesHandler.Instance.SaveTrophieData(1, TrophiesHandler.Instance.FilePathKudo, TrophiesHandler.Instance.KudoTrophies);
            }

            if (TrophiesHandler.Instance.trophyVariables["TotalDaysPlayed"] >= 30)
            {
                if (!TrophiesHandler.Instance.MasterTrophies[1].received)
                {
                    TrophiesHandler.Instance.MasterTrophies[1].Newlyreceived = true;
                    TrophiesHandler.Instance.SaveTrophieData(1, TrophiesHandler.Instance.FilePathMaster, TrophiesHandler.Instance.MasterTrophies);
                }
            }

            if (TrophiesHandler.Instance.trophyVariables["TotalDaysPlayed"] >= 150)
            {
                if (!TrophiesHandler.Instance.GoatTrophies[1].received)
                {
                    TrophiesHandler.Instance.GoatTrophies[1].Newlyreceived = true;
                    TrophiesHandler.Instance.SaveTrophieData(1, TrophiesHandler.Instance.FilePathGoat, TrophiesHandler.Instance.GoatTrophies);
                }
            }
        }

        //Invite 3 friends and win a trophy
        if (WonGame)
        {
            if (!GameConfigration.instance.offline)
            {
                if (TrophiesHandler.Instance.trophyVariables["WinGamesfrom4th"] >= 5)
                {
                    if (!TrophiesHandler.Instance.KudoTrophies[19].received)
                    {
                        TrophiesHandler.Instance.KudoTrophies[19].Newlyreceived = true;
                        TrophiesHandler.Instance.SaveTrophieData(19, TrophiesHandler.Instance.FilePathKudo, TrophiesHandler.Instance.KudoTrophies);
                    }
                }
                if (TrophiesHandler.Instance.trophyVariables["WinGamesfrom4th"] >= 25)
                {
                    if (!TrophiesHandler.Instance.MasterTrophies[19].received)
                    {
                        TrophiesHandler.Instance.MasterTrophies[19].Newlyreceived = true;
                        TrophiesHandler.Instance.SaveTrophieData(19, TrophiesHandler.Instance.FilePathMaster, TrophiesHandler.Instance.MasterTrophies);
                    }
                }
                if (TrophiesHandler.Instance.trophyVariables["WinGamesfrom4th"] >= 100)
                {
                    if (!TrophiesHandler.Instance.GoatTrophies[19].received)
                    {
                        TrophiesHandler.Instance.GoatTrophies[19].Newlyreceived = true;
                        TrophiesHandler.Instance.SaveTrophieData(19, TrophiesHandler.Instance.FilePathGoat, TrophiesHandler.Instance.GoatTrophies);
                    }
                }
            }
            
            if (TrophiesHandler.Instance.trophyVariables["LeastSwappiesTM"]  == 0 || TrophiesHandler.Instance.trophyVariables["LeastSwappiesTM"] >= Gamemanager.instance.CurrentPlayer.mySwappies.Count)
            {
                TrophiesHandler.Instance.trophyVariables["LeastSwappiesTM"] = Gamemanager.instance.CurrentPlayer.mySwappies.Count;
            }
        }
        if (Gamemanager.instance.CurrentPlayer.mySwappies.Count < 100)
        {
            if (!TrophiesHandler.Instance.KudoTrophies[3].received)
            {
                TrophiesHandler.Instance.KudoTrophies[3].Newlyreceived = true;
                TrophiesHandler.Instance.trophyVariables["LeastSwappies"] = Gamemanager.instance.CurrentPlayer.mySwappies.Count;
                TrophiesHandler.Instance.SaveTrophieData(3, TrophiesHandler.Instance.FilePathKudo, TrophiesHandler.Instance.KudoTrophies);
            }
        }
        if (Gamemanager.instance.CurrentPlayer.mySwappies.Count < 75)
        {
            if (!TrophiesHandler.Instance.MasterTrophies[3].received)
            {
                TrophiesHandler.Instance.MasterTrophies[3].Newlyreceived = true;
                TrophiesHandler.Instance.trophyVariables["LeastSwappies"] = Gamemanager.instance.CurrentPlayer.mySwappies.Count;
                TrophiesHandler.Instance.SaveTrophieData(3, TrophiesHandler.Instance.FilePathMaster, TrophiesHandler.Instance.MasterTrophies);
            }
        }
        if (Gamemanager.instance.CurrentPlayer.mySwappies.Count < 50)
        {
            if (!TrophiesHandler.Instance.GoatTrophies[3].received)
            {
                TrophiesHandler.Instance.GoatTrophies[3].Newlyreceived = true;
                TrophiesHandler.Instance.trophyVariables["LeastSwappies"] = Gamemanager.instance.CurrentPlayer.mySwappies.Count;
                TrophiesHandler.Instance.SaveTrophieData(3, TrophiesHandler.Instance.FilePathGoat, TrophiesHandler.Instance.GoatTrophies);
            }
        }
        if (Gamemanager.instance._PlayersList.Count == 2)        
        {
            if (WonGame == true)
            {
                if (totalpoints >= 200f)
                {
                    if (!TrophiesHandler.Instance.KudoTrophies[4].received)
                    {
                        TrophiesHandler.Instance.KudoTrophies[4].Newlyreceived = true;
                        TrophiesHandler.Instance.SaveTrophieData(4, TrophiesHandler.Instance.FilePathKudo, TrophiesHandler.Instance.KudoTrophies);
                    }
                }
            }
            else
            {
                if (totalpoints >= 15f)
                {
                    if (!TrophiesHandler.Instance.KudoTrophies[7].received)
                    {
                        TrophiesHandler.Instance.KudoTrophies[7].Newlyreceived = true;
                        TrophiesHandler.Instance.SaveTrophieData(7, TrophiesHandler.Instance.FilePathKudo, TrophiesHandler.Instance.KudoTrophies);
                    }
                }
            }
            if (totalpoints >= 177f)
            {
                if (!TrophiesHandler.Instance.KudoTrophies[5].received)
                {
                    TrophiesHandler.Instance.KudoTrophies[5].Newlyreceived = true;
                    TrophiesHandler.Instance.SaveTrophieData(5, TrophiesHandler.Instance.FilePathKudo, TrophiesHandler.Instance.KudoTrophies);
                }
            }
        }
        if (Gamemanager.instance._PlayersList.Count == 3)
        {
            if (WonGame == true)
            {
                if (totalpoints >= 300f)
                {
                    if (!TrophiesHandler.Instance.MasterTrophies[4].received)
                    {
                        TrophiesHandler.Instance.MasterTrophies[4].Newlyreceived = true;
                        TrophiesHandler.Instance.SaveTrophieData(4, TrophiesHandler.Instance.FilePathMaster, TrophiesHandler.Instance.MasterTrophies);
                    }
                }
            }
            else
            {
                if (totalpoints >= 10f)
                {
                    if (!TrophiesHandler.Instance.MasterTrophies[7].received)
                    {
                        TrophiesHandler.Instance.MasterTrophies[7].Newlyreceived = true;
                        TrophiesHandler.Instance.SaveTrophieData(7, TrophiesHandler.Instance.FilePathMaster, TrophiesHandler.Instance.MasterTrophies);
                    }
                }
            }
            if (totalpoints >= 377f)
            {
                if (!TrophiesHandler.Instance.MasterTrophies[5].received)
                {
                    TrophiesHandler.Instance.MasterTrophies[5].Newlyreceived = true;
                    TrophiesHandler.Instance.SaveTrophieData(5, TrophiesHandler.Instance.FilePathMaster, TrophiesHandler.Instance.MasterTrophies);
                }
            }
        }
        if (Gamemanager.instance._PlayersList.Count == 4)
        {
            if (WonGame == true)
            {
                if (totalpoints >= 500f)
                {
                    if (!TrophiesHandler.Instance.GoatTrophies[4].received)
                    {
                        TrophiesHandler.Instance.GoatTrophies[4].Newlyreceived = true;
                        TrophiesHandler.Instance.SaveTrophieData(4, TrophiesHandler.Instance.FilePathGoat, TrophiesHandler.Instance.GoatTrophies);
                    }
                }
            }
            else
            {
                if (totalpoints >= 5f)
                {
                    if (!TrophiesHandler.Instance.GoatTrophies[7].received)
                    {
                        TrophiesHandler.Instance.GoatTrophies[7].Newlyreceived = true;
                        TrophiesHandler.Instance.SaveTrophieData(7, TrophiesHandler.Instance.FilePathGoat, TrophiesHandler.Instance.GoatTrophies);
                    }
                }
            }

            if (totalpoints >= 577f)
            {
                if (!TrophiesHandler.Instance.GoatTrophies[5].received)
                {
                    TrophiesHandler.Instance.GoatTrophies[5].Newlyreceived = true;
                    TrophiesHandler.Instance.SaveTrophieData(5, TrophiesHandler.Instance.FilePathGoat, TrophiesHandler.Instance.GoatTrophies);
                }

            }
        }

        //Won 5 2 player Game online in a row

        if (GameConfigration.instance._SwappyPlayer.Count == 2)
        {
            if (totalpoints <= 15)
            {
                if (!TrophiesHandler.Instance.KudoTrophies[7].received)
                {
                    TrophiesHandler.Instance.KudoTrophies[7].Newlyreceived = true;
                    TrophiesHandler.Instance.SaveTrophieData(7, TrophiesHandler.Instance.FilePathKudo, TrophiesHandler.Instance.KudoTrophies);
                }
            }
        }

        if (GameConfigration.instance._SwappyPlayer.Count == 3)
        {
            if (totalpoints <= 10)
            {
                if (!TrophiesHandler.Instance.MasterTrophies[7].received)
                {
                    TrophiesHandler.Instance.MasterTrophies[7].Newlyreceived = true;
                    TrophiesHandler.Instance.SaveTrophieData(7, TrophiesHandler.Instance.FilePathMaster, TrophiesHandler.Instance.MasterTrophies);
                }
            }
        }

        if (GameConfigration.instance._SwappyPlayer.Count == 4)
        {
            if (totalpoints <= 5)
            {
                if (!TrophiesHandler.Instance.GoatTrophies[7].received)
                {
                    TrophiesHandler.Instance.GoatTrophies[7].Newlyreceived = true;
                    TrophiesHandler.Instance.SaveTrophieData(7, TrophiesHandler.Instance.FilePathGoat, TrophiesHandler.Instance.GoatTrophies);
                }
            }
        }

        //Won 3 player Game online in a row
        //Won 4 player Game online in a row


        if (TrophiesHandler.Instance.trophyVariables["MaxSwappyCombo"] >= 15)
        {
            if (!TrophiesHandler.Instance.KudoTrophies[10].received)
            {
                TrophiesHandler.Instance.KudoTrophies[10].Newlyreceived = true;
                TrophiesHandler.Instance.SaveTrophieData(10, TrophiesHandler.Instance.FilePathKudo, TrophiesHandler.Instance.KudoTrophies);
            }
            if (TrophiesHandler.Instance.trophyVariables["MaxSwappyCombo"] >= 25)
            {
                if (!TrophiesHandler.Instance.MasterTrophies[10].received)
                {
                    TrophiesHandler.Instance.MasterTrophies[10].Newlyreceived = true; 
                    TrophiesHandler.Instance.SaveTrophieData(10, TrophiesHandler.Instance.FilePathMaster, TrophiesHandler.Instance.MasterTrophies);
                }
            }
            if (TrophiesHandler.Instance.trophyVariables["MaxSwappyCombo"] >= 30)
            {
                if (!TrophiesHandler.Instance.GoatTrophies[10].received)
                {
                    TrophiesHandler.Instance.GoatTrophies[10].Newlyreceived = true;
                    TrophiesHandler.Instance.SaveTrophieData(10, TrophiesHandler.Instance.FilePathGoat, TrophiesHandler.Instance.GoatTrophies);
                }
            }
        }
        if (GameConfigration.instance.GameMode == mode.Classic)
        {
            if (TrophiesHandler.Instance.trophyVariables["DefeatOppInClassicM"] >= 50)
            {
                if (!TrophiesHandler.Instance.KudoTrophies[11].received)
                {
                    TrophiesHandler.Instance.KudoTrophies[11].Newlyreceived = true;
                    TrophiesHandler.Instance.SaveTrophieData(11, TrophiesHandler.Instance.FilePathKudo, TrophiesHandler.Instance.KudoTrophies);
                }
                if (TrophiesHandler.Instance.trophyVariables["DefeatOppInClassicM"] >= 500)
                {
                    if (!TrophiesHandler.Instance.MasterTrophies[11].received)
                    {
                        TrophiesHandler.Instance.MasterTrophies[11].Newlyreceived = true;
                        TrophiesHandler.Instance.SaveTrophieData(11, TrophiesHandler.Instance.FilePathMaster, TrophiesHandler.Instance.MasterTrophies);
                    }
                }
                if (TrophiesHandler.Instance.trophyVariables["DefeatOppInClassicM"] >= 5000)
                {
                    if (!TrophiesHandler.Instance.GoatTrophies[11].received)
                    {
                        TrophiesHandler.Instance.GoatTrophies[11].Newlyreceived = true;
                        TrophiesHandler.Instance.SaveTrophieData(11, TrophiesHandler.Instance.FilePathGoat, TrophiesHandler.Instance.GoatTrophies);
                    }
                }
            }
        }
        if (GameConfigration.instance.GameMode == mode.Fast)
        {
            if (TrophiesHandler.Instance.trophyVariables["DefeatOppInFastM"] >= 50)
            {
                if (!TrophiesHandler.Instance.KudoTrophies[12].received)
                {
                    TrophiesHandler.Instance.KudoTrophies[12].Newlyreceived = true;
                    TrophiesHandler.Instance.SaveTrophieData(12, TrophiesHandler.Instance.FilePathKudo, TrophiesHandler.Instance.KudoTrophies);
                }
                if (TrophiesHandler.Instance.trophyVariables["DefeatOppInFastM"] >= 500)
                {
                    if (!TrophiesHandler.Instance.MasterTrophies[12].received)
                    {
                        TrophiesHandler.Instance.MasterTrophies[12].Newlyreceived = true;
                        TrophiesHandler.Instance.SaveTrophieData(12, TrophiesHandler.Instance.FilePathMaster, TrophiesHandler.Instance.MasterTrophies);
                    }
                }
                if (TrophiesHandler.Instance.trophyVariables["DefeatOppInFastM"] >= 5000)
                {
                    if (!TrophiesHandler.Instance.GoatTrophies[12].received)
                    {
                        TrophiesHandler.Instance.GoatTrophies[12].Newlyreceived = true;
                        TrophiesHandler.Instance.SaveTrophieData(12, TrophiesHandler.Instance.FilePathGoat, TrophiesHandler.Instance.GoatTrophies);
                    }
                }
            }
        }
        if (GameConfigration.instance.offline)
        {
            if (TrophiesHandler.Instance.trophyVariables["DefeatOppInOfflineM"] >= 100)
            {
                if (!TrophiesHandler.Instance.KudoTrophies[13].received)
                {
                    TrophiesHandler.Instance.KudoTrophies[13].Newlyreceived = true;
                    TrophiesHandler.Instance.SaveTrophieData(13, TrophiesHandler.Instance.FilePathKudo, TrophiesHandler.Instance.KudoTrophies);
                }
                if (TrophiesHandler.Instance.trophyVariables["DefeatOppInOfflineM"] >= 1000)
                {
                    if (!TrophiesHandler.Instance.MasterTrophies[13].received)
                    {
                        TrophiesHandler.Instance.MasterTrophies[13].Newlyreceived = true;
                        TrophiesHandler.Instance.SaveTrophieData(13, TrophiesHandler.Instance.FilePathMaster, TrophiesHandler.Instance.MasterTrophies);
                    }
                }
                if (TrophiesHandler.Instance.trophyVariables["DefeatOppInOfflineM"] >= 10000)
                {
                    if (!TrophiesHandler.Instance.GoatTrophies[13].received)
                    {
                        TrophiesHandler.Instance.GoatTrophies[13].Newlyreceived = true;
                        TrophiesHandler.Instance.SaveTrophieData(13, TrophiesHandler.Instance.FilePathGoat, TrophiesHandler.Instance.GoatTrophies);
                    }
                }
            }
        }
        if (!GameConfigration.instance.offline) 
        {
            if (TrophiesHandler.Instance.trophyVariables["DefeatOppInOnlineM"] >= 100)
            {
                if (!TrophiesHandler.Instance.KudoTrophies[14].received)
                {
                    TrophiesHandler.Instance.KudoTrophies[14].Newlyreceived = true;
                    TrophiesHandler.Instance.SaveTrophieData(14, TrophiesHandler.Instance.FilePathKudo, TrophiesHandler.Instance.KudoTrophies);
                }
                if (TrophiesHandler.Instance.trophyVariables["DefeatOppInOnlineM"] >= 1000)
                {
                    if (!TrophiesHandler.Instance.MasterTrophies[14].received)
                    {
                        TrophiesHandler.Instance.MasterTrophies[14].Newlyreceived = true;
                        TrophiesHandler.Instance.SaveTrophieData(14, TrophiesHandler.Instance.FilePathMaster, TrophiesHandler.Instance.MasterTrophies);
                    }
                }
                if (TrophiesHandler.Instance.trophyVariables["DefeatOppInOnlineM"] >= 10000)
                {
                    if (!TrophiesHandler.Instance.GoatTrophies[14].received)
                    {
                        TrophiesHandler.Instance.GoatTrophies[14].Newlyreceived = true;
                        TrophiesHandler.Instance.SaveTrophieData(14, TrophiesHandler.Instance.FilePathGoat, TrophiesHandler.Instance.GoatTrophies);
                    }
                }
            }
        }
        if (GameConfigration.instance.GameMode == mode.Power)
        {
            if (TrophiesHandler.Instance.trophyVariables["DefeatOppInPowerM"] >= 50)
            {
                if (!TrophiesHandler.Instance.KudoTrophies[15].received)
                {
                    TrophiesHandler.Instance.KudoTrophies[15].Newlyreceived = true;
                    TrophiesHandler.Instance.SaveTrophieData(15, TrophiesHandler.Instance.FilePathKudo, TrophiesHandler.Instance.KudoTrophies);
                }
                if (TrophiesHandler.Instance.trophyVariables["DefeatOppInPowerM"] >= 500)
                {
                    if (!TrophiesHandler.Instance.MasterTrophies[15].received)
                    {
                        TrophiesHandler.Instance.MasterTrophies[15].Newlyreceived = true;
                        TrophiesHandler.Instance.SaveTrophieData(15, TrophiesHandler.Instance.FilePathMaster, TrophiesHandler.Instance.MasterTrophies);
                    }
                }
                if (TrophiesHandler.Instance.trophyVariables["DefeatOppInPowerM"] >= 5000)
                {
                    if (!TrophiesHandler.Instance.GoatTrophies[15].received)
                    {
                        TrophiesHandler.Instance.GoatTrophies[15].Newlyreceived = true;
                        TrophiesHandler.Instance.SaveTrophieData(15, TrophiesHandler.Instance.FilePathGoat, TrophiesHandler.Instance.GoatTrophies);
                    }
                }
            }
        }
        if (TrophiesHandler.Instance.trophyVariables["TotalPlayedGames"] >= 100)
        {
            if (!TrophiesHandler.Instance.KudoTrophies[16].received)
            {
                TrophiesHandler.Instance.KudoTrophies[16].Newlyreceived = true;
                TrophiesHandler.Instance.SaveTrophieData(16, TrophiesHandler.Instance.FilePathKudo, TrophiesHandler.Instance.KudoTrophies);
            }

            if (TrophiesHandler.Instance.trophyVariables["TotalPlayedGames"] >= 1000)
            {
                if (!TrophiesHandler.Instance.MasterTrophies[16].received)
                {
                    TrophiesHandler.Instance.MasterTrophies[16].Newlyreceived = true;
                    TrophiesHandler.Instance.SaveTrophieData(16, TrophiesHandler.Instance.FilePathMaster, TrophiesHandler.Instance.MasterTrophies);
                }

                if (TrophiesHandler.Instance.trophyVariables["TotalPlayedGames"] >= 10000)
                {
                    if (!TrophiesHandler.Instance.GoatTrophies[16].received)
                    {
                        TrophiesHandler.Instance.GoatTrophies[16].Newlyreceived = true;
                        TrophiesHandler.Instance.SaveTrophieData(16, TrophiesHandler.Instance.FilePathGoat, TrophiesHandler.Instance.GoatTrophies);
                    }
                }
            }
        }
        //Player game before 1st jan 2025
        if (TrophiesHandler.Instance.trophyVariables["CompleteGameBefore1stJan2025"] == 1)
        {
            if (!TrophiesHandler.Instance.KudoTrophies[17].received)
            {
                TrophiesHandler.Instance.KudoTrophies[17].Newlyreceived = true;
                TrophiesHandler.Instance.SaveTrophieData(17, TrophiesHandler.Instance.FilePathKudo, TrophiesHandler.Instance.KudoTrophies);
            }
        }
        if (TrophiesHandler.Instance.trophyVariables["CompleteGameBefore1stOct2024"] == 1)
        {
            if (!TrophiesHandler.Instance.MasterTrophies[17].received)
            {
                TrophiesHandler.Instance.MasterTrophies[17].Newlyreceived = true;
                TrophiesHandler.Instance.SaveTrophieData(17, TrophiesHandler.Instance.FilePathMaster, TrophiesHandler.Instance.MasterTrophies);
            }
        }
        if (TrophiesHandler.Instance.trophyVariables["Betaversion"] == 1)
        {
            if (!TrophiesHandler.Instance.GoatTrophies[17].received)
            {
                TrophiesHandler.Instance.GoatTrophies[17].Newlyreceived = true;
                TrophiesHandler.Instance.SaveTrophieData(17, TrophiesHandler.Instance.FilePathGoat, TrophiesHandler.Instance.GoatTrophies);
            }
        }
        //Purchase Something in the shop
        //Win 5 games online in a row Last Pos

        if (TrophiesHandler.Instance.trophyVariables["TotalXp"] >= 10000)
        {
            if (!TrophiesHandler.Instance.KudoTrophies[20].received)
            {
                TrophiesHandler.Instance.KudoTrophies[20].Newlyreceived = true;
                TrophiesHandler.Instance.SaveTrophieData(20, TrophiesHandler.Instance.FilePathKudo, TrophiesHandler.Instance.KudoTrophies);
            }

            if (TrophiesHandler.Instance.trophyVariables["TotalXp"] >= 50000)
            {
                if (!TrophiesHandler.Instance.MasterTrophies[20].received)
                {
                    TrophiesHandler.Instance.MasterTrophies[20].Newlyreceived = true;
                    TrophiesHandler.Instance.SaveTrophieData(20, TrophiesHandler.Instance.FilePathMaster, TrophiesHandler.Instance.MasterTrophies);
                }
            }
            if (TrophiesHandler.Instance.trophyVariables["TotalXp"] >= 250000)
            {
                if (!TrophiesHandler.Instance.GoatTrophies[20].received)
                {
                    TrophiesHandler.Instance.GoatTrophies[20].Newlyreceived = true;
                    TrophiesHandler.Instance.SaveTrophieData(20, TrophiesHandler.Instance.FilePathGoat, TrophiesHandler.Instance.GoatTrophies);
                }
            }
        }

        //Exrtras
        if (GameConfigration.instance._SwappyPlayer.Count == 2)
        {
            if (totalpoints > TrophiesHandler.Instance.trophyVariables["2-PlayerMP"])
            {
                TrophiesHandler.Instance.trophyVariables["2-PlayerMP"] = Mathf.RoundToInt(totalpoints);
            }
        }
        else if (GameConfigration.instance._SwappyPlayer.Count == 3)
        {
            if (totalpoints > TrophiesHandler.Instance.trophyVariables["3-PlayerMP"])
            {
                TrophiesHandler.Instance.trophyVariables["3-PlayerMP"] = Mathf.RoundToInt(totalpoints);
            }
        }
        else if (GameConfigration.instance._SwappyPlayer.Count == 4)
        {
            if (totalpoints > TrophiesHandler.Instance.trophyVariables["4-PlayerMP"])
            {
                TrophiesHandler.Instance.trophyVariables["4-PlayerMP"] = Mathf.RoundToInt(totalpoints);
            }
        }
    }

}
