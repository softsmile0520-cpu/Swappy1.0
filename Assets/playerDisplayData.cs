using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class playerDisplayData : MonoBehaviour
{

    public Transform TimerPos;

    public List<Image> Box;
    public Image Bar;

    public Image Swappy;

    public TextMeshProUGUI AISwappyName;

    public GameObject DeadImage;

    public GameObject ScoreDiffPos;
    public TextMeshProUGUI ScoreDiff;

    public TextMeshProUGUI PlayerName;

    public Image profilePic;

    public Image Country;
    public TextMeshProUGUI Countryname;

    public TextMeshProUGUI Score;

    public List<Sprite> BGDisplayesSquare;
    public List<Sprite> BGDisplayesRectangle;

    public List<SwappyPlayer> PlayerList;
    public int i;
    // Start is called before the first frame update
    void Start()
    {
        PlayerList = Gamemanager.instance._PlayersList;
        Invoke(nameof(DisplayerPlayersData),0.05f);
        PlayerName.color = Color.black;
        Countryname.color = Color.black;
    }

    void DisplayerPlayersData()
    {
        i = transform.GetSiblingIndex();

        for (int j = 0; j < PlayerList.Count; j++)
        {
            if (PlayerList[i] == PlayerList[j])
            {
                if (PlayerList[j].AiSwappy)
                {
                    AISwappyName.text = PlayerList[j].PlayerDifficulty.ToString() + " " + (i + 1);
                    if (PlayerList[j].PlayerDifficulty == Difficulty.Easy)
                    {
                        for (int k = 0; k < Box.Count; k++)
                        {
                            Box[k].sprite = BGDisplayesSquare[0];
                        }
                        Bar.sprite = BGDisplayesRectangle[0];
                    }
                    else if (PlayerList[j].PlayerDifficulty == Difficulty.Medium)
                    {
                        for (int k = 0; k < Box.Count; k++)
                        {
                            Box[k].sprite = BGDisplayesSquare[1];
                        }
                        Bar.sprite = BGDisplayesRectangle[1];
                    }
                    else if (PlayerList[j].PlayerDifficulty == Difficulty.Hard)
                    {
                        for (int k = 0; k < Box.Count; k++)
                        {
                            Box[k].sprite = BGDisplayesSquare[2];
                        }
                        Bar.sprite = BGDisplayesRectangle[2];
                    }
                }
                else
                {
                    AISwappyName.gameObject.SetActive(false);
                    PlayerName.gameObject.SetActive(true);
                    PlayerName.text = TrophiesHandler.Instance.playerName;
                    Country.sprite = GameConfigration.instance.countries[TrophiesHandler.Instance.trophyVariables["CountryIndex"]];
                    Countryname.text = GameConfigration.instance.countries[TrophiesHandler.Instance.trophyVariables["CountryIndex"]].name;
                    profilePic.sprite = GameConfigration.instance.ProfilePic;
                    float a = PlayerPrefs.GetFloat("PicSize", 1);
                    profilePic.transform.localScale = new Vector3(a, a, a);
                    for (int k = 0; k < Box.Count; k++)
                    {
                        Box[k].sprite = BGDisplayesSquare[3];
                    }
                    Bar.sprite = BGDisplayesRectangle[3];
                }
                Swappy.sprite = PlayerList[j].SwapieDisplay.ViewImage;
                Score.text = PlayerList[i].score.ToString();

                GamePlayCanvas.instance.PreviousScore[i] = PlayerList[i].score;

            }
        }
    }
}
