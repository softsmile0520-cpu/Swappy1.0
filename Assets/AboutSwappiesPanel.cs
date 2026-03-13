using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class AboutSwappiesPanel : MonoBehaviour
{
    public static AboutSwappiesPanel instance;

    [SerializeField]
    public UnityEngine.UI.Image PointsCalImagesPos;

    public GameObject PointsCalImagesPos0;
    public TextMeshProUGUI NextButtonText;
    [SerializeField]
    public UnityEngine.UI.Image TipsImagesPos;

    public GameObject TipsImagesPos0;
    public TextMeshProUGUI NextButtonTextTips;
    public int b;
    public List<Sprite> PointsCalImages = new List<Sprite>();
    public List<Sprite> TipsImages = new List<Sprite>();

    public static AboutSwappiesPanel ShowUI()
    {
        if (instance == null)
        {
            GameObject obj = Instantiate(Resources.Load("AboutSwappiesPanel")) as GameObject;

            obj.gameObject.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);

            instance = obj.GetComponent<AboutSwappiesPanel>();
        }

        return instance;
    }

    public void ShowPanel(int a)
    {
        b = 5;
        if (a == 0)
        {
            PointsCalImagesPos0.SetActive(true);
            if (b == 2)
                NextButtonText.text = "Close";

            if (b == 3)
            {
                PointsCalImagesPos0.SetActive(false);
                return;
            }
            PointsCalImagesPos.sprite = PointsCalImages[b];
            b++;
        }
        else
        {
            TipsImagesPos0.SetActive(true);
            if (b == 1)
                NextButtonText.text = "Close";

            if (b == 2)
            {
                TipsImagesPos0.SetActive(false);
                return;
            }
            TipsImagesPos.sprite = TipsImages[b];
            b++;
        }
    }
    public void HowToCountPoints()
    {
        PointsCalImagesPos0.SetActive(true);
        if (b == 2)
            NextButtonText.text = "Close";

        if (b == 3)
        {
            PointsCalImagesPos0.SetActive(false);
            b = 0;
            return;
        }
        PointsCalImagesPos.sprite = PointsCalImages[b];
        b++;
    }
    public void SomeTips()
    {
        TipsImagesPos0.SetActive(true);
        if (b == 1)
            NextButtonTextTips.text = "Close";

        if (b == 2)
        {
            TipsImagesPos0.SetActive(false);
            b = 0;
            return;
        }
        TipsImagesPos.sprite = TipsImages[b];
        b++;
    }
    public void BackToMenu()
    {
        //ModeSelectionPanelScript.ShowUI();
        backPressed();
    }
    public void backPressed()
    {
        GameConfigration.instance.PlayerSound(0);
        Destroy(this.gameObject);
    }

}
