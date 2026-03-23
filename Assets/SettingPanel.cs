using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour
{
    public static SettingPanel instance;

    public TMP_InputField Name;

    [NonSerialized]
    public PlayerProfileInfoTab playerInfo;

    public PlayerProfileInfoTab PlayerInfoTabPrefab;

    public Transform ProfileBar;

    public GameObject UploadImagePanel;
    public GameObject OtherThingsPanel;

    public Image logoDisplay;
    public Sprite logoDisplayImageSaved;
    public Image DecidingLogoPanel;

    public TextMeshProUGUI CountryName;
    public Image CountryImage;

    public GameObject DisclaimerPanel;
    public Image CountryImageDisclaimer;
    public TextMeshProUGUI DisclaimerTextCountry;
    public TextMeshProUGUI DisclaimerTextName;
    public TextMeshProUGUI InBank;

    [Tooltip("TMP for name validation errors (assign your 'Name Error' label here). If empty, auto-finds a child named 'Name Error'.")]
    public TextMeshProUGUI nameChangeFeedbackText;

    /// <summary>Created at runtime if nameChangeFeedbackText is not assigned in the prefab.</summary>
    private TextMeshProUGUI _runtimeNameFeedback;

    private bool _nameErrorLookupDone;

    public Slider ImageSize;
    public TextAsset SwearWords;
    public Texture2D PlaceholderTexture;
    public List<string> swearWords = new List<string>();

    public const int NameChangeCostCoins = 1000;
    /// <summary>Coin cost for profile picture change confirmation (separate from name change).</summary>
    public const int ProfilePictureChangeCostCoins = 1000;
    /// <summary>Coin cost to open upload picture panel.</summary>
    public const int UploadPicturePanelCostCoins = 1000;

    public const string MsgNotEnoughCoins = "You don't have enough coins";
    public const string MsgNameFormat = "Use 3-16 characters with letters, numbers, or underscores only";
    public const string MsgNameDisallowed = "Name not allowed. Choose a respectful, safe username.";
    public const string MsgNameInUse = "This name is already in use.  Please choose another one.";

    private static readonly Regex s_playerNameRegex = new Regex(@"^[a-zA-Z0-9_]{3,16}$", RegexOptions.Compiled);

    public static SettingPanel ShowUI()
    {
        if (instance == null)
        {
            GameObject obj = Instantiate(Resources.Load("SettingPanel")) as GameObject;

            obj.gameObject.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);

            instance = obj.GetComponent<SettingPanel>();
        }

        return instance;
    }
    private void Start()
    {
        ImageSize.value = PlayerPrefs.GetFloat("PicSize", 1);
        playerInfo = Instantiate(PlayerInfoTabPrefab, ProfileBar);
        InBank.text = TrophiesHandler.Instance.trophyVariables["Coins"] + "";
        logoDisplay.sprite = GameConfigration.instance.ProfilePic;
        int ci = GameConfigration.instance.CountryUiIndex;
        if (GameConfigration.instance.countries != null && GameConfigration.instance.countries.Count > 0)
        {
            CountryImage.sprite = GameConfigration.instance.countries[ci];
            CountryName.text = GameConfigration.instance.countries[ci].name;
        }

        if (SwearWords != null && swearWords.Count == 0)
        {
            foreach (string line in SwearWords.text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
            {
                string t = line.Trim();
                if (t.Length > 0) swearWords.Add(t);
            }
        }

        if (Name != null && TrophiesHandler.Instance != null)
            Name.text = TrophiesHandler.Instance.playerName ?? "";

        TryBindNameErrorLabel();
    }

    private void Awake()
    {
        TryBindNameErrorLabel();
    }

    /// <summary>Finds a TMP named "Name Error" (or similar) under this panel so Inspector wiring is optional.</summary>
    private void TryBindNameErrorLabel()
    {
        if (_nameErrorLookupDone) return;
        if (nameChangeFeedbackText != null)
        {
            _nameErrorLookupDone = true;
            return;
        }

        foreach (string nm in new[] { "Name Error", "NameError", "name error", "NameErrorText" })
        {
            Transform t = FindDeepChildByName(transform, nm);
            if (t == null) continue;
            TextMeshProUGUI tmp = t.GetComponent<TextMeshProUGUI>();
            if (tmp == null)
                tmp = t.GetComponentInChildren<TextMeshProUGUI>(true);
            if (tmp != null)
            {
                nameChangeFeedbackText = tmp;
                break;
            }
        }

        _nameErrorLookupDone = true;
    }

    private static Transform FindDeepChildByName(Transform parent, string childName)
    {
        if (parent == null || string.IsNullOrEmpty(childName)) return null;
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform c = parent.GetChild(i);
            if (string.Equals(c.name, childName, StringComparison.OrdinalIgnoreCase))
                return c;
            Transform deep = FindDeepChildByName(c, childName);
            if (deep != null) return deep;
        }
        return null;
    }

    private static void EnsureParentsActiveForDisplay(Transform leaf)
    {
        if (leaf == null) return;
        Transform t = leaf;
        while (t != null)
        {
            if (!t.gameObject.activeSelf)
                t.gameObject.SetActive(true);
            t = t.parent;
        }
    }

    /// <summary>Wire the "Change Name" button to this (or use <see cref="DisclaimerNameChange"/> with argument 1).</summary>
    public void ChangeNameButtonClicked()
    {
        DisclaimerNameChange(1);
    }

    private void SetNameFeedback(string message, bool isError)
    {
        if (string.IsNullOrEmpty(message))
        {
            ClearNameFeedback();
            return;
        }

        TryBindNameErrorLabel();

        TextMeshProUGUI target = nameChangeFeedbackText;
        if (target == null)
            target = EnsureRuntimeNameFeedbackLabel();

        if (target != null)
        {
            EnsureParentsActiveForDisplay(target.transform);
            target.gameObject.SetActive(true);
            target.enabled = true;
            target.text = message;
            target.color = isError ? new Color(0.8f, 0.1f, 0.1f) : Color.black;
            target.canvasRenderer.SetAlpha(1f);
        }
        else
        {
            Debug.LogWarning("[Change Name] " + message);
        }
    }

    private void ClearNameFeedback()
    {
        if (nameChangeFeedbackText != null)
        {
            nameChangeFeedbackText.text = "";
            // Keep inactive when empty so "Name Error" doesn't show an empty reserved strip unless you prefer always-on.
            nameChangeFeedbackText.gameObject.SetActive(false);
        }
        if (_runtimeNameFeedback != null)
        {
            _runtimeNameFeedback.text = "";
            _runtimeNameFeedback.gameObject.SetActive(false);
        }
    }

    /// <summary>Creates a red TMP line under the name field when the prefab has no feedback text assigned.</summary>
    private TextMeshProUGUI EnsureRuntimeNameFeedbackLabel()
    {
        if (_runtimeNameFeedback != null)
            return _runtimeNameFeedback;
        if (Name == null || Name.textComponent == null)
            return null;

        var go = new GameObject("NameChangeValidationFeedback");
        go.transform.SetParent(Name.transform.parent, false);
        go.transform.SetSiblingIndex(Name.transform.GetSiblingIndex() + 1);

        var rt = go.AddComponent<RectTransform>();
        var nameRt = Name.GetComponent<RectTransform>();
        if (nameRt != null)
        {
            rt.anchorMin = nameRt.anchorMin;
            rt.anchorMax = nameRt.anchorMax;
            rt.pivot = new Vector2(0.5f, 1f);
            rt.anchoredPosition = nameRt.anchoredPosition + new Vector2(0f, -nameRt.rect.height - 4f);
            rt.sizeDelta = new Vector2(nameRt.sizeDelta.x, Mathf.Max(28f, Name.textComponent.fontSize + 8f));
        }

        _runtimeNameFeedback = go.AddComponent<TextMeshProUGUI>();
        _runtimeNameFeedback.font = Name.textComponent.font;
        _runtimeNameFeedback.fontSharedMaterial = Name.textComponent.fontSharedMaterial;
        _runtimeNameFeedback.fontSize = Mathf.Max(14f, Name.textComponent.fontSize - 2f);
        _runtimeNameFeedback.alignment = TextAlignmentOptions.Midline;
        _runtimeNameFeedback.enableWordWrapping = true;
        _runtimeNameFeedback.color = new Color(0.85f, 0.15f, 0.15f);
        go.SetActive(false);
        return _runtimeNameFeedback;
    }

    private static string GetLoggedInEmail()
    {
        string e = PlayerPrefs.GetString("lastLoginEmail", PlayerPrefs.GetString("rememberedEmail", ""));
        if (!string.IsNullOrEmpty(e))
            return e;

        // OAuth / edge cases: infer from registry + current profile name
        if (TrophiesHandler.Instance == null)
            return "";
        string pn = TrophiesHandler.Instance.playerName;
        if (string.IsNullOrEmpty(pn) || pn == "Guest")
            return "";

        e = PlayerNameRegistry.GetOwnerEmailForDisplayName(pn);
        if (!string.IsNullOrEmpty(e))
            return e;

        return PlayerNameRegistry.FindEmailWithStoredPlayerName(pn);
    }

    /// <summary>Validates proposed display name before showing the coin confirmation dialog.</summary>
    public bool TryValidateNameChange(string proposed, out string errorMessage)
    {
        errorMessage = null;
        string current = TrophiesHandler.Instance != null ? TrophiesHandler.Instance.playerName : "";

        if (TrophiesHandler.Instance.trophyVariables["Coins"] < NameChangeCostCoins)
        {
            errorMessage = MsgNotEnoughCoins;
            return false;
        }

        if (string.IsNullOrWhiteSpace(proposed))
        {
            errorMessage = MsgNameFormat;
            return false;
        }

        string trimmed = proposed.Trim();
        if (!s_playerNameRegex.IsMatch(trimmed))
        {
            errorMessage = MsgNameFormat;
            return false;
        }

        if (string.Equals(trimmed, current, StringComparison.OrdinalIgnoreCase))
        {
            errorMessage = null;
            return false;
        }

        foreach (string bad in swearWords)
        {
            if (string.IsNullOrEmpty(bad)) continue;
            if (trimmed.IndexOf(bad, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                errorMessage = MsgNameDisallowed;
                return false;
            }
        }

        bool isGuest = PlayerPrefs.GetInt("IsGuest", 0) == 1;
        // Rebuild registry so Owner_* and email list match prefs (same as Signup).
        PlayerNameRegistry.RebuildOwnerKeysFromEmailList();
        // Guests have no email: pass null so any name owned by a registered account on this device is blocked.
        string myEmailForCheck = isGuest ? null : GetLoggedInEmail();
        if (PlayerNameRegistry.IsNameTakenByAnother(trimmed, myEmailForCheck))
        {
            errorMessage = MsgNameInUse;
            return false;
        }

        return true;
    }
    public int p = 0;
    public void DisclaimerNameChange(int n)
    {
        GameConfigration.instance.PlayerSound(0);
        p = n;
        if (p == 0)
        {
            GameConfigration.instance.PlayerSound(27);
            return;
        }

        if (p == 1)
        {
            if (TrophiesHandler.Instance.trophyVariables["Coins"] < NameChangeCostCoins)
            {
                SetNameFeedback(MsgNotEnoughCoins, true);
                GameConfigration.instance.PlayerSound(27);
                return;
            }

            if (Name == null)
            {
                Debug.LogError("SettingPanel: Assign the Name (TMP_InputField) in the Inspector.");
                return;
            }

            ClearNameFeedback();
            if (!TryValidateNameChange(Name.text, out string err))
            {
                if (!string.IsNullOrEmpty(err))
                {
                    SetNameFeedback(err, true);
                    GameConfigration.instance.PlayerSound(27);
                }
                else
                    GameConfigration.instance.PlayerSound(27);
                return;
            }

            DisclaimerTextName.gameObject.SetActive(true);
            DisclaimerTextName.text = "Would you like to Change Your Profile Name To " + " " + Name.text.Trim() + " for";
            DisclaimerPanel.SetActive(true);
            return;
        }

        if (p == 2)
        {
            if (logoDisplayImageSaved == null)
            {
                GameConfigration.instance.PlayerSound(27);
                return;
            }

            if (logoDisplayImageSaved == GameConfigration.instance.ProfilePic)
            {
                GameConfigration.instance.PlayerSound(27);
                return;
            }

            if (TrophiesHandler.Instance.trophyVariables["Coins"] < ProfilePictureChangeCostCoins)
            {
                GameConfigration.instance.PlayerSound(27);
                return;
            }

            CountryImageDisclaimer.gameObject.SetActive(true);
            CountryImageDisclaimer.sprite = logoDisplayImageSaved;
            DisclaimerTextCountry.text = "Would you like to Change Your profile Picture for";
            DisclaimerPanel.SetActive(true);
        }
    }
    public void CloseDisclaimer()
    {
        GameConfigration.instance.PlayerSound(0);
        CountryImageDisclaimer.gameObject.SetActive(false);
        DisclaimerTextName.gameObject.SetActive(false);
        DisclaimerPanel?.SetActive(false);
    }
    public void OpenCountryPanel()
    {
        Startgame.Instace.n = 1;
        GameConfigration.instance.PlayerSound(0);
        CountryPanel.ShowUI();
    }
    public void ChangingSettings()
    {
        GameConfigration.instance.PlayerSound(0);
        if (p == 0)
        {
        }
        else if (p == 1)
        {
            if (!TryValidateNameChange(Name.text, out string err))
            {
                if (!string.IsNullOrEmpty(err))
                    SetNameFeedback(err, true);
                CloseDisclaimer();
                return;
            }

            string newName = Name.text.Trim();
            string oldName = TrophiesHandler.Instance.playerName;
            if (TrophiesHandler.Instance.trophyVariables["Coins"] >= NameChangeCostCoins)
            {
                GameConfigration.instance.PlayerSound(31);
                TrophiesHandler.Instance.playerName = newName;
                TrophiesHandler.Instance.trophyVariables["Coins"] -= NameChangeCostCoins;

                if (PlayerPrefs.GetInt("IsGuest", 0) != 1)
                {
                    string myEmail = GetLoggedInEmail();
                    if (!string.IsNullOrEmpty(myEmail))
                        PlayerNameRegistry.ChangeRegisteredName(myEmail, oldName, newName);
                }

                ClearNameFeedback();
                playerInfo.AssignPlayerData();
            }
        }
        else if (p == 2)
        {
            GameConfigration.instance.PlayerSound(31);
            TrophiesHandler.Instance.trophyVariables["Coins"] = TrophiesHandler.Instance.trophyVariables["Coins"] - ProfilePictureChangeCostCoins;
            GameConfigration.instance.ProfilePic = logoDisplayImageSaved;
            logoDisplay.sprite = GameConfigration.instance.ProfilePic;
            float a = PlayerPrefs.GetFloat("PicSize", 1);
            logoDisplay.transform.localScale = new Vector3(a, a, a);
            DecidingLogoPanel.sprite = GameConfigration.instance.ProfilePic;
            playerInfo.AssignPlayerData();

        }
        InBank.text = TrophiesHandler.Instance.trophyVariables["Coins"] + "";
        ModeSelectionPanelScript.instance.playerInfo.StorePreviousValues();
        CloseDisclaimer();
        TrophiesHandler.Instance.SaveData();
    }
    public void UploadImagePanelShow()
    {
        if (TrophiesHandler.Instance.trophyVariables["Coins"] >= UploadPicturePanelCostCoins)
        {
            UploadImagePanel.SetActive(true);
        OtherThingsPanel.SetActive(false);
        }
        else
        {
            GameConfigration.instance.PlayerSound(27);
        }
    }
    public void SetImageSize()
    {
        float a = ImageSize.value;
        PlayerPrefs.SetFloat("PicSize",a);
        TrophiesHandler.Instance.PicSize = a;
        DecidingLogoPanel.transform.localScale = new Vector3(a, a, a);
    }
    Texture2D CopyTextureWithReadable(Texture2D originalTexture)
    {
        // Create a temporary RenderTexture
        RenderTexture renderTexture = RenderTexture.GetTemporary(originalTexture.width, originalTexture.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);

        // Set the active RenderTexture
        RenderTexture.active = renderTexture;

        // Copy the content of the original texture to the RenderTexture
        Graphics.Blit(originalTexture, renderTexture);

        // Create a new Texture2D
        Texture2D copyTexture = new Texture2D(originalTexture.width, originalTexture.height, originalTexture.format, originalTexture.mipmapCount > 1);

        // Read the pixels from the RenderTexture
        copyTexture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);

        // Apply changes to make the copy texture readable
        copyTexture.Apply();

        // Release the temporary RenderTexture
        RenderTexture.ReleaseTemporary(renderTexture);

        // Return the copy texture
        return copyTexture;
    }
    public void UploadImageButton()
    {
        NativeGallery.GetImageFromGallery((path) =>
        {
            if (string.IsNullOrEmpty(path))
                return;

            // Save selected image path
            PlayerPrefs.SetString("SelectedImagePath", path);
            Debug.Log("Selected image path: " + path);

            // Load image
            Texture2D texture = NativeGallery.LoadImageAtPath(path);
            if (texture == null)
            {
                Debug.Log("Failed to load image");
                return;
            }

            // Make readable copy
            Texture2D copyTexture = CopyTextureWithReadable(texture);

            // Save image
            byte[] imageData = copyTexture.EncodeToPNG();
            string base64Image = Convert.ToBase64String(imageData);
            PlayerPrefs.SetString("SavedImage", base64Image);
            PlayerPrefs.Save();

            // Apply to UI
            logoDisplayImageSaved = Sprite.Create(
                copyTexture,
                new Rect(0, 0, copyTexture.width, copyTexture.height),
                Vector2.zero
            );

            DecidingLogoPanel.sprite = logoDisplayImageSaved;

        }, "Select an image", "image/*");
    }

    void OpenGalleryInternal()
    {
        NativeGallery.GetImageFromGallery((path) =>
        {
            if (path == null)
                return;

            PlayerPrefs.SetString("SelectedImagePath", path);
            Debug.Log("Selected image path: " + path);

            Texture2D texture = NativeGallery.LoadImageAtPath(path);
            if (texture == null)
            {
                Debug.Log("Failed to load image");
                return;
            }

            Texture2D copyTexture = CopyTextureWithReadable(texture);

            byte[] imageData = copyTexture.EncodeToPNG();
            string base64Image = Convert.ToBase64String(imageData);
            PlayerPrefs.SetString("SavedImage", base64Image);
            PlayerPrefs.Save();

            logoDisplayImageSaved = Sprite.Create(
                copyTexture,
                new Rect(0, 0, copyTexture.width, copyTexture.height),
                Vector2.zero
            );

            DecidingLogoPanel.sprite = logoDisplayImageSaved;

        },
        "Select an image",   // ✅ title (string)
        "image/*");
    }

    public void CloseUploadImagePanel()
    {
        UploadImagePanel.SetActive(false);
        OtherThingsPanel.SetActive(true);
    }
    public void Slangrestrictions()
    {


        

    }
    public void LeaveProfilePanel()
    {
        SoundsPanel.ShowUI();
        backPressed();
    }
    public void backPressed()
    {
        GameConfigration.instance.PlayerSound(0);
        Destroy(this.gameObject);
    }
}
