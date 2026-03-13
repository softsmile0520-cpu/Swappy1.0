using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flag : MonoBehaviour
{
    public GameObject CheckMark;
    public int CountryIndex;

    private void Start()
    {
        if (CountryIndex == TrophiesHandler.Instance.trophyVariables["CountryIndex"])
        {
            CheckMark.SetActive(true);
        }
        else
        {
            CheckMark.SetActive(false);
        }
    }
    
    public void CountrySelected()
    {
        if (Startgame.Instace.n == 1)
        {
            GameConfigration.instance.CountryIndex = CountryIndex;
            CheckMark.SetActive(true);
            for (int i = 0; i < CountryPanel.instance.flagList.Count; i++)
            {
                if (CountryPanel.instance.flagList[i] != this)
                {
                    CheckMark.SetActive(false);
                }
            }
            SettingPanel.instance.CountryName.text = GameConfigration.instance.countries[CountryIndex].name;
            SettingPanel.instance.CountryImage.sprite = this.GetComponent<Image>().sprite;
            GameConfigration.instance.CountryIndex = CountryIndex;
            CountryPanel.instance.backPressed();
        }
        else if (Startgame.Instace.n == 2) 
        {
            GameConfigration.instance.CountryIndex = CountryIndex;
            CheckMark.SetActive(true);
            for (int i = 0; i < CountryPanel.instance.flagList.Count; i++)
            {
                if (CountryPanel.instance.flagList[i] != this)
                {
                    CheckMark.SetActive(false);
                }
            }
            SignupSystem.instance.CountryName.text = GameConfigration.instance.countries[CountryIndex].name;
            SignupSystem.instance.CountryImage.sprite = this.GetComponent<Image>().sprite;
            GameConfigration.instance.CountryIndex = CountryIndex;
            CountryPanel.instance.backPressed();
        }
    }
   
}
