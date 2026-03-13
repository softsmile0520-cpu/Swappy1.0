using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Reflection;

public class AiDifficultyScript : MonoBehaviour
{
    public static AiDifficultyScript instance;
    public Image modename;
    
    public List<Sprite> ModeSprites;

    public Sprite Selected;
    public Sprite NotSelected;
    public List<GameObject> CPUSDifficulties;
    public List<Image> firstCPU;
    public List<Image> secondCPU;
    public List<Image> thirdCPU;

    public Image CPU1;
    public Image CPU2;
    public Image CPU3;

    public List<Sprite> CPUSpritesClassic;
    public List<Sprite> CPUSpritesFast;
    public List<Sprite> CPUSpritesPower;

    public List<bool> diffSelected = new List<bool>(3);

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    private void Start()
    {
        diffSelected = diffSelected.Select(b => false).ToList();
        for (int b = 0; b < GameConfigration.instance._SwappyPlayer.Count - 1; b++)
        {
            CPUSDifficulties[b].SetActive(true);
        }
        if (GameConfigration.instance.RandomSelected)
        {
            for (int i = 0; i < GameConfigration.instance._SwappyPlayer.Count - 1; i++)
            {
                int b = Random.Range(0, 3);
                if (i == 0)
                {
                    firstPlayerDifficulty(b);
                }
                else if (i == 1)
                {
                    secondPlayerDifficulty(b);
                }
                else if (i == 2)
                {
                    thirdPlayerDifficulty(b);
                }
            }
        }

        modename.sprite = ModeSprites[(int)GameConfigration.instance.GameMode];
        switch (GameConfigration.instance.GameMode)
        {
            case mode.Classic:
                CPU1.sprite = CPUSpritesClassic[0];
                CPU2.sprite = CPUSpritesClassic[1];
                CPU3.sprite = CPUSpritesClassic[2];
                return;
            case mode.Fast:
                CPU1.sprite = CPUSpritesFast[0];
                CPU2.sprite = CPUSpritesFast[1];
                CPU3.sprite = CPUSpritesFast[2];
                return;
            case mode.Power:
                CPU1.sprite = CPUSpritesPower[0];
                CPU2.sprite = CPUSpritesPower[1];
                CPU3.sprite = CPUSpritesPower[2];
                return;
        }
    }        


    
    public static AiDifficultyScript ShowUI()
    {
        if (instance == null)
        {
            GameObject obj = Instantiate(Resources.Load("Ai Difficulty")) as GameObject;

            obj.gameObject.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);

            instance = obj.GetComponent<AiDifficultyScript>();
        }

        return instance;
    }
    public void firstPlayerDifficulty(int diff)
    {
        GameConfigration.instance.PlayerSound(0);
        //GameConfigration.instance.CPUs[0] = diff;
        for (int i = 0; i < firstCPU.Count; i++)
        {
            if (i == diff)
            {
                firstCPU[i].sprite = Selected;
            }
            else
            {
                firstCPU[i].sprite = NotSelected;
            }
        }
        diffSelected[0] = true;
        GameConfigration.instance._SwappyPlayer[1].PlayerDifficulty = (Difficulty)diff;
    }


    public void secondPlayerDifficulty(int diff)
    {
        GameConfigration.instance.PlayerSound(0);

        for (int i = 0; i < firstCPU.Count; i++)
        {
            if (i == diff)
            {
                secondCPU[i].sprite = Selected;
            }
            else
            {
                secondCPU[i].sprite = NotSelected;
            }
        }
        diffSelected[1] = true;
        GameConfigration.instance._SwappyPlayer[2].PlayerDifficulty = (Difficulty)diff;

    }
    public void thirdPlayerDifficulty(int diff)
    {
        GameConfigration.instance.PlayerSound(0);

        for (int i = 0; i < firstCPU.Count; i++)
        {
            if (i == diff)
            {
                thirdCPU[i].sprite = Selected;
            }
            else
            {
                thirdCPU[i].sprite = NotSelected;
            }
        }
        diffSelected[2] = true;
        GameConfigration.instance._SwappyPlayer[3].PlayerDifficulty = (Difficulty)diff;

    }
    public void NextScene()
    {
        for (int i = 0; i < GameConfigration.instance._SwappyPlayer.Count - 1; i++)
        {
            if (diffSelected[i] != true)
            {
                GameConfigration.instance.PlayerSound(27);
                return;
            }
        }
        GameConfigration.instance.PlayerSound(0);
        GameConfigration.instance.gameStarted = true;
        SceneManager.LoadScene(2);
    }
    public void goBack()
    {
        GameConfigration.instance.PlayerSound(0);
        PlayerSelectionScript.ShowUI();
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
