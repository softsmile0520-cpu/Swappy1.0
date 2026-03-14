using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UI;

public class BoardSelectionScript : MonoBehaviour
{
    public static BoardSelectionScript instance;

    public Transform content;

    public GameObject BoardStylePrefab;

    public GameObject BuyButton;

    public TextMeshProUGUI BoardPrice;

    public Boards CurrentBoard;

    public List<Boards> ListOfBoards=new List<Boards>();

    public Sprite Coin;
    public Sprite Jem;


    //Purchasing Menu
    public TextMeshProUGUI InBank;
    public TextMeshProUGUI Cost;
    public TextMeshProUGUI WarningText;
    public TextMeshProUGUI WarningTextPrice;

    public Image InBankCurrency;
    public Image CostCurrency;
    public Image BoardToBuy;
    public Image WarningTextPriceCurrency;
    public Image BoughtThumb;

    //ProfileBar
    [NonSerialized]
    public PlayerProfileInfoTab playerInfo;

    public PlayerProfileInfoTab PlayerInfoTabPrefab;

    public Transform ProfileBar;

    public GameObject MainObj;
    public GameObject OkButton;
    public GameObject YesButton;
    public GameObject NoButton;

    public TextMeshProUGUI SpentAmount;
    //End
    private void Awake()
    {
        instance = this;
        playerInfo = Instantiate(PlayerInfoTabPrefab, ProfileBar);

        SetInfoStart();
    }
    // Start is called before the first frame update
    public static BoardSelectionScript ShowUI()
    {
        if (instance == null)
        {
            GameObject obj = Instantiate(Resources.Load("BoardSelection")) as GameObject;

            obj.gameObject.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);

            instance = obj.GetComponent<BoardSelectionScript>();
        }

