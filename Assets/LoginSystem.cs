using AppleAuth;
using DG.Tweening.Core.Easing;
using Firebase.Auth;
using System.Net;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginSystem : MonoBehaviour
{
    public static LoginSystem instance;

    public TMP_InputField emailInputField;       // Email input field
    public TMP_InputField passwordInputField;    // Password input field
    public TextMeshProUGUI feedbackText;
    public Toggle rememberMeToggle;              // Checkbox for "Remember Me"
    public Color errorColor = Color.red;         // Color for error messages
    public Color defaultColor = Color.black;     // Default color for feedback text (set as needed)

    private string rememberEmailKey = "rememberedEmail";
    private string rememberPasswordKey = "rememberedPassword";  // Optional, if you want to store password

    // Email validation regex pattern
    private string emailPattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";



    public static LoginSystem ShowUI()
    {
        if (instance == null)
        {
            GameObject obj = Instantiate(Resources.Load("LoginSystem")) as GameObject;

            obj.gameObject.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);

            instance = obj.GetComponent<LoginSystem>();
        }

        return instance;
    }
    private void Start()
    {
        instance = this;
        // Check if "Remember Me" data exists, and populate the fields
        if (PlayerPrefs.HasKey(rememberEmailKey))
        {
            emailInputField.text = PlayerPrefs.GetString(rememberEmailKey);

            // Optional: Pre-fill password (if you want to store it, not recommended for security reasons)
            if (PlayerPrefs.HasKey(rememberPasswordKey))
            {
                passwordInputField.text = PlayerPrefs.GetString(rememberPasswordKey);
            }

            // Set the checkbox to true as we have saved data
            rememberMeToggle.isOn = true;
        }

        feedbackText.color = defaultColor; // Set feedback text to default color at the start
    }

    public void Login()
    {
        string email = emailInputField.text;
        string password = passwordInputField.text;

        // Validate email format
        if (!System.Text.RegularExpressions.Regex.IsMatch(email, emailPattern))
        {
            feedbackText.text = "Invalid email format!";
            feedbackText.color = errorColor; // Set feedback text to red for error
            return;
        }

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            feedbackText.text = "Please enter your email and password.";
            feedbackText.color = errorColor; // Set feedback text to red for error
            return;
        }

        // Check if the email is registered
        if (PlayerPrefs.HasKey(email + "_email"))
        {
            // Retrieve the stored password using the email key
            string storedPassword = PlayerPrefs.GetString(email + "_password");

            // Check if the password matches the stored one
            if (password == storedPassword)
            {
                feedbackText.text = "Login successful!";
                feedbackText.color = defaultColor; // Reset feedback text to default color on success

                TrophiesHandler.Instance.trophyVariables["CountryIndex"] = GameConfigration.instance.CountryIndex;
                PlayerPrefs.SetInt("LoggedIn", 1);
                // Handle "Remember Me" functionality
                if (rememberMeToggle.isOn)
                {
                    // Save email and optionally password
                    PlayerPrefs.SetString(rememberEmailKey, email);
                    PlayerPrefs.SetString(rememberPasswordKey, password); // Optional: Save the password
                }
                else
                {
                    // Clear remembered data if checkbox is not selected
                    PlayerPrefs.DeleteKey(rememberEmailKey);
                    PlayerPrefs.DeleteKey(rememberPasswordKey);  // If you are saving password
                }

                PlayerPrefs.Save();
                ModeSelectionPanelScript.instance.playerInfo.AssignPlayerData();
                Invoke("LoginPanelDestroy", 1f);
                // Add logic for a successful login, like transitioning to another scene
            }
            else
            {
                feedbackText.text = "Incorrect email or password.";
                feedbackText.color = errorColor; // Set feedback text to red for error
            }
        }
        else
        {
            feedbackText.text = "No user registered with this email!";
            feedbackText.color = errorColor; // Set feedback text to red for error
        }
    }
    public void SignupBtn()
    {
        SignupSystem.ShowUI();

        // Destroy(gameObject);
    }
    public void LoginPanelDestroy()
    {
        if (Startgame.Instace != null)
            Startgame.Instace.ShowProfileBarAfterLogin();
        Destroy(gameObject);
    }

    public void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        feedbackText.text = "PlayerPrefs cleared!";
        feedbackText.color = defaultColor; // Reset feedback text color
    }







    private IAppleAuthManager _appleAuthManager;
    public FirebaseAuth auth;
    public FirebaseUser user;



    //public void Initializ()
    //{

    //}


}
