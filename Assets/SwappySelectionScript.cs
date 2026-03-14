using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UI;

public class SwappySelectionScript : MonoBehaviour
{
    public static SwappySelectionScript instance;
    public TextMeshProUGUI SwappyPrice;

    public Transform content;
    public GameObject SwappyStylePrefab;
    public GameObject BuyButton;
    public SwaapyStyles CurrentSwappy;
    public Sprite Coin;
    public Sprite Jem;


    public List<SwaapyStyles> ListOfSwapies = new List<SwaapyStyles>();

    //Purchasing Menu
    public TextMeshProUGUI InBank;
    public TextMeshProUGUI Cost;
    public TextMeshProUGUI WarningText;
    public TextMeshProUGUI WarningTextPrice;

    public Image InBankCurrency;
    public Image CostCurrency;
    public Image SwappyToBuy;
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
    }

    private void Start()
    {
        playerInfo = Instantiate(PlayerInfoTabPrefab, ProfileBar);

        SetInfoStart();
    }
    // Start is called before the first frame update
    public static SwappySelectionScript ShowUI()
    {
        if (instance == null)
        {
            GameObject obj = Instantiate(Resources.Load("SwappySelection")) as GameObject;

            obj.gameObject.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);

            instance = obj.GetComponent<SwappySelectionScript>();
        }

        return instance;
    }
    private void SetInfoStart()
    {
        for (int i = 0; i < GameConfigration.instance.Swappies.Count; i++)
        {
            GameObject obj = Instantiate(SwappyStylePrefab, content);

            SwaapyStyles CurrentSwappy = obj.GetComponent<SwaapyStyles>();
            ListOfSwapies.Add(CurrentSwappy);
            CurrentSwappy.SwappyIndex = i;
            CurrentSwappy.SwappyName.text = GameConfigration.instance.Swappies[i].SwapydName;
            CurrentSwappy.SwappyIcon.sprite = GameConfigration.instance.Swappies[i].ViewImage;
            CurrentSwappy.SwappyPrice = GameConfigration.instance.Swappies[i].Price;


            if (GameConfigration.instance.Swappies[i].currency == "Coin")
            {
                CurrentSwappy.SwappyCurrency.sprite = Coin;
                if (TrophiesHandler.Instance.trophyVariables["Coins"] < CurrentSwappy.SwappyPrice)
                {
                    CurrentSwappy.Blocked.SetActive(true);
                    CurrentSwappy.BlockedB = true;
                }
            }
            else if (GameConfigration.instance.Swappies[i].currency == "Jem")
            {
                CurrentSwappy.SwappyCurrency.sprite = Jem;
                if (TrophiesHandler.Instance.trophyVariables["Jems"] < CurrentSwappy.SwappyPrice)
                {
                    CurrentSwappy.Blocked.SetActive(true);
                    CurrentSwappy.BlockedB = true;
                }
            }
            if (GameConfigration.instance.Swappies[i].Unlocked)
            {
                CurrentSwappy.Locked.SetActive(false);
                CurrentSwappy.Blocked.SetActive(false);
                CurrentSwappy.BlockedB = false;
            }
            else
            {
                CurrentSwappy.Locked.SetActive(true);
            }

            Check(CurrentSwappy,i);

        }
    }

    void Check(SwaapyStyles tempswappy,int i)
    {
        if (GameConfigration.instance.Swappies[i].currency == "Coin")
        {
            tempswappy.SwappyCurrency.sprite = Coin;
            if (TrophiesHandler.Instance.trophyVariables["Coins"] < tempswappy.SwappyPrice)
            {
                tempswappy.Blocked.SetActive(true);
                tempswappy.BlockedB = true;
            }
        }
        else if (GameConfigration.instance.Swappies[i].currency == "Jem")
        {
            tempswappy.SwappyCurrency.sprite = Jem;
            if (TrophiesHandler.Instance.trophyVariables["Jems"] < tempswappy.SwappyPrice)
            {
                tempswappy.Blocked.SetActive(true);
                tempswappy.BlockedB = true;
            }
        }
        if (GameConfigration.instance.Swappies[i].Unlocked)
        {
            tempswappy.Locked.SetActive(false);
            tempswappy.Blocked.SetActive(false);
            tempswappy.BlockedB = false;
        }
        else
        {
            tempswappy.Locked.SetActive(true);
        }
    }

    //public void BuySwappy()
    //{
    //    print(CurrentSwappy.SwappyIndex);
    //    if (GameConfigration.instance.Swappies[CurrentSwappy.SwappyIndex].currency == "Coin")
    //    {
    //        if (TrophiesHandler.Instance.trophyVariables["Coins"] >= CurrentSwappy.SwappyPrice)
    //        {
    //            BuyButton.GetComponent<Image>().color = Color.white;
    //            GameConfigration.instance.updateCoins(-CurrentSwappy.SwappyPrice);

    //            GameConfigration.instance.PlayerSound(1);
    //            GameConfigration.instance.UnlockSwapies(CurrentSwappy.SwappyIndex);
    //            CurrentSwappy.Locked.SetActive(false);
    //            BuyButton.SetActive(false);
    //        }
    //        else
    //        {
    //            BuyButton.GetComponent<Image>().color = Color.red;
    //            GameConfigration.instance.PlayerSound(27);
    //        }
    //    }
    //    else if (GameConfigration.instance.Swappies[CurrentSwappy.SwappyIndex].currency == "Jem")
    //    {
    //        if (TrophiesHandler.Instance.trophyVariables["Jems"] >= CurrentSwappy.SwappyPrice)
    //        {
    //            BuyButton.GetComponent<Image>().color = Color.white;
    //            GameConfigration.instance.updateJem(-CurrentSwappy.SwappyPrice);
    //            GameConfigration.instance.PlayerSound(1);
    //            GameConfigration.instance.UnlockSwapies(CurrentSwappy.SwappyIndex);
    //            CurrentSwappy.Locked.SetActive(false);
    //            BuyButton.SetActive(false);
    //        }
    //        else
    //        {
    //            BuyButton.GetComponent<Image>().color = Color.red;
    //            GameConfigration.instance.PlayerSound(27);
    //        }
    //    }
    //}

    public void BuyingMenu()
    {
        MainObj.SetActive(true);
        if (GameConfigration.instance.Swappies[CurrentSwappy.SwappyIndex].currency == "Coin")
        {
            InBank.text = TrophiesHandler.Instance.trophyVariables["Coins"] + "";
            InBankCurrency.sprite = Coin;
            Cost.text = GameConfigration.instance.Swappies[CurrentSwappy.SwappyIndex].Price.ToString();
            CostCurrency.sprite = Coin;
            WarningTextPrice.text = GameConfigration.instance.Swappies[CurrentSwappy.SwappyIndex].Price.ToString();
            WarningTextPriceCurrency.sprite = Coin;
        }
        else
        {
            InBank.text = TrophiesHandler.Instance.trophyVariables["Jems"] + "";
            InBankCurrency.sprite = Jem;
            Cost.text = GameConfigration.instance.Swappies[CurrentSwappy.SwappyIndex].Price.ToString();
            CostCurrency.sprite = Jem;
            WarningTextPrice.text = GameConfigration.instance.Swappies[CurrentSwappy.SwappyIndex].Price.ToString();
            WarningTextPriceCurrency.sprite = Jem;
        }

        SwappyToBuy.sprite = GameConfigration.instance.Swappies[CurrentSwappy.SwappyIndex].ViewImage;
        WarningText.text = "would you like to purchase" + " " + GameConfigration.instance.Swappies[CurrentSwappy.SwappyIndex].SwapydName + " For";
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
        for (int i = 0; i < ListOfSwapies.Count; i++)
        {
            if (ListOfSwapies[i] != null)
                Check(ListOfSwapies[i], i);
        }
        //ListOfSwapies.Clear();
        //SetInfoStart();
    }

    public void BuyingSwappy()
    {
        if (GameConfigration.instance.Swappies[CurrentSwappy.SwappyIndex].currency == "Coin")
        {
            GameConfigration.instance.updateCoins(-CurrentSwappy.SwappyPrice);
            playerInfo.AssignPlayerData();
            ModeSelectionPanelScript.instance.playerInfo.StorePreviousValues();
            SpentAmount.gameObject.SetActive(true);
            SpentAmount.text = "- " + CurrentSwappy.SwappyPrice;
            InBank.text = TrophiesHandler.Instance.trophyVariables["Coins"] + "";

        }
        else
        {
            GameConfigration.instance.updateJem(-CurrentSwappy.SwappyPrice);
            playerInfo.AssignPlayerData();
            ModeSelectionPanelScript.instance.playerInfo.StorePreviousValues();
            SpentAmount.gameObject.SetActive(true);
            SpentAmount.text = "- " + CurrentSwappy.SwappyPrice;
            InBank.text = TrophiesHandler.Instance.trophyVariables["Jems"] + "";
        }
        GameConfigration.instance.PlayerSound(1);
        GameConfigration.instance.UnlockSwapies(CurrentSwappy.SwappyIndex);
        CurrentSwappy.Locked.SetActive(false);

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
        ModeSelectionPanelScript.instance.LoadSwappyImage();

        backPressed();
    }
    public void backPressed()
    {
        Destroy(this.gameObject);
    }
}
