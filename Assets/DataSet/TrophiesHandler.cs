using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

public class TrophiesHandler : MonoBehaviour
{
    public static TrophiesHandler Instance;

    public int TotalTrophies;//

    public List<Trophy> KudoTrophies = new List<Trophy>();
    public List<Trophy> MasterTrophies = new List<Trophy>();
    public List<Trophy> GoatTrophies = new List<Trophy>();

    //public List<bool> KudoTrophiesB = new List<bool>();
    //public List<bool> MasterTrophiesB = new List<bool>();
    //public List<bool> GoatTrophiesB = new List<bool>();

    [SerializeField]
    public Dictionary<string, int> trophyVariables = new Dictionary<string, int>()
        {
        { "TotalTrophies", 0 },
        { "TotalTrophiesTM", 0 },
        { "TotalDaysPlayed", 0 },
        { "TotalDaysPlayedCar", 1 },
        { "InviteAndGetTrophy", 0 },
        { "TotalSwapOnBoardAfterWin", 0 },
        { "TwoPGwithAtlest200p", 0 },
        { "ThreePGwithAtlest300p", 0 },
        { "FourPGwithAtlest400p", 0 },
        { "TwoPGwithAtlest222p", 0 },
        { "ThreePGwithAtlest333p", 0 },
        { "FourPGwithAtlest444p", 0 },
        { "Win2pGonline", 0 },
        { "TwoPGwith15p", 0 },
        { "threePGwith10p", 0 },
        { "fourPGwith5p", 0 },
        { "Win3PGonline", 0 },
        { "Win4PGonline", 0 },
        { "MaxSwappyCombo", 0 },
        { "MaxSwappyComboTM", 0 },
        { "DefeatOppInClassicM", 0 },
        { "DefeatOppInClassicMTM", 0 },
        { "DefeatOppInFastM", 0 },
        { "DefeatOppInFastMTM", 0 },
        { "DefeatOppInOnlineM", 0 },
        { "DefeatOppInOnlineMTM", 0 },
        { "DefeatOppInOfflineM", 0 },
        { "DefeatOppInOfflineMTM", 0 },
        { "DefeatOppInPowerM", 0 },
        { "DefeatOppInPowerMTM", 0 },
        { "TotalPlayedGames", 0 },
        { "TotalPlayedGamesTM", 0 },
        { "CompleteGameBefore1stJan2025", 0 },
        { "CompleteGameBefore1stOct2024", 0 },
        { "Betaversion", 1 },
        { "CompleteGameBeforeBetaVersion", 0 },
        { "purchaseSomethingInShop", 0 },
        { "DoaMonthlyPayment", 0 },
        { "Doa50DPurchase", 0 },
        { "WinGamesfrom4th", 0 },
        { "ReachXPpoints", 0 },
        { "Coins", 500000 },
        { "Jems", 10000 },
        { "TotalXp", 0 },
        { "TotalXpTM", 0 },
        { "LeastSwappies", 0 },
        { "LeastSwappiesTM", 0 },
        { "2-PlayerMP" ,0},
        { "3-PlayerMP" ,0},
        { "4-PlayerMP" ,0},
        };

    public string playerName;
    public float PicSize;

    public TextAsset KudoRecord;
    public TextAsset MasterRecord;
    public TextAsset GoatRecord;

    public string FilePathKudo;
    public string FilePathMaster;
    public string FilePathGoat;
    private void Awake()
    {
        LoadReceiveTrophies();

        playerName = PlayerPrefs.GetString("_playerName", playerName);
        print(playerName);
        PicSize = PlayerPrefs.GetFloat("PicSize", PicSize);

        string jsonString = PlayerPrefs.GetString("trophyVariables", JsonConvert.SerializeObject(trophyVariables));
        trophyVariables = JsonConvert.DeserializeObject<Dictionary<string, int>>(jsonString);

        // Restore name ownership + email list so duplicate-name checks work for existing accounts (legacy installs).
        PlayerNameRegistry.RebuildOwnerKeysFromEmailList();
    }
    private void Start()
    {
        Instance = this;
        if (PlayerPrefs.GetInt("IsGuest", 0) == 1)
            ResetToGuestState();
        if (trophyVariables.ContainsKey("CountryIndex"))
        {
            trophyVariables.Remove("CountryIndex");
            SaveData();
        }

        //LoadFiles();
        //LoadDataInRelatedLists();


    }

