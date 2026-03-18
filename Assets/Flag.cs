using UnityEngine;
using UnityEngine.UI;

public class Flag : MonoBehaviour
{
    public GameObject CheckMark;
    public int CountryIndex;

    private void Start()
    {
        if (CheckMark != null)
            CheckMark.SetActive(false);
    }

    public void CountrySelected()
    {
        if (Startgame.Instace.n == 1)
        {
            if (SettingPanel.instance != null && GameConfigration.instance != null &&
                GameConfigration.instance.countries != null &&
                CountryIndex >= 0 && CountryIndex < GameConfigration.instance.countries.Count)
            {
                SettingPanel.instance.CountryName.text = GameConfigration.instance.countries[CountryIndex].name;
                SettingPanel.instance.CountryImage.sprite = GameConfigration.instance.countries[CountryIndex];
            }
            CountryPanel.instance.backPressed();
        }
        else if (Startgame.Instace.n == 2)
        {
            CountryPanel.instance.backPressed();
        }
    }
}
