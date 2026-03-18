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

        if (playerName.Length < 2)
        {
            feedbackText.text = "Player name must be at least 2 characters.";
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
        feedbackText.text = "Registration successful!";
        Invoke(nameof(DestroyPanel), 1f);
    }

    public void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        feedbackText.text = "PlayerPrefs cleared!";
    }
}