    void LoadReceiveTrophies()
    {
        for (int i = 0; i < KudoTrophies.Count; i++)
        {
            KudoTrophies[i].received = PlayerPrefs.GetInt("KudoTrophieReceived" + i, 0) == 1;
        }
        for (int i = 0; i < KudoTrophies.Count; i++)
        {
            MasterTrophies[i].received = PlayerPrefs.GetInt("MasterTrophieReceived" + i, 0) == 1;
        }
        for (int i = 0; i < KudoTrophies.Count; i++)
        {
            GoatTrophies[i].received = PlayerPrefs.GetInt("GoatTrophieReceived" + i, 0) == 1;
        }
    }

    void SaveReceiveTrophies()
    {
        for (int i = 0; i < KudoTrophies.Count; i++)
        {
            PlayerPrefs.SetInt("KudoTrophieReceived" + i, KudoTrophies[i].received ? 1 : 0);
        }
        for (int i = 0; i < KudoTrophies.Count; i++)
        {
            PlayerPrefs.SetInt("MasterTrophieReceived" + i, KudoTrophies[i].received ? 1 : 0);
        }
        for (int i = 0; i < KudoTrophies.Count; i++)
        {
            PlayerPrefs.SetInt("GoatTrophieReceived" + i, KudoTrophies[i].received ? 1 : 0);
        }
    }

    public void ResetMonthlyRecords()
    {
        trophyVariables["TotalPlayedGamesTM"] = 0;
        trophyVariables["TotalXpTM"] = 0;
        trophyVariables["DefeatOppInOfflineMTM"] = 0;
        trophyVariables["DefeatOppInOnlineMTM"] = 0;
        trophyVariables["DefeatOppInPowerMTM"] = 0;
        trophyVariables["DefeatOppInClassicMTM"] = 0;
        trophyVariables["DefeatOppInFastMTM"] = 0;
        trophyVariables["MaxSwappyComboTM"] = 0;
        trophyVariables["2-PlayerMP"] = 0;
        trophyVariables["3-PlayerMP"] = 0;
        trophyVariables["4-PlayerMP"] = 0;
        trophyVariables["LeastSwappiesTM"] = 0;
        trophyVariables["TotalTrophiesTM"] = 0;
    }
    public void SaveData(bool UpdateProfeile = false,PlayerProfileInfoTab playerInfo = null)
    {
        if (PlayerPrefs.GetInt("IsGuest", 0) == 1)
            return;
        SaveReceiveTrophies();
        if (UpdateProfeile)
        {
            playerInfo.AssignPlayerData();
        }
        string jsonString = JsonConvert.SerializeObject(trophyVariables);
        PlayerPrefs.SetString("trophyVariables", jsonString);
        PlayerPrefs.SetString("_playerName", playerName);
        PlayerPrefs.SetFloat("PicSize", PicSize);
    }

    /// <summary>Resets to guest state: name "Guest", 0 XP, 0 coins/jems, no medals. Used when playing as guest.</summary>
    public void ResetToGuestState()
    {
        playerName = "Guest";
        trophyVariables = new Dictionary<string, int>()
        {
            { "TotalTrophies", 0 },
            { "TotalTrophiesTM", 0 },
            { "TotalDaysPlayed", 0 },
            { "TotalDaysPlayedCar", 0 },
            { "InviteAndGetTrophy", 0 },
            { "TotalSwapOnBoardAfterWin", 0 },
            { "TwoPGwithAtlest200p", 0 },
            { "ThreePGwithAtlest300p", 0 },
            { "FourPGwithAtlest400p", 0 },
            { "TwoPGwithAtlest222p", 0 },
            { "ThreePGwithAtlest333p", 0 },
            { "FourPGwithAtlest444p", 0 },
            { "Win2pGonline", 0 },
            { "TwoPGwith15p", 0 },
            { "threePGwith10p", 0 },
            { "fourPGwith5p", 0 },
            { "Win3PGonline", 0 },
            { "Win4PGonline", 0 },
            { "MaxSwappyCombo", 0 },
            { "MaxSwappyComboTM", 0 },
            { "DefeatOppInClassicM", 0 },
            { "DefeatOppInClassicMTM", 0 },
            { "DefeatOppInFastM", 0 },
            { "DefeatOppInFastMTM", 0 },
            { "DefeatOppInOnlineM", 0 },
            { "DefeatOppInOnlineMTM", 0 },
            { "DefeatOppInOfflineM", 0 },
            { "DefeatOppInOfflineMTM", 0 },
            { "DefeatOppInPowerM", 0 },
            { "DefeatOppInPowerMTM", 0 },
            { "TotalPlayedGames", 0 },
            { "TotalPlayedGamesTM", 0 },
            { "CompleteGameBefore1stJan2025", 0 },
            { "CompleteGameBefore1stOct2024", 0 },
            { "Betaversion", 1 },
            { "CompleteGameBeforeBetaVersion", 0 },
            { "purchaseSomethingInShop", 0 },
            { "DoaMonthlyPayment", 0 },
            { "Doa50DPurchase", 0 },
            { "WinGamesfrom4th", 0 },
            { "ReachXPpoints", 0 },
            { "Coins", 0 },
            { "Jems", 0 },
            { "TotalXp", 0 },
            { "TotalXpTM", 0 },
            { "LeastSwappies", 0 },
            { "LeastSwappiesTM", 0 },
            { "2-PlayerMP", 0 },
            { "3-PlayerMP", 0 },
            { "4-PlayerMP", 0 },
        };
        for (int i = 0; i < KudoTrophies.Count; i++)
            KudoTrophies[i].received = false;
        for (int i = 0; i < MasterTrophies.Count; i++)
            MasterTrophies[i].received = false;
        for (int i = 0; i < GoatTrophies.Count; i++)
            GoatTrophies[i].received = false;
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
        {
            SaveData();
        }
    }
    void LoadFiles()
    {
        FilePathKudo = Application.persistentDataPath + "/TrophiesRecordKudo.csv";
        if (!File.Exists(FilePathKudo))
        {
            ReadAndSaveFile("TrophiesRecordKudo");
            FilePathKudo = Application.persistentDataPath + "/TrophiesRecordKudo.csv";
            if (!File.Exists(FilePathKudo))
            {
                print("aaaaaaa");
            }
        }

        FilePathMaster = Application.persistentDataPath + "/TrophiesRecordMaster.csv";
        if (!File.Exists(FilePathMaster))
        {
            ReadAndSaveFile("TrophiesRecordMaster");
            FilePathMaster = Application.persistentDataPath + "/TrophiesRecordMaster.csv";
            if (!File.Exists(FilePathMaster))
            {
                print("aaaaaaa");
            }
        }

        FilePathGoat = Application.persistentDataPath + "/TrophiesRecordGoat.csv";
        if (!File.Exists(FilePathGoat))
        {
            ReadAndSaveFile("TrophiesRecordGoat");
            FilePathGoat = Application.persistentDataPath + "/TrophiesRecordGoat.csv";
            if (!File.Exists(FilePathGoat))
            {
                print("aaaaaaa");
            }
        }

    }

