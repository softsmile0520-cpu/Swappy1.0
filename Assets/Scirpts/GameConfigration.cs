using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Runtime.Serialization;
using System;
using System.Reflection;
using UnityEngine.SceneManagement;
using System.IO;

public class GameConfigration : MonoBehaviour
{
    public static GameConfigration instance;

    public int Jem = 500;
    public string PlayerName;
    public string PlayerCountryName;
    public Sprite ProfilePic;
    public Sprite CountryPic;

    public bool AIInputMode = false;
    public bool AIturnFirst = false;
    public int n = 1;
    public int basWinXp = 0;
    public int winPointsMultiplyer;
    public bool gameStarted = false;

    public List<Board> boards;
    public List<Swapie> Swappies;
    public int currentBoardIndex = 0;
    public int currentSwappyIndex = 0;
    public List<AudioClip> Sounds;
    public List<AudioClip> BackGroundSounds;
    public AudioSource SoundSource;
    public AudioSource BGSoundSource;
    public bool testingAI = false;

    public bool offline = true;

    //CurrentGameData
    public bool RandomSelected = false;
    public mode GameMode;


    public List<DragAndDrop> AvailableSwappies = new List<DragAndDrop>();

    public List<SwappyPlayer> _SwappyPlayer = new List<SwappyPlayer>();


    public PlayerData playerData = new PlayerData();

    public List<Sprite> countries;

    /// <summary>Country picker removed; UI uses the first country asset when present.</summary>
    public int CountryUiIndex => countries != null && countries.Count > 0 ? 0 : 0;