        return instance;
    }
    private void SetInfoStart()
    {
        for (int i = 0; i < GameConfigration.instance.boards.Count; i++)
        {
            GameObject obj = Instantiate(BoardStylePrefab, content);

            Boards CurrentBoard = obj.GetComponent<Boards>();
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

    //public void BuyBoard()
    //{
    //    if (GameConfigration.instance.boards[CurrentBoard.BoardIndex].currency == "Coin")
    //    {
    //        if (TrophiesHandler.Instance.trophyVariables["Coins"] >= CurrentBoard.BoardPrice)
    //        {
    //            BuyButton.GetComponent<Image>().color = Color.white;

    //            GameConfigration.instance.updateCoins(-CurrentBoard.BoardPrice);
    //            playerInfo.AssignPlayerData();
    //            ModeSelectionPanelScript.instance.playerInfo.StorePreviousValues();
    //            GameConfigration.instance.PlayerSound(1);
    //            GameConfigration.instance.UnlockBoard(CurrentBoard.BoardIndex);
    //            CurrentBoard.Locked.SetActive(false);
    //            BuyButton.SetActive(false);
    //        }
    //        else
    //        {
    //            BuyButton.GetComponent<Image>().color = Color.red;
    //            GameConfigration.instance.PlayerSound(27);
    //        }
    //    }
    //    else if (GameConfigration.instance.boards[CurrentBoard.BoardIndex].currency == "Jem")
    //    {
    //        if (TrophiesHandler.Instance.trophyVariables["Jems"] >= CurrentBoard.BoardPrice)
    //        {
    //            BuyButton.GetComponent<Image>().color = Color.white;
    //            GameConfigration.instance.updateJem(-CurrentBoard.BoardPrice);
    //            GameConfigration.instance.PlayerSound(1);
    //            GameConfigration.instance.UnlockBoard(CurrentBoard.BoardIndex);
    //            CurrentBoard.Locked.SetActive(false);
    //            BuyButton.SetActive(false);
    //        }
    //        else
    //        {
    //            BuyButton.GetComponent<Image>().color = Color.red;
    //            GameConfigration.instance.PlayerSound(27);
    //        }
    //    }

    //    //GameConfigration.instance.PlayerSound(1);
    //    //if (GameConfigration.instance.Coins >= CurrentBoard.BoardPrice) 
    //    //{
    //    //    GameConfigration.instance.updateCoins(-CurrentBoard.BoardPrice);
    //    //    CurrentBoard.Locked.SetActive(false);
    //    //    BuyButton.SetActive(false);
    //    //    GameConfigration.instance.UnlockBoard(CurrentBoard.BoardIndex);
    //    //}

    //}
    public void BuyingMenu()
    {
        MainObj.SetActive(true);
        if (GameConfigration.instance.boards[CurrentBoard.BoardIndex].currency == "Coin")
        {
            InBank.text = TrophiesHandler.Instance.trophyVariables["Coins"] + "";
            InBankCurrency.sprite = Coin;
            Cost.text = GameConfigration.instance.boards[CurrentBoard.BoardIndex].Price.ToString();
            CostCurrency.sprite = Coin;
            WarningTextPrice.text = GameConfigration.instance.boards[CurrentBoard.BoardIndex].Price.ToString();
            WarningTextPriceCurrency.sprite = Coin;
        }
        else
        {
            InBank.text = TrophiesHandler.Instance.trophyVariables["Jems"] + "";
            InBankCurrency.sprite = Jem;
            Cost.text = GameConfigration.instance.boards[CurrentBoard.BoardIndex].Price.ToString();
            CostCurrency.sprite = Jem;
            WarningTextPrice.text = GameConfigration.instance.boards[CurrentBoard.BoardIndex].Price.ToString();
            WarningTextPriceCurrency.sprite = Jem;
        }

        BoardToBuy.sprite = GameConfigration.instance.boards[CurrentBoard.BoardIndex].ViewImage;
        WarningText.text = "would you like to purchase" + " " + GameConfigration.instance.boards[CurrentBoard.BoardIndex].BoardName+ " For";
    }

    public void closingBuyingMenu()
    {
        OkButton.SetActive(false);
        BoughtThumb.gameObject.SetActive(false);
        Cost.transform.parent.gameObject.SetActive(true);
        WarningTextPrice.transform.parent.gameObject.SetActive(true);
        YesButton.SetActive(true);
        NoButton.SetActive(true);
        MainObj.SetActive(false);
        BuyButton.SetActive(false);
        SpentAmount.gameObject.SetActive(false);
        Reseting();
    }
    void Reseting()
    {
        for (int i = 0; i < ListOfBoards.Count; i++)
        {
            if (ListOfBoards[i] != null)
                Check(ListOfBoards[i], i);
        }
        //ListOfSwapies.Clear();
        //SetInfoStart();
    }
    void Check(Boards tempBoard, int i)
    {
        if (GameConfigration.instance.boards[i].currency == "Coin")
        {
            tempBoard.BoardCurrency.sprite = Coin;
            if (TrophiesHandler.Instance.trophyVariables["Coins"] < tempBoard.BoardPrice)
            {
                tempBoard.Blocked.SetActive(true);
                tempBoard.BlockedB = true;
            }
        }
        else if (GameConfigration.instance.boards[i].currency == "Jem")
        {
            tempBoard.BoardCurrency.sprite = Jem;
            if (TrophiesHandler.Instance.trophyVariables["Jems"] < tempBoard.BoardPrice)
            {
                tempBoard.Blocked.SetActive(true);
                tempBoard.BlockedB = true;
            }
        }
        if (GameConfigration.instance.boards[i].Unlocked)
        {
            tempBoard.Locked.SetActive(false);
            tempBoard.Blocked.SetActive(false);
            tempBoard.BlockedB = false;
        }
        else
        {
            tempBoard.Locked.SetActive(true);
        }
    }
    public void BuyingSwappy()
    {
        if (GameConfigration.instance.boards[CurrentBoard.BoardIndex].currency == "Coin")
        {
            GameConfigration.instance.updateCoins(-CurrentBoard.BoardPrice);
            playerInfo.AssignPlayerData();
            ModeSelectionPanelScript.instance.playerInfo.StorePreviousValues();
            SpentAmount.gameObject.SetActive(true);
            SpentAmount.text = "- " + CurrentBoard.BoardPrice;
            InBank.text = TrophiesHandler.Instance.trophyVariables["Coins"] + "";

        }
        else
        {
            GameConfigration.instance.updateJem(-CurrentBoard.BoardPrice);
            playerInfo.AssignPlayerData();
            ModeSelectionPanelScript.instance.playerInfo.StorePreviousValues();
            SpentAmount.gameObject.SetActive(true);

            SpentAmount.text = "- " + CurrentBoard.BoardPrice;
            InBank.text = TrophiesHandler.Instance.trophyVariables["Jems"] + "";
        }
        GameConfigration.instance.PlayerSound(1);
        GameConfigration.instance.UnlockBoard(CurrentBoard.BoardIndex);
        CurrentBoard.Locked.SetActive(false);

        OkButton.SetActive(true);
        BoughtThumb.gameObject.SetActive(true);
        Cost.transform.parent.gameObject.SetActive(false);
        WarningTextPrice.transform.parent.gameObject.SetActive(false);
        YesButton.SetActive(false);
        NoButton.SetActive(false);
    }


    public void goBack()
    {
        GameConfigration.instance.PlayerSound(0);
        ModeSelectionPanelScript.instance.LoadBoardImage();
        backPressed();
    }
    public void backPressed()
    {
        Destroy(this.gameObject);
    }
}
