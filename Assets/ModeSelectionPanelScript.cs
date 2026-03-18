using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ModeSelectionPanelScript : MonoBehaviour
{
    public static ModeSelectionPanelScript instance;
    public Sprite selectedImage;

    public Image Swappy;
    public Image board;

    public PlayerProfileInfoTab playerInfo;

    public PlayerProfileInfoTab PlayerInfoTabPrefab;

    public Transform ProfileBar;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    public static ModeSelectionPanelScript ShowUI()
    {
        if (instance == null)
        {
            GameObject obj = Instantiate(Resources.Load("ModeSelection")) as GameObject;

            obj.gameObject.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);

            instance = obj.GetComponent<ModeSelectionPanelScript>();
        }

        return instance;
    }
    private void Start()
    {
        Time.timeScale = 1;
        GameConfigration.instance.BGSoundPlayer(1);
        playerInfo = Instantiate(PlayerInfoTabPrefab, ProfileBar);

        LoadBoardImage();
        LoadSwappyImage();
    }

    public void LoadSwappyImage()
    {
        Swappy.sprite = GameConfigration.instance.Swappies[PlayerPrefs.GetInt("SwappyStyle")].ViewImage;
    }

    public void LoadBoardImage()
    {
        board.sprite = GameConfigration.instance.boards[PlayerPrefs.GetInt("BoardStyle")].ViewImage;
    }

    public void SoundPanel()
    {
        GameConfigration.instance.PlayerSound(0);
        SoundsPanel.ShowUI();
    }
    public void AboutSwappies()
    {
        AboutSwappiesPanel.ShowUI();
        //backPressed();
    }
    /// <summary>Menu Settings / OPTIONS — opens sound & volume panel.</summary>
    public void SettingPanel()
    {
        GameConfigration.instance.PlayerSound(0);
        SoundsPanel.ShowUI();
    }
    public void TimePanel()
    {
        GameConfigration.instance.AIInputMode = true;
        TimePanelScript.ShowUI();
        GameConfigration.instance.PlayerSound(0);
    }

    public void boardsPanel()
    {
        BoardSelectionScript.ShowUI();
        GameConfigration.instance.PlayerSound(0);
        //backPressed();
    }
    public void SwappyPanel()
    {
        SwappySelectionScript.ShowUI();
        //SwappySelectionScript.instance.mode = this;
        GameConfigration.instance.PlayerSound(0);
        //backPressed();
    }
    public void OnlineVsHuman()
    {
        TimePanelScript.ShowUI();
        backPressed();
    }
    public void Quit()
    {
        string jsonString = JsonConvert.SerializeObject(TrophiesHandler.Instance.trophyVariables);
        PlayerPrefs.SetString("trophyVariables", jsonString);
        PlayerPrefs.SetString("playerName", TrophiesHandler.Instance.playerName);
        PlayerPrefs.SetFloat("PicSize", TrophiesHandler.Instance.PicSize);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    public void backPressed()
    {
        GameConfigration.instance.PlayerSound(0);
        Destroy(this.gameObject);
    }
    public void TutorialsButton()
    {
        SceneManager.LoadScene("Tutorial1");
    }
}
