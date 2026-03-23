using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
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
    /// <summary>Buying/Disclaimer panel — purchase price (2500 change name / 500 change picture). Often &quot;InBank (1)&quot;.</summary>
    public TextMeshProUGUI disclaimerCoinCostText;
    /// <summary>Buying panel path: question (1) / cost — same 2500 / 500 as above (assign or auto-found under DisclaimerPanel).</summary>
    public TextMeshProUGUI buyingPanelQuestionCostText;
    /// <summary>Buying/Disclaimer panel — player&apos;s current coin balance. Often &quot;InBank&quot; (distinct from main profile InBank).</summary>
    public TextMeshProUGUI disclaimerInBankBalanceText;
    /// <summary>Main profile UI — coins in bank (top bar).</summary>
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

    public const int NameChangeCostCoins = 2500;
    /// <summary>Coin cost for profile picture change confirmation (separate from name change).</summary>
    public const int ProfilePictureChangeCostCoins = 500;
    /// <summary>Coin cost to open upload picture panel.</summary>
    public const int UploadPicturePanelCostCoins = 500;

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
            DisclaimerTextName.text =
                "Would you like to Change Your Profile Name To " + Name.text.Trim() + " for " + NameChangeCostCoins + " coins?";
            UpdateBuyingPanelCoinLabels(NameChangeCostCoins);
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
            DisclaimerTextCountry.text =
                "Would you like to Change Your profile Picture for " + ProfilePictureChangeCostCoins + " coins?";
            UpdateBuyingPanelCoinLabels(ProfilePictureChangeCostCoins);
            DisclaimerPanel.SetActive(true);
        }
    }

    /// <summary>Auto-wires InBank = balance, InBank (1) = price under DisclaimerPanel when references are left empty.</summary>
    private void TryBindDisclaimerBuyingPanelInBankTexts()
    {
        if (DisclaimerPanel == null) return;

        foreach (TextMeshProUGUI tmp in DisclaimerPanel.GetComponentsInChildren<TextMeshProUGUI>(true))
        {
            if (tmp.name == "InBank (1)" && disclaimerCoinCostText == null)
                disclaimerCoinCostText = tmp;
            else if (tmp.name == "InBank" && disclaimerInBankBalanceText == null)
                disclaimerInBankBalanceText = tmp;
        }
    }

    /// <summary>BuyingPanel / question (1) — same search path as the prefab hierarchy.</summary>
    private Transform FindBuyingQuestion1Transform()
    {
        if (DisclaimerPanel == null) return null;

        Transform scope = DisclaimerPanel.transform;
        Transform buyingPanel = FindDeepChildByName(scope, "BuyingPanel");
        if (buyingPanel != null)
            scope = buyingPanel;

        Transform question = FindDeepChildByName(scope, "question (1)");
        if (question == null)
            question = FindDeepChildByName(DisclaimerPanel.transform, "question (1)");

        return question;
    }

    /// <summary>Auto-wires BuyingPanel / question (1) / cost when empty (matches common hierarchy).</summary>
    private void TryBindBuyingPanelQuestionCostText()
    {
        if (buyingPanelQuestionCostText != null || DisclaimerPanel == null)
            return;

        Transform question = FindBuyingQuestion1Transform();
        if (question == null)
            return;

        Transform costTr = question.Find("cost");
        if (costTr == null)
            costTr = FindDeepChildByName(question, "cost");

        if (costTr != null)
            buyingPanelQuestionCostText = costTr.GetComponent<TextMeshProUGUI>();
    }

    /// <summary>Leading price on a cost line (before TMP sprites / ?). Prefab may use 1000, 500, or 2500; picture flow must rewrite 2500 → 500.</summary>
    private const string LeadingStalePricePattern = @"^\s*(1000|2500|500)\b";

    /// <summary>
    /// Prefab often has a second line (e.g. "2500 &lt;sprite&gt; ?" left from name-change layout) while picture flow needs 500.
    /// TMP sprite tags break pure-numeric regex; replace leading 1000/2500/500 with the active <paramref name="purchaseCostCoins"/>.
    /// </summary>
    private void SyncAllQuestion1CostAmountTexts(int purchaseCostCoins)
    {
        Transform question = FindBuyingQuestion1Transform();
        if (question == null) return;

        string priceText = purchaseCostCoins.ToString(CultureInfo.InvariantCulture);

        foreach (TextMeshProUGUI tmp in question.GetComponentsInChildren<TextMeshProUGUI>(true))
        {
            string raw = tmp.text;
            if (string.IsNullOrEmpty(raw)) continue;

            if (raw.Length <= 180 && Regex.IsMatch(raw, LeadingStalePricePattern))
            {
                tmp.text = Regex.Replace(raw, LeadingStalePricePattern, priceText);
                continue;
            }

            string t = raw.Trim();
            Match m = Regex.Match(t, @"^(\d{3,5})(\s*\??)?\s*$");
            if (!m.Success) continue;

            if (!int.TryParse(m.Groups[1].Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out int oldVal))
                continue;

            bool nameLooksLikeCost = tmp.name.IndexOf("cost", StringComparison.OrdinalIgnoreCase) >= 0
                || tmp.name.IndexOf("price", StringComparison.OrdinalIgnoreCase) >= 0;
            bool looksLikeStalePriceDefault = oldVal == 1000 || oldVal == 500 || oldVal == 2500;

            if (!nameLooksLikeCost && !looksLikeStalePriceDefault)
                continue;

            string suffix = m.Groups[2].Success ? m.Groups[2].Value : "";
            tmp.text = purchaseCostCoins.ToString(CultureInfo.InvariantCulture) + suffix;
        }
    }

    /// <summary>Same stale price line may live outside question (1) but still under DisclaimerPanel.</summary>
    private void FixStaleLeadingPriceUnderEntireDisclaimer(int purchaseCostCoins)
    {
        if (DisclaimerPanel == null) return;

        string priceText = purchaseCostCoins.ToString(CultureInfo.InvariantCulture);

        foreach (TextMeshProUGUI tmp in DisclaimerPanel.GetComponentsInChildren<TextMeshProUGUI>(true))
        {
            string raw = tmp.text;
            if (string.IsNullOrEmpty(raw) || raw.Length > 180) continue;
            if (!Regex.IsMatch(raw, LeadingStalePricePattern)) continue;

            tmp.text = Regex.Replace(raw, LeadingStalePricePattern, priceText);
        }
    }

    /// <summary>Buying panel: cost = 2500 (name) or 500 (picture); InBank = current player coins.</summary>
    private void UpdateBuyingPanelCoinLabels(int purchaseCostCoins)
    {
        TryBindDisclaimerBuyingPanelInBankTexts();
        TryBindBuyingPanelQuestionCostText();

        if (disclaimerCoinCostText != null)
            disclaimerCoinCostText.text = purchaseCostCoins.ToString();

        if (buyingPanelQuestionCostText != null)
            buyingPanelQuestionCostText.text = purchaseCostCoins.ToString();

        SyncAllQuestion1CostAmountTexts(purchaseCostCoins);
        FixStaleLeadingPriceUnderEntireDisclaimer(purchaseCostCoins);

        int balance = TrophiesHandler.Instance != null ? TrophiesHandler.Instance.trophyVariables["Coins"] : 0;
        if (disclaimerInBankBalanceText != null)
            disclaimerInBankBalanceText.text = balance.ToString();
    }

    public void CloseDisclaimer()
    {
        GameConfigration.instance.PlayerSound(0);
        CountryImageDisclaimer.gameObject.SetActive(false);
        DisclaimerTextName.gameObject.SetActive(false);
        if (disclaimerCoinCostText != null)
            disclaimerCoinCostText.text = "";
        if (buyingPanelQuestionCostText != null)
            buyingPanelQuestionCostText.text = "";
        if (disclaimerInBankBalanceText != null)
            disclaimerInBankBalanceText.text = "";
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
    /// <summary>Wired to the Upload Picture button — Editor/Windows/macOS use a file dialog; mobile uses the gallery.</summary>
    public void UploadImageButton()
    {
        ProfileImageFilePicker.PickImageForProfile(ApplySelectedProfileImagePath);
    }

    /// <summary>Loads the chosen file from disk and updates the preview (works with PC paths and mobile gallery paths).</summary>
    private void ApplySelectedProfileImagePath(string path)
    {
        if (string.IsNullOrEmpty(path))
            return;

        PlayerPrefs.SetString("SelectedImagePath", path);
        Debug.Log("Selected image path: " + path);

        Texture2D texture = null;
        try
        {
            texture = NativeGallery.LoadImageAtPath(path);
        }
        catch (Exception e)
        {
            Debug.LogWarning("LoadImageAtPath failed: " + e.Message);
            return;
        }

        if (texture == null)
        {
            Debug.Log("Failed to load image");
            return;
        }

        Texture2D copyTexture = CopyTextureWithReadable(texture);
        Destroy(texture);

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