    void ReadAndSaveFile(string fileName)
    {
        // Load the file from the Resources folder
        TextAsset file = Resources.Load<TextAsset>(fileName);

        if (file != null)
        {
            // Get the file content
            string fileContent = file.text;

            // Get the destination file path in the persistent data path
            string destinationFilePath = Path.Combine(Application.persistentDataPath, fileName + ".csv");

            // Write the file content to the destination path
            File.WriteAllText(destinationFilePath, fileContent);
        }

    }

    public string ReadData(string filePath)
    {
        string fileContent;
        using (StreamReader reader = new StreamReader(filePath))
        {
            fileContent = reader.ReadToEnd();
        }
        return fileContent;
    }
    void LoadTrophiesData(string PathFile, List<Trophy> Trophies, int size)
    {
        string csvText = ReadData(PathFile);
        string[] lines = csvText.Split('\n'); // Split the text into lines
        for (int i = 0; i < size; i++)
        {
            string[] col = lines[i].Split(','); // Split the line into fields

            Trophies[i].received = (int.Parse(col[1]) == 1);
        }
    }

    public void LoadDataInRelatedLists()
    {
        LoadTrophiesData(FilePathKudo, KudoTrophies, 21);
        LoadTrophiesData(FilePathMaster, MasterTrophies, 21);
        LoadTrophiesData(FilePathGoat, GoatTrophies, 21);

    }

    public void SaveTrophieData(int index, string filePath, List<Trophy> trophyList)
    {
        trophyList[index].received = true;
        // Read the existing content of the file
        //string fileContent;
        //using (StreamReader reader = new StreamReader(filePath))
        //{
        //    fileContent = reader.ReadToEnd();
        //}

        //// Append data to the CSV file
        //string updatedContent = AppendGameData(index, fileContent);

        //// Write the updated content to the same file, replacing the previous content
        //using (StreamWriter writer = new StreamWriter(filePath))
        //{
        //    writer.Write(updatedContent);
        //}
    }

    string AppendGameData(int n, string fileContent)
    {
        string[] col;

        string[] lines = fileContent.Split('\n');

        col = lines[n].Split(',');

        col[1] = "1";

        lines[n] = string.Join(",", col);

        string updatedContent = string.Join("\n", lines);
        return updatedContent;
    }
}

[Serializable]
public class Trophy
{
    public int tier;
    public Sprite TrophyImage;
    public string TrophyName;
    public bool received;
    public bool Newlyreceived;
    public int CoinValue;
    public int JemValue;
    public string WinnigText;
    public bool workedOn;
}
