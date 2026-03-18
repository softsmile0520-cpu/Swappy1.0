using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGameResultData : MonoBehaviour
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
        if ((player.Dead) || (Gamemanager.instance._PlayersList.Count(a => !a.Dead) == 1))
        {
            swappyimageText.gameObject.SetActive(false);
            SwappyImage.gameObject.SetActive(true);

            if (player.AiSwappy)
            {
                BGRectImage.sprite = PointsCalculator.instance.BGDisplayesRectangle[(int)player.PlayerDifficulty];
            }
            else
            {
                BGRectImage.sprite = PointsCalculator.instance.BGDisplayesRectangle[PointsCalculator.instance.BGDisplayesRectangle.Count - 1];
            }

            for (int i = 0; i < BGBoxImages.Count; i++)
            {
                if (player.AiSwappy)
                {
                    BGBoxImages[i].sprite = PointsCalculator.instance.BGDisplayesSquare[(int)player.PlayerDifficulty];
                }
                else
                {
                    BGBoxImages[i].sprite = PointsCalculator.instance.BGDisplayesSquare[PointsCalculator.instance.BGDisplayesSquare.Count - 1];
                }
            }

            Position.text = SetPosition(player.Position);
            Score.text = player.score.ToString();
            SwappyImage.sprite = player.SwapieDisplay.ViewImage;

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
            }
        }
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
