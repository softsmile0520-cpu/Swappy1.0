using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerSelectionScript : MonoBehaviour
{
    public static PlayerSelectionScript instance;
    public Image modename;

    public List<Sprite> ModeSprites;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    public static PlayerSelectionScript ShowUI()
    {
        if (instance == null)
        {
            GameObject obj = Instantiate(Resources.Load("PlayerSelection")) as GameObject;

            obj.gameObject.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);

            instance = obj.GetComponent<PlayerSelectionScript>();
        }

        return instance;
    }
    private void Start()
    {
        modename.sprite = ModeSprites[(int)GameConfigration.instance.GameMode];
    }

    void AddPlayer()
    {
        SwappyPlayer MyPlayer = new SwappyPlayer();
        MyPlayer.PlayerName = "Raheel";
        MyPlayer.CountryName = "Pakistan";
        MyPlayer.AiSwappy = false;
        MyPlayer.SwapieDisplay = GameConfigration.instance.Swappies[GameConfigration.instance.currentSwappyIndex];

        GameConfigration.instance._SwappyPlayer.Add(MyPlayer);
    }

    public void SelectPlayerNo(int i)
    {
        GameConfigration.instance.PlayerSound(0);
        GameConfigration.instance._SwappyPlayer.Clear();
        AddPlayer();

        for (int j = 0; j < i; j++)
        {
            GameConfigration.instance._SwappyPlayer.Add(new SwappyPlayer());
        }

        AiDifficultyScript.ShowUI();
        backPressed();
    }
    public void RandomSelected()
    {
        int i = Random.Range(0,3);
        GameConfigration.instance._SwappyPlayer.Clear();
        AddPlayer();

        for (int j = 0; j <= i; j++)
        {
            GameConfigration.instance._SwappyPlayer.Add(new SwappyPlayer());
        }
        GameConfigration.instance.RandomSelected = true;

        for (int c = 0; c < GameConfigration.instance._SwappyPlayer.Count - 1; c++)
        {
            int b = Random.Range(0, 3);
            if (c == 0)
            {
                GameConfigration.instance._SwappyPlayer[1].PlayerDifficulty = (Difficulty)b;
            }
            else if (c == 1)
            {
                GameConfigration.instance._SwappyPlayer[2].PlayerDifficulty = (Difficulty)b;
            }
            else if (c == 2)
            {
                GameConfigration.instance._SwappyPlayer[3].PlayerDifficulty = (Difficulty)b;
            }
        }

        GameConfigration.instance.PlayerSound(0);
        GameConfigration.instance.gameStarted = true;
        SceneManager.LoadScene(2);
    }
  
    public void goBack()
    {
        GameConfigration.instance.PlayerSound(0);
        TimePanelScript.ShowUI();
        GameConfigration.instance.RandomSelected = false;
        backPressed();
    }
    public void BackToMenu()
    {
        GameConfigration.instance.PlayerSound(0);
        ModeSelectionPanelScript.ShowUI();
        GameConfigration.instance.RandomSelected = false;
        backPressed();
    }
    public void backPressed()
    {
        Destroy(this.gameObject);
    }
}
