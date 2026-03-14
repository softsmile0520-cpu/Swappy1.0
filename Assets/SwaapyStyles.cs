using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SwaapyStyles : MonoBehaviour
{
    public int SwappyIndex = 0;
    public int valueToConvert;
    public string converted;
    public int SwappyPrice;
    public bool BlockedB = false;

    public Image SwappyIcon;
    public TextMeshProUGUI SwappyName;
    public TextMeshProUGUI SwappyShowPrice;
    public Image SwappyCurrency;

    public GameObject CheckMark;
    public GameObject Locked;
    public GameObject Blocked;

    private void Start()
    {
        if (SwappyIndex == GameConfigration.instance.currentSwappyIndex)
        {
            CheckMark.SetActive(true);
        }
        else
        {
            CheckMark.SetActive(false);
        }
        valueToConvert = GameConfigration.instance.Swappies[SwappyIndex].Price;
        if (valueToConvert >= 1000)
        {
            converted = (valueToConvert / 1000f) + "K";
            SwappyShowPrice.text = converted;
        }
        else
            SwappyShowPrice.text = GameConfigration.instance.Swappies[SwappyIndex].Price.ToString();

        if (GameConfigration.instance.Swappies[SwappyIndex].Price == 0)
        {
            SwappyShowPrice.text = "Free";
            SwappyCurrency.gameObject.SetActive(false); 
        }
    }
    public void SelectSwappy()
    {
        if (SwappyIndex == GameConfigration.instance.currentSwappyIndex)
        {
            CheckMark.SetActive(true);
        }
        else
        {
            CheckMark.SetActive(false);
        }
        SwappyPrice = GameConfigration.instance.Swappies[SwappyIndex].Price;
    }

    public void SwappiesOptions()
    {
        SwappySelectionScript.instance.BuyButton.SetActive(false);
        if (GameConfigration.instance.Swappies[SwappyIndex].Unlocked)
        {

            SelectSwapie();
        }
        else 
        {
            if (BlockedB == false)
            {
                SwappySelectionScript.instance.BuyButton.SetActive(true);
                SwappySelectionScript.instance.CurrentSwappy = this;
            }
        }

        {
            //ShowPrice();
        }
    }

    void SelectSwapie()
    {
        GameConfigration.instance.PlayerSound(10);
        GameConfigration.instance.updateSwappies(SwappyIndex);
        CheckMark.SetActive(true);
        for (int i = 0; i < SwappySelectionScript.instance.ListOfSwapies.Count; i++)
        {
            if (i != SwappyIndex)
            {
                SwappySelectionScript.instance.ListOfSwapies[i].CheckMark.SetActive(false);
            }
        }
    }
    

    void ShowPrice()
    {
        //if (GameConfigration.instance.Swappies[SwappyIndex].currency == "Coin")
        //{
        //    if (GameConfigration.instance.Coins >= SwappyPrice)
        //    {
        //        SwappySelectionScript.instance.BuyButton.SetActive(true);
        //        SwappySelectionScript.instance.BuyButton.GetComponent<Image>().color = Color.white;
        //        SwappySelectionScript.instance.CurrentSwappy = this;
        //    }
        //    else
        //    {
        //        SwappySelectionScript.instance.BuyButton.SetActive(true);
        //        SwappySelectionScript.instance.BuyButton.GetComponent<Image>().color = Color.red;
        //        SwappySelectionScript.instance.CurrentSwappy = this;
        //    }
        //}
        //else if (GameConfigration.instance.Swappies[SwappyIndex].currency == "Jem")
        //{
        //    if (GameConfigration.instance.Jem >= SwappyPrice)
        //    {
        //        SwappySelectionScript.instance.BuyButton.SetActive(true);
        //        SwappySelectionScript.instance.BuyButton.GetComponent<Image>().color = Color.white;
        //        SwappySelectionScript.instance.CurrentSwappy = this;
        //    }
        //    else
        //    {
        //        SwappySelectionScript.instance.BuyButton.SetActive(true);
        //        SwappySelectionScript.instance.BuyButton.GetComponent<Image>().color = Color.red;
        //        SwappySelectionScript.instance.CurrentSwappy = this;

        //    }
        //}
    }
}