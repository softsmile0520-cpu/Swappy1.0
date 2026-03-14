using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SignupSystem : MonoBehaviour
{
    public static SignupSystem instance;
    public TMP_InputField emailInputField;
    public TMP_InputField confirmEmailInputField;
    public TextMeshProUGUI countryInputField;
    public TMP_InputField playerNameInputField;
    public TMP_InputField passwordInputField;
    public TextMeshProUGUI feedbackText;
    public TextMeshProUGUI CountryName;
    public Image CountryImage;
    // Email validation regex pattern
    private string emailPattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";

    // Password validation pattern (at least 8 characters, 1 letter, and 1 number)
    private string passwordPattern = @"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$";
    private void Awake()
    {
        instance = this;
    }
    public static SignupSystem ShowUI()
    {
        if (instance == null)
        {
            GameObject obj = Instantiate(Resources.Load("SignUpPanel")) as GameObject;

            obj.gameObject.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);

            instance = obj.GetComponent<SignupSystem>();
            instance.transform.SetSiblingIndex(3);
        }

        return instance;
    }
    private void Start()
    {
        CountryImage.sprite = GameConfigration.instance.countries[TrophiesHandler.Instance.trophyVariables["CountryIndex"]];
        CountryName.text = GameConfigration.instance.countries[TrophiesHandler.Instance.trophyVariables["CountryIndex"]].name;
    }
    public void Destroy()
    {
        Destroy(gameObject);    
    }
    public void OpenCountryPanel()
    {
        Startgame.Instace.n = 2;
        GameConfigration.instance.PlayerSound(0);
        CountryPanel.ShowUI();
    }
    public void Register()
    {
        string email = emailInputField.text;
        string confirmEmail = confirmEmailInputField.text;
        string country = countryInputField.text;
        string playerName = playerNameInputField.text;
        string password = passwordInputField.text;
        
        // Check if any field is empty
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(confirmEmail) || string.IsNullOrEmpty(country) || string.IsNullOrEmpty(playerName) || string.IsNullOrEmpty(password))
        {
            feedbackText.text = "All fields must be filled in.";
            return;
        }

        // Check if emails match
        if (email != confirmEmail)
        {
            feedbackText.text = "Emails do not match!";
            return;
        }

        // Validate email format
        if (!Regex.IsMatch(email, emailPattern))
        {
            feedbackText.text = "Invalid email format! Please use a format like abc@abc.com.";
            return;
        }

        // Validate password strength
        if (!Regex.IsMatch(password, passwordPattern))
        {
            feedbackText.text = "Password must be at least 8 characters long, contain at least one letter and one number.";
            return;
        }

        // Check if email is already registered
        if (PlayerPrefs.HasKey(email + "_email"))
        {
            feedbackText.text = "This email is already registered. Please use another email or log in.";
            return;
        }

        // Save the registration details using the email as the key
        PlayerPrefs.SetString(email + "_email", email);
        PlayerPrefs.SetString(email + "_country", country);
        PlayerPrefs.SetString(email + "_playerName", playerName);
        PlayerPrefs.SetString(email + "_password", password);
        TrophiesHandler.Instance.trophyVariables["CountryIndex"] = GameConfigration.instance.CountryIndex;
        PlayerPrefs.Save();

        print(playerNameInputField.text);
          TrophiesHandler.Instance.playerName = playerNameInputField.text;
        print(playerNameInputField.text);   
        feedbackText.text = "Registration successful!";
        Invoke("Destroy", 1f);
    }

    public void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        feedbackText.text = "PlayerPrefs cleared!";
    }
}
