using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TrophiesCollectedSection : MonoBehaviour
{

    public TextMeshProUGUI TrophieName;
    public TextMeshProUGUI TrophieName2nd;
    public TextMeshProUGUI careerTrophies;
    public TextMeshProUGUI careerTrophiesPoints;
    public TextMeshProUGUI thisMonthTrophies;
    public TextMeshProUGUI thisMonthTrophiesPoints;
    public TextMeshProUGUI fourPlayers; 
    public TextMeshProUGUI fourPlayersPoints;

    public GameObject _noPlayersData;
    public TextMeshProUGUI _twoPlayer;
    public TextMeshProUGUI _threePlayer;
    public TextMeshProUGUI _fourPlayer;

    public Image Kudo;
    public Image Master;
    public Image Goat;

    public Sprite KudoS;
    public Sprite MasterS;
    public Sprite GoatS;

    public GameObject availableSoon;

    // Start is called before the first frame update
    void Start()
    {
        //TrophieName.color = Color.black;
        //TrophieName2nd.color = Color.black;
        //careerTrophies.color = Color.black;
        //careerTrophiesPoints.color = Color.black;
        //thisMonthTrophies.color = Color.black;
        //thisMonthTrophiesPoints.color = Color.black;
        //_twoPlayer.color = Color.black;
        //_threePlayer.color = Color.black;
        //_fourPlayer.color = Color.black;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetTrophieData(int Pos)
    {
        switch (Pos){
            case 0:
                TrophieName.text = "Games Played";
                TrophieName2nd.text = "Gamer's trophies";
                careerTrophiesPoints.text = TrophiesHandler.Instance.trophyVariables["TotalPlayedGames"] + "";
                thisMonthTrophies.gameObject.SetActive(true);
                thisMonthTrophiesPoints.gameObject.SetActive(true);
                thisMonthTrophiesPoints.text = TrophiesHandler.Instance.trophyVariables["TotalPlayedGamesTM"] + "";
                if (TrophiesHandler.Instance.KudoTrophies[16].received == true)
                    Kudo.sprite = TrophiesHandler.Instance.KudoTrophies[16].TrophyImage;
                else
                    Kudo.gameObject.SetActive(false);
                if (TrophiesHandler.Instance.MasterTrophies[16].received == true)
                    Master.sprite = TrophiesHandler.Instance.MasterTrophies[16].TrophyImage;
                else
                    Master.gameObject.SetActive(false);
                if (TrophiesHandler.Instance.GoatTrophies[16].received == true)
                    Goat.sprite = TrophiesHandler.Instance.GoatTrophies[16].TrophyImage;
                else
                    Goat.gameObject.SetActive(false);

                break;
            case 1:
                TrophieName.text = "Xp Points";
                TrophieName2nd.text = "Xp trophies";
                careerTrophiesPoints.text = TrophiesHandler.Instance.trophyVariables["TotalXp"] + "";
                thisMonthTrophies.gameObject.SetActive(true);
                thisMonthTrophiesPoints.gameObject.SetActive(true);
                thisMonthTrophiesPoints.text = TrophiesHandler.Instance.trophyVariables["TotalXpTM"] + "";
                if (TrophiesHandler.Instance.KudoTrophies[20].received == true)
                    Kudo.sprite = TrophiesHandler.Instance.KudoTrophies[20].TrophyImage;
                else
                    Kudo.gameObject.SetActive(false);
                if (TrophiesHandler.Instance.MasterTrophies[20].received == true)
                    Master.sprite = TrophiesHandler.Instance.MasterTrophies[20].TrophyImage;
                else
                    Master.gameObject.SetActive(false);
                if (TrophiesHandler.Instance.GoatTrophies[20].received == true)
                    Goat.sprite = TrophiesHandler.Instance.GoatTrophies[20].TrophyImage;
                else
                    Goat.gameObject.SetActive(false);
                break;
            case 2:
                TrophieName.text = "Defeated Opponents (Offline)";
                TrophieName.fontSize = 26;
                TrophieName2nd.text = "Dino trophies";
                careerTrophiesPoints.text = TrophiesHandler.Instance.trophyVariables["DefeatOppInOfflineM"] + "";
                thisMonthTrophies.gameObject.SetActive(true);
                thisMonthTrophiesPoints.gameObject.SetActive(true);
                thisMonthTrophiesPoints.text = TrophiesHandler.Instance.trophyVariables["DefeatOppInOfflineMTM"] + "";
                if (TrophiesHandler.Instance.KudoTrophies[13].received == true)
                    Kudo.sprite = TrophiesHandler.Instance.KudoTrophies[13].TrophyImage;
                else
                    Kudo.gameObject.SetActive(false);
                if (TrophiesHandler.Instance.MasterTrophies[13].received == true)
                    Master.sprite = TrophiesHandler.Instance.MasterTrophies[13].TrophyImage;
                else
                    Master.gameObject.SetActive(false);
                if (TrophiesHandler.Instance.GoatTrophies[13].received == true)
                    Goat.sprite = TrophiesHandler.Instance.GoatTrophies[13].TrophyImage;
                else
                    Goat.gameObject.SetActive(false);
                break;
            case 3:
                TrophieName.text = "Defeated Opponents (Online)";
                TrophieName.fontSize = 26;
                TrophieName2nd.gameObject.SetActive(false);
                availableSoon.SetActive(true);
                careerTrophiesPoints.text = TrophiesHandler.Instance.trophyVariables["DefeatOppInOnlineM"] + "";
                thisMonthTrophies.gameObject.SetActive(true);
                thisMonthTrophiesPoints.gameObject.SetActive(true);
                thisMonthTrophiesPoints.text = TrophiesHandler.Instance.trophyVariables["DefeatOppInOnlineMTM"] + "";
                if (TrophiesHandler.Instance.KudoTrophies[14].received == true)
                    Kudo.sprite = TrophiesHandler.Instance.KudoTrophies[14].TrophyImage;
                else
                    Kudo.gameObject.SetActive(false);
                if (TrophiesHandler.Instance.MasterTrophies[14].received == true)
                    Master.sprite = TrophiesHandler.Instance.MasterTrophies[14].TrophyImage;
                else
                    Master.gameObject.SetActive(false);
                if (TrophiesHandler.Instance.GoatTrophies[14].received == true)
                    Goat.sprite = TrophiesHandler.Instance.GoatTrophies[14].TrophyImage;
                else
                    Goat.gameObject.SetActive(false);
                break;
            case 4:
                TrophieName.text = "Defeated Opponents (Power Mode)";
                TrophieName.fontSize = 26;
                TrophieName2nd.text = "Power trophies";
                careerTrophiesPoints.text = TrophiesHandler.Instance.trophyVariables["DefeatOppInPowerM"] + "";
                thisMonthTrophies.gameObject.SetActive(true);
                thisMonthTrophiesPoints.gameObject.SetActive(true);
                thisMonthTrophiesPoints.text = TrophiesHandler.Instance.trophyVariables["DefeatOppInPowerMTM"] + "";
                if (TrophiesHandler.Instance.KudoTrophies[15].received == true)
                    Kudo.sprite = TrophiesHandler.Instance.KudoTrophies[15].TrophyImage;
                else
                    Kudo.gameObject.SetActive(false);
                if (TrophiesHandler.Instance.MasterTrophies[15].received == true)
                    Master.sprite = TrophiesHandler.Instance.MasterTrophies[15].TrophyImage;
                else
                    Master.gameObject.SetActive(false);
                if (TrophiesHandler.Instance.GoatTrophies[15].received == true)
                    Goat.sprite = TrophiesHandler.Instance.GoatTrophies[15].TrophyImage;
                else
                    Goat.gameObject.SetActive(false);
                break;
            case 5:
                TrophieName.text = "Defeated Opponents (Classic Mode)";
                TrophieName.fontSize = 26;
                TrophieName2nd.text = "Classic trophies";
                careerTrophiesPoints.text = TrophiesHandler.Instance.trophyVariables["DefeatOppInClassicM"] + "";
                thisMonthTrophies.gameObject.SetActive(true);
                thisMonthTrophiesPoints.gameObject.SetActive(true);
                thisMonthTrophiesPoints.text = TrophiesHandler.Instance.trophyVariables["DefeatOppInClassicMTM"] + "";
                if (TrophiesHandler.Instance.KudoTrophies[11].received == true)
                    Kudo.sprite = TrophiesHandler.Instance.KudoTrophies[11].TrophyImage;
                else
                    Kudo.gameObject.SetActive(false);
                if (TrophiesHandler.Instance.MasterTrophies[11].received == true)
                    Master.sprite = TrophiesHandler.Instance.MasterTrophies[11].TrophyImage;
                else
                    Master.gameObject.SetActive(false);
                if (TrophiesHandler.Instance.GoatTrophies[11].received == true)
                    Goat.sprite = TrophiesHandler.Instance.GoatTrophies[11].TrophyImage;
                else
                    Goat.gameObject.SetActive(false);
                break;
            case 6:
                TrophieName.text = "Defeated Opponents (Fast Mode)";
                TrophieName.fontSize = 26;
                TrophieName2nd.text = "Fast trophies";
                careerTrophiesPoints.text = TrophiesHandler.Instance.trophyVariables["DefeatOppInFastM"] + "";
                thisMonthTrophies.gameObject.SetActive(true);
                thisMonthTrophiesPoints.gameObject.SetActive(true);
                thisMonthTrophiesPoints.text = TrophiesHandler.Instance.trophyVariables["DefeatOppInFastMTM"] + "";
                if (TrophiesHandler.Instance.KudoTrophies[12].received == true)
                    Kudo.sprite = TrophiesHandler.Instance.KudoTrophies[12].TrophyImage;
                else
                    Kudo.gameObject.SetActive(false);
                if (TrophiesHandler.Instance.MasterTrophies[12].received == true)
                    Master.sprite = TrophiesHandler.Instance.MasterTrophies[12].TrophyImage;
                else
                    Master.gameObject.SetActive(false);
                if (TrophiesHandler.Instance.GoatTrophies[12].received == true)
                    Goat.sprite = TrophiesHandler.Instance.GoatTrophies[12].TrophyImage;
                else
                    Goat.gameObject.SetActive(false);
                break;
            case 7:
                TrophieName.text = "Biggest Combo";
                TrophieName2nd.text = "Combo trophies";
                careerTrophiesPoints.text = TrophiesHandler.Instance.trophyVariables["MaxSwappyCombo"] + "";
                //thisMonthTrophies.gameObject.SetActive(true); 
                //thisMonthTrophiesPoints.gameObject.SetActive(true);
                //thisMonthTrophiesPoints.text = TrophiesHandler.Instance.trophyVariables["MaxSwappyComboTM"] + "";
                if (TrophiesHandler.Instance.KudoTrophies[10].received == true)
                    Kudo.sprite = TrophiesHandler.Instance.KudoTrophies[10].TrophyImage;
                else
                    Kudo.gameObject.SetActive(false);
                if (TrophiesHandler.Instance.MasterTrophies[10].received == true)
                    Master.sprite = TrophiesHandler.Instance.MasterTrophies[10].TrophyImage;
                else
                    Master.gameObject.SetActive(false);
                if (TrophiesHandler.Instance.GoatTrophies[10].received == true)
                    Goat.sprite = TrophiesHandler.Instance.GoatTrophies[10].TrophyImage;
                else
                    Goat.gameObject.SetActive(false);
                break;
            case 8:
                TrophieName.text = "DEDICATION (days in a row with 1 game played)";
                TrophieName.fontSize = 25;
                TrophieName2nd.text = "Dedication trophies";
                careerTrophiesPoints.text = TrophiesHandler.Instance.trophyVariables["TotalDaysPlayedCar"] + "";
                thisMonthTrophies.gameObject.SetActive(true);
                thisMonthTrophiesPoints.gameObject.SetActive(true);
                thisMonthTrophies.text = "right now";
                thisMonthTrophiesPoints.text = TrophiesHandler.Instance.trophyVariables["TotalDaysPlayed"] + "";
                if (TrophiesHandler.Instance.KudoTrophies[1].received == true)
                    Kudo.sprite = TrophiesHandler.Instance.KudoTrophies[1].TrophyImage;
                else
                    Kudo.gameObject.SetActive(false);
                if (TrophiesHandler.Instance.MasterTrophies[1].received == true)
                    Master.sprite = TrophiesHandler.Instance.MasterTrophies[1].TrophyImage;
                else
                    Master.gameObject.SetActive(false);
                if (TrophiesHandler.Instance.GoatTrophies[1].received == true)
                    Goat.sprite = TrophiesHandler.Instance.GoatTrophies[1].TrophyImage;
                else
                    Goat.gameObject.SetActive(false);
                break;
            case 9:
                TrophieName.text = "Pioneer (date of first completed game) " + PlayerPrefs.GetString("SavedDate");
                TrophieName.fontSize = 26;
                TrophieName2nd.text = "Pioneer trophies";
                careerTrophies.gameObject.SetActive(false);
                careerTrophiesPoints.gameObject.SetActive(false);
                if (TrophiesHandler.Instance.KudoTrophies[17].received == true)
                    Kudo.sprite = TrophiesHandler.Instance.KudoTrophies[17].TrophyImage;
                else 
                    Kudo.gameObject.SetActive(false);
                if (TrophiesHandler.Instance.MasterTrophies[17].received == true)
                    Master.sprite = TrophiesHandler.Instance.MasterTrophies[17].TrophyImage;
                else
                    Master.gameObject.SetActive(false);
                if (TrophiesHandler.Instance.GoatTrophies[17].received == true)
                    Goat.sprite = TrophiesHandler.Instance.GoatTrophies[17].TrophyImage;
                else
                    Goat.gameObject.SetActive(false);
                break;
            case 10:
                TrophieName.text = "Most points in a game";
                TrophieName2nd.text = "Tough win trophies";
                _noPlayersData.gameObject.SetActive(true);
                careerTrophiesPoints.gameObject.SetActive(false);
                careerTrophies.gameObject.SetActive(false);
                _twoPlayer.text = TrophiesHandler.Instance.trophyVariables["2-PlayerMP"] + "";
                _threePlayer.text = TrophiesHandler.Instance.trophyVariables["3-PlayerMP"] + "";
                _fourPlayer.text = TrophiesHandler.Instance.trophyVariables["4-PlayerMP"] + "";
                if (TrophiesHandler.Instance.KudoTrophies[5].received == true)
                    Kudo.sprite = TrophiesHandler.Instance.KudoTrophies[5].TrophyImage;
                else
                    Kudo.gameObject.SetActive(false);
                if (TrophiesHandler.Instance.MasterTrophies[5].received == true)
                    Master.sprite = TrophiesHandler.Instance.MasterTrophies[5].TrophyImage;
                else
                    Master.gameObject.SetActive(false);
                if (TrophiesHandler.Instance.GoatTrophies[5].received == true)
                    Goat.sprite = TrophiesHandler.Instance.GoatTrophies[5].TrophyImage;
                else
                    Goat.gameObject.SetActive(false);
                break;
            case 11:
                TrophieName.text = "??????";
                TrophieName2nd.text = "Secret trophies";
                _noPlayersData.gameObject.SetActive(true);

                careerTrophiesPoints.gameObject.SetActive(false);
                careerTrophies.gameObject.SetActive(false);
                if (TrophiesHandler.Instance.KudoTrophies[7].received == true)
                {
                    Kudo.sprite = TrophiesHandler.Instance.KudoTrophies[7].TrophyImage;
                    _twoPlayer.text = "15";
                }
                else
                {
                    Kudo.gameObject.SetActive(false);
                    _twoPlayer.gameObject.transform.parent.gameObject.SetActive(false);
                }
                if (TrophiesHandler.Instance.MasterTrophies[7].received == true)
                {
                    Master.sprite = TrophiesHandler.Instance.MasterTrophies[7].TrophyImage;
                    _threePlayer.text = "10";
                }
                else
                {
                    Master.gameObject.SetActive(false);
                    _threePlayer.gameObject.transform.parent.gameObject.SetActive(false);
                }
                if (TrophiesHandler.Instance.GoatTrophies[7].received == true)
                {
                    Goat.sprite = TrophiesHandler.Instance.GoatTrophies[7].TrophyImage;
                    _fourPlayer.text = "5";
                }
                else
                {
                    Goat.gameObject.SetActive(false);
                    _fourPlayer.gameObject.transform.parent.gameObject.SetActive(false);
                }
                Kudo.gameObject.SetActive(false);
                Master.gameObject.SetActive(false);
                Goat.gameObject.SetActive(false);
                break;
            case 12:
                TrophieName.text = "Least Swappies In a Winning Game";
                TrophieName2nd.text = "Efficient trophies";
                careerTrophiesPoints.text = TrophiesHandler.Instance.trophyVariables["LeastSwappies"] + "";
                thisMonthTrophies.gameObject.SetActive(true);
                thisMonthTrophiesPoints.gameObject.SetActive(true);
                thisMonthTrophiesPoints.text = TrophiesHandler.Instance.trophyVariables["LeastSwappiesTM"] + "";
                if (TrophiesHandler.Instance.KudoTrophies[3].received == true)
                    Kudo.sprite = TrophiesHandler.Instance.KudoTrophies[3].TrophyImage;
                else
                    Kudo.gameObject.SetActive(false);
                if (TrophiesHandler.Instance.MasterTrophies[3].received == true)
                    Master.sprite = TrophiesHandler.Instance.MasterTrophies[3].TrophyImage;
                else
                    Master.gameObject.SetActive(false);
                if (TrophiesHandler.Instance.GoatTrophies[3].received == true)
                    Goat.sprite = TrophiesHandler.Instance.GoatTrophies[3].TrophyImage;
                else
                    Goat.gameObject.SetActive(false);
                break;
            case 13:
                TrophieName.text = "Game Won as Player Started 4th";
                TrophieName2nd.text = "Last being first trophies";
                careerTrophiesPoints.text = TrophiesHandler.Instance.trophyVariables["WinGamesfrom4th"] + "";
                if (TrophiesHandler.Instance.KudoTrophies[19].received == true)
                    Kudo.sprite = TrophiesHandler.Instance.KudoTrophies[19].TrophyImage;
                else
                    Kudo.gameObject.SetActive(false);
                if (TrophiesHandler.Instance.MasterTrophies[14].received == true)
                    Master.sprite = TrophiesHandler.Instance.MasterTrophies[19].TrophyImage;
                else
                    Master.gameObject.SetActive(false);
                if (TrophiesHandler.Instance.GoatTrophies[14].received == true)
                    Goat.sprite = TrophiesHandler.Instance.GoatTrophies[19].TrophyImage;
                else
                    Goat.gameObject.SetActive(false);
                break;
            case 14:
                TrophieName.text = "Trophies Won";
                TrophieName2nd.text = "Trophies trophies";
                careerTrophiesPoints.text = TrophiesHandler.Instance.trophyVariables["TotalTrophies"] + "";
                thisMonthTrophies.gameObject.SetActive(true);
                thisMonthTrophiesPoints.gameObject.SetActive(true);
                thisMonthTrophiesPoints.text = TrophiesHandler.Instance.trophyVariables["TotalTrophiesTM"] + "";
                if (TrophiesHandler.Instance.KudoTrophies[0].received == true)
                    Kudo.sprite = TrophiesHandler.Instance.KudoTrophies[0].TrophyImage;
                else
                    Kudo.gameObject.SetActive(false);
                if (TrophiesHandler.Instance.MasterTrophies[0].received == true)
                    Master.sprite = TrophiesHandler.Instance.MasterTrophies[0].TrophyImage;
                else
                    Master.gameObject.SetActive(false);
                if (TrophiesHandler.Instance.GoatTrophies[0].received == true)
                    Goat.sprite = TrophiesHandler.Instance.GoatTrophies[0].TrophyImage;
                else
                    Goat.gameObject.SetActive(false);
                break;
        }
    }

}
