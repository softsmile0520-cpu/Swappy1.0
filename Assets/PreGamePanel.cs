using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;


public class PreGamePanel : MonoBehaviour
{

    public static PreGamePanel instance;

    public Image modename;
    public List<Sprite> ModeSprites;

    // Player Profile Data
    public PlayerDataPreGame PlayerFinalData;

    // GameStart Things
    public TextMeshProUGUI ModeNameChange;
    public TextMeshProUGUI NoOfPlayers;
    public TextMeshProUGUI RandomMode;
    public TextMeshProUGUI StartPos;

    //PointsToBeearned
    // GameStart Things
    public TextMeshProUGUI Wingame;
    public TextMeshProUGUI Finish2nd;
    public TextMeshProUGUI Finish3nd;
    public TextMeshProUGUI LoseGame;

    //PointstoBeEarned
    public GameObject WinGame;
    public GameObject Place2nd;
    public GameObject Place3nd;
    public GameObject PlaceLast;


    //Detailed Stats
    public TextMeshProUGUI WinnigBonusPerc;
    public TextMeshProUGUI WinnigBonusTotal;

    public TextMeshProUGUI FinishaGamePerc;
    public TextMeshProUGUI FinishaGameTotal;

    public TextMeshProUGUI ModeName;
    public TextMeshProUGUI ModeNamePerc;
    public TextMeshProUGUI ModeNameTotal;

    public TextMeshProUGUI RandomPerc;
    public TextMeshProUGUI RandomTotal;

    public TextMeshProUGUI PositionPerc;
    public TextMeshProUGUI PositionTotal;

    public TextMeshProUGUI EasyEnemy;
    public TextMeshProUGUI MedimEnemy;
    public TextMeshProUGUI HardEnemy;

    public TextMeshProUGUI EasyEnemyV;
    public TextMeshProUGUI MedimEnemyV;
    public TextMeshProUGUI HardEnemyV;

    public TextMeshProUGUI EasyEnemyT;
    public TextMeshProUGUI MedimEnemyT;
    public TextMeshProUGUI HardEnemyT;

    public TextMeshProUGUI DefeatedEnemies;
    public TextMeshProUGUI TotalEnemies;
    public TextMeshProUGUI TotalPercOfCorePoints;
    public TextMeshProUGUI FinalCorePoints;

    public TextMeshProUGUI MaxCorePoints;

    public TextMeshProUGUI TotalPoints;

    public TextMeshProUGUI WintheGame;

    public List<Sprite> BGDisplayesSquare;
    public List<Sprite> BGDisplayesRectangle;



    public Transform content;


    public int winnigPoints = 0;
    public int Points2nd = 0;
    public int Points3nd = 0;
    public int Points4nd = 0;

    public int PosPlayer = 0;
    // Start is called before the first frame update

    public static PreGamePanel ShowUI()
    {
        if (instance == null)
        {
            GameObject obj = Instantiate(Resources.Load("PreGamePanel")) as GameObject;

            obj.gameObject.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);

            instance = obj.GetComponent<PreGamePanel>();
        }

        

