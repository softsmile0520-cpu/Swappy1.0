using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;
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

    public Slider ImageSize;
    public TextAsset SwearWords;
    public Texture2D PlaceholderTexture;
    public List<string> swearWords = new List<string>();

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
        CountryImage.sprite = GameConfigration.instance.countries[TrophiesHandler.Instance.trophyVariables["CountryIndex"]];
        CountryName.text = GameConfigration.instance.countries[TrophiesHandler.Instance.trophyVariables["CountryIndex"]].name;
    }
    public int p = 0;
    public void DisclaimerNameChange(int n)
    {
        GameConfigration.instance.PlayerSound(0);
        p = n;
        if (TrophiesHandler.Instance.trophyVariables["Coins"] >= 1000)
        {
            if (p == 0)
            {
                if (GameConfigration.instance.CountryIndex != TrophiesHandler.Instance.trophyVariables["CountryIndex"])
                {
                    CountryImageDisclaimer.gameObject.SetActive(true);
                    CountryImageDisclaimer.sprite = CountryImage.sprite;
                    DisclaimerTextCountry.text = "Would you like to Change Your Country Name To " + " " + CountryName.text + " for";
                    DisclaimerPanel.SetActive(true);
                }
                else
                    GameConfigration.instance.PlayerSound(27);
            }
            else if (p == 1)
            {
                if (!string.IsNullOrWhiteSpace(Name.text))
                {
                    if (Name.text != TrophiesHandler.Instance.playerName)
                    {
                        string[] list = SwearWords.text.Split('\n');
                        foreach (string arrItem in list)
                        {
                            swearWords.Add(arrItem);

                        }
                        for (int i = 0; i < swearWords.Count; i++)
                        {
                            if (Name.text.Contains(swearWords[i]))
                            {
                                GameConfigration.instance.PlayerSound(27);
                                return;
                            }
                        }
                        DisclaimerTextName.gameObject.SetActive(true);
                        DisclaimerTextName.text = "Would you like to Change Your Profile Name To " + " " + Name.text + " for";
                        DisclaimerPanel.SetActive(true);
                    }
                    else
                        GameConfigration.instance.PlayerSound(27);
                }
                else
                    GameConfigration.instance.PlayerSound(27);
            }
            else if (p == 2)
            {
                if (logoDisplayImageSaved == null)
                {
                    GameConfigration.instance.PlayerSound(27);
                }
                else
                {
                    if (logoDisplayImageSaved != GameConfigration.instance.ProfilePic)
                    {
                        if (TrophiesHandler.Instance.trophyVariables["Coins"] >= 1000)
                        {
                            CountryImageDisclaimer.gameObject.SetActive(true);
                            CountryImageDisclaimer.sprite = logoDisplayImageSaved;
                            DisclaimerTextCountry.text = "Would you like to Change Your profile Picture for";
                            DisclaimerPanel.SetActive(true);
                        }
                        else
                        {
                            GameConfigration.instance.PlayerSound(27);
                        }
                    }
                    else
                    {
                        GameConfigration.instance.PlayerSound(27);
                    }
                }
            }
        }
        else
        {
            GameConfigration.instance.PlayerSound(27);
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
            if (TrophiesHandler.Instance.trophyVariables["Coins"] >= 1000)
            {
                GameConfigration.instance.PlayerSound(31);
                //PlayerPrefs.SetString("CountryName", CountryName.text);
                TrophiesHandler.Instance.trophyVariables["CountryIndex"] = GameConfigration.instance.CountryIndex;
                TrophiesHandler.Instance.trophyVariables["Coins"] = TrophiesHandler.Instance.trophyVariables["Coins"] - 1000;
                playerInfo.AssignPlayerData();
            }
        }
        else if (p == 1)
        {
            if (!string.IsNullOrEmpty(Name.text))
            {
                if (!Name.text.Contains(TrophiesHandler.Instance.playerName))
                {
                    if (TrophiesHandler.Instance.trophyVariables["Coins"] >= 1000)
                    {
                        GameConfigration.instance.PlayerSound(31);
                        TrophiesHandler.Instance.playerName = Name.text;
                        print(TrophiesHandler.Instance.playerName);
                        print(Name.text);
                        TrophiesHandler.Instance.trophyVariables["Coins"] = TrophiesHandler.Instance.trophyVariables["Coins"] - 1000;
                        playerInfo.AssignPlayerData();
                    }
                }
            }
        }
        else if (p == 2)
        {
            GameConfigration.instance.PlayerSound(31);
            TrophiesHandler.Instance.trophyVariables["Coins"] = TrophiesHandler.Instance.trophyVariables["Coins"] - 1000;
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
        if (TrophiesHandler.Instance.trophyVariables["Coins"] >= 1000)
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
