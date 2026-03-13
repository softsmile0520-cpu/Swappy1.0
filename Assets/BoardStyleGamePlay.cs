using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoardStyleGamePlay : MonoBehaviour
{
    public int BoardIndex = 0;
    public Image BoardIcon;
    public TextMeshProUGUI BoradName;
    public TextMeshProUGUI BoradShowPrice;
    public int BoardPrice;
    public GameObject CheckMark;
    public GameObject Locked;
    public Image BoardCurrency;
    public int valueToConvert;
    public string converted;
    public GameObject Blocked;
    public bool BlockedB = false;
    // Start is called before the first frame update

    private void Start()
    {
        if (BoardIndex == GameConfigration.instance.currentBoardIndex)
        {
            CheckMark.SetActive(true);
        }
        else
        {
            CheckMark.SetActive(false);
        }
        valueToConvert = GameConfigration.instance.boards[BoardIndex].Price;
        if (valueToConvert >= 1000)
        {
            converted = (valueToConvert / 1000f) + "K";
            BoradShowPrice.text = converted;
        }
        else
            BoradShowPrice.text = GameConfigration.instance.boards[BoardIndex].Price.ToString();
        BoardPrice = GameConfigration.instance.boards[BoardIndex].Price;
    }
    public void SelectBoard()
    {
        if (GameConfigration.instance.boards[BoardIndex].Unlocked)
        {
            GameConfigration.instance.PlayerSound(10);
            GameConfigration.instance.updateBoard(BoardIndex);
            CheckMark.SetActive(true);
            for (int i = 0; i < GamePlaySettings.instance.ListOfBoards.Count; i++)
            {
                if (i != BoardIndex)
                {
                    GamePlaySettings.instance.ListOfBoards[i].CheckMark.SetActive(false);
                }
            }
            GamePlayCanvas.instance.ChangeBoard();
        }
    }
}