        return instance;
    }
    void Start()
    {
        
        modename.sprite = ModeSprites[(int)GameConfigration.instance.GameMode];

        ModeNameChange.text = GameConfigration.instance.GameMode.ToString() + " Mode,";
        NoOfPlayers.text = Gamemanager.instance._PlayersList.Count + " Players";
        if (GameConfigration.instance.RandomSelected)
        {
            RandomMode.text = "Random,";
        }
        else
        {
            RandomMode.text = "No Random,";
        }
        StartPos.text = "Start" + " " + (Gamemanager.instance._PlayersList.FindIndex(player => !player.AiSwappy) + 1) + "";


        for (int i = 0; i < Gamemanager.instance._PlayersList.Count; i++)
        {
            PlayerDataPreGame   PlayerData = Instantiate(PlayerFinalData, content);

            PlayerData.SetResult(Gamemanager.instance._PlayersList[i]);
        }
        Invoke(nameof(LateStart), 0.1f);
        PointstoBeEarned();
        PointstoBeEarnedPlace();
        PointsCalculations();

    }
    void LateStart()
    {
        
    }

    public int corePoints;
    public int winnigBonus;
    public int finalcorePoints2nd;
    public int finalcorePoints3rd;
    public int finalcorePoints4th;
    public int startedfrom4;
    public int startedfrom3;
    public int startedfrom2;
    public int startedfrom1;
    public int FinalPosBonuswin;
    public int FinalPosBonus2nd;
    public int FinalPosBonus3rd;
    public int FinalPosBonusLose;
    public int Playerpos;
    public int RandomBonus;
    public int modeBonus;
    public int finalcorePointswon;

    public int modeBonus1;
    public int modeBonus2;
    // New References
    public Image targetImage; 
    public Sprite[] sprites;
    public GameObject Players;
    private int currentIndex = -1;
    public Vector3[] localPositions;
    public float RandomSelectionPoints;
    public float obtainPoints;
    public float totalpoints = 0;
    public float winningBonus = 0;
    public float ModePoints = 0;
    float PositionIndexPoints = 0;

    void PointstoBeEarned()
    {
        corePoints = 0;
        for (int i = 0; i < GameConfigration.instance._SwappyPlayer.Count; i++)
        {
            if (GameConfigration.instance._SwappyPlayer[i].AiSwappy)
            {
                if (GameConfigration.instance._SwappyPlayer[i].PlayerDifficulty == Difficulty.Easy)
                {
                    corePoints = corePoints + 30;
                }
                else if (GameConfigration.instance._SwappyPlayer[i].PlayerDifficulty == Difficulty.Medium)
                {
                    corePoints = corePoints + 60;
                }
                else if (GameConfigration.instance._SwappyPlayer[i].PlayerDifficulty == Difficulty.Hard)
                {
                    corePoints = corePoints + 90;
                }
            }
        }
        winnigBonus = 0;

        finalcorePointswon = corePoints;
        winnigBonus = Mathf.RoundToInt(finalcorePointswon / 2);
        

        finalcorePoints2nd = 0;
        finalcorePoints3rd = 0;
        if (Gamemanager.instance._PlayersList.Count == 4)
        {
            finalcorePoints2nd = Mathf.RoundToInt(corePoints * 0.67f);
            finalcorePoints3rd = Mathf.RoundToInt(corePoints * 0.33f);
        }
        else if (Gamemanager.instance._PlayersList.Count == 3)
        {
            finalcorePoints2nd = Mathf.RoundToInt(corePoints * 0.50f);
            finalcorePoints3rd = 0;
        }
        finalcorePoints4th = 0;
        startedfrom4 = 0;
        startedfrom4 = Mathf.RoundToInt(corePoints * 0.30f);
        startedfrom3 = 0;
        startedfrom3 = Mathf.RoundToInt(finalcorePoints2nd * 0.20f);
        startedfrom2 = 0;
        startedfrom2 = Mathf.RoundToInt(finalcorePoints3rd * 0.10f);
        startedfrom1 = 0;

         FinalPosBonuswin = 0;
         FinalPosBonus2nd = 0;
         FinalPosBonus3rd = 0;
         FinalPosBonusLose = 0;


         Playerpos = Gamemanager.instance._PlayersList.FindIndex(player => !player.AiSwappy) + 1;
        if (Playerpos == 4)
        {
            FinalPosBonuswin = startedfrom4;
            FinalPosBonus2nd = startedfrom3;
            FinalPosBonus3rd = startedfrom2;
        }
        else if (Playerpos == 3)
        {
            FinalPosBonuswin = Mathf.RoundToInt(corePoints * 0.20f);
            FinalPosBonus2nd = Mathf.RoundToInt(finalcorePoints2nd * 0.10f);
            FinalPosBonus3rd = 0;
            if (GameConfigration.instance._SwappyPlayer.Count == 4)
            {
                FinalPosBonusLose = -5;
            }
        }
        else if (Playerpos == 2)
        {
            FinalPosBonuswin = Mathf.RoundToInt(corePoints * 0.10f);
            FinalPosBonus2nd = 0;
            if (GameConfigration.instance._SwappyPlayer.Count == 4)
            {
                FinalPosBonus3rd = -5;
                FinalPosBonusLose = -10;
            }
            if (GameConfigration.instance._SwappyPlayer.Count == 3)
            {
                FinalPosBonus3rd = -5;
            }
        }
        else if (Playerpos == 1)
        {
            FinalPosBonuswin = 0;
            FinalPosBonus2nd = -5;
            FinalPosBonus3rd = -10;
            FinalPosBonusLose = -15;
            if (GameConfigration.instance._SwappyPlayer.Count == 4)
            {
                FinalPosBonus2nd = -5;
                FinalPosBonus3rd = -10;
                FinalPosBonusLose = -15;
            }
            else if (GameConfigration.instance._SwappyPlayer.Count == 3)
            {
                FinalPosBonusLose = -10;
            }
            else if (GameConfigration.instance._SwappyPlayer.Count == 2)
            {
                FinalPosBonusLose = -5;
            }
        }
         RandomBonus = 0;
        if (GameConfigration.instance.RandomSelected)
        {
            RandomBonus = Mathf.RoundToInt(corePoints * 0.10f);
        }
        else
        {
            RandomBonus = 0;
        }

          modeBonus = 0;
        
        if (GameConfigration.instance.GameMode == mode.Classic)
        {
            modeBonus = Mathf.RoundToInt(corePoints * 0.10f);
            modeBonus1 = Mathf.RoundToInt(finalcorePoints2nd * 0.10f);
            modeBonus2 = Mathf.RoundToInt(finalcorePoints3rd * 0.10f);
        }
        else if (GameConfigration.instance.GameMode == mode.Fast)
        {
            modeBonus = Mathf.RoundToInt(corePoints * 0.30f);
            modeBonus1 = Mathf.RoundToInt(finalcorePoints2nd * 0.30f);
            modeBonus2 = Mathf.RoundToInt(finalcorePoints3rd * 0.30f);
        }
        else if (GameConfigration.instance.GameMode == mode.Power)
        {
            modeBonus = 0;
            modeBonus1 = 0;
            modeBonus2 = 0;
        }
        winnigPoints = finalcorePointswon + winnigBonus + FinalPosBonuswin + RandomBonus + modeBonus + 20;
        Points2nd = (finalcorePoints2nd   + RandomBonus + modeBonus1 + 20) + FinalPosBonus2nd;
        Points3nd = (finalcorePoints3rd  + RandomBonus + modeBonus2 + 20) + FinalPosBonus3rd;
        Points4nd = (20) + FinalPosBonusLose;

        Wingame.text = winnigPoints + "";
        Finish2nd.text = Points2nd + "";
        Finish3nd.text = Points3nd + "";
        LoseGame.text = Points4nd + "";
    }

    void PointstoBeEarnedPlace()
    {

        int n = Gamemanager.instance._PlayersList.Count;
        switch (n)
        {
            case 4:


                return;

            case 3:
                Place3nd.SetActive(false);
                return;

            case 2:
                Place2nd.SetActive(false);
                Place3nd.SetActive(false);
                return;
        }
    }
    public void PointsCalculations()
    {
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

       /* totalpoints = (obtainPoints + winningBonus + 20 + ModePoints + RandomSelectionPoints) + PositionIndexPoints;

        TotalPoints.text = Mathf.RoundToInt(totalpoints) + "";*/
    }
    public void SettingPanel_()
    {
        GamePlaySettings.ShowUI();
    }
    public void backPressed()
    {
        Time.timeScale = 1;
        Destroy(this.gameObject);
    }
    public void ChangeImage(int index)
    {

        int PositionJumped = 0;
        int add = 10;
        int sub = 5;
        float ModePoints;
        switch (index)
        {

            case 0:

               // Players.SetActive(false);
                targetImage.enabled = true;
                targetImage.gameObject.SetActive(true);
                targetImage.rectTransform.localPosition = localPositions[index];
                targetImage.sprite = sprites[index];
                WinnigBonusTotal.text = winnigBonus + "";
                WinnigBonusPerc.text = 50 + "";
                FinishaGamePerc.text = 20 + "";
                FinishaGameTotal.text = 20 + "";
                WintheGame.text = "WIN THE GAME";
                PositionJumped = GameConfigration.instance._SwappyPlayer.IndexOf(GameConfigration.instance._SwappyPlayer.Find(a => !a.AiSwappy)) -index;


                ModePoints = (10 / 100f) * finalcorePointswon;
                ModeNameTotal.text = Mathf.RoundToInt(ModePoints) + "";
                //
                TotalPercOfCorePoints.text = 100 + "";
                FinalCorePoints.text = finalcorePointswon+"";
                if (PositionJumped > 0)
                {
                    PositionPerc.text = (add * PositionJumped) + "";
                    float PositionIndexPoints = ((add * PositionJumped) / 100f) * corePoints;
                    PositionTotal.text = Mathf.RoundToInt(PositionIndexPoints) + "";
                }
                else if (PositionJumped < 0)
                {
                    PositionPerc.text = (sub * PositionJumped) + "";
                    float PositionIndexPoints = (sub * PositionJumped);
                    PositionTotal.text = Mathf.RoundToInt(PositionIndexPoints) + "";
                }
                else if (PositionJumped == 0)
                {
                    PositionPerc.text = 0 + "";
                    PositionTotal.text = 0 + "";
                }

                int DeadPlayers = Gamemanager.instance._PlayersList.Count - 1;
                    int OtherPlayers;
                OtherPlayers = Gamemanager.instance._PlayersList.Count;
                DefeatedEnemies.text = DeadPlayers + "";
                TotalEnemies.text = OtherPlayers + "";
                TotalPoints.text = winnigPoints + "";

                break;


            case 1:

               // Players.SetActive(false);
                targetImage.enabled = true;
                targetImage.gameObject.SetActive(true);
                targetImage.rectTransform.localPosition = localPositions[index];
                targetImage.sprite = sprites[index];
                WinnigBonusTotal.text = 0 + "";
                WinnigBonusPerc.text = 0 + "";
                FinishaGamePerc.text = 20 + "";
                FinishaGameTotal.text = 20 + "";
                WintheGame.text = "FINISH 2nd";
                PositionJumped = GameConfigration.instance._SwappyPlayer.IndexOf(GameConfigration.instance._SwappyPlayer.Find(a => !a.AiSwappy)) - index;

                ModePoints = (10 / 100f) * finalcorePoints2nd;
                ModeNameTotal.text = Mathf.RoundToInt(ModePoints) + "";



                if (GameConfigration.instance._SwappyPlayer.Count == 4)
                {
                    TotalPercOfCorePoints.text = 67 + "";
                    FinalCorePoints.text = finalcorePoints2nd + "";
                }
                else if (GameConfigration.instance._SwappyPlayer.Count == 3)
                {
                    TotalPercOfCorePoints.text = 50 + "";
                    FinalCorePoints.text = finalcorePoints2nd + "";
                } else if (GameConfigration.instance._SwappyPlayer.Count == 2)
                {
                    TotalPercOfCorePoints.text = 0 + "";
                    FinalCorePoints.text = finalcorePoints2nd + "";
                }



                if (PositionJumped > 0)
                {
                    PositionPerc.text = (add * PositionJumped) + "";
                    float PositionIndexPoints = ((add * PositionJumped) / 100f) * finalcorePoints2nd;
                    PositionTotal.text = Mathf.RoundToInt(PositionIndexPoints) + "";
                }
                else if (PositionJumped < 0)
                {
                    PositionPerc.text = (sub * PositionJumped) + "";
                    float PositionIndexPoints = (sub * PositionJumped);
                    PositionTotal.text = Mathf.RoundToInt(PositionIndexPoints) + "";
                   
                }
                else if(PositionJumped == 0)
                {
                    PositionPerc.text = 0 + "";
                    PositionTotal.text = 0 + "";
                }


                if (GameConfigration.instance.RandomSelected == true)
                {
                    RandomPerc.text = 10 + "";
                    RandomSelectionPoints = (10 / 100f) * obtainPoints;
                    RandomTotal.text = Mathf.RoundToInt(RandomSelectionPoints) + "";
                }
                else
                {
                    RandomPerc.text = 0 + "";
                    RandomSelectionPoints = (0 / 100f) * obtainPoints;
                    RandomTotal.text = Mathf.RoundToInt(RandomSelectionPoints) + "";
                }

                int DeadPlayers2 = Gamemanager.instance._PlayersList.Count - 2;
                int OtherPlayers2;
                OtherPlayers2 = Gamemanager.instance._PlayersList.Count;
                DefeatedEnemies.text = DeadPlayers2 + "";
                TotalEnemies.text = OtherPlayers2 + "";
                TotalPoints.text = Points2nd + "";

                break;

                
            case 2:

                //Players.SetActive(false);
                targetImage.enabled = true;
                targetImage.gameObject.SetActive(true);
                targetImage.rectTransform.localPosition = localPositions[index];
                targetImage.sprite = sprites[index];
                WinnigBonusTotal.text = 0 + "";
                WinnigBonusPerc.text = 0 + "";
                FinishaGamePerc.text = 20 + "";
                FinishaGameTotal.text = 20 + "";
                WintheGame.text = "FINISH 3rd";
                PositionJumped = GameConfigration.instance._SwappyPlayer.IndexOf(GameConfigration.instance._SwappyPlayer.Find(a => !a.AiSwappy)) - index;
                ModePoints = (10 / 100f) * finalcorePoints3rd;
                ModeNameTotal.text = Mathf.RoundToInt(ModePoints) + "";
                if (PositionJumped > 0)
                {
                    PositionPerc.text = (add * PositionJumped) + "";
                    float PositionIndexPoints = ((add * PositionJumped) / 100f) * finalcorePoints3rd;
                    PositionTotal.text = Mathf.RoundToInt(PositionIndexPoints) + "";
                }
                else if (PositionJumped < 0)
                {
                    PositionPerc.text = (sub * PositionJumped) + "";
                    float PositionIndexPoints = (sub * PositionJumped);
                    PositionTotal.text = Mathf.RoundToInt(PositionIndexPoints) + "";
                }
                else if (PositionJumped == 0)
                {
                    PositionPerc.text = 0 + "";
                    PositionTotal.text = 0 + "";
                }
                if (GameConfigration.instance.RandomSelected == true)
                {
                    RandomPerc.text = 10 + "";
                    RandomSelectionPoints = (10 / 100f) * obtainPoints;
                    RandomTotal.text = Mathf.RoundToInt(RandomSelectionPoints) + "";
                }
                else
                {
                    RandomPerc.text = 0 + "";
                    RandomSelectionPoints = (0 / 100f) * obtainPoints;
                    RandomTotal.text = Mathf.RoundToInt(RandomSelectionPoints) + "";
                }

                if (Gamemanager.instance._PlayersList.Count == 4)
                {
                    TotalPercOfCorePoints.text = 33 + "";
                    FinalCorePoints.text = finalcorePoints3rd + "";
                }
                else if (Gamemanager.instance._PlayersList.Count == 3)
                {
                    TotalPercOfCorePoints.text = 0 + "";
                    FinalCorePoints.text = finalcorePoints3rd + "";
                }

                int DeadPlayers3 = Gamemanager.instance._PlayersList.Count - 3;
                int OtherPlayers3;
                OtherPlayers3 = Gamemanager.instance._PlayersList.Count;
                DefeatedEnemies.text = DeadPlayers3 + "";
                TotalEnemies.text = OtherPlayers3 + "";
                TotalPoints.text = Points3nd + "";
                break;
            case 3:

                //Players.SetActive(false);
                targetImage.enabled = true;
                targetImage.gameObject.SetActive(true);
                targetImage.rectTransform.localPosition = localPositions[index];
                targetImage.sprite = sprites[index];
                WinnigBonusTotal.text = 0 + "";
                WinnigBonusPerc.text = 0 + "";
                FinishaGamePerc.text = 20 + "";
                FinishaGameTotal.text = 20 + "";
                WintheGame.text = "LOSE THE GAME";
                PositionJumped = GameConfigration.instance._SwappyPlayer.IndexOf(GameConfigration.instance._SwappyPlayer.Find(a => !a.AiSwappy)) - index;
                ModePoints = (10 / 100f) * finalcorePoints4th;
                ModeNameTotal.text = Mathf.RoundToInt(ModePoints) + "";


                    TotalPercOfCorePoints.text = 0 + "";
                    FinalCorePoints.text = 0 + "";

                if (PositionJumped > 0)
                {
                    PositionPerc.text = (add * PositionJumped) + "";
                    float PositionIndexPoints = ((add * PositionJumped) / 100f) * finalcorePoints4th;
                    PositionTotal.text = Mathf.RoundToInt(PositionIndexPoints) + "";
                }
                else if (PositionJumped < 0)
                {
                    PositionPerc.text = (sub * PositionJumped) + "";
                    float PositionIndexPoints = (sub * PositionJumped);
                    PositionTotal.text = Mathf.RoundToInt(PositionIndexPoints) + "";
                }
                else if (PositionJumped == 0)
                {
                    PositionPerc.text = 0 + "";
                    PositionTotal.text = 0 + "";
                }
                if (GameConfigration.instance.RandomSelected == true)
                {
                    RandomPerc.text = 10 + "";
                    RandomSelectionPoints = (10 / 100f) * obtainPoints;
                    RandomTotal.text = Mathf.RoundToInt(RandomSelectionPoints) + "";
                }
                else
                {
                    RandomPerc.text = 0 + "";
                    RandomSelectionPoints = (0 / 100f) * obtainPoints;
                    RandomTotal.text = Mathf.RoundToInt(RandomSelectionPoints) + "";
                }
                int DeadPlayers4 = Gamemanager.instance._PlayersList.Count - 4;
                int OtherPlayers4;
                OtherPlayers4 = Gamemanager.instance._PlayersList.Count;
                DefeatedEnemies.text = DeadPlayers4 + "";
                TotalEnemies.text = OtherPlayers4 + "";
                TotalPoints.text = Points4nd + "";
                break;



        }
        switch (GameConfigration.instance.GameMode)
        {
            case mode.Classic:

                ModeNamePerc.text = 10 + "";

                break;

            case mode.Fast:

                ModeNamePerc.text = 30 + "";
               // ModePoints = (30 / 100f) * obtainPoints;
               // ModeNameTotal.text = Mathf.RoundToInt(ModePoints) + "";

                break;

            case mode.Power:

                ModeNamePerc.text = 0 + "";
               // ModePoints = (0 / 100f) * obtainPoints;
               // ModeNameTotal.text = Mathf.RoundToInt(ModePoints) + "";

                break;
        }

        if (GameConfigration.instance.RandomSelected == true)
        {
            RandomPerc.text = 10 + "";
            RandomSelectionPoints = (10 / 100f) * obtainPoints;
            RandomTotal.text = Mathf.RoundToInt(RandomSelectionPoints) + "";
        }
        else
        {
            RandomPerc.text = 0 + "";
            RandomSelectionPoints = (0 / 100f) * obtainPoints;
            RandomTotal.text = Mathf.RoundToInt(RandomSelectionPoints) + "";
        }
    }
    public void paneloff()
    {
        targetImage.gameObject.SetActive(false );
        Players.SetActive(true);
    }
}
