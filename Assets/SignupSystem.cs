using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class SignupSystem : MonoBehaviour
{
    public static SignupSystem instance;
    public TMP_InputField emailInputField;
    public TMP_InputField playerNameInputField;
    public TMP_InputField passwordInputField;
    public TextMeshProUGUI feedbackText;

    private string emailPattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
    private string passwordPattern = @"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$";
    /// <summary>Same rule as settings: 3–16 chars, letters, numbers, underscores.</summary>
    private string playerNamePattern = @"^[a-zA-Z0-9_]{3,16}$";

    private void Awake()
    {
        instance = this;
    }

    private void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }

    public static SignupSystem ShowUI()
    {
        if (instance == null)
        {
            GameObject obj = Instantiate(Resources.Load("SignInPanel")) as GameObject;
            obj.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);
            instance = obj.GetComponent<SignupSystem>();
            instance.transform.SetAsLastSibling();
        }

        return instance;
    }

    public void DestroyPanel()
    {
        Destroy(gameObject);
    }

    /// <summary>Alias for close/back buttons wired in the prefab.</summary>
    public void Destroy()
    {
        DestroyPanel();
    }

    /// <summary>Back button — closes this Sign In panel and returns to the previous step (Login panel).</summary>
    public void BackToPreviousStep()
    {
        DestroyPanel();
    }

    public void Register()
    {
        string email = emailInputField != null ? emailInputField.text.Trim() : "";
        string playerName = playerNameInputField != null ? playerNameInputField.text.Trim() : "";
        string password = passwordInputField != null ? passwordInputField.text : "";

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(playerName) || string.IsNullOrEmpty(password))
        {
            feedbackText.text = "Please enter email, player name, and password.";
            return;
        }

        if (!Regex.IsMatch(playerName, playerNamePattern))
        {
            feedbackText.text = "Use 3-16 characters with letters, numbers, or underscores only";
            return;
        }

        // Sync registry from prefs (last login / remembered email + stored _playerName) before checking duplicates.
        PlayerNameRegistry.RebuildOwnerKeysFromEmailList();
        // New account: pass null so we only treat the name as free if no other account owns it.
        if (PlayerNameRegistry.IsNameTakenByAnother(playerName, null))
        {
            feedbackText.text = "This name is already in use.  Please choose another one.";
            return;
        }

        if (!Regex.IsMatch(email, emailPattern))
        {
            feedbackText.text = "Invalid email format. Use something like name@example.com.";
            return;
        }

        if (!Regex.IsMatch(password, passwordPattern))
        {
            feedbackText.text = "Password must be at least 8 characters with at least one letter and one number.";
            return;
        }

        if (PlayerPrefs.HasKey(email + "_email"))
        {
            feedbackText.text = "This email is already registered. Log in or use another email.";
            return;
        }

        PlayerPrefs.SetString(email + "_email", email);
        PlayerPrefs.SetString(email + "_playerName", playerName);
        PlayerPrefs.SetString(email + "_password", password);
        PlayerPrefs.Save();

        TrophiesHandler.Instance.playerName = playerName;
        PlayerNameRegistry.RegisterNameForEmail(email, playerName);
        PlayerPrefs.SetString("lastLoginEmail", email);
        feedbackText.text = "Registration successful!";
        Invoke(nameof(DestroyPanel), 1f);
    }

    public void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        feedbackText.text = "PlayerPrefs cleared!";
    }
}
