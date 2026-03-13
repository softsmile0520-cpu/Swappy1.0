using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Startgame : MonoBehaviour
{
    public GameObject OnlinePanel, AIDiffivulty;

    public static Startgame Instace;
    public int n;
    private void Start()
    {
        Instace = this;
        print("11111");
        Invoke(nameof(SetPlayerData), 0.5f);
        GameConfigration.instance.CountryIndex = TrophiesHandler.Instance.trophyVariables["CountryIndex"];
        Invoke(nameof(StartLegalFlow), 1f);
    }

    /// <summary>
    /// Flow: Privacy Policy -> Terms of Service -> Login (if not logged in).
    /// </summary>
    private void StartLegalFlow()
    {
        if (PlayerPrefs.GetInt(PrivacyPolicyPanel.PrivacyAcceptedKey, 0) != 1)
        {
            ShowPrivacyPanel();
            return;
        }
        if (PlayerPrefs.GetInt(TermsOfServicePanel.TermsAcceptedKey, 0) != 1)
        {
            ShowTermsPanel();
            return;
        }
        LoginPanelOn();
    }

    private void ShowPrivacyPanel()
    {
        var canvas = GameObject.FindGameObjectWithTag("MainCanvas");
        if (canvas == null)
        {
            Debug.LogWarning("Startgame: MainCanvas not found, skipping Privacy.");
            StartLegalFlow();
            return;
        }
        var go = Instantiate(Resources.Load("Privacy Policy")) as GameObject;
        go.transform.SetParent(canvas.transform, false);
        var panel = go.GetComponent<PrivacyPolicyPanel>() ?? go.AddComponent<PrivacyPolicyPanel>();
        panel.Setup(OnPrivacyAccepted);
    }

    private void OnPrivacyAccepted()
    {
        if (PlayerPrefs.GetInt(TermsOfServicePanel.TermsAcceptedKey, 0) != 1)
        {
            ShowTermsPanel();
            return;
        }
        LoginPanelOn();
    }

    private void ShowTermsPanel()
    {
        var canvas = GameObject.FindGameObjectWithTag("MainCanvas");
        if (canvas == null)
        {
            Debug.LogWarning("Startgame: MainCanvas not found, skipping Terms.");
            LoginPanelOn();
            return;
        }
        var go = Instantiate(Resources.Load("Terms of Service")) as GameObject;
        go.transform.SetParent(canvas.transform, false);
        var panel = go.GetComponent<TermsOfServicePanel>() ?? go.AddComponent<TermsOfServicePanel>();
        panel.Setup(OnTermsAccepted);
    }

    private void OnTermsAccepted()
    {
        LoginPanelOn();
    }

    public void LoginPanelOn()
    {
        print("2222");
        if (PlayerPrefs.GetInt("LoggedIn",0) != 1)
        {
            LoginSystem.ShowUI();
        }
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