    public const string DateKey = "SavedDate";
    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);

        FillData();

        if (PlayerPrefs.HasKey("SelectedImagePath"))
        {
            string savedImagePath = PlayerPrefs.GetString("SelectedImagePath");
            if (!string.IsNullOrEmpty(savedImagePath))
            {
                // Load the saved image data from PlayerPrefs
                string base64Image = PlayerPrefs.GetString("SavedImage");
                byte[] imageData = System.Convert.FromBase64String(base64Image);

                // Create a new Texture2D and load the image data
                Texture2D savedTexture = new Texture2D(2, 2);
                savedTexture.LoadImage(imageData);

                // Assign the loaded texture to the Image component
                ProfilePic = Sprite.Create(savedTexture, new Rect(0, 0, savedTexture.width, savedTexture.height), Vector2.zero);
            }
        }
        else
        {
            Debug.Log("No saved image path found");
        }

        BGSoundSource.volume = PlayerPrefs.GetFloat("BGVolumeValue", 1);
        SoundSource.volume = PlayerPrefs.GetFloat("FXVolumeValue", 1);
    }


    // Start is called before the first frame update

    private void RequestPermissionAndLoadImage()
    {
        // Check if permission is already granted
        if (!UnityEngine.Android.Permission.HasUserAuthorizedPermission(UnityEngine.Android.Permission.ExternalStorageRead))
        {
            // Request permission
            UnityEngine.Android.Permission.RequestUserPermission(UnityEngine.Android.Permission.ExternalStorageRead);

            // Wait for a short time before attempting to load the image
            StartCoroutine(WaitAndLoadImage());
        }
        else
        {
            // Permission is already granted, load the image
            LoadSavedImage();
        }
    }

    private IEnumerator WaitAndLoadImage()
    {
        // Wait for a short time before attempting to load the image
        yield return new WaitForSeconds(0.5f);

        // Load the image after waiting
        LoadSavedImage();
    }

    private void LoadSavedImage()
    {
        string savedImagePath = PlayerPrefs.GetString("SelectedImagePath");
        if (!string.IsNullOrEmpty(savedImagePath))
        {
            if (File.Exists(savedImagePath))
            {
                // Load the image data from the saved path
                byte[] imageData = File.ReadAllBytes(savedImagePath);

                // Create a new Texture2D and load the image data
                Texture2D savedTexture = new Texture2D(2, 2);
                savedTexture.LoadImage(imageData);

                // Assign the loaded texture to the Image component
                ProfilePic = Sprite.Create(savedTexture, new Rect(0, 0, savedTexture.width, savedTexture.height), Vector2.zero);
            }
            else
            {
                Debug.Log("File does not exist at saved image path: " + savedImagePath);
            }
        }
        else
        {
            Debug.Log("No saved image path found");
        }
    }

    void Start()
    {
        //RequestPermissionAndLoadImage();

        //if (PlayerPrefs.HasKey("SelectedImagePath"))
        //{
        //    string savedImagePath = PlayerPrefs.GetString("SelectedImagePath");
        //    if (!string.IsNullOrEmpty(savedImagePath))
        //    {
        //        // Load the saved image data from PlayerPrefs
        //        string base64Image = PlayerPrefs.GetString("SavedImage");
        //        byte[] imageData = System.Convert.FromBase64String(base64Image);

        //        // Create a new Texture2D and load the image data
        //        Texture2D savedTexture = new Texture2D(2, 2);
        //        savedTexture.LoadImage(imageData);
        //        // Assign the loaded texture to the Image component
        //        GameConfigration.instance.ProfilePic = Sprite.Create(savedTexture, new Rect(0, 0, savedTexture.width, savedTexture.height), Vector2.zero);
        //    }
        //}
        //else
        //{
        //    Debug.Log("No saved image path found");
        //}
        // Check if the month has changed
        int currentMonth = System.DateTime.Now.Month;
        int lastMonth = PlayerPrefs.GetInt("LastMonth", currentMonth);

        if (currentMonth != lastMonth)
        {
            // Reset the game count for the new month
            TrophiesHandler.Instance.ResetMonthlyRecords();
            PlayerPrefs.SetInt("LastMonthKey", currentMonth);
        }

        // Example: Save the current date when the game starts
        SaveCurrentDate();

        // Example: Retrieve and print the saved date
        DateTime savedDate = GetSavedDate();
        Debug.Log("Saved Date: " + savedDate.ToString("MMMM yyyy dd"));
    }

    public void SaveCurrentDate()
    {
        DateTime currentDate = DateTime.Now;
        PlayerPrefs.SetString(DateKey, currentDate.ToString("yyyy-MM-dd"));
    }

    public DateTime GetSavedDate()
    {
        string savedDateString = PlayerPrefs.GetString(DateKey, DateTime.Now.ToString("yyyy-MM-dd"));
        DateTime savedDate;
        if (DateTime.TryParse(savedDateString, out savedDate))
        {
            return savedDate;
        }
        else
        {
            Debug.LogWarning("Failed to parse saved date. Returning current date.");
            return DateTime.Now;
        }
    }

    public bool IsNewMonth()
    {
        DateTime currentDate = DateTime.Now;
        DateTime savedDate = GetSavedDate();
        return currentDate.Year != savedDate.Year || currentDate.Month != savedDate.Month;
    }

    public void VolumeControll(float a)
    {
        BGSoundSource.volume = a;
    }
    
    public void FXVolumeControll(float a)
    {
        SoundSource.volume = a;
    }
    public void BGSoundPlayer(int i)
    {
        BGSoundSource.Stop();
        BGSoundSource.clip = BackGroundSounds[i];
        BGSoundSource.Play();
    }
    public void PlayerSound(int i)
    {
        SoundSource.PlayOneShot(Sounds[i]);
    }
    public void FillData()
    {
        currentBoardIndex = PlayerPrefs.GetInt("BoardStyle", 0);
        currentSwappyIndex = PlayerPrefs.GetInt("SwappyStyle", 0);
        for (int i = 2; i < boards.Count; i++)
        {
            if(PlayerPrefs.GetInt("Board" + i, 0)==1)
            {
                boards[i].Unlocked = true;
            }
            else
            {
                boards[i].Unlocked = false;
            }
        }
        for (int i =4 ; i < Swappies.Count; i++)
        {
            if (PlayerPrefs.GetInt("Swapy" + i, 0) == 1)
            {
                Swappies[i].Unlocked = true;
            }
            else
            {
                Swappies[i].Unlocked = false;
            }
        }
    }

    public void updateCoins(int n)
    {
        print(n);
        TrophiesHandler.Instance.trophyVariables["Coins"] = TrophiesHandler.Instance.trophyVariables["Coins"] + n;
        TrophiesHandler.Instance.SaveData();
    }
    public void updateJem(int n)
    {
        TrophiesHandler.Instance.trophyVariables["Jems"] += n;
        TrophiesHandler.Instance.SaveData();
    }

    public void updateBoard(int index)
    {
        currentBoardIndex = index;
        PlayerPrefs.SetInt("BoardStyle", currentBoardIndex);
    }
    public void updateSwappies(int index)
    {
        currentSwappyIndex = index;
        PlayerPrefs.SetInt("SwappyStyle", currentSwappyIndex);
    }
    
    public void UnlockBoard(int index)
    {
        boards[index].Unlocked = true;
        PlayerPrefs.SetInt("Board" + index, 1);
    }

    public void UnlockSwapies(int index)
    {
        Swappies[index].Unlocked = true;
        PlayerPrefs.SetInt("Swapy" + index, 1);
    }

}

public enum mode
{
    Classic,
    Fast,
    Power
}

public enum Difficulty
{
    Hard,
    Medium,
    Easy
}

public enum BoardPartition
{
    Tleft,
    TRight,
    BLeft,
    BRight
}

[Serializable]
public class Board
{
    public Sprite ViewImage;
    public Texture Box1;
    public Texture Box2;
    public string BoardName;
    public bool Unlocked;
    public int Price;
    public string currency;
}
[Serializable]
public class Swapie
{
    public Sprite ViewImage;
    public Texture swapieDisplay;
    public string SwapydName;
    public bool Unlocked;
    public int Price;
    public string currency;
}

[Serializable]
public class PlayerData
{
    public Sprite PlayerImage;
    public Sprite CountryImage;
    public string PlayerName = "The Builder";
    public string CountryName = "Canada";
    public int Coins;
    public int Jems;
    public int XpPoints;
}

[Serializable]
public class SwappyPlayer
{
    public bool Dead = false;

    public Swapie SwapieDisplay;

    public Sprite Country;

    public Sprite ProfilePic;

    public string CountryName;
    public string PlayerName;

    public int score = 0;
    public int Position = 1;

    public DragAndDrop swappyPrefab;

    public List<DragAndDrop> mySwappies = new List<DragAndDrop>();

    public Tiles FirstSwappyPlaced;

    public bool AiSwappy = true;

    public bool ForcedAi = false;
    public Difficulty PlayerDifficulty;
    public BoardPartition _BoardPartition;
    public bool PartitionAssigned;

}



