using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePlayCanvas : MonoBehaviour
{
    public static GamePlayCanvas instance;
    // Start is called before the first frame update
    public playerDisplayData PlayerInfoPref;
    public Transform content;
    public List<GameObject> PlayersInfo;
    public List<playerDisplayData> Displays;
    public GameObject PopUpText;

    public Image GameModeImage;

    public List<Sprite> ModeSprites;

    public List<SwappyPlayer> Players;
    public List<int> PreviousScore = new List<int>();

    public GameObject ScoreDiffPrefab;

    public GameObject BoardStyle;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        Players = Gamemanager.instance._PlayersList;
        ChangeBoard();

        for (int i = 0; i < Players.Count ; i++)
        {
            Displays.Add(Instantiate(PlayerInfoPref, content));
        }
        Instantiate(Gamemanager.instance.TimerPrefab, Displays[Gamemanager.instance.CurrentPlayerIndex].TimerPos);

        GameModeImage.sprite = ModeSprites[(int)GameConfigration.instance.GameMode];
    }

    public void ChangeBoard()
    {
        BoardStyle.GetComponent<MeshRenderer>().material.mainTexture = GameConfigration.instance.boards[GameConfigration.instance.currentBoardIndex].ViewImage.texture;
    }
    public IEnumerator ShowScoreDiff(int Score)
    {
        PopUpText.SetActive(true);
        PopUpText.GetComponent<TextMeshProUGUI>().text = "" + Score;
        yield return new WaitForSeconds(1);
        PopUpText.SetActive(false);
    }
    // Update is called once per frame

    public void UpdatePlayerScore()
    {
        for (int i = 0; i < Players.Count; i++)
        {
            if (Players[i].Dead)
            {
                Displays[i].DeadImage.SetActive(true);
                Displays[i].Score.gameObject.SetActive(false);
            }
            else if (!Players[i].Dead)
            {
                if (PreviousScore[i] != Players[i].score)
                {
                    int ScoreDiff = Players[i].score - PreviousScore[i];
                    if (PreviousScore[i] > Players[i].score)
                    {
                        Displays[i].Score.text = ScoreDiff.ToString();
                        Displays[i].Score.color = Color.red;
                        Displays[i].ScoreDiff.text = ScoreDiff.ToString();
                    }
                    else
                    {
                        if (ScoreDiff > Gamemanager.instance.maxComboSwappies)
                        {
                            Gamemanager.instance.maxComboSwappies = ScoreDiff;
                        }
                        //if (TrophiesHandler.Instance.trophyVariables["MaxSwappyComboTM"] < Gamemanager.instance.maxComboSwappies)
                        //{
                        //    TrophiesHandler.Instance.trophyVariables["MaxSwappyComboTM"] = Gamemanager.instance.maxComboSwappies;
                        //}
                        Displays[i].Score.color = Color.green;
                        Displays[i].ScoreDiff.text = "+" + ScoreDiff.ToString();
                    }
                    Displays[i].ScoreDiff.gameObject.SetActive(true);
                    Displays[i].ScoreDiffPos.SetActive(true);
                    StartCoroutine(CloseScoreDiff(i));
                }
                if (Players[i].AiSwappy == false)
                {
                    Gamemanager.instance.GiveScoreAppreciation(Players[i].score, PreviousScore[i]);
                }
                Displays[i].Score.text = "" + Players[i].score.ToString();
                PreviousScore[i] = Players[i].score;
            }
        }
    }
    IEnumerator CloseScoreDiff(int i)
    {
        yield return new WaitForSeconds(0.5f);
        Displays[i].ScoreDiffPos.SetActive(false);
    }
    public void SetingPanel()
    {
        GameConfigration.instance.PlayerSound(0);
        Gamemanager.instance.SettingOpened = true;
        if (PopUpTimer.instance != null)
        {
            PopUpTimer.instance.audio.Pause();
        }
        Time.timeScale = 0;
        GamePlaySettings.ShowUI();
    }
    public void MenuScene()
    {
        GameConfigration.instance.PlayerSound(0);
        GameConfigration.instance.RandomSelected = false;
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }
    public void Restart()
    {
        GameConfigration.instance.PlayerSound(0);
        GameConfigration.instance.RandomSelected = false;
        Time.timeScale = 1;
        SceneManager.LoadScene(2);
    }

}
