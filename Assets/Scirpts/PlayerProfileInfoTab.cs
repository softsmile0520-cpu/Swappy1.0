using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerProfileInfoTab : MonoBehaviour
{
    public TextMeshProUGUI PlayerName;
    public TextMeshProUGUI CountryName;
    public TextMeshProUGUI Coins;
    public TextMeshProUGUI CoinsAdded;
    public TextMeshProUGUI Jems;
    public TextMeshProUGUI JemsAdded;
    public TextMeshProUGUI XpPoints;
    public TextMeshProUGUI XpPointsadded;
    public TextMeshProUGUI GoatTrophies;
    public TextMeshProUGUI GoatTrophiesAdded;
    public TextMeshProUGUI MasterTrophiesAdded;
    public TextMeshProUGUI MasterTrophies;
    public TextMeshProUGUI KudoTrophiesAdded;
    public TextMeshProUGUI KudoTrophies;
    PlayerData playerData;
    public Image ProfilePic;
    public Image CountryImage;

    int coins2;
    int Xp2;
    int Jems2;
    int Kudo2;
    int master2;
    int goat2;
    int a = 0;

    private void Awake()
    {
       
    }
    private void Start()
    {
        playerData = GameConfigration.instance.playerData;
        StorePreviousValues();
        PlayerName.text = TrophiesHandler.Instance.playerName;
        //PlayerName.color = Color.black;
        //CountryName.color = Color.black;
    }
    private void Update()
    {
        PlayerName.text = TrophiesHandler.Instance.playerName;
    }

    public void StorePreviousValues()
    {
        coins2 = TrophiesHandler.Instance.trophyVariables["Coins"];
        Xp2 = TrophiesHandler.Instance.trophyVariables["TotalXp"];
        Jems2 = TrophiesHandler.Instance.trophyVariables["Jems"];
        goat2 = TrophiesHandler.Instance.GoatTrophies.Count(b => b.received == true);
        master2 = TrophiesHandler.Instance.MasterTrophies.Count(b => b.received == true);
        Kudo2 = TrophiesHandler.Instance.KudoTrophies.Count(b => b.received == true);
        Coins.text = TrophiesHandler.Instance.trophyVariables["Coins"] + "";
        XpPoints.text = TrophiesHandler.Instance.trophyVariables["TotalXp"] + "";
        Jems.text = TrophiesHandler.Instance.trophyVariables["Jems"] + "";
        GoatTrophies.text = TrophiesHandler.Instance.GoatTrophies.Count(b => b.received == true) + "";
        MasterTrophies.text = TrophiesHandler.Instance.MasterTrophies.Count(b => b.received == true) + "";
        KudoTrophies.text = TrophiesHandler.Instance.KudoTrophies.Count(b => b.received == true) + "";

        int ci = GameConfigration.instance.CountryUiIndex;
        if (GameConfigration.instance.countries != null && GameConfigration.instance.countries.Count > 0)
        {
            CountryImage.sprite = GameConfigration.instance.countries[ci];
            CountryName.text = GameConfigration.instance.countries[ci].name;
        }
        PlayerName.text = TrophiesHandler.Instance.playerName;

        float c = PlayerPrefs.GetFloat("PicSize", 1);
        ProfilePic.transform.localScale = new Vector3(c, c, c);
        ProfilePic.sprite = GameConfigration.instance.ProfilePic;
    }

    public void AssignPlayerData()
    {
        
        if (coins2 != TrophiesHandler.Instance.trophyVariables["Coins"])
        {
            int diff = TrophiesHandler.Instance.trophyVariables["Coins"] - coins2;
            coins2 = TrophiesHandler.Instance.trophyVariables["Coins"];
              
            StartCoroutine(TurnOnAdded(CoinsAdded, diff));
        }
        Coins.text = TrophiesHandler.Instance.trophyVariables["Coins"] + "";

        if (Xp2 != TrophiesHandler.Instance.trophyVariables["TotalXp"])
        {
            int diff = TrophiesHandler.Instance.trophyVariables["TotalXp"] - Xp2;
            Xp2 = TrophiesHandler.Instance.trophyVariables["TotalXp"];
            StartCoroutine(TurnOnAdded(XpPointsadded, diff));
        }
        XpPoints.text = TrophiesHandler.Instance.trophyVariables["TotalXp"] + "";

        if (Jems2 != TrophiesHandler.Instance.trophyVariables["Jems"])
        {
            int diff = TrophiesHandler.Instance.trophyVariables["Jems"] - Jems2;
            Jems2 = TrophiesHandler.Instance.trophyVariables["Jems"];
            StartCoroutine(TurnOnAdded(JemsAdded, diff));
        }
        Jems.text = TrophiesHandler.Instance.trophyVariables["Jems"] + "";

        if (goat2 != TrophiesHandler.Instance.GoatTrophies.Count(b => b.received == true))
        {
            int diff = TrophiesHandler.Instance.GoatTrophies.Count(b => b.received == true) - goat2;
            goat2 = TrophiesHandler.Instance.GoatTrophies.Count(b => b.received == true);
            StartCoroutine(TurnOnAdded(GoatTrophiesAdded, diff));
        }
        GoatTrophies.text = TrophiesHandler.Instance.GoatTrophies.Count(b => b.received == true) + "";

        if (master2 != TrophiesHandler.Instance.MasterTrophies.Count(b => b.received == true))
        {
            int diff = TrophiesHandler.Instance.MasterTrophies.Count(b => b.received == true) - master2;
            master2 = TrophiesHandler.Instance.MasterTrophies.Count(b => b.received == true);
            StartCoroutine(TurnOnAdded(MasterTrophiesAdded, diff));
        }
        MasterTrophies.text = TrophiesHandler.Instance.MasterTrophies.Count(b => b.received == true) + "";

        if (Kudo2 != TrophiesHandler.Instance.KudoTrophies.Count(b => b.received == true))
        {
            int diff = TrophiesHandler.Instance.KudoTrophies.Count(b => b.received == true) - Kudo2;
            Kudo2 = TrophiesHandler.Instance.KudoTrophies.Count(b => b.received == true);
            StartCoroutine(TurnOnAdded(KudoTrophiesAdded, diff));
        }
        KudoTrophies.text = TrophiesHandler.Instance.KudoTrophies.Count(b => b.received == true) + "";
        PlayerName.text = TrophiesHandler.Instance.playerName;
        int ci2 = GameConfigration.instance.CountryUiIndex;
        if (GameConfigration.instance.countries != null && GameConfigration.instance.countries.Count > 0)
        {
            CountryName.text = GameConfigration.instance.countries[ci2].name;
            CountryImage.sprite = GameConfigration.instance.countries[ci2];
        }
        float c = PlayerPrefs.GetFloat("PicSize", 1);
        ProfilePic.transform.localScale = new Vector3(c, c, c);
        ProfilePic.sprite = GameConfigration.instance.ProfilePic;
    }
    IEnumerator TurnOnAdded(TextMeshProUGUI ChangedValue, int ChnagedValue)
    {
        ChangedValue.gameObject.SetActive(true);
        if (ChnagedValue < 0)
        {
            //ChangedValue.color = Color.red;
            ChangedValue.text = "<color=red>" + ChnagedValue;
        }
        else
        {
            ChangedValue.text = "<color=green>+" + ChnagedValue;

            //ChangedValue.color = Color.green;   
        }
        yield return new WaitForSeconds(1f);
        ChangedValue.gameObject.SetActive(false);    
    }
    public void OpenPlayerProfile()
    {
        PlayerProfileInfo.ShowUI();
    }
}
