using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDataPreGame : MonoBehaviour
{
    public TextMeshProUGUI Position;

    public List<Image> BGBoxImages;
    public Image BGRectImage;

    public Image SwappyImage;
    public Image PlayerCountryPic;
    public Image PlayerProfilePic;

    public TextMeshProUGUI AISwappyName;
    public TextMeshProUGUI PlayerName;
    public TextMeshProUGUI PlayerCountryName;

    public TextMeshProUGUI Score;

    public TextMeshProUGUI swappyimageText;


    public void SetResult(SwappyPlayer player)
    {
        if (player.AiSwappy)
        {
            BGRectImage.sprite = PreGamePanel.instance.BGDisplayesRectangle[(int)player.PlayerDifficulty];
            AISwappyName.text = player.PlayerDifficulty.ToString();
        }
        else
        {
            BGRectImage.sprite = PreGamePanel.instance.BGDisplayesRectangle[PreGamePanel.instance.BGDisplayesRectangle.Count - 1];
        }

        for (int i = 0; i < BGBoxImages.Count; i++)
        {
            if (player.AiSwappy)
            {
                BGBoxImages[i].sprite = PreGamePanel.instance.BGDisplayesSquare[(int)player.PlayerDifficulty];
            }
            else
            {
                BGBoxImages[i].sprite = PreGamePanel.instance.BGDisplayesSquare[PreGamePanel.instance.BGDisplayesSquare.Count - 1];
            }
        }
        if (player.AiSwappy)
        {
            AISwappyName.text = player.PlayerDifficulty.ToString();
        }
        else
        {
            AISwappyName.gameObject.SetActive(false);
            PlayerName.gameObject.SetActive(true);
            PlayerName.text = TrophiesHandler.Instance.playerName;
            int ci = GameConfigration.instance.CountryUiIndex;
            if (GameConfigration.instance.countries != null && GameConfigration.instance.countries.Count > 0)
            {
                PlayerCountryPic.sprite = GameConfigration.instance.countries[ci];
                PlayerCountryName.text = GameConfigration.instance.countries[ci].name;
            }
            PlayerProfilePic.sprite = GameConfigration.instance.ProfilePic;
            float a = PlayerPrefs.GetFloat("PicSize", 1);
            PlayerProfilePic.transform.localScale = new Vector3(a, a, a);
            SettingPanel.ApplySavedProfilePicPan(PlayerProfilePic.rectTransform);
        }
        swappyimageText.gameObject.SetActive(false);
        SwappyImage.gameObject.SetActive(true);     
        SwappyImage.sprite = player.SwapieDisplay.ViewImage;
        PreGamePanel.instance.PosPlayer++;
        Position.text = SetPosition(PreGamePanel.instance.PosPlayer);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    string SetPosition(int n)
    {
        switch (n)
        {
            case 1:

                return "1st";

            case 2:

                return "2nd";

            case 3:

                return "3rd";

            case 4:

                return "4th";

        }
        return "";
    }
}
