using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePlaySettings : MonoBehaviour
{
    public static GamePlaySettings instance;

    public Transform content;

    public GameObject BoardStylePrefab;

    public List<BoardStyleGamePlay> ListOfBoards = new List<BoardStyleGamePlay>();

    public Sprite Coin;
    public Sprite Jem;

    public Slider BGVolumeSlider;
    public Slider FXVolumeSlider;
    public static GamePlaySettings ShowUI()
    {
        if (instance == null)
        {
            GameObject obj = Instantiate(Resources.Load("GamePlaySettings")) as GameObject;

            obj.gameObject.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);

            instance = obj.GetComponent<GamePlaySettings>();
        }

        return instance;
    }

    private void Start()
    {
        SetInfoStart();
        BGVolumeSlider.value = PlayerPrefs.GetFloat("BGVolumeValue", 1);
        FXVolumeSlider.value = PlayerPrefs.GetFloat("FXVolumeValue", 1);
    }
    private void SetInfoStart()
    {
        for (int i = 0; i < GameConfigration.instance.boards.Count; i++)
        {
            GameObject obj = Instantiate(BoardStylePrefab, content);

            BoardStyleGamePlay CurrentBoard = obj.GetComponent<BoardStyleGamePlay>();
            ListOfBoards.Add(CurrentBoard);
            CurrentBoard.BoardIndex = i;
            CurrentBoard.BoradName.text = GameConfigration.instance.boards[i].BoardName;
            CurrentBoard.BoardIcon.sprite = GameConfigration.instance.boards[i].ViewImage;
            CurrentBoard.BoardPrice = GameConfigration.instance.boards[i].Price;
            if (GameConfigration.instance.boards[i].currency == "Coin")
            {
                CurrentBoard.BoardCurrency.sprite = Coin;
                if (TrophiesHandler.Instance.trophyVariables["Coins"] < CurrentBoard.BoardPrice)
                {
                    CurrentBoard.Blocked.SetActive(true);
                    CurrentBoard.BlockedB = true;
                }
            }
            else if (GameConfigration.instance.boards[i].currency == "Jem")
            {
                CurrentBoard.BoardCurrency.sprite = Jem;
                if (TrophiesHandler.Instance.trophyVariables["Jems"] < CurrentBoard.BoardPrice)
                {
                    CurrentBoard.Blocked.SetActive(true);
                    CurrentBoard.BlockedB = true;
                }
            }

            if (GameConfigration.instance.boards[i].Unlocked)
            {
                CurrentBoard.Locked.SetActive(false);
            }
            else
            {
                CurrentBoard.Locked.SetActive(true);
            }
        }
    }
    public void ControlBGVolumeButton()
    {
        float a = BGVolumeSlider.value;
        GameConfigration.instance.VolumeControll(a);
        PlayerPrefs.SetFloat("BGVolumeValue", a);
    }

    public void ControlFXVolumeButton()
    {
        GameConfigration.instance.PlayerSound(0);
        float a = FXVolumeSlider.value;
        GameConfigration.instance.FXVolumeControll(a);
        PlayerPrefs.SetFloat("FXVolumeValue", a);
    }
    public void backPressed()
    {
        Time.timeScale = 1;
        Gamemanager.instance.SettingOpened = false;
        Destroy(this.gameObject);
    }
    public void BackTOMenu()
    {
        GameConfigration.instance.PlayerSound(0);
        Gamemanager.instance.SettingOpened = false;
        SceneManager.LoadScene("MenuScene");
    }
}
