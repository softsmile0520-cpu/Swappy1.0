using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Startgame : MonoBehaviour
{
    public GameObject OnlinePanel, AIDiffivulty;

    [Tooltip("Profile bar (avatar, XP, currency, settings) – hidden until after login. Assign in Inspector.")]
    public GameObject profileBar;

    public static Startgame Instace;
    public int n;
    private void Start()
    {
        Instace = this;
        print("11111");
        if (profileBar != null)
            profileBar.SetActive(false);
        Invoke(nameof(SetPlayerData), 0.5f);
        Invoke(nameof(LoginPanelOn), 1f);
    }

    public void LoginPanelOn()
    {
        LoginSystem.ShowUI();
    }

    /// <summary>Shows the profile bar after successful login.</summary>
    public void ShowProfileBarAfterLogin()
    {
        if (profileBar != null)
            profileBar.SetActive(true);
    }
    public void AIMode()
    {
        GameConfigration.instance.AIInputMode = !GameConfigration.instance.AIInputMode;
        AIDiffivulty.SetActive(true);
        OnlinePanel.SetActive(false);
    }

    public void SetPlayerData() 
    {
        GameConfigration.instance.playerData.XpPoints = TrophiesHandler.Instance.trophyVariables["TotalXp"];
        GamePlayedInARow();
    }
    public void GamePlayedInARow()
    {
        DateTime currentDate = DateTime.Now.Date;

        if (currentDate < new DateTime(2025, 1, 1))
        {
            TrophiesHandler.Instance.trophyVariables["CompleteGameBefore1stJan2025"] = 1;
        }
        if (currentDate < new DateTime(2024, 10, 1))
        {
            TrophiesHandler.Instance.trophyVariables["CompleteGameBefore1stOct2024"] = 1;
        }

        // Retrieve the last played date from PlayerPrefs
        string lastPlayedDateString = PlayerPrefs.GetString("LastPlayedDate", "");
        DateTime lastPlayedDate;

        // If last played date exists, parse it
        if (!string.IsNullOrEmpty(lastPlayedDateString))
        {
            lastPlayedDate = DateTime.Parse(lastPlayedDateString);

            // Check if the player missed a day
            if ((currentDate - lastPlayedDate).Days > 1)
            {
                // Reset consecutive days count
                PlayerPrefs.SetInt("ConsecutiveDays", 0);
            }

            // If last played date is not today, update the consecutive days count
            if (currentDate > lastPlayedDate)
            {
                // Increment consecutive days count
                int consecutiveDays = PlayerPrefs.GetInt("ConsecutiveDays", 0);
                consecutiveDays++;
                PlayerPrefs.SetInt("ConsecutiveDays", consecutiveDays);

                TrophiesHandler.Instance.trophyVariables["TotalDaysPlayed"] = consecutiveDays;

                if (TrophiesHandler.Instance.trophyVariables["TotalDaysPlayed"] > TrophiesHandler.Instance.trophyVariables["TotalDaysPlayedCar"])
                {
                    TrophiesHandler.Instance.trophyVariables["TotalDaysPlayedCar"] = TrophiesHandler.Instance.trophyVariables["TotalDaysPlayed"];
                }

                // Save the current date as last played date
                PlayerPrefs.SetString("LastPlayedDate", currentDate.ToString("yyyy-MM-dd"));
            }
        }
        else
        {
            // If last played date doesn't exist, set it to today and start the counter
            PlayerPrefs.SetString("LastPlayedDate", currentDate.ToString("yyyy-MM-dd"));
            PlayerPrefs.SetInt("ConsecutiveDays", 1);
            TrophiesHandler.Instance.trophyVariables["TotalDaysPlayed"] = 1;
        }

        PlayerPrefs.Save();

        // Output consecutive days count to the console
        int updatedConsecutiveDays = PlayerPrefs.GetInt("ConsecutiveDays", 0);
        Debug.Log("Consecutive Days Played: " + updatedConsecutiveDays);
    }

    public void OfflineMode()
    {
        OnlinePanel.SetActive(false);
    }
}
