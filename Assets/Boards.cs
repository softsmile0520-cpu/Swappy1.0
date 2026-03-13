using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Boards : MonoBehaviour
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

    private void Start()
    {
        if(BoardIndex==GameConfigration.instance.currentBoardIndex)
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

        if (GameConfigration.instance.boards[BoardIndex].Price == 0)
        {
            BoradShowPrice.text = "Free";
            BoardCurrency.gameObject.SetActive(false);
        }
    }

    public void BoardOptions()
    {
        BoardSelectionScript.instance.BuyButton.SetActive(false);
        if (GameConfigration.instance.boards[BoardIndex].Unlocked)
        {

            SelectBoard();
        }
        else
        {
            if (BlockedB == false)
            {
                BoardSelectionScript.instance.BuyButton.SetActive(true);
                BoardSelectionScript.instance.CurrentBoard = this;
            }
        }
        //{
        //    ShowPrice();
        //}
    }

    void SelectBoard()
    {
        GameConfigration.instance.PlayerSound(10);
        GameConfigration.instance.updateBoard(BoardIndex);
        CheckMark.SetActive(true);
        for (int i = 0; i < BoardSelectionScript.instance.ListOfBoards.Count; i++)
        {
            if (i != BoardIndex)
            {
                BoardSelectionScript.instance.ListOfBoards[i].CheckMark.SetActive(false);
            }
        }
    }

    void ShowPrice()
    {
        //if (GameConfigration.instance.boards[BoardIndex].currency == "Coin")
        //{
        //    if (GameConfigration.instance.Coins >= BoardPrice)
        //    {
        //        BoardSelectionScript.instance.BuyButton.SetActive(true);
        //        BoardSelectionScript.instance.BuyButton.GetComponent<Image>().color = Color.white;
        //        BoardSelectionScript.instance.CurrentBoard = this;
        //    }
        //    else
        //    {
        //        BoardSelectionScript.instance.BuyButton.SetActive(true);
        //        BoardSelectionScript.instance.BuyButton.GetComponent<Image>().color = Color.red;
        //        BoardSelectionScript.instance.CurrentBoard = this;
        //    }
        //}
        //else if (GameConfigration.instance.boards[BoardIndex].currency == "Jem")
        //{
        //    if (GameConfigration.instance.Jem >= BoardPrice)
        //    {
        //        BoardSelectionScript.instance.BuyButton.SetActive(true);
        //        BoardSelectionScript.instance.BuyButton.GetComponent<Image>().color = Color.white;
        //        BoardSelectionScript.instance.CurrentBoard = this;
        //    }
        //    else
        //    {
        //        BoardSelectionScript.instance.BuyButton.SetActive(true);
        //        BoardSelectionScript.instance.BuyButton.GetComponent<Image>().color = Color.red;
        //        BoardSelectionScript.instance.CurrentBoard = this;

        //    }
        //}
    }
}
